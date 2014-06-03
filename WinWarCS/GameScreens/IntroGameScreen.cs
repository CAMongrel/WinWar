using FLCLib;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinWarRT.Data;
using WinWarRT.Gui.Rendering;
using System.IO;

namespace WinWarRT.GameScreens
{
   internal delegate void IntroFinished(bool wasCancelled);

   class IntroGameScreen : BaseGameScreen
   {
      private Texture2D curTexture;
      private FLCPlayer player;

      private IntroFinished introFinished;

      private IntroStoryboard storyboard;

      public IntroGameScreen(IntroFinished setIntroFinished)
      {
         curTexture = null;
         introFinished = setIntroFinished;

         storyboard = new IntroStoryboard();
         storyboard.OnStageSwitched += storyboard_OnStageSwitched;
         storyboard.OnChangeMovieStatus += storyboard_OnChangeMovieStatus;

         player = new FLCPlayer(MainGame.Device);
         player.OnFrameUpdated += player_OnFrameUpdated;
         player.OnPlaybackFinished += player_OnPlaybackFinished;
      }

      void storyboard_OnChangeMovieStatus(bool shouldPlay)
      {
         if (shouldPlay)
         {
            if (player.IsPlaying && player.IsPaused)
               player.Play();
         }
         else
         {
            if (player.IsPlaying)
               player.Stop();
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
               await player.Open(await GetMovieFile("HINTRO1.WAR"));
               player.PauseAfterFirstFrame = true;
               player.ShouldLoop = false;
               break;

            case IntroStage.CastleLoop:
               await player.Open(await GetMovieFile("HINTRO2.WAR"));
               player.PauseAfterFirstFrame = false;
               player.ShouldLoop = true;
               break;

            case IntroStage.Swamp:
               await player.Open(await GetMovieFile("OINTRO1.WAR"));
               player.PauseAfterFirstFrame = true;
               player.ShouldLoop = false;
               break;

            case IntroStage.SwampLoop:
               await player.Open(await GetMovieFile("OINTRO2.WAR"));
               player.PauseAfterFirstFrame = false;
               player.ShouldLoop = true;
               break;

            case IntroStage.SwampFortressEnter:
               await player.Open(await GetMovieFile("OINTRO3.WAR"));
               player.PauseAfterFirstFrame = false;
               player.ShouldLoop = false;
               break;

            case IntroStage.CaveEnter:
               await player.Open(await GetMovieFile("CAVE1.WAR"));
               player.PauseAfterFirstFrame = false;
               player.ShouldLoop = false;
               break;

            case IntroStage.CaveLoop:
               await player.Open(await GetMovieFile("CAVE2.WAR"));
               player.PauseAfterFirstFrame = false;
               player.ShouldLoop = true;
               break;

            case IntroStage.CaveExit:
               await player.Open(await GetMovieFile("CAVE3.WAR"));
               player.PauseAfterFirstFrame = false;
               player.ShouldLoop = false;
               break;

            case IntroStage.BlizzardLogo:
               await player.Open(await GetMovieFile("TITLE.WAR"));
               player.PauseAfterFirstFrame = false;
               player.ShouldLoop = false;
               break;
         }

         player.Play();
      }

      internal FileStream GetMovieFile(string filename)
      {
         //var localStorage = global::Windows.ApplicationModel.Package.Current.InstalledLocation;
         //localStorage = await localStorage.GetFolderAsync("Assets\\Data");
         return null;//await localStorage.GetFileAsync(filename);
      }

      void player_OnPlaybackFinished(FLCFile file, bool didFinishNormally)
      {
         /*if (didFinishNormally)
         {
            storyboard.NotifyMovieDidFinish();
         }
         else*/
         {
            if (introFinished != null)
               introFinished(true);
            else
               MainGame.WinWarGame.SetNextGameScreen(new MenuGameScreen(false));
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

         if (curTexture != null)
         {
            float scale = MainGame.AppWidth / curTexture.Width;

            Rectangle rect = new Rectangle(0, 0, (int)(curTexture.Width * scale), (int)(curTexture.Height * scale));
            rect.Y = MainGame.AppHeight / 2 - rect.Height / 2;

            MainGame.SpriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointWrap, DepthStencilState.Default, RasterizerState.CullNone);
            MainGame.SpriteBatch.Draw(curTexture, rect, Color.FromNonPremultiplied(new Vector4(Vector3.One, storyboard.CurrentAlpha)));
            MainGame.SpriteBatch.End();

            string introText = storyboard.GetCurrentIntroText();
            FontRenderer.DrawStringDirect(MainGame.DefaultFont, introText, 0, 0, Color.White);
         }
      }

      internal override void PointerUp(Microsoft.Xna.Framework.Vector2 position)
      {
         if (player != null)
            player.Stop();
      }
   }
}
