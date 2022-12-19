﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace WinWarCS.Data
{
    public interface IAssetProvider
    {
        bool isFullVersion { get; }
        string InstalledLocation { get; }
        string AssetsDirectory { get; }
        string FullDataDirectory { get; }
        string DemoDataDirectory { get; }
        string DataDirectory { get; }

        FileStream OpenGameDataFile(string relativeFilename, bool readOnly = true);
        FileStream OpenAssetFile(string relativeFilename, bool readOnly = true);
    }
}
