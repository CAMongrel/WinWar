using System;

namespace WinWarCS.Data.Game
{
   internal class OrcFarm : Building
   {
      public OrcFarm ()
      {
         sprite = new Sprite (WarFile.GetSpriteResource (KnowledgeBase.IndexByName ("Orc Farm")));
      }
   }
}

