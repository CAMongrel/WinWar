using System;
using System.Collections.Generic;
using System.Text;

namespace WinWarGame.Data.Resources
{
   public class ImageResource : BasicResource
   {
      internal ushort width;
      internal ushort height;
      internal byte[] image_data;

      internal ImageResource(WarResource imgResource, WarResource palette, WarResource addPalette)
      {
         Type = ContentFileType.FileImage;

         CreateImageData(imgResource, palette, addPalette);
      }

      internal void CreateImageData(WarResource imgResource, WarResource palette, WarResource addPalette)
      {
         bool bForceGrayscale = palette == null;

         WarResource addPal = addPalette;

         int offset = 0;
         width = ReadUShort (offset, imgResource.data); offset += 2;
         height = ReadUShort (offset, imgResource.data); offset += 2;
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

                  image_data [cnt] = imgResource.data [idx];
                  cnt++;
                  image_data [cnt] = imgResource.data [idx];
                  cnt++;
                  image_data [cnt] = imgResource.data [idx];
                  cnt++;
                  image_data [cnt] = 255;
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
               }
            }
         }
      }
   }
}
