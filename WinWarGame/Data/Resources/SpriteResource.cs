using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinWarCS.Data.Resources
{
   internal struct SpriteResourceFrame
   {
      internal byte disp_x;
      internal byte disp_y;
      internal byte width;
      internal byte height;
      internal int offset;

      internal byte[] image_data;
   }

   internal class SpriteResource : BasicResource
   {
      internal int MaxWidth;
      internal int MaxHeight;
      internal int FrameCount;
      internal SpriteResourceFrame[] Frames;

      private WarResource palette;

      internal SpriteResource(WarResource data, WarResource palette, WarResource addPalette)
      {
         Type = ContentFileType.FileSprite;

         Frames = null;

         CreateImageData(data, palette, addPalette);
      }

      internal void CreateImageData(WarResource imgResource, WarResource palette, WarResource addPalette)
      {
         int i, offset, x, y, c;

         WarResource addPal = addPalette;

         unsafe
         {
            fixed (byte* org_ptr = &imgResource.data[0])
            {
               FrameCount = org_ptr[0] + (org_ptr[1] << 8);
               MaxWidth = org_ptr[2];
               MaxHeight = org_ptr[3];

               Frames = new SpriteResourceFrame[FrameCount];

               offset = 4;
               for (i = 0; i < FrameCount; i++)
               {
                  Frames[i].disp_x = org_ptr[offset];
                  Frames[i].disp_y = org_ptr[offset + 1];
                  Frames[i].width = org_ptr[offset + 2];
                  Frames[i].height = org_ptr[offset + 3];
                  Frames[i].offset = org_ptr[offset + 4] + (org_ptr[offset + 5] << 8)
                                  + (org_ptr[offset + 6] << 16) + (org_ptr[offset + 7] << 24);
                  offset += 8;
               }

               for (i = 0; i < FrameCount; i++)
               {
                  offset = Frames[i].offset;
                  Frames[i].image_data = new byte[MaxWidth * MaxHeight * 4];

                  int temp_index;

                  if (palette == null)
                  {	// No palette for this image or grayscale forced ... use grayscale palette
                     for (y = Frames[i].disp_y; y < (Frames[i].height + Frames[i].disp_y); y++)
                     {
                        for (x = Frames[i].disp_x; x < (Frames[i].width + Frames[i].disp_x); x++)
                        {
                           temp_index = (x + (y * MaxWidth)) * 4;

                           for (c = 0; c < 3; c++)
                              Frames[i].image_data[temp_index + c] = org_ptr[offset];

                           Frames[i].image_data[temp_index + 3] = 255;

                           offset++;
                        }
                     }
                  }
                  else
                  {
                     // We have a palette ... use it!
                     fixed (byte* pal_org_ptr = &palette.data[0])
                     {
                        byte* pal_dataptr = pal_org_ptr;
                        int pal_index;

                        for (y = Frames[i].disp_y; y < (Frames[i].height + Frames[i].disp_y); y++)
                        {
                           for (x = Frames[i].disp_x; x < (Frames[i].width + Frames[i].disp_x); x++)
                           {
                              temp_index = (x + (y * MaxWidth)) * 4;
                              pal_index = org_ptr[offset++] * 3;

                              if (pal_index / 3 == 96)
                              {
                                 // Shadow
                                 Frames[i].image_data[temp_index] = 0;
                                 Frames[i].image_data[temp_index + 1] = 0;
                                 Frames[i].image_data[temp_index + 2] = 0;
                                 Frames[i].image_data[temp_index + 3] = 127;
                                 continue;
                              }

                              for (c = 0; c < 3; c++)
                                 Frames[i].image_data[temp_index + c] = (byte)(pal_dataptr[pal_index + c] * 4);

                              Frames[i].image_data[temp_index + 3] = (((Frames[i].image_data[temp_index] == 0) && (Frames[i].image_data[temp_index + 1] == 0) &&
                                                             (Frames[i].image_data[temp_index + 2] == 0)) ? (byte)0 : (byte)255);

                              if ((Frames[i].image_data[temp_index] == 228) &&
                                 (Frames[i].image_data[temp_index + 1] == 108) &&
                                 (Frames[i].image_data[temp_index + 2] == 228))
                              {
                                 Frames[i].image_data[temp_index] = (byte)(addPal.data[pal_index + 0] * 4);
                                 Frames[i].image_data[temp_index + 1] = (byte)(addPal.data[pal_index + 1] * 4);
                                 Frames[i].image_data[temp_index + 2] = (byte)(addPal.data[pal_index + 2] * 4);
                                 Frames[i].image_data[temp_index + 3] = 255;
                              }
                           }
                        }
                     }
                  }
               }
            }
         }
      }
   }
}
