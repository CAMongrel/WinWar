// Author: Henning
// Project: WinWarEngine
// Path: P:\Projekte\WinWarCS\WinWarEngine\Data
// Creation date: 18.11.2009 10:13
// Last modified: 27.11.2009 10:10

#region Using directives
using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using WinWarCS.Data.Resources;
using System.Threading.Tasks;

#endregion
namespace WinWarCS.Data
{
   internal enum DataWarFileType
   {
      Unknown,
      Demo,
      Retail,
      RetailCD
   }

   internal class WarFile
   {
      #region Members

      //static string dataFilename;
      private static int nrOfEntries;
      private static int[] offsets;
      private static List<WarResource> resources;

      /// <summary>
      /// The maximum number of entries allowed for the Data.WAR that's currently la
      /// </summary>
      private static int maxNrOfEntries;

      internal static KnowledgeBase KnowledgeBase;

      #endregion

      #region Properties
      public static DataWarFileType Type { get; private set; }

      public static bool IsDemo
      {
         get
         {
            return Type == DataWarFileType.Demo;
         }
      }
      public static bool HasIntroSpeech
      {
         get
         {
            return Type == DataWarFileType.RetailCD;
         }
      }
      #endregion

      #region LoadResources

      internal static async Task LoadResources ()
      {
         Stream stream = null;
         BinaryReader reader = null;
         try
         {
             stream = await WinWarCS.Platform.IO.OpenContentFile("Assets" + Platform.IO.DirectorySeparatorChar + "Data" + 
                 Platform.IO.DirectorySeparatorChar + "DATA.WAR");

            reader = new BinaryReader (stream);

            reader.ReadInt32 ();							// ID
            nrOfEntries = (int)reader.ReadInt16 ();		// Number of entries
            reader.ReadInt16 ();							// File ID

            offsets = new int[nrOfEntries];
            for (int i = 0; i < nrOfEntries; i++)
               offsets [i] = reader.ReadInt32 ();

            resources = new List<WarResource> (nrOfEntries);
            int actualNumberOfResources = ReadResources (reader);

            switch (actualNumberOfResources)
            {
            case 299:
               Type = DataWarFileType.Demo;
               break;
            case 486:
               Type = DataWarFileType.Retail;            
               break;
            case 583:
               Type = DataWarFileType.RetailCD;
               break;
            default:
               Type = DataWarFileType.Unknown;
               break;
            }

            KnowledgeBase = new KnowledgeBase(Type);
         } finally
         {
            if (reader != null)
               reader.Dispose ();
            stream.Dispose ();
         }
      }

      #endregion

      #region ReadResources

      private static int GetLength(BinaryReader br, int index)
      {
         if (offsets [index] == -1)
            return 0;

         if (index == nrOfEntries - 1)
            return (int)br.BaseStream.Length - offsets [index];

         int counter = 1;
         int nextOffset = offsets [index + counter++];
         while (nextOffset == -1) 
         {
            if (index + counter >= offsets.Length) 
            {
               nextOffset = (int)br.BaseStream.Length;
               break;
            }

            nextOffset = offsets [index + counter++];
         }

         return nextOffset - offsets[index];
      }

      private static int ReadResources (BinaryReader br)
      {
         int result = 0;
         for (int i = 0; i < nrOfEntries; i++)
         {
            // Happens with demo data
            if (offsets [i] == -1)
            {
               resources.Add (null);
               continue;
            }

            br.BaseStream.Seek ((long)offsets [i], SeekOrigin.Begin);

            int length = GetLength (br, i);

            resources.Add (new WarResource (br, offsets [i], length, i));
            result++;
         }
         return result;
      }

      #endregion

      #region DumpResources
#if !NETFX_CORE
      internal static void DumpResources(string path)
      {
         for (int i = 0; i < resources.Count; i++)
         {
            ContentFileType fileType = ContentFileType.FileUnknown;
            if (i < KnowledgeBase.Count)
               fileType = KnowledgeBase[i].type;
            string filename = Path.Combine (path, "res" + i + "." + fileType);
            File.WriteAllBytes (filename, resources [i].data);
         }
      }
#endif
      #endregion

      #region GetImageResource

      internal static ImageResource GetImageResource (int id)
      {
         if ((id < 0 || id >= KnowledgeBase.Count))
            return null;

         if (KnowledgeBase[id].type != ContentFileType.FileImage)
            return null;

         return new ImageResource (GetResource (id), GetResource (KnowledgeBase[id].param));
      }

      #endregion

      #region GetCursorResource

      internal static CursorResource GetCursorResource (int id)
      {
         if ((id < 0 || id >= KnowledgeBase.Count))
            return null;

         if (KnowledgeBase[id].type != ContentFileType.FileCursor)
            return null;

         WarResource pal = null;
         if (KnowledgeBase[KnowledgeBase[id].param].type == ContentFileType.FilePalette)
            pal = GetResource (KnowledgeBase[id].param);

         return new CursorResource (GetResource (id), pal);
      }

      #endregion

      #region GetSpriteResource

      internal static SpriteResource GetSpriteResource (int id)
      {
         if ((id < 0 || id >= KnowledgeBase.Count))
            return null;

         if (KnowledgeBase[id].type != ContentFileType.FileSprite)
            return null;

         WarResource pal = null;
         if (KnowledgeBase[KnowledgeBase[id].param].type == ContentFileType.FilePalette)
            pal = GetResource (KnowledgeBase[id].param);

         return new SpriteResource (GetResource (id), pal);
      }

      #endregion

      #region GetUIResource

      internal static UIResource GetUIResource(int id)
      {
         if ((id < 0 || id >= KnowledgeBase.Count))
            return null;

         if (KnowledgeBase[id].type != ContentFileType.FileText)
            return null;

         return new UIResource(GetResource(id));
      }

      #endregion

      #region GetResource

      internal static WarResource GetResource (int index)
      {
         if ((index < 0) || (index >= Count))
            return null;

         return resources [index];
      }

      #endregion

      #region GetResourceByName

      /// <summary>
      /// Returns the resource using a hash table indexed by the names
      /// </summary>
      /// <param name="name">Name of the resource</param>
      /// <returns>The resource or null if no resource of the given name exists</returns>
      internal static WarResource GetResourceByName (string name)
      {
         int idx = KnowledgeBase.IndexByName (name);
         if (idx == -1)
            return null;

         return resources [idx];
      }

      #endregion

      #region Properties

      internal static int Count
      {
         get 
         {
            if (resources == null)
               return 0;

            return resources.Count; 
         }
      }

      #endregion
   }
}
