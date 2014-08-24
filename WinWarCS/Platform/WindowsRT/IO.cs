using System;
using System.IO;
using System.Threading.Tasks;

namespace WinWarCS.Platform
{
    public static class IO
    {
        public static char DirectorySeparatorChar
        {
            get
            {
                return '\\';
            }
        }

        public static async Task<Stream> GetFileStream(Windows.Storage.StorageFile file)
        {
            return await file.OpenStreamForReadAsync();
        }

        /// <summary>
        /// Opens a file inside the content directory
        /// </summary>
        /// <returns>The content file relative to the installed location/application directoy</returns>
        /// <param name="relativeFilename">Relative filename.</param>
        public static async Task<Stream> OpenContentFile(string relativeFilename)
        {
            Stream resultStream = null;
            var installedLocation = Windows.ApplicationModel.Package.Current.InstalledLocation;

            try
            {
                resultStream = await GetFileStream(await installedLocation.GetFileAsync(relativeFilename));
            }
            catch (Exception ex)
            {

            }

            if (resultStream == null)
            {
                var localStorage = Windows.Storage.ApplicationData.Current.LocalFolder;
                resultStream = await GetFileStream(await localStorage.GetFileAsync(relativeFilename));
            }                

            return resultStream;
        }

		public static string ExpectedDataDirectory()
		{
			var installedLocation = Windows.ApplicationModel.Package.Current.InstalledLocation;
			return System.IO.Path.Combine(installedLocation.Path, "Assets" + Platform.IO.DirectorySeparatorChar + "Data" + Platform.IO.DirectorySeparatorChar);
		}
    }
}

