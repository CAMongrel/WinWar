using System;

namespace WinWarCS.Data.Game
{
   internal class OrcWizard : Unit
   {
      public OrcWizard (Map currentMap)
         : base(currentMap)
      {
         sprite = new UnitSprite (WarFile.GetSpriteResource (WarFile.KnowledgeBase.IndexByName ("Orc Wizard")));
      }
   }
}

