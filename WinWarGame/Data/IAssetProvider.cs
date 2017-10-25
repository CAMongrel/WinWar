using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace WinWarGame.Data
{
   public interface IAssetProvider
   {
      string InstalledLocation { get; }
      string ExpectedDataDirectory { get; }

      char DirectorySeparatorChar { get; }

      Task<FileStream> OpenContentFile(string relativeFilename, bool readOnly = true);
   }
}
