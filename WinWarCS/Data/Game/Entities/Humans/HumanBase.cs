using System;

namespace WinWarCS.Data.Game
{
   internal class HumanBase : Building
   {
      public HumanBase (Map currentMap)
         : base(currentMap)
      {
         sprite = new Sprite (WarFile.GetSpriteResource (KnowledgeBase.IndexByName ("Human Base")));
      }
   }
}

