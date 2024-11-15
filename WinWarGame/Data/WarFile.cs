// Author: Henning
// Project: WinWarEngine
// Path: P:\Projekte\WinWarCS\WinWarEngine\Data
// Creation date: 18.11.2009 10:13
// Last modified: 27.11.2009 10:10

using WinWarGame.Data.Resources;
using WinWarGame.Util;

#region Using directives
using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
#endregion

namespace WinWarGame.Data
{
    public enum DataWarFileType
    {
        Unknown,
        Demo,
        Retail,
        RetailCD
    }

    public static class WarFile
    {
        #region Members

        private static int fileID;    // 0x18 = Full; 0x19 = Demo
        private static int nrOfEntries;
        private static uint[] offsets;

        /// <summary>
        /// List containing all raw resources from DATA.WAR
        /// </summary>
        private static List<WarResource> rawResources;
        /// <summary>
        /// Dictionary with strongly typed resources. Lazy-loaded at runtime.
        /// </summary>
        private static Dictionary<int, BasicResource> resourcesDict;
        /// <summary>
        /// The knowledge base for the currently loaded DATA.WAR
        /// Available after loading calling LoadResources()
        /// </summary>
        public static KnowledgeBase KnowledgeBase;

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

        #region Constructor
        static WarFile()
        {
            rawResources = null;
            resourcesDict = new Dictionary<int, BasicResource>();
        }
        #endregion

        #region LoadResources

        public static void LoadResources(IAssetProvider assetProvider)
        {
            Stream stream = null;
            try
            {
                stream = assetProvider.OpenGameDataFile("DATA.WAR");
                LoadResourcesFromStream(stream);
            }
            finally
            {
                stream?.Dispose();
                stream = null;
            }
        }

        public static void LoadResourcesFromStream(Stream stream)
        {
            BinaryReader reader = null;
            try
            {
                Performance.Push("Loading DATA.WAR");

                reader = new BinaryReader(stream);

                fileID = reader.ReadInt32();                            // ID
                nrOfEntries = reader.ReadInt32();       // Number of entries

                offsets = new uint[nrOfEntries];
                for (int i = 0; i < nrOfEntries; i++)
                    offsets[i] = reader.ReadUInt32();

                Type = DataWarFileType.Unknown;
                switch (fileID)
                {
                    case 0x19:
                        Type = DataWarFileType.Demo;
                        break;
                    case 0x18:
                        Type = DataWarFileType.RetailCD;
                        break;
                }

                // Create KnowledgeBase based on type of DATA.WAR
                KnowledgeBase = new KnowledgeBase(Type);

                // Load resources
                rawResources = new List<WarResource>(nrOfEntries);
                ReadResources(reader);

                Performance.Pop();

                Log.Write(LogType.Resources, LogSeverity.Status, "KnowledgeBase contains " + KnowledgeBase.Count + " entries. Loaded " + rawResources.Count + " raw resources");
            }
            finally
            {
                if (reader != null)
                {
                    reader.Dispose();
                    reader = null;
                }
            }
        }

        #endregion

        #region Unload
        public static void Unload()
        {
            rawResources = null;
            KnowledgeBase = null;
        }
        #endregion

        #region ReadResources

        private static int GetLength(BinaryReader br, int index)
        {
            if (offsets[index] == 0xFFFFFFFF)
                return 0;

            if (index == nrOfEntries - 1)
                return (int)br.BaseStream.Length - (int)offsets[index];

            int counter = 1;
            uint nextOffset = offsets[index + counter++];
            while (nextOffset == 0xFFFFFFFF)
            {
                if (index + counter >= offsets.Length)
                {
                    nextOffset = (uint)br.BaseStream.Length;
                    break;
                }

                nextOffset = offsets[index + counter++];
            }

            return (int)(nextOffset - offsets[index]);
        }

        private static BasicResource CreateResource(int index)
        {
            WarResource resource = rawResources[index];

            KnowledgeEntry ke = KnowledgeBase[index];

            ContentFileType fileType = ContentFileType.FileUnknown;
            if (ke != null)
            {
                fileType = ke.type;
            }

            switch (fileType)
            {
                case ContentFileType.FileCursor:
                    {
                        WarResource palRes = null;
                        if (ke != null)
                        {
                            palRes = rawResources[ke.param];
                        }

                        return new CursorResource(resource, palRes);
                    }

                case ContentFileType.FileImage:
                    {
                        WarResource palRes = null;
                        if (ke != null)
                        {
                            palRes = rawResources[ke.param];
                        }

                        return new ImageResource(resource, palRes, rawResources[191]);
                    }

                case ContentFileType.FileLevelInfo:
                    return new LevelInfoResource(resource);

                case ContentFileType.FileLevelPassable:
                    return new LevelPassableResource(resource);

                case ContentFileType.FileLevelVisual:
                    return new LevelVisualResource(resource);

                case ContentFileType.FilePalette:
                    return new PaletteResource(resource);

                case ContentFileType.FilePaletteShort:
                    return new PaletteResource(resource);

                case ContentFileType.FileSprite:
                    {
                        WarResource palRes = null;
                        if (ke != null)
                        {
                            palRes = rawResources[ke.param];
                        }

                        return new SpriteResource(resource, palRes, rawResources[191]);
                    }

                case ContentFileType.FileTable:
                    return new TableResource(resource);

                case ContentFileType.FileText:
                    return new TextResource(resource);

                case ContentFileType.FileTiles:
                    return new RawResource(resource, ContentFileType.FileTiles);

                case ContentFileType.FileTileSet:
                    return new RawResource(resource, ContentFileType.FileTileSet);

                case ContentFileType.FileUI:
                    return new UIResource(resource);

                case ContentFileType.FileVOC:
                    return new RawResource(resource, ContentFileType.FileVOC);

                case ContentFileType.FileWave:
                    return new RawResource(resource, ContentFileType.FileWave);

                case ContentFileType.FileXMID:
                    return new RawResource(resource, ContentFileType.FileXMID);

                default:
                    return new RawResource(resource, ContentFileType.FileUnknown);
            }
        }

        private static int ReadResources(BinaryReader br)
        {
            // Read all raw resources from DATA.WAR without processing them (yet)
            int result = 0;
            for (int i = 0; i < nrOfEntries; i++)
            {
                // Happens with demo data
                if (offsets[i] == 0xFFFFFFFF)
                {
                    rawResources.Add(null);
                    continue;
                }

                br.BaseStream.Seek((long)offsets[i], SeekOrigin.Begin);

                int compr_length = GetLength(br, i);
                long offset = br.BaseStream.Position;

                WarResource resource = new WarResource(br, offset, compr_length, i);
                rawResources.Add(resource);

                result++;
            }

            return result;
        }

        private static void WriteResource(BinaryWriter writer, WarResource res)
        {
            writer.Write((ushort)res.data.Length);
            writer.Write((byte)0);

            byte comprFlag = 0;
            writer.Write(comprFlag);

            writer.Write(res.data);
        }

        #endregion

        #region WriteWarFile
        public static void WriteWarFile(string outputFile, bool forceStrongTyped)
        {
            using (BinaryWriter writer = new BinaryWriter(File.Create(outputFile)))
            {
                writer.Write(fileID);
                writer.Write(rawResources.Count);

                uint offsetOfOffsetTable = (uint)writer.BaseStream.Position;
                // Write empty offset table
                for (int i = 0; i < rawResources.Count; i++)
                    writer.Write((int)0);

                uint curOffset = (uint)writer.BaseStream.Position;

                // Write each resource and remember offset
                for (int i = 0; i < rawResources.Count; i++)
                {
                    if (rawResources[i] == null)
                    {
                        offsets[i] = 0xFFFFFFFF;
                        continue;
                    }

                    offsets[i] = curOffset;

                    if (forceStrongTyped)
                    {
                        // Force loading of resource
                        BasicResource res = GetResource(i);
                        res.WriteToStream(writer);
                    }
                    else
                    {
                        WriteResource(writer, rawResources[i]);
                    }

                    curOffset = (uint)writer.BaseStream.Position;
                }

                // Write full offset table
                writer.BaseStream.Seek(offsetOfOffsetTable, SeekOrigin.Begin);
                for (int i = 0; i < rawResources.Count; i++)
                    writer.Write(offsets[i]);
            }
        }
        #endregion

        #region GetImageResource

        public static ImageResource GetImageResource(int id)
        {
            if ((id < 0 || id >= KnowledgeBase.Count))
            {
                return null;
            }

            if (KnowledgeBase[id].type != ContentFileType.FileImage)
            {
                return null;
            }

            return GetResource(id) as ImageResource;
        }

        #endregion

        #region GetCursorResource

        public static CursorResource GetCursorResource(int id)
        {
            if ((id < 0 || id >= KnowledgeBase.Count))
                return null;

            if (KnowledgeBase[id].type != ContentFileType.FileCursor)
                return null;

            return GetResource(id) as CursorResource;
        }

        #endregion

        #region GetSpriteResource

        public static SpriteResource GetSpriteResource(int id)
        {
            Performance.Push("GetSpriteResource");
            try
            {
                if ((id < 0 || id >= KnowledgeBase.Count))
                    return null;

                if (KnowledgeBase[id].type != ContentFileType.FileSprite)
                    return null;

                return GetResource(id) as SpriteResource;
            }
            finally
            {
                Performance.Pop();
            }
        }

        #endregion

        #region GetUIResource

        public static UIResource GetUIResource(int id)
        {
            if ((id < 0 || id >= KnowledgeBase.Count))
                return null;

            if (KnowledgeBase[id].type != ContentFileType.FileUI)
                return null;

            return GetResource(id) as UIResource;
        }

        #endregion

        #region GetResource

        public static BasicResource GetResource(int index)
        {
            if ((index < 0) || (index >= Count))
            {
                return null;
            }

            if (resourcesDict.ContainsKey(index))
            {
                return resourcesDict[index];
            }

            if (rawResources[index] == null)
            {
                return null;
            }

            // Lazy-load the requested resource
            Performance.Push("Load resource " + index);
            BasicResource res = CreateResource(index);
            resourcesDict.Add(index, res);
            Performance.Pop();

            Log.Write(LogType.Resources, LogSeverity.Debug, "Created resource of type '" + res.GetType().Name + "' for index " + index);

            return res;
        }

        #endregion

        #region GetResourceByName

        /// <summary>
        /// Returns the resource using a hash table indexed by the names
        /// </summary>
        /// <param name="name">Name of the resource</param>
        /// <returns>The resource or null if no resource of the given name exists</returns>
        public static BasicResource GetResourceByName(string name)
        {
            int idx = KnowledgeBase.IndexByName(name);
            if (idx == -1)
            {
                return null;
            }

            return GetResource(idx);
        }

        #endregion

        #region Properties

        public static bool AreResoucesLoaded
        {
            get
            {
                return rawResources != null && KnowledgeBase != null;
            }
        }

        public static int Count
        {
            get
            {
                if (rawResources == null)
                    return 0;

                return rawResources.Count;
            }
        }

        #endregion

        #region DumpResourcesToAssetPath

        public static void DumpResourcesToAssetPath(string subfolder, bool onlyKnownTypes)
        {
            string fn = Path.Combine(MainGame.AssetProvider.AssetsDirectory, subfolder);
            if (Directory.Exists(fn) == false)
            {
                Directory.CreateDirectory(fn);
            }
            for (int i = 0; i < WarFile.Count; i++)
            {
                string outfn = Path.Combine(fn, "res_" + i.ToString("D4"));

                var res = WarFile.GetResource(i);
                switch (res.Type)
                {
                    case ContentFileType.FileImage:
                    {
                        var imgRes = (ImageResource)res;
                        imgRes.WriteToFile(outfn + ".png");
                    }
                        break;
                    
                    case ContentFileType.FileSprite:
                    {
                        var sprRes = (SpriteResource)res;
                        sprRes.WriteToFile(outfn + ".png");
                    }
                        break;
                    
                    default:
                        if (onlyKnownTypes == false)
                        {
                            res.WriteToFile(outfn + ".res");
                        }
                        break;
                }
            }
        }
        #endregion
    }
}
