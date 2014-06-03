using System;
using System.IO;

namespace WinWarCS.Platform
{
   public static class IO
   {
      public static FileStream GetFileStream(string filename)
      {
         return new FileStream (filename, FileMode.OpenOrCreate);
      }

      /// <summary>
      /// Opens a file inside the content directory
      /// </summary>
      /// <returns>The content file relative to the installed location/application directoy</returns>
      /// <param name="relativeFilename">Relative filename.</param>
      public static FileStream OpenContentFile(string relativeFilename)
      {
         string installedLocation = AppDomain.CurrentDomain.BaseDirectory;
         return GetFileStream(System.IO.Path.Combine(installedLocation, relativeFilename));
      }

      /*var localStorage = global::Windows.ApplicationModel.Package.Current.InstalledLocation;
         localStorage = await localStorage.GetFolderAsync("Assets\\Data");
         var resultFile = await localStorage.GetFileAsync("TITLE.WAR");*/

      /*internal async static Task<Windows.Storage.StorageFile> GetDataWarFile()
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
      }*/
   }
}

