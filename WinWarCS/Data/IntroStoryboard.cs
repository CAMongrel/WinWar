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

   internal delegate void StageSwitched(IntroStage newStage);
   internal delegate void ChangeMovieStatus(bool shouldPlay);

   class IntroStoryboard
   {
      internal IntroStage Stage { get; private set; }

      internal event StageSwitched OnStageSwitched;
      internal event ChangeMovieStatus OnChangeMovieStatus;

      private double elapsedTimeInStage;

      internal float CurrentAlpha;

      internal IntroStoryboard()
      {
         CurrentAlpha = 1.0f;
         Stage = IntroStage.None;
      }

      internal void Update(GameTime gameTime)
      {
         elapsedTimeInStage += gameTime.ElapsedGameTime.TotalSeconds;

         switch (Stage)
         {
            case IntroStage.None:
               EnterStage(IntroStage.Castle);
               break;

            case IntroStage.Castle:
               CurrentAlpha = (float)(elapsedTimeInStage / 2.5);
               if (CurrentAlpha > 1.0f)
                  CurrentAlpha = 1.0f;

               if (elapsedTimeInStage >= 2.5)
               {
                  if (OnChangeMovieStatus != null)
                     OnChangeMovieStatus(true);
               }
               break;

            case IntroStage.CastleLoop:
               if (elapsedTimeInStage >= 5.0)
               {
                  CurrentAlpha = 1.0f - (float)((elapsedTimeInStage - 5.0) / 2.5);
                  if (CurrentAlpha > 1.0f)
                     CurrentAlpha = 1.0f;
                  if (CurrentAlpha < 0.0f)
                     CurrentAlpha = 0.0f;

                  if (elapsedTimeInStage >= 7.5)
                     EnterStage(IntroStage.Swamp);
               }
               break;

            case IntroStage.Swamp:
               CurrentAlpha = (float)(elapsedTimeInStage / 2.5);
               if (CurrentAlpha > 1.0f)
                  CurrentAlpha = 1.0f;

               if (elapsedTimeInStage >= 2.5)
               {
                  if (OnChangeMovieStatus != null)
                     OnChangeMovieStatus(true);
               }
               break;

            case IntroStage.SwampLoop:
               if (elapsedTimeInStage >= 5.0)
                  EnterStage(IntroStage.SwampFortressEnter);
               break;

            case IntroStage.CaveLoop:
               if (elapsedTimeInStage >= 5.0)
                  EnterStage(IntroStage.CaveExit);
               break;
         }
      }

      internal string GetCurrentIntroText()
      {
         switch (Stage)
         {
            default:
            case IntroStage.None:
               return string.Empty;

            case IntroStage.Castle:
               return "IntroStage.Castle";

            case IntroStage.CastleLoop:
               return "IntroStage.CastleLoop";

            case IntroStage.Swamp:
               return "IntroStage.Swamp";

            case IntroStage.SwampLoop:
               return "IntroStage.SwampLoop";

            case IntroStage.CaveLoop:
               return "IntroStage.CaveLoop";

            case IntroStage.SwampFortressEnter:
               return "IntroStage.SwampFortressEnter";

            case IntroStage.CaveEnter:
               return "IntroStage.CaveEnter";

            case IntroStage.CaveExit:
               return "IntroStage.CaveExit";

            case IntroStage.BlizzardLogo:
               return "IntroStage.BlizzardLogo";
         }
      }

      private void EnterStage(IntroStage setStage)
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
            OnStageSwitched(setStage);
      }

      internal void NotifyMovieDidFinish()
      {
         switch (Stage)
         {
            case IntroStage.Castle:
               EnterStage(IntroStage.CastleLoop);
               break;

            case IntroStage.Swamp:
               EnterStage(IntroStage.SwampLoop);
               break;

            case IntroStage.SwampFortressEnter:
               EnterStage(IntroStage.CaveEnter);
               break;

            case IntroStage.CaveEnter:
               EnterStage(IntroStage.CaveLoop);
               break;

            case IntroStage.CaveExit:
               EnterStage(IntroStage.BlizzardLogo);
               break;
         }
      }
   }
}
