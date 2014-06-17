using System;

namespace WinWarCS.Data.Game
{
   internal class HumanFarm : Building
   {
      public HumanFarm (Map currentMap)
         : base(currentMap)
      {
         sprite = new Sprite (WarFile.GetSpriteResource (KnowledgeBase.IndexByName ("Human Farm")));
      }
   }
}

