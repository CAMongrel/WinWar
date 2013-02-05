using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Text;
using WinWarRT.GameScreens;

namespace WinWarRT
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class MainGame : Game
    {
        #region Variables
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private SpriteFont _spriteFont;
        private BaseGameScreen currentGameScreen;
        private BaseGameScreen nextGameScreen;

        private Color backgroundClearColor;

        public static MainGame WinWarGame { get; private set; }
        #endregion

        #region Properties
        public const int OriginalAppWidth = 320;
        public const int OriginalAppHeight = 200;

        public static int AppWidth
        {
            get
            {
                return WinWarGame.Window.ClientBounds.Width;
            }
        }
        public static int AppHeight
        {
            get
            {
                return WinWarGame.Window.ClientBounds.Height;
            }
        }

        public static float ScaleX
        {
            get
            {
                float aspect = ((float)AppWidth / (float)AppHeight) / 1.6f;        // Original WarCraft has an aspect ratio of 1.6
                if (aspect > 1.0f)
                    return (int)(((float)AppWidth / (float)OriginalAppWidth) / aspect);
                return (float)AppWidth / (float)OriginalAppWidth;
            }
        }
        public static float ScaleY
        {
            get
            {
                float aspect = ((float)AppWidth / (float)AppHeight) / 1.6f;        // Original WarCraft has an aspect ratio of 1.6
                if (aspect < 1.0f)
                    return (int)(((float)AppHeight / (float)OriginalAppHeight) * aspect);
                return (float)AppHeight / (float)OriginalAppHeight;
            }
        }

        public static GraphicsDevice Device
        {
            get
            {
                return WinWarGame.GraphicsDevice;
            }
        }

        public static SpriteBatch SpriteBatch
        {
            get
            {
                return WinWarGame._spriteBatch;
            }
        }

        public static SpriteFont SpriteFont
        {
            get
            {
                return WinWarGame._spriteFont;
            }
        }
        #endregion

        public MainGame()
        {
            MainGame.WinWarGame = this;

            backgroundClearColor = new Color(0x7F, 0x00, 0x00);

            currentGameScreen = null;
            nextGameScreen = null;

            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Assets";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override async void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();

            bool success = true;

            try
            {
                await WinWarRT.Data.WarFile.LoadResources();
                WinWarRT.Data.Game.MapTileset.LoadAllTilesets();
            }
            catch (Exception)
            {
                success = false;
            }

            if (!success)
            {
                Windows.UI.Popups.MessageDialog dlg = new Windows.UI.Popups.MessageDialog("DATA.WAR not found in local documents store.", "WinWarRT - WarCraft for Windows Modern UI");
                await dlg.ShowAsync();
                return;
            }

            MovieGameScreen.PlayMovie("TITLE.WAR", 
                delegate
                {
                    SetNextGameScreen(new MenuGameScreen());
                });
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            _spriteFont = this.Content.Load<SpriteFont>("DefaultFont");
            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        public void SetNextGameScreen(BaseGameScreen setNextGameScreen)
        {
            nextGameScreen = setNextGameScreen;
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (nextGameScreen != null)
            {
                if (currentGameScreen != null)
                    currentGameScreen.Close();

                currentGameScreen = nextGameScreen;
                if (currentGameScreen != null)
                    currentGameScreen.InitUI();

                nextGameScreen = null;
            }

            if (currentGameScreen != null)
            {
                currentGameScreen.Update(gameTime);
            }

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            if (currentGameScreen != null)
            {
                GraphicsDevice.Clear(currentGameScreen.BackgroundColor);

                currentGameScreen.Draw(gameTime);
            }
            else
            {
                GraphicsDevice.Clear(backgroundClearColor);
            }

            base.Draw(gameTime);
        }

        public void PointerPressed(Microsoft.Xna.Framework.Vector2 position)
        {
            if (currentGameScreen != null)
            {
                currentGameScreen.PointerDown(new Microsoft.Xna.Framework.Vector2(position.X / ScaleX, position.Y / ScaleY));
            }
        }

        public void PointerReleased(Microsoft.Xna.Framework.Vector2 position)
        {
            if (currentGameScreen != null)
            {
                currentGameScreen.PointerUp(new Microsoft.Xna.Framework.Vector2(position.X / ScaleX, position.Y / ScaleY));
            }
        }

        public void PointerMoved(Microsoft.Xna.Framework.Vector2 position)
        {
            if (currentGameScreen != null)
            {
                currentGameScreen.PointerMoved(new Microsoft.Xna.Framework.Vector2(position.X / ScaleX, position.Y / ScaleY));
            }
        }
    }
}
