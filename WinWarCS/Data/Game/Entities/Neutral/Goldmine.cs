using System;
using WinWarCS.Data.Resources;

namespace WinWarCS.Data.Game
{
   internal class Goldmine : Building
   {
      public Goldmine ()
      {
         sprite = new Sprite (WarFile.GetSpriteResource (KnowledgeBase.IndexByName ("Goldmine")));
      }
   }
}

