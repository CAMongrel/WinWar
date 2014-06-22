using FLCLib;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using WinWarCS.Gui;

namespace WinWarCS.GameScreens
{
   internal delegate void MovieFinished(bool wasCancelled);

   class MovieGameScreen : BaseGameScreen
   {
      private Texture2D curTexture;
      private FLCPlayer player;

      private MovieFinished OnMovieFinished;

      internal async static void PlayMovie(string moviename, MovieFinished OnMovieFinished)
      {
         Stream resultFile = WinWarCS.Platform.IO.GetFileStream (Path.Combine ("Assets" + Path.DirectorySeparatorChar + "Data", moviename));
         if (resultFile == null)
            // TODO: Log error
            return;

         PlayMovie(resultFile, OnMovieFinished);
      }

      internal static void PlayMovie(Stream file, MovieFinished OnMovieFinished)
      {
         MovieGameScreen movieGS = new MovieGameScreen();
         movieGS.OnMovieFinished = OnMovieFinished;
         movieGS.PlayMovie(file);

         MainGame.WinWarGame.SetNextGameScreen(movieGS);
      }

      internal override Color BackgroundColor
      {
         get
         {
            return Color.Black;
         }
      }

      private async void PlayMovie(Stream file)
      {
         curTexture = null;

         player = new FLCPlayer(MainGame.Device);
         player.OnFrameUpdated += player_OnFrameUpdated;
         player.OnPlaybackFinished += player_OnPlaybackFinished;

         player.Open(file);
         player.ShouldLoop = false;
         player.Play();
      }

      void player_OnPlaybackFinished(FLCFile file, bool didFinishNormally)
      {
         if (OnMovieFinished != null)
         {
            OnMovieFinished(!didFinishNormally);
         }
         else
         {
            // Don't know what to do, so just return to the main menu
            MainGame.WinWarGame.SetNextGameScreen(new MenuGameScreen(false));
         }
      }

      void player_OnFrameUpdated(Texture2D texture, FLCFile file)
      {
         curTexture = texture;
      }

      internal override void InitUI()
      {
         MouseCursor.State = MouseCursorState.None;
      }

      internal override void Close()
      {
         //
      }

      internal override void Draw(Microsoft.Xna.Framework.GameTime gameTime)
      {
         if (curTexture != null)
         {
            float unscaledOffset = (MainGame.OriginalAppHeight - curTexture.Height) / 2;

            Rectangle rect = new Rectangle(MainGame.ScaledOffsetX, MainGame.ScaledOffsetY + (int)(unscaledOffset * MainGame.ScaleY), 
               (int)(curTexture.Width * MainGame.ScaleX), (int)(curTexture.Height * MainGame.ScaleY));

            MainGame.SpriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointWrap, DepthStencilState.Default, RasterizerState.CullNone);
            MainGame.SpriteBatch.Draw(curTexture, rect, Color.White);
            MainGame.SpriteBatch.End();
         }
      }

      internal override void PointerDown(Microsoft.Xna.Framework.Vector2 position)
      {
         //
      }

      internal override void PointerUp(Microsoft.Xna.Framework.Vector2 position)
      {
         if (player != null && player.IsPlaying)
            player.Stop();
      }
   }
}
