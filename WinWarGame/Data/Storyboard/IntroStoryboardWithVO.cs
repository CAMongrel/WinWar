using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinWarCS.Data.Storyboard
{
   /// <summary>
   /// Storyboard for Intro movie with VoiceOver
   /// </summary>
   internal class IntroStoryboardWithVO : BaseStoryboard
   {
      internal override void Update(GameTime gameTime)
      {
         base.Update(gameTime);

         switch (Stage)
         {
            case IntroStage.None:
               EnterStage(IntroStage.SwampFortressEnter);
               ChangeAudioState(AudioStage.None);
               break;

            case IntroStage.Castle:
               CurrentAlpha = (float)(elapsedTimeInStage / 2.5);
               if (CurrentAlpha > 1.0f)
               {
                  CurrentAlpha = 1.0f;
               }

               if (elapsedTimeInStage >= 0.1)
               {
                  ChangeAudioState(AudioStage.InTheAgeOfChaos);
               }

               if (elapsedTimeInStage >= 5.1)
               {
                  ChangeAudioState(AudioStage.TheKingdomOfAzeroth);
                  InvokeChangeMovieStatus(true);                  
               }
               break;

            case IntroStage.CastleLoop:
               if (elapsedTimeInStage >= 13.0)
               {
                  CurrentAlpha = 1.0f - (float)((elapsedTimeInStage - 13.0) / 2.5);
                  if (CurrentAlpha > 1.0f)
                  {
                     CurrentAlpha = 1.0f;
                  }
                  if (CurrentAlpha < 0.0f)
                  {
                     CurrentAlpha = 0.0f;
                  }

                  if (elapsedTimeInStage >= 15.5)
                  {
                     EnterStage(IntroStage.Swamp);
                  }
               }
               break;

            case IntroStage.Swamp:
               CurrentAlpha = (float)(elapsedTimeInStage / 2.5);
               if (CurrentAlpha > 1.0f)
               {
                  CurrentAlpha = 1.0f;
               }

               if (elapsedTimeInStage >= 0.1)
               {
                  ChangeAudioState(AudioStage.NoOneKnewWhere);
                  InvokeChangeMovieStatus(true);
               }
               break;

            case IntroStage.SwampLoop:
               if (elapsedTimeInStage >= 7.0)
               {
                  EnterStage(IntroStage.SwampFortressEnter);
               }
               break;

            case IntroStage.SwampFortressEnter:
               if (elapsedTimeInStage >= 0.25)
               {
                  ChangeAudioState(AudioStage.OpenGate);
               }
               break;

            case IntroStage.CaveEnter:
               if (elapsedTimeInStage >= 0.25)
               {
                  ChangeAudioState(AudioStage.WithAnIngenious);
               }
               break;

            case IntroStage.CaveLoop:
               if (elapsedTimeInStage >= 3.0)
               {
                  EnterStage(IntroStage.CaveExit);
               }
               break;

            case IntroStage.CaveExit:
               if (elapsedTimeInStage >= 0.01)
               {
                  ChangeAudioState(AudioStage.WelcomeToTheWorld);
               }
               break;
         }
      }
   }
}
