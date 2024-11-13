using System;

namespace WinWarGame.Data.Game
{
   internal class OrcWarlock : Unit
   {
      public OrcWarlock (Map currentMap)
         : base(currentMap)
      {
         sprite = new UnitSprite (WarFile.GetSpriteResource (WarFile.KnowledgeBase.IndexByName ("Orc Wizard")));
      }
   }
}

