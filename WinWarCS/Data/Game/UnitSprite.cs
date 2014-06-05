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
         ApplyWarriorAnimationSet ();

         SetCurrentAnimationByName("Idle");
      }

      public void ApplyWarriorAnimationSet()
      {
         // Applicable for: Orc Grunt, Human Warrior, Orc Peon, Human Peasant, Medivh, Lothar

         AddAnimation ("Idle", 0.5, SpriteAnimationParams.RandomDuration | SpriteAnimationParams.FiveFrameDirection | SpriteAnimationParams.Loop, 0);

         AddAnimation ("Death1", 0.5, SpriteAnimationParams.None, 10, 25, 40);
         AddAnimation ("Death2", 0.5, SpriteAnimationParams.None, 12, 27, 42);

         AddAnimation ("Walk", 0.5, SpriteAnimationParams.FiveFrameDirection | SpriteAnimationParams.Loop, 15, 30, 15, 0, 55, 45, 55, 0);

         AddAnimation ("Attack", 0.3, SpriteAnimationParams.FiveFrameDirection, 5, 20, 35, 50, 60, 50, 35, 20, 5);
      }
   }
}
