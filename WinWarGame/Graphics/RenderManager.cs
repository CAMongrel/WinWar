using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace WinWarGame.Graphics
{
   public static class RenderManager
   {
      public static bool IsBatching { get; private set; }

      static RenderManager ()
      {
         IsBatching = false;
      }

      internal static void StartBatch(BlendState blendState = null, SpriteSortMode sortMode = SpriteSortMode.Texture)
      {
         if (blendState == null)
            blendState = BlendState.NonPremultiplied;

         MainGame.SpriteBatch.Begin (sortMode, blendState, SamplerState.PointWrap, DepthStencilState.None, RasterizerState.CullNone);

         IsBatching = true;
      }

      internal static void EndBatch()
      {
         IsBatching = false;

         MainGame.SpriteBatch.End ();
      }
   }
}

