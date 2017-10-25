using System;

namespace WinWarCS.Data.Resources
{
   internal class CursorResource : BasicResource
   {
      internal ushort width;
      internal ushort height;
      internal byte[] image_data;

      public ushort HotSpotX { get; private set; }
      public ushort HotSpotY { get; private set; }

      internal CursorResource(WarResource data, WarResource palette)
      {
         Type = ContentFileType.FileCursor;

         CreateImageData(data, palette);
      }

      internal void CreateImageData(WarResource imgResource, WarResource palette)
      {
         if (imgResource == null)
            return;

         unsafe
         {
            fixed (byte* org_ptr = &imgResource.data[0])
            {
               ushort* usptr = (ushort*)org_ptr;

               HotSpotX = *usptr++;
               HotSpotY = *usptr++;

               width = *usptr++;
               height = *usptr++;

               byte* b_ptr = (byte*)usptr;

               image_data = new byte[width * height * 4];

               int cnt = 0;

               int x, y;

               if (palette == null)  // No palette for this image or grayscale forced ... use grayscale palette
               {
                  for (y = 0; y < height; y++)
                     for (x = 0; x < width; x++)
                     {
                        image_data[cnt] = b_ptr[x + y * width];
                        cnt++;
                        image_data[cnt] = b_ptr[x + y * width];
                        cnt++;
                        image_data[cnt] = b_ptr[x + y * width];
                        cnt++;
                        image_data[cnt] = 255;
                        cnt++;
                     }
               }
               else
               {
                  // We have a palette ... use it!
                  fixed (byte* pal_org_ptr = &palette.data[0])
                  {
                     byte* pal_dataptr = pal_org_ptr;
                     int pal_index;

                     for (y = 0; y < height; y++)
                     {
                        for (x = 0; x < width; x++)
                        {
                           pal_index = b_ptr[x + y * width] * 3;

                           image_data[cnt] = (byte)(pal_dataptr[pal_index + 0] * 4);
                           cnt++;
                           image_data[cnt] = (byte)(pal_dataptr[pal_index + 1] * 4);
                           cnt++;
                           image_data[cnt] = (byte)(pal_dataptr[pal_index + 2] * 4);
                           cnt++;
                           image_data[cnt] = (byte)(((image_data[cnt - 3] == 0) && (image_data[cnt - 2] == 0) && (image_data[cnt - 1] == 0)) ? 0 : 255);
                           cnt++;
                        }
                     }
                  }
               }
            }
         }
      }
   }
}

