using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinWarRT.Data.Resources;

namespace WinWarRT.Data.Game
{
   class UnitSprite : Sprite
   {
      public UnitSprite(SpriteResource resource)
         : base(resource)
      {
         AddAnimation("Walk", 0.5, 15, 30, 46, 47, 48, 49, 50);

         AddAnimation("Attack", 1.0, 5, 5);

         SetCurrentAnimation("Walk");
      }
   }
}
