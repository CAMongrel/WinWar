using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Input;
using System.IO;
using System.Threading.Tasks;
using MouseCursor = WinWarGame.Gui.MouseCursor;
using WinWarGame.Audio;
using WinWarGame.Data;
using WinWarGame.Data.Game;
using WinWarGame.GameScreens;
using WinWarGame.Graphics;
using WinWarGame.Gui;
using WinWarGame.Util;

#nullable enable

namespace WinWarGame
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class MainGame : Game
    {
        public static readonly int MajorVersion = 0;
        public static readonly int MinorVersion = 2;
        public static readonly int RevisionVersion = 6;

        public static readonly string Version = MajorVersion + "." + MinorVersion + "." + RevisionVersion;

        #region Variables

        private readonly SpriteBatch spriteBatch;
        private Font? spriteFont;
        private BaseGameScreen? currentGameScreen;
        private BaseGameScreen? nextGameScreen;

        internal SystemGameScreen? SystemGameScreen;

        private readonly MusicManager musicManager;
        private readonly SoundManager soundManager;

        private readonly Color backgroundClearColor;

        internal static MainGame WinWarGame { get; private set; }

        #endregion

        #region Properties

        internal const int OriginalAppWidth = 320;
        internal const int OriginalAppHeight = 200;

        internal static int AppWidth => WinWarGame.Window.ClientBounds.Width;
        internal static int AppHeight => WinWarGame.Window.ClientBounds.Height;

        internal static int ScaledOffsetX
        {
            get
            {
                float fullWidth = (float)OriginalAppWidth * ScaleX;
                return (int)((AppWidth - (float)fullWidth) * 0.5f);
            }
        }

        internal static int ScaledOffsetY
        {
            get
            {
                float fullHeight = (float)OriginalAppHeight * ScaleY;
                return (int)((AppHeight - (float)fullHeight) * 0.5f);
            }
        }

        internal static float ScaleX
        {
            get
            {
                float aspect = ((float)AppWidth / (float)AppHeight) / 1.6f;        // Original WarCraft has an aspect ratio of 1.6
                if (aspect > 1.0f)
                    return (float)(((float)AppWidth / (float)OriginalAppWidth) / aspect);
                return (float)AppWidth / (float)OriginalAppWidth;
            }
        }

        internal static float ScaleY
        {
            get
            {
                float aspect = ((float)AppWidth / (float)AppHeight) / 1.6f;        // Original WarCraft has an aspect ratio of 1.6
                if (aspect < 1.0f)
                    return (float)(((float)AppHeight / (float)OriginalAppHeight) * aspect);
                return (float)AppHeight / (float)OriginalAppHeight;
            }
        }

        internal static GraphicsDevice Device
        {
            get
            {
                return WinWarGame.GraphicsDevice;
            }
        }

        internal static SpriteBatch SpriteBatch
        {
            get
            {
                return WinWarGame.spriteBatch;
            }
        }

        internal static Font? DefaultFont
        {
            get
            {
                return WinWarGame.spriteFont;
            }
        }

        internal static MusicManager MusicManager
        {
            get
            {
                return WinWarGame.musicManager;
            }
        }

        internal static SoundManager SoundManager
        {
            get
            {
                return WinWarGame.soundManager;
            }
        }

        internal static IAssetProvider AssetProvider => WinWarGame.assetProvider!;
        private readonly IAssetProvider assetProvider;

        internal static Dictionary<string, string> StartParameters = new Dictionary<string, string>();
        #endregion

        public MainGame(IAssetProvider setAssetProvider, Dictionary<string, string> setStartParameters)
        {
            MainGame.WinWarGame = this;
            
            StartParameters = setStartParameters;
            assetProvider = setAssetProvider;

            Log.Severity = LogSeverity.Fatal;
            Log.Type = LogType.Performance;

            this.IsMouseVisible = false;

            this.IsFixedTimeStep = false;

            backgroundClearColor = new Color(0x7F, 0x00, 0x00);

            currentGameScreen = null;
            nextGameScreen = null;

            var graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth = 320 * 3;
            graphics.PreferredBackBufferHeight = 200 * 3;
            graphics.ApplyChanges();

            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            
            Content.RootDirectory = "Assets";

            soundManager = new SoundManager();
            musicManager = new MusicManager();
            
#if IOS
            MouseCursor.IsVisible = false;
#endif
        }

        private bool ValidateDataWar()
        {
            Stream? stream = null;
            try
            {
                stream = MainGame.AssetProvider?.OpenGameDataFile("DATA.WAR");
                return stream != null;
            }
            catch (Exception)
            {
                // If anything goes wrong, just return false
                return false;
            }
            finally
            {
                stream?.Dispose();
            }
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            base.Initialize();

            SystemGameScreen = new SystemGameScreen();

            Log.Write(Util.LogType.Generic, Util.LogSeverity.Status, "WinWarCS -- Version: " + Version);

            bool result = ValidateDataWar();
            if (result == false)
            {
                Platform.UI.ShowMessageDialog("DATA.WAR not found or not loadable at expected location '" +
                    AssetProvider?.FullDataDirectory + "' or '" + AssetProvider?.DemoDataDirectory +
                    "'. Please copy the DATA.WAR from the demo or the full version to that location.\r\nIf you have the full version, " +
                    "please also copy all the other .WAR files from the data directory.", "Exit", () =>
                    {
                        Environment.Exit(0);
                    });
                return;
            }

            try
            {
                Entity.LoadDefaultValues(AssetProvider);
                WarFile.LoadResources(AssetProvider);
                MapTileset.LoadAllTilesets();
            }
            catch (Exception ex)
            {
                Platform.UI.ShowMessageDialog("An error occured during loading of DATA.WAR (" + ex + ").");
                return;
            }

            if (WarFile.IsDemo)
            {
                SetNextGameScreen(new MenuGameScreen(false));
            }
            else
            {
                bool bPlayIntro = HasStartParameter("skipintro") == false;

                // Play intro
                if (bPlayIntro)
                {
                    SetNextGameScreen(new IntroGameScreen(
                        delegate (bool wasCancelled)
                        {
                            SetNextGameScreen(new MenuGameScreen(!wasCancelled));
                        }));
                }
                else
                {
                    SetNextGameScreen(new MenuGameScreen(false));
                }
            }
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            spriteFont = new Font(this.Content.Load<SpriteFont>("DefaultFont"));
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        internal void SetNextGameScreen(BaseGameScreen setNextGameScreen)
        {
            // Ensure that all audio is stopped when switching game screens
            soundManager?.StopAll();

            nextGameScreen = setNextGameScreen;
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            Performance.Push("Game loop");
            soundManager?.Update(gameTime);
            Platform.Input.UpdateInput(gameTime);

            if (nextGameScreen != null)
            {
                currentGameScreen?.Close();

                currentGameScreen = nextGameScreen;
                currentGameScreen?.InitUI();

                nextGameScreen = null;
            }

            currentGameScreen?.Update(gameTime);

            if (SystemGameScreen is { IsActive: true })
            {
                SystemGameScreen.Draw(gameTime);
            }

            base.Update(gameTime);
            Performance.Pop();
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            Performance.Push("Render loop");
            if (currentGameScreen != null)
            {
                GraphicsDevice.Clear(currentGameScreen.BackgroundColor);

                currentGameScreen.Draw(gameTime);
            }
            else
            {
                GraphicsDevice.Clear(backgroundClearColor);
            }

            if (SystemGameScreen is { IsActive: true })
            {
                SystemGameScreen.Draw(gameTime);
            }

            base.Draw(gameTime);

            Gui.MouseCursor.Render(gameTime);
            Performance.Pop();
        }

        internal void PointerPressed(Vector2 scaledPosition, PointerType pointerType)
        {
            if (SystemGameScreen is { IsActive: true })
            {
                SystemGameScreen.PointerDown(scaledPosition, pointerType);
                return;
            }

            currentGameScreen?.PointerDown(scaledPosition, pointerType);
        }

        internal void PointerReleased(Vector2 scaledPosition, PointerType pointerType)
        {
            if (SystemGameScreen is { IsActive: true })
            {
                SystemGameScreen.PointerUp(scaledPosition, pointerType);
                return;
            }

            currentGameScreen?.PointerUp(scaledPosition, pointerType);
        }

        internal void PointerMoved(Vector2 scaledPosition)
        {
            if (SystemGameScreen is { IsActive: true })
            {
                SystemGameScreen.PointerMoved(scaledPosition);
                return;
            }

            currentGameScreen?.PointerMoved(scaledPosition);
        }

        internal void SetSystemGameScreenActive(bool isActive)
        {
            if (SystemGameScreen != null)
            {
                SystemGameScreen.IsActive = isActive;
            }
            IsMouseVisible = isActive;
        }

        internal static bool HasStartParameter(string param)
        {
            return GetStartParameter(param) != null;
        }
        
        internal static string? GetStartParameter(string param)
        {
            param = param.ToUpperInvariant();
            foreach (var pair in StartParameters)
            {
                var key = pair.Key.ToUpperInvariant();
                key = key.TrimStart('-');
                if (key == param)
                {
                    return pair.Value;
                }
            }
            return null;
        }
    }
}
