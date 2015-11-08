using System;
using System.IO;
using Foundation;
using System.Threading.Tasks;

namespace WinWarCS.Platform
{
   public static class IO
   {
      public static char DirectorySeparatorChar
      {
         get
         {
            return Path.DirectorySeparatorChar;
         }
      }

      public static async Task<FileStream> GetFileStream(string filename, bool readOnly = true)
      {
         return new FileStream (filename, FileMode.Open, readOnly ? FileAccess.Read : FileAccess.ReadWrite);
      }

      /// <summary>
      /// Opens a file inside the content directory
      /// </summary>
      /// <returns>The content file relative to the installed location/application directoy</returns>
      /// <param name="relativeFilename">Relative filename.</param>
      public static async Task<FileStream> OpenContentFile(string relativeFilename)
      {
         string installedLocation = "";//NSBundle.MainBundle.ResourcePath;
         return await GetFileStream(System.IO.Path.Combine(installedLocation, relativeFilename));
      }

		public static string ExpectedDataDirectory()
		{
         string installedLocation = "";//NSBundle.MainBundle.ResourcePath;
			return Path.Combine(installedLocation, "Assets" + Platform.IO.DirectorySeparatorChar + "Data" + Platform.IO.DirectorySeparatorChar);
		}
   }
}

