using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using WinWarCS.Data;

namespace WinWar
{
    class NetCoreAssetProvider : IAssetProvider
    {
        public string InstalledLocation => throw new NotImplementedException();

        public string ExpectedDataDirectory => throw new NotImplementedException();

        public char DirectorySeparatorChar => throw new NotImplementedException();

        public Task<FileStream> OpenContentFile(string relativeFilename, bool readOnly = true)
        {
            throw new NotImplementedException();
        }
    }
}
