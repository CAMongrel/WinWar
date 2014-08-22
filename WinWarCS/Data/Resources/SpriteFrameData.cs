using System;
using WinWarCS.Data.Game;
using Microsoft.Xna.Framework.Graphics;
using WinWarCS.Util;
using WinWarCS.Graphics;
using System.Collections.Generic;

namespace WinWarCS.Data.Resources
{
   internal class SpriteFrameData
   {
      private static Dictionary<SpriteResource, SpriteFrameData> spriteDict;

      internal SpriteFrame[] Frames;

      internal int MaxWidth { get; private set; }
      internal int MaxHeight { get; private set; }

      static SpriteFrameData()
      {
         spriteDict = new Dictionary<SpriteResource, SpriteFrameData>();
      }

      internal static SpriteFrameData LoadSpriteFrameData(SpriteResource resource)
      {
         if (spriteDict.ContainsKey(resource))
            return spriteDict[resource];

         SpriteFrameData spr = new SpriteFrameData(resource);
         spriteDict.Add(resource, spr);
         return spr;
      }

      private SpriteFrameData(SpriteResource resource)
      {
         MaxWidth = resource.MaxWidth;
         MaxHeight = resource.MaxHeight;

         Frames = new SpriteFrame[resource.FrameCount];

         for (int i = 0; i < resource.FrameCount; i++)
         {
            Texture2D DXTexture = new Texture2D(MainGame.Device, resource.MaxWidth, resource.MaxHeight, false, SurfaceFormat.Color);
            Performance.Push("SetData");
            DXTexture.SetData<byte>(resource.Frames[i].image_data);
            Performance.Pop();

            Frames[i] = new SpriteFrame ();
            Frames[i].OffsetX = resource.Frames [i].disp_x;
            Frames[i].OffsetY = resource.Frames [i].disp_y;
            Frames[i].Width = resource.Frames [i].width;
            Frames[i].Height = resource.Frames [i].height;
            Performance.Push("FromDXTexture");
            Frames[i].texture = WWTexture.FromDXTexture(DXTexture);
            Performance.Pop();
         }
      }
   }
}

