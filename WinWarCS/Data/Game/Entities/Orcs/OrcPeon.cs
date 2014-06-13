using System;

namespace WinWarCS.Data.Game
{
   internal class OrcPeon : Unit
   {
      public OrcPeon ()
      {
         sprite = new UnitSprite (WarFile.GetSpriteResource (KnowledgeBase.IndexByName ("Orc Peon")));
      }
   }
}

