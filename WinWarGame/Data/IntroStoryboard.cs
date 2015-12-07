using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinWarCS.Data
{
   enum IntroStage
   {
      None,
      Castle,
      CastleLoop,
      Swamp,
      SwampLoop,
      SwampFortressEnter,
      CaveEnter,
      CaveLoop,
      CaveExit,
      BlizzardLogo
   }
   internal delegate void StageSwitched (IntroStage newStage);
   internal delegate void ChangeMovieStatus (bool shouldPlay);
   class IntroStoryboard
   {
      internal IntroStage Stage { get; private set; }

      internal event StageSwitched OnStageSwitched;
      internal event ChangeMovieStatus OnChangeMovieStatus;

      private double elapsedTimeInStage;
      internal float CurrentAlpha;

      internal IntroStoryboard ()
      {
         CurrentAlpha = 1.0f;
         Stage = IntroStage.None;
      }

      internal void Update (GameTime gameTime)
      {
         elapsedTimeInStage += gameTime.ElapsedGameTime.TotalSeconds;

         switch (Stage)
         {
         case IntroStage.None:
            EnterStage (IntroStage.Castle);
            break;

         case IntroStage.Castle:
            CurrentAlpha = (float)(elapsedTimeInStage / 2.5);
            if (CurrentAlpha > 1.0f)
               CurrentAlpha = 1.0f;

            if (elapsedTimeInStage >= 2.5)
            {
               if (OnChangeMovieStatus != null)
                  OnChangeMovieStatus (true);
            }
            break;

         case IntroStage.CastleLoop:
            if (elapsedTimeInStage >= 31.0)
            {
               CurrentAlpha = 1.0f - (float)((elapsedTimeInStage - 31.0) / 2.5);
               if (CurrentAlpha > 1.0f)
                  CurrentAlpha = 1.0f;
               if (CurrentAlpha < 0.0f)
                  CurrentAlpha = 0.0f;

               if (elapsedTimeInStage >= 33.5)
                  EnterStage (IntroStage.Swamp);
            }
            break;

         case IntroStage.Swamp:
            CurrentAlpha = (float)(elapsedTimeInStage / 2.5);
            if (CurrentAlpha > 1.0f)
               CurrentAlpha = 1.0f;

            if (elapsedTimeInStage >= 2.5)
            {
               if (OnChangeMovieStatus != null)
                  OnChangeMovieStatus (true);
            }
            break;

         case IntroStage.SwampLoop:
            if (elapsedTimeInStage >= 17.0)
               EnterStage (IntroStage.SwampFortressEnter);
            break;

         case IntroStage.CaveLoop:
            if (elapsedTimeInStage >= 25.0)
               EnterStage (IntroStage.CaveExit);
            break;
         }
      }
      // 4,41 In the Age of Chaos two factions battled for dominance.
      // 5,66 The kingdom of Azeroth was a prosperous one.\nThe humans who dwelled there turned the land\ninto a paradise.
      // 8,08 The Knights of Stormwind and the Clerics of Northshire Abbey\nroamed far and wide, serving the King's people with honor and justice.
      // 10,64 The well trained armies of the King maintained\na lasting peace for many generations.
      // 7,05 Then came the Orcish hordes...
      // 6.05 No one knew where these creatures came from,\nand none were prepared for the terror that they spawned.
      // 9,00 Their warriors wielded axe and spear with deadly\nproficiency, while others rode dark wolves\nas black as the moonless night.
      // 9,31 Unimagined were the destructive powers\nof their evil magiks, derived from\nthe fires of the underworld.
      // 7,26 With an ingenious arsenal of weaponry and powerful\nmagic, these two forces collide in a contest of cunning,\nintellect and brute strength
      // 3,53 with the victor claiming dominance over the whole of Azeroth.
      // 5,75 Welcome to the World of Warcraft.
      // 4,56
      // 5,26
      internal string GetCurrentIntroText ()
      {
         switch (Stage)
         {
         default:
         case IntroStage.None:
            return string.Empty;

         case IntroStage.Castle:
            {
               if (elapsedTimeInStage >= 2.5)
               {
                  return "\nIn the Age of Chaos two factions battled for dominance.";
               } else
                  return string.Empty;
            }               

         case IntroStage.CastleLoop:
            {
               if (elapsedTimeInStage >= 0.1 && elapsedTimeInStage < 8.0)
               {
                  return "The kingdom of Azeroth was a prosperous one.\nThe humans who dwelled there turned the land\ninto a paradise.";
               } else if (elapsedTimeInStage >= 8.1 && elapsedTimeInStage < 18.0)
               {
                  return "The Knights of Stormwind and the Clerics of Northshire\nAbbey roamed far and wide, serving the King's people\nwith honor and justice.";
               } else if (elapsedTimeInStage >= 18.1 && elapsedTimeInStage < 25.0)
               {
                  return "The well trained armies of the King maintained\na lasting peace for many generations.";
               } else if (elapsedTimeInStage >= 25.1 && elapsedTimeInStage < 31.0)
               {
                  return "\nThen came the Orcish hordes...";
               } else
                  return string.Empty;
            }

         case IntroStage.Swamp:
            {
               if (elapsedTimeInStage >= 2.5 && elapsedTimeInStage < 19.5)
               {
                  return "\nNo one knew where these creatures came from, and none\nwere prepared for the terror that they spawned.";
               } else 
                  return "";
            }

         case IntroStage.SwampLoop:
            {
               if (elapsedTimeInStage >= 0.0 && elapsedTimeInStage < 9.5)
               {
                  return "Their warriors wielded axe and spear with deadly\nproficiency, while others rode dark wolves as black\nas the moonless night.";
               } else if (elapsedTimeInStage >= 9.6 && elapsedTimeInStage < 17.0)
               {
                  return "Unimagined were the destructive powers\nof their evil magiks, derived from\nthe fires of the underworld.";
               } else
                  return string.Empty;
            }

         case IntroStage.SwampFortressEnter:
            return string.Empty;

         case IntroStage.CaveEnter:
            return "With an ingenious arsenal of weaponry and powerful\nmagic, these two forces collide in a contest of cunning,\nintellect and brute strength";

         case IntroStage.CaveLoop:
            {
               if (elapsedTimeInStage >= 0.0 && elapsedTimeInStage < 9.5)
               {
                  return "With an ingenious arsenal of weaponry and powerful\nmagic, these two forces collide in a contest of cunning,\nintellect and brute strength";
               } else if (elapsedTimeInStage >= 9.6 && elapsedTimeInStage < 17.0)
               {
                  return "with the victor claiming dominance over\nthe whole of Azeroth.";
               } else if (elapsedTimeInStage >= 17.1)
               {
                  return "\nWelcome to the World of Warcraft.";
               } else
                  return string.Empty;
            }

         case IntroStage.CaveExit:
            return string.Empty;

         case IntroStage.BlizzardLogo:
            return string.Empty;
         }
      }

      private void EnterStage (IntroStage setStage)
      {
         Stage = setStage;
         elapsedTimeInStage = 0;

         switch (setStage)
         {
         case IntroStage.Castle:
            CurrentAlpha = 0.0f;
            break;
         }

         if (OnStageSwitched != null)
            OnStageSwitched (setStage);
      }

      internal void NotifyMovieDidFinish ()
      {
         switch (Stage)
         {
         case IntroStage.Castle:
            EnterStage (IntroStage.CastleLoop);
            break;

         case IntroStage.Swamp:
            EnterStage (IntroStage.SwampLoop);
            break;

         case IntroStage.SwampFortressEnter:
            EnterStage (IntroStage.CaveEnter);
            break;

         case IntroStage.CaveEnter:
            EnterStage (IntroStage.CaveLoop);
            break;

         case IntroStage.CaveExit:
            EnterStage (IntroStage.BlizzardLogo);
            break;
         }
      }
   }
}
