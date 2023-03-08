using System;

namespace WinWarGame.Data.Game
{
   internal class HumanPeasant : Unit
   {
      public HumanPeasant (Map currentMap)
         : base(currentMap)
      {
         sprite = new UnitSprite (WarFile.GetSpriteResource (WarFile.KnowledgeBase.IndexByName ("Human Peasant")));
      }

      public override bool CanHarvest
      {
         get
         {
            return !IsDead;
         }
      }

      public override bool CanBuild
      {
         get
         {
            return !IsDead;
         }
      }

      public override bool CanRepair
      {
         get
         {
            return !IsDead;
         }
      }
   }
}

