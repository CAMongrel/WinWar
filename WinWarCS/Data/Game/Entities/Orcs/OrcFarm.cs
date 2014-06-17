using System;

namespace WinWarCS.Data.Game
{
   internal class OrcFarm : Building
   {
      public OrcFarm (Map currentMap)
         : base(currentMap)
      {
         sprite = new Sprite (WarFile.GetSpriteResource (KnowledgeBase.IndexByName ("Orc Farm")));
      }
   }
}

