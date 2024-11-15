﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Advanced;
using SixLabors.ImageSharp.PixelFormats;

namespace WinWarGame.Data.Resources
{
   public struct SpriteResourceFrame
   {
      internal byte disp_x;
      internal byte disp_y;
      internal byte width;
      internal byte height;
      internal int offset;

      internal byte[] image_data;
   }

   public class SpriteResource : BasicResource
   {
      internal int MaxWidth;
      internal int MaxHeight;
      internal int FrameCount;
      internal SpriteResourceFrame[] Frames;

      internal SpriteResource(WarResource data, WarResource palette, WarResource addPalette)
         : base(data)
      {
         Type = ContentFileType.FileSprite;

         Frames = null;

         CreateImageData(data, palette, addPalette);
      }

      internal void CreateImageData(WarResource imgResource, WarResource palette, WarResource addPalette)
      {
         int i, offset, x, y, c;

         WarResource addPal = addPalette;

         FrameCount = imgResource.data[0] + (imgResource.data[1] << 8);
         MaxWidth = imgResource.data[2];
         MaxHeight = imgResource.data[3];

         Frames = new SpriteResourceFrame[FrameCount];

         offset = 4;
         for (i = 0; i < FrameCount; i++)
         {
            Frames[i].disp_x = imgResource.data[offset];
            Frames[i].disp_y = imgResource.data[offset + 1];
            Frames[i].width = imgResource.data[offset + 2];
            Frames[i].height = imgResource.data[offset + 3];
            Frames[i].offset = imgResource.data[offset + 4] + (imgResource.data[offset + 5] << 8) + 
               (imgResource.data[offset + 6] << 16) + (imgResource.data[offset + 7] << 24);
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
                     {
                        Frames [i].image_data [temp_index + c] = imgResource.data [offset];
                     }

                     Frames[i].image_data[temp_index + 3] = 255;

                     offset++;
                  }
               }
            }
            else
            {
               // We have a palette ... use it!
               for (y = Frames[i].disp_y; y < (Frames[i].height + Frames[i].disp_y); y++)
               {
                  for (x = Frames[i].disp_x; x < (Frames[i].width + Frames[i].disp_x); x++)
                  {
                     temp_index = (x + (y * MaxWidth)) * 4;
                     int pal_index = imgResource.data[offset++] * 3;

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
                     {
                        Frames [i].image_data [temp_index + c] = (byte)(palette.data [pal_index + c] * 4);
                     }
                        
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

                     // Some manual fixes
                     // TODO: Figure out how this really works
                  
                     if ((Frames[i].image_data[temp_index + 0] == 0xCC) &&
                         (Frames[i].image_data[temp_index + 1] == 0) &&
                         (Frames[i].image_data[temp_index + 2] == 0xD4))
                     {
                        Frames[i].image_data[temp_index + 0] = 0x14;
                        Frames[i].image_data[temp_index + 1] = 0x30;
                        Frames[i].image_data[temp_index + 2] = 0x4d;
                     }
                  
                     if ((Frames[i].image_data[temp_index + 0] == 0xFC) &&
                         (Frames[i].image_data[temp_index + 1] == 0) &&
                         (Frames[i].image_data[temp_index + 2] == 0xFC))
                     {
                        Frames[i].image_data[temp_index + 0] = 0x28;
                        Frames[i].image_data[temp_index + 1] = 0x30;
                        Frames[i].image_data[temp_index + 2] = 0x30;
                     }
                  }
               }
            }
         }
      }
      
      internal override void WriteToFile(string filename)
      {
         if (FrameCount < 1)
         {
            return;
         }

         SpriteResourceFrame frame1 = Frames[0];
         Image<Rgba32> image = new Image<Rgba32>(MaxWidth, MaxHeight);
         for (int y = 0; y < frame1.height; y++)
         {
            var span = image.DangerousGetPixelRowMemory(y).Span;
            for (int x = 0; x < frame1.width; x++)
            {
               int idx = ((y + frame1.disp_y) * MaxWidth + (x + frame1.disp_x)) * 4;
               span[x].R = frame1.image_data[idx + 0];
               span[x].G = frame1.image_data[idx + 1];
               span[x].B = frame1.image_data[idx + 2];
               span[x].A = frame1.image_data[idx + 3];
            }
         }
         image.Save(filename);
      }
   }
}
