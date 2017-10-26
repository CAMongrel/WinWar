using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Foundation;
using WinWarCS.Data;

namespace WinWarCS.iOS
{
   class IOSAssetProvider : IAssetProvider
   {
      public string InstalledLocation => NSFileManager.DefaultManager.GetUrl (NSSearchPathDirectory.DocumentDirectory, NSSearchPathDomain.User, null, false, out NSError error).Path;

      public string ExpectedDataDirectory => Path.Combine(InstalledLocation, "Assets" + DirectorySeparatorChar + "Data" + DirectorySeparatorChar);

      public char DirectorySeparatorChar => Path.DirectorySeparatorChar;

      public async Task<FileStream> OpenContentFile(string relativeFilename, bool readOnly = true)
      {
         string filename = System.IO.Path.Combine(InstalledLocation, relativeFilename);
         return new FileStream(filename, readOnly ? FileMode.Open : FileMode.OpenOrCreate, readOnly ? FileAccess.Read : FileAccess.ReadWrite);
      }
   }
}
