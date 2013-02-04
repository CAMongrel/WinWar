using FLCLib.Metro;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace WinWarRT.GameScreens
{
    public delegate void MovieFinished();

    class MovieGameScreen : BaseGameScreen
    {
        private Texture2D curTexture;

        private MovieFinished OnMovieFinished;

        public async static void PlayMovie(string moviename, MovieFinished OnMovieFinished)
        {
            var localStorage = global::Windows.ApplicationModel.Package.Current.InstalledLocation;
            localStorage = await localStorage.GetFolderAsync("Assets\\Data");
            var resultFile = await localStorage.GetFileAsync(moviename);

            PlayMovie(resultFile, OnMovieFinished);
        }

        public static void PlayMovie(StorageFile file, MovieFinished OnMovieFinished)
        {
            MovieGameScreen movieGS = new MovieGameScreen();
            movieGS.OnMovieFinished = OnMovieFinished;
            movieGS.PlayMovie(file);

            MainGame.WinWarGame.SetNextGameScreen(movieGS);
        }

        public override Color BackgroundColor
        {
            get
            {
                return Color.Black;
            }
        }

        private async void PlayMovie(StorageFile file)
        {
            curTexture = null;

            FLCPlayer player = new FLCPlayer(MainGame.Device);
            player.OnFrameUpdated += player_OnFrameUpdated;
            player.OnPlaybackFinished += player_OnPlaybackFinished;

            await player.Open(file);
            player.ShouldLoop = false;
            player.Play();
        }

        void player_OnPlaybackFinished(FLCLib.FLCFile file)
        {
            if (OnMovieFinished != null)
            {
                OnMovieFinished();
            }
            else
            {
                // Don't know what to do, so just return to the main menu
                MainGame.WinWarGame.SetNextGameScreen(new MenuGameScreen());
            }
        }

        void player_OnFrameUpdated(Texture2D texture, FLCLib.FLCFile file)
        {
            curTexture = texture;
        }

        public override void InitUI()
        {
            //
        }

        public override void Close()
        {
            //
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            //
        }

        public override void Draw(Microsoft.Xna.Framework.GameTime gameTime)
        {
            if (curTexture != null)
            {
                float scale = MainGame.AppWidth / curTexture.Width;

                Rectangle rect = new Rectangle(0, 0, (int)(curTexture.Width * scale), (int)(curTexture.Height * scale));
                rect.Y = MainGame.AppHeight / 2 - rect.Height / 2;

                MainGame.SpriteBatch.Begin();
                MainGame.SpriteBatch.Draw(curTexture, rect, Color.White);
                MainGame.SpriteBatch.End();
            }
        }

        public override void PointerDown(Microsoft.Xna.Framework.Vector2 position)
        {
            //
        }

        public override void PointerUp(Microsoft.Xna.Framework.Vector2 position)
        {
            //
        }
    }
}
