using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinWarCS.Data.Resources;

namespace WinWarCS.Data.Game
{
   class UnitSprite : Sprite
   {
      public UnitSprite(SpriteResource resource)
         : base(resource)
      {
         AddAnimation("WalkAll", 0.5, 15, 30, 46, 47, 48, 49, 50);

         int[] frames = new int[80];
         for (int i = 0; i < frames.Length; i++)
            frames[i] = i;

         AddAnimation("Walk", 0.5, frames);

         AddAnimation("Attack", 1.0, 5, 5);

         SetCurrentAnimation("Walk");
      }
   }
}
