using System;

namespace WinWarGame.MacOS
{
   internal class BaseAbility
   {
      public int IconIndex { get; protected set; }

      internal BaseAbility()
      {
         IconIndex = 30;
      }
   }
}

