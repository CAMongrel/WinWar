using System;

namespace WinWarCS.Data.Game
{
   internal class OrcPeon : Unit
   {
      public OrcPeon (Map currentMap)
         : base(currentMap)
      {
         sprite = new UnitSprite (WarFile.GetSpriteResource (WarFile.KnowledgeBase.IndexByName ("Orc Peon")));
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

