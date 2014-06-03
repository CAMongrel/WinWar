using System;
using System.IO;

namespace WinWarCS.Platform
{
   public static class IO
   {
      public static Stream GetFileStream(string filename)
      {
         return new FileStream (filename, FileMode.OpenOrCreate);
      }

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

