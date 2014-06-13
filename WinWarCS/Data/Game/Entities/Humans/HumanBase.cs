using System;

namespace WinWarCS.Data.Game
{
   internal class HumanBase : Building
   {
      public HumanBase ()
      {
         sprite = new Sprite (WarFile.GetSpriteResource (KnowledgeBase.IndexByName ("Human Base")));
      }
   }
}

