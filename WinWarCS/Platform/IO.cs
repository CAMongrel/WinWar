﻿using System;
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
#if WINFX_CORE
            return '\\';
#else
            return Path.DirectorySeparatorChar;
#endif
         }
      }

#if WINFX_CORE
      public static async Task<Stream> GetFileStream(Windows.Storage.StorageFile file)
      {
         return await file.OpenStreamForReadAsync();
      }
#else
      public static async Task<FileStream> GetFileStream(string filename)
      {
         return new FileStream (filename, FileMode.OpenOrCreate);
      }
#endif

      /// <summary>
      /// Opens a file inside the content directory
      /// </summary>
      /// <returns>The content file relative to the installed location/application directoy</returns>
      /// <param name="relativeFilename">Relative filename.</param>
      public static async Task<FileStream> OpenContentFile(string relativeFilename)
      {
#if WINFX_CORE
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
#else
         string installedLocation = AppDomain.CurrentDomain.BaseDirectory;
         return await GetFileStream(System.IO.Path.Combine(installedLocation, relativeFilename));
#endif
      }

      public static string ExpectedDataDirectory()
      {
#if WINFX_CORE
         string installedLocation = Windows.ApplicationModel.Package.Current.InstalledLocation;
#else
         string installedLocation = AppDomain.CurrentDomain.BaseDirectory;
#endif
         return Path.Combine(installedLocation, "Assets" + Platform.IO.DirectorySeparatorChar + "Data" + Platform.IO.DirectorySeparatorChar);
      }
   }
}

