using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using WinWarGame.Data;

namespace WinWar.Desktop
{
    class NetAssetProvider : IAssetProvider
    {
        public bool IsFullVersion { get; private set; } = false;

        public string InstalledLocation => Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);

        public string AssetsDirectory => Path.Combine(InstalledLocation, "Assets");

        public string FullDataDirectory => Path.Combine(AssetsDirectory, "DATA");

        public string DemoDataDirectory => Path.Combine(AssetsDirectory, "DEMODATA");

        public string DataDirectory
        {
            get
            {
                if (Directory.Exists(FullDataDirectory))
                {
                    IsFullVersion = true;
                    return FullDataDirectory;
                }
                else if (Directory.Exists(DemoDataDirectory))
                {
                    IsFullVersion = false;
                    return DemoDataDirectory;
                }
                else
                {
                    return String.Empty;
                }
            }
        }

        public FileStream OpenGameDataFile(string relativeFilename, bool readOnly = true)
        {
            string dataDir = DataDirectory;
            if (string.IsNullOrEmpty(dataDir))
            {
                throw new FileNotFoundException("DATA directory not found");
            }

            FileStream result = null;
            if (readOnly)
            {
                result = File.OpenRead(Path.Combine(dataDir, relativeFilename));
            }
            else
            {
                result = File.Open(Path.Combine(dataDir, relativeFilename), FileMode.Open);
            }

            return result;
        }

        public FileStream OpenAssetFile(string relativeFilename, bool readOnly = true)
        {
            string assetDir = AssetsDirectory;
            if (string.IsNullOrEmpty(assetDir))
            {
                throw new FileNotFoundException("Asset directory not found");
            }

            FileStream result = null;
            if (readOnly)
            {
                result = File.OpenRead(Path.Combine(assetDir, relativeFilename));
            }
            else
            {
                result = File.Open(Path.Combine(assetDir, relativeFilename), FileMode.Open);
            }

            return result;
        }
    }
}
