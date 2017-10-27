using FLCLib;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinWarCS.Data;
using WinWarCS.Gui.Rendering;
using System.IO;
using WinWarCS.Gui;
using WinWarCS.Data.Storyboard;

namespace WinWarCS.GameScreens
{
   internal delegate void IntroFinished(bool wasCancelled);

   class IntroGameScreen : BaseGameScreen
   {
      private bool useVoiceOver;

      private Texture2D curTexture;
      private FLCPlayer player;

      private IntroFinished introFinished;

      private BaseStoryboard storyboard;

      public IntroGameScreen(IntroFinished setIntroFinished, bool setUseVoiceOver = true)
      {
         useVoiceOver = setUseVoiceOver;
         curTexture = null;
         introFinished = setIntroFinished;

         MouseCursor.State = MouseCursorState.None;

         if (useVoiceOver)
         {
            storyboard = new IntroStoryboardWithVO();

            MainGame.SoundManager.LoadSound(546);
            MainGame.SoundManager.LoadSound(547);
            MainGame.SoundManager.LoadSound(548);
            MainGame.SoundManager.LoadSound(549);
            MainGame.SoundManager.LoadSound(550);
         }
         else
         {
            storyboard = new IntroStoryboardNoVO();
         }
         storyboard.OnStageSwitched += storyboard_OnStageSwitched;
         storyboard.OnChangeMovieStatus += storyboard_OnChangeMovieStatus;
         storyboard.OnAudioStageSwitched += Storyboard_OnAudioStageSwitched;

         player = new FLCPlayer(MainGame.Device);
         player.OnFrameUpdated += player_OnFrameUpdated;
         player.OnPlaybackFinished += player_OnPlaybackFinished;
      }

      private void Storyboard_OnAudioStageSwitched(AudioStage newStage)
      {
         switch (newStage)
         {
            case AudioStage.None:
               break;
            case AudioStage.InTheAgeOfChaos:
               MainGame.SoundManager.PlaySound(546);
               break;
            case AudioStage.TheKingdomOfAzeroth:
               MainGame.SoundManager.PlaySound(547);
               break;
            case AudioStage.NoOneKnewWhere:
               MainGame.SoundManager.PlaySound(548);
               break;
            case AudioStage.OpenGate:
               MainGame.SoundManager.PlaySound(473);
               break;
            case AudioStage.WithAnIngenious:
               MainGame.SoundManager.PlaySound(549, true);
               break;
            case AudioStage.WelcomeToTheWorld:
               MainGame.SoundManager.PlaySound(550);
               break;
         }
      }

      void storyboard_OnChangeMovieStatus(bool shouldPlay)
      {
         if (shouldPlay)
         {
            if (player.IsPlaying && player.IsPaused)
            {
               player.Play();
            }
         }
         else
         {
            if (player.IsPlaying)
            {
               player.Stop();
            }
         }
      }

      internal override Color BackgroundColor
      {
         get
         {
            return Color.Black;
         }
      }

      async void storyboard_OnStageSwitched(IntroStage newStage)
      {
         switch (newStage)
         {
            case IntroStage.Castle:
               player.Open(await GetMovieFile("HINTRO1.WAR"));
               player.PauseAfterFirstFrame = true;
               player.ShouldLoop = false;
               break;

            case IntroStage.CastleLoop:
               player.Open(await GetMovieFile("HINTRO2.WAR"));
               player.PauseAfterFirstFrame = false;
               player.ShouldLoop = true;
               break;

            case IntroStage.Swamp:
               player.Open(await GetMovieFile("OINTRO1.WAR"));
               player.PauseAfterFirstFrame = true;
               player.ShouldLoop = false;
               break;

            case IntroStage.SwampLoop:
               player.Open(await GetMovieFile("OINTRO2.WAR"));
               player.PauseAfterFirstFrame = false;
               player.ShouldLoop = true;
               break;

            case IntroStage.SwampFortressEnter:
               player.Open(await GetMovieFile("OINTRO3.WAR"));
               player.PauseAfterFirstFrame = false;
               player.ShouldLoop = false;
               break;

            case IntroStage.CaveEnter:
               player.Open(await GetMovieFile("CAVE1.WAR"));
               player.PauseAfterFirstFrame = false;
               player.ShouldLoop = false;
               break;

            case IntroStage.CaveLoop:
               player.Open(await GetMovieFile("CAVE2.WAR"));
               player.PauseAfterFirstFrame = false;
               player.ShouldLoop = true;
               break;

            case IntroStage.CaveExit:
               player.Open(await GetMovieFile("CAVE3.WAR"));
               player.PauseAfterFirstFrame = false;
               player.ShouldLoop = false;
               break;

            case IntroStage.BlizzardLogo:
               player.Open(await GetMovieFile("TITLE.WAR"));
               player.PauseAfterFirstFrame = false;
               player.ShouldLoop = false;
               break;
         }

         player.Play();
      }

      internal async Task<Stream> GetMovieFile(string filename)
      {
          return await MainGame.AssetProvider.OpenContentFile(Path.Combine("Assets" + MainGame.AssetProvider.DirectorySeparatorChar + "Data", filename));
      }

      void player_OnPlaybackFinished(FLCFile file, bool didFinishNormally)
      {
         if (didFinishNormally)
         {
            storyboard.NotifyMovieDidFinish();
         }
         else
         {
            if (introFinished != null)
            {
               introFinished(true);
            }
            else
            {
               MainGame.WinWarGame.SetNextGameScreen(new MenuGameScreen(false));
            }
         }
      }

      void player_OnFrameUpdated(Texture2D texture, FLCLib.FLCFile file)
      {
         curTexture = texture;
      }

      internal override void Update(Microsoft.Xna.Framework.GameTime gameTime)
      {
         base.Update(gameTime);

         storyboard.Update(gameTime);
      }

      internal override void Draw(Microsoft.Xna.Framework.GameTime gameTime)
      {
         base.Draw(gameTime);

         MainGame.DefaultFont.Spacing = 0.8f;

         if (curTexture != null)
         {
            float unscaledOffset = (MainGame.OriginalAppHeight - curTexture.Height) / 2;

            int yPos = MainGame.ScaledOffsetY + (int)(unscaledOffset * MainGame.ScaleY); // Centered Position
            if (useVoiceOver == false)
            {
               yPos = 5;
            }

            Rectangle rect = new Rectangle(MainGame.ScaledOffsetX, yPos, 
               (int)(curTexture.Width * MainGame.ScaleX), (int)(curTexture.Height * MainGame.ScaleY));

            MainGame.SpriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointWrap, DepthStencilState.Default, RasterizerState.CullNone);
            MainGame.SpriteBatch.Draw(curTexture, rect, Color.FromNonPremultiplied(new Vector4(Vector3.One, storyboard.CurrentAlpha)));
            MainGame.SpriteBatch.End();

            if (useVoiceOver == false)
            {
               int yStart = 147;

               string introText = storyboard.GetCurrentIntroText();
               FontRenderer.DrawStringDirect(MainGame.DefaultFont, introText, 0, yStart, MainGame.OriginalAppWidth, MainGame.OriginalAppHeight - yStart, Color.White);
            }
         }
      }

      internal override void PointerUp(Microsoft.Xna.Framework.Vector2 position, PointerType pointerType)
      {
         if (player != null)
         {
            player.Stop();
         }
      }
   }
}
