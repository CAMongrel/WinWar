using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace WinWarCS.Graphics
{
   public static class RenderManager
   {
      public static bool IsBatching { get; private set; }

      static RenderManager ()
      {
         IsBatching = false;
      }

      internal static void StartBatch()
      {
         MainGame.SpriteBatch.Begin (SpriteSortMode.Texture, BlendState.NonPremultiplied, SamplerState.PointWrap, DepthStencilState.None, RasterizerState.CullNone);

         IsBatching = true;
      }

      internal static void EndBatch()
      {
         IsBatching = false;

         MainGame.SpriteBatch.End ();
      }
   }
}

