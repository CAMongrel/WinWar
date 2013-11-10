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
using WinWarRT.Data.Resources;
using Windows.Storage.Streams;
using System.Threading.Tasks;
#endregion

namespace WinWarRT.Data
{
   internal class WarFile
   {
      #region Members
      //static string dataFilename;
      static int nrOfEntries;
      static int[] offsets;
      static List<WarResource> resources;
      #endregion

      #region LoadResources
      internal async static Task<Windows.Storage.StorageFile> GetDataWarFile()
      {
         Windows.Storage.StorageFile resultFile = null;

         try
         {
            var localStorage = Windows.Storage.ApplicationData.Current.LocalFolder;
            resultFile = await localStorage.GetFileAsync("DATAA.WAR");
         }
         catch (Exception)
         {
         }

         if (resultFile != null)
            return resultFile;

         try
         {
            var localStorage = Windows.ApplicationModel.Package.Current.InstalledLocation;
            localStorage = await localStorage.GetFolderAsync("Assets");
            resultFile = await localStorage.GetFileAsync("DATA.WAR");
         }
         catch (Exception)
         {
         }

         return resultFile;
      }

      internal async static Task LoadResources()
      {
         Windows.Storage.StorageFile dataFile = await GetDataWarFile();

         Stream stream = null;
         BinaryReader reader = null;
         try
         {
            stream = await dataFile.OpenStreamForReadAsync();

            reader = new BinaryReader(stream);

            reader.ReadInt32();							// ID
            nrOfEntries = (int)reader.ReadInt16();		// Number of entries
            reader.ReadInt16();							// File ID

            offsets = new int[nrOfEntries];
            for (int i = 0; i < nrOfEntries; i++)
               offsets[i] = reader.ReadInt32();

            resources = new List<WarResource>(nrOfEntries);

            ReadResources(reader);
         }
         finally
         {
            reader.Dispose();
            stream.Dispose();
         }
      }
      #endregion

      #region ReadResources
      private static void ReadResources(BinaryReader br)
      {
         for (int i = 0; i < nrOfEntries; i++)
         {
            // Happens with demo data
            if (offsets[i] == -1)
            {
               resources.Add(null);
               continue;
            }

            br.BaseStream.Seek((long)offsets[i], SeekOrigin.Begin);

            int length = 0;
            if (i < nrOfEntries - 1)
               length = offsets[i + 1] - offsets[i];
            else
               length = (int)br.BaseStream.Length - offsets[i];

            resources.Add(new WarResource(br, offsets[i], length, i));
         }
      }
      #endregion

      #region GetImageResource
      internal static ImageResource GetImageResource(int id)
      {
         if ((id < 0 || id >= KnowledgeBase.KB_List.Length))
            return null;

         if (KnowledgeBase.KB_List[id].type != WarFileType.FileImage)
            return null;

         return new ImageResource(GetResource(id), GetResource(KnowledgeBase.KB_List[id].param));
      }
      #endregion

      #region GetSpriteResource
      internal static SpriteResource GetSpriteResource(int id)
      {
         if ((id < 0 || id >= KnowledgeBase.KB_List.Length))
            return null;

         if (KnowledgeBase.KB_List[id].type != WarFileType.FileSprite)
            return null;

         return new SpriteResource(GetResource(id), GetResource(KnowledgeBase.KB_List[id].param));
      }
      #endregion

      #region GetTextResource
      internal static TextResource GetTextResource(int id)
      {
         if ((id < 0 || id >= KnowledgeBase.KB_List.Length))
            return null;

         if (KnowledgeBase.KB_List[id].type != WarFileType.FileText)
            return null;

         return new TextResource(GetResource(id));
      }
      #endregion

      #region GetResource
      internal static WarResource GetResource(int index)
      {
         if ((index < 0) || (index >= Count))
            return null;

         return resources[index];
      }
      #endregion

      #region GetResourceByName
      /// <summary>
      /// Returns the resource using a hash table indexed by the names
      /// </summary>
      /// <param name="name">Name of the resource</param>
      /// <returns>The resource or null if no resource of the given name exists</returns>
      internal static WarResource GetResourceByName(string name)
      {
         int idx = KnowledgeBase.IndexByName(name);
         if (idx == -1)
            return null;

         return resources[idx];
      }
      #endregion

      #region Properties
      internal static int Count
      {
         get { return resources.Count; }
      }
      #endregion
   }
}
