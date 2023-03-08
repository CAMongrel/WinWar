using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Advanced;
using SixLabors.ImageSharp.PixelFormats;

namespace WinWarGame.Data.Resources
{
   public class ImageResource : BasicResource
   {
      internal ushort width;
      internal ushort height;
      internal byte[] image_data;

      internal ImageResource(WarResource imgResource, WarResource palette, WarResource addPalette)
         : base(imgResource)
      {
         Type = ContentFileType.FileImage;

         CreateImageData(imgResource, palette, addPalette);
      }

      internal void CreateImageData(WarResource imgResource, WarResource palette, WarResource addPalette)
      {
         bool bForceGrayscale = palette == null;

         WarResource addPal = addPalette;

         int offset = 0;
         width = ReadUShort(offset, imgResource.data); offset += 2;
         height = ReadUShort(offset, imgResource.data); offset += 2;
         image_data = new byte[width * height * 4];

         int cnt = 0;

         int x, y;

         if ((palette == null) || (bForceGrayscale))  // No palette for this image or grayscale forced ... use grayscale palette
         {
            for (y = 0; y < height; y++) 
            {
               for (x = 0; x < width; x++) 
               {
                  int idx = offset + x + y * width;

                  image_data[cnt] = imgResource.data[idx];
                  cnt++;
                  image_data[cnt] = imgResource.data[idx];
                  cnt++;
                  image_data[cnt] = imgResource.data[idx];
                  cnt++;
                  image_data[cnt] = 255;
                  cnt++;
               }
            }
         }
         else
         {
            // We have a palette ... use it!
            for (y = 0; y < height; y++)
            {
               for (x = 0; x < width; x++)
               {
                  int pal_index = imgResource.data[offset + x + y * width] * 3;

                  image_data[cnt] = (byte)(palette.data[pal_index + 0] * 4);
                  cnt++;
                  image_data[cnt] = (byte)(palette.data[pal_index + 1] * 4);
                  cnt++;
                  image_data[cnt] = (byte)(palette.data[pal_index + 2] * 4);
                  cnt++;
                  image_data[cnt] = 255;// (byte)(((image_data[cnt - 3] == 0) && (image_data[cnt - 2] == 0) && (image_data[cnt - 1] == 0)) ? 0 : 255);
                  cnt++;
                  
                  if ((image_data[cnt - 4] == 228) &&
                      (image_data[cnt - 3] == 108) &&
                      (image_data[cnt - 2] == 228))
                  {
                     image_data[cnt - 4] = (byte)(addPal.data[pal_index] * 4);
                     image_data[cnt - 3] = (byte)(addPal.data[pal_index + 1] * 4);
                     image_data[cnt - 2] = (byte)(addPal.data[pal_index + 2] * 4);
                  }
                  
                  // Some manual fixes
                  // TODO: Figure out how this really works
                  
                  if ((image_data[cnt - 4] == 0xCC) &&
                      (image_data[cnt - 3] == 0) &&
                      (image_data[cnt - 2] == 0xD4))
                  {
                     image_data[cnt - 4] = 0x14;
                     image_data[cnt - 3] = 0x30;
                     image_data[cnt - 2] = 0x4d;
                  }
               }
            }
         }
      }

      internal override void WriteToFile(string filename)
      {
         Image<Rgba32> image = new Image<Rgba32>(width, height);
         for (int y = 0; y < height; y++)
         {
            var span = image.DangerousGetPixelRowMemory(y).Span;
            for (int x = 0; x < width; x++)
            {
               int idx = (y * width + x) * 4;
               span[x].R = image_data[idx + 0];
               span[x].G = image_data[idx + 1];
               span[x].B = image_data[idx + 2];
               span[x].A = image_data[idx + 3];
            }
         }
         image.Save(filename);
      }
   }
}