using System;

namespace WinWarCS.Data.Game
{
   internal class OrcBase : Building
   {
      public OrcBase ()
      {
         sprite = new Sprite (WarFile.GetSpriteResource (KnowledgeBase.IndexByName ("Orc Base")));
      }
   }
}

