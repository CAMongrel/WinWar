using System;

namespace WinWarCS.Data.Game
{
   internal class OrcBase : Building
   {
      public OrcBase (Map currentMap)
         : base(currentMap)
      {
         sprite = new Sprite (WarFile.GetSpriteResource (KnowledgeBase.IndexByName ("Orc Base")));
      }
   }
}

