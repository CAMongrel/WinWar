using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Text;
using WinWarCS.GameScreens;
using Microsoft.Xna.Framework.Input;
using WinWarCS.Data;
using WinWarCS.Gui;
using System.IO;
using System.Threading.Tasks;
using WinWarCS.Util;
using MouseCursor = WinWarCS.Gui.MouseCursor;
using WinWarCS.Data.Game;

namespace WinWarCS
{
   /// <summary>
   /// This is the main type for your game
   /// </summary>
   public class MainGame : Game
   {
      public static int MajorVersion = 0;
      public static int MinorVersion = 2;
      public static int RevisionVersion = 3;

      public static string Version = MajorVersion + "." + MinorVersion + "." + RevisionVersion;

      #region Variables

      private GraphicsDeviceManager _graphics;
      private SpriteBatch _spriteBatch;
      private SpriteFont _spriteFont;
      private BaseGameScreen currentGameScreen;
      private BaseGameScreen nextGameScreen;

      private Color backgroundClearColor;

      internal static MainGame WinWarGame { get; private set; }

      #endregion

      #region Properties

      internal const int OriginalAppWidth = 320;
      internal const int OriginalAppHeight = 200;

      internal static int AppWidth {
         get {
            return WinWarGame.Window.ClientBounds.Width;
         }
      }

      internal static int AppHeight {
         get {
            return WinWarGame.Window.ClientBounds.Height;
         }
      }

      internal static int ScaledOffsetX {
         get {
            float fullWidth = (float)OriginalAppWidth * ScaleX;
            return (int)((AppWidth - (float)fullWidth) * 0.5f);
         }
      }

      internal static int ScaledOffsetY {
         get {
            float fullHeight = (float)OriginalAppHeight * ScaleY;
            return (int)((AppHeight - (float)fullHeight) * 0.5f);
         }
      }

      internal static float ScaleX {
         get {
            float aspect = ((float)AppWidth / (float)AppHeight) / 1.6f;        // Original WarCraft has an aspect ratio of 1.6
            if (aspect > 1.0f)
               return (float)(((float)AppWidth / (float)OriginalAppWidth) / aspect);
            return (float)AppWidth / (float)OriginalAppWidth;
         }
      }

      internal static float ScaleY {
         get {
            float aspect = ((float)AppWidth / (float)AppHeight) / 1.6f;        // Original WarCraft has an aspect ratio of 1.6
            if (aspect < 1.0f)
               return (float)(((float)AppHeight / (float)OriginalAppHeight) * aspect);
            return (float)AppHeight / (float)OriginalAppHeight;
         }
      }

      internal static GraphicsDevice Device {
         get {
            return WinWarGame.GraphicsDevice;
         }
      }

      internal static SpriteBatch SpriteBatch {
         get {
            return WinWarGame._spriteBatch;
         }
      }

      internal static SpriteFont DefaultFont {
         get {
            return WinWarGame._spriteFont;
         }
      }

      internal static IAssetProvider AssetProvider { get; private set; }
      #endregion

      public MainGame(IAssetProvider setAssetProvider)
      {
         AssetProvider = setAssetProvider;
         MainGame.WinWarGame = this;

         Log.Severity = LogSeverity.Fatal;
         Log.Type = LogType.Performance;

         this.IsMouseVisible = false;

         this.IsFixedTimeStep = false;

         backgroundClearColor = new Color (0x7F, 0x00, 0x00);

         currentGameScreen = null;
         nextGameScreen = null;

         _graphics = new GraphicsDeviceManager (this);
         _graphics.PreferredBackBufferWidth = 320 * 3;
         _graphics.PreferredBackBufferHeight = 200 * 3;
         _graphics.ApplyChanges ();

         Content.RootDirectory = "Assets";

#if IOS
         MouseCursor.IsVisible = false;
#endif
      }

      private async Task<bool> ValidateDataWar ()
      {
         Stream stream = null;
         try
         {
            stream = await MainGame.AssetProvider.OpenContentFile ("Assets" + MainGame.AssetProvider.DirectorySeparatorChar + "Data" + MainGame.AssetProvider.DirectorySeparatorChar + "DATA.WAR");
            return true;
         } catch (Exception ex)
         {
            return false;
         } finally
         {
            if (stream != null)
            {
               stream.Dispose ();
               stream = null;
            }
         }
      }

      /// <summary>
      /// Allows the game to perform any initialization it needs to before starting to run.
      /// This is where it can query for any required services and load any non-graphic
      /// related content.  Calling base.Initialize will enumerate through any components
      /// and initialize them as well.
      /// </summary>
      protected override async void Initialize ()
      {
         base.Initialize ();

         WinWarCS.Util.Log.Write (Util.LogType.Generic, Util.LogSeverity.Status, "WinWarCS -- Version: " + Version);

         bool result = await ValidateDataWar ();
         if (result == false)
         {
            await Platform.UI.ShowMessageDialog ("DATA.WAR not found at expected location '" + AssetProvider.ExpectedDataDirectory +
            "'. Please copy the DATA.WAR from the demo or the full version to that location.\r\nIf you have the full version, " +
            "please also copy all the other .WAR files from the data directory.");
            return;
         }

         Exception loadingException = null;

         try
         {
            Entity.LoadDefaultValues(AssetProvider);
            WarFile.LoadResources(AssetProvider);
            MapTileset.LoadAllTilesets();
         } catch (Exception ex)
         {
            loadingException = ex;
         }

         Audio.SoundManager mgr = new Audio.SoundManager();
         mgr.PlaySound(551);

         if (loadingException != null)
         {
            await Platform.UI.ShowMessageDialog ("An error occured during loading of DATA.WAR (" + loadingException + ").");
            return;
         }

         if (WarFile.IsDemo)
         {
            SetNextGameScreen (new MenuGameScreen (false));
         } else
         {
            // Play demo
            SetNextGameScreen (new IntroGameScreen (
               delegate(bool wasCancelled) {
                  SetNextGameScreen (new MenuGameScreen (!wasCancelled));
               }));
         }
      }

      /// <summary>
      /// LoadContent will be called once per game and is the place to load
      /// all of your content.
      /// </summary>
      protected override void LoadContent ()
      {
         // Create a new SpriteBatch, which can be used to draw textures.
         _spriteBatch = new SpriteBatch (GraphicsDevice);

         _spriteFont = this.Content.Load<SpriteFont> ("DefaultFont");
         // TODO: use this.Content to load your game content here
      }

      /// <summary>
      /// UnloadContent will be called once per game and is the place to unload
      /// all content.
      /// </summary>
      protected override void UnloadContent ()
      {
         // TODO: Unload any non ContentManager content here
      }

      internal void SetNextGameScreen (BaseGameScreen setNextGameScreen)
      {
         nextGameScreen = setNextGameScreen;
      }

      /// <summary>
      /// Allows the game to run logic such as updating the world,
      /// checking for collisions, gathering input, and playing audio.
      /// </summary>
      /// <param name="gameTime">Provides a snapshot of timing values.</param>
      protected override void Update (GameTime gameTime)
      {
         Performance.Push("Game loop");
         Platform.Input.UpdateInput (gameTime);

         if (nextGameScreen != null) 
         {
            if (currentGameScreen != null)
               currentGameScreen.Close ();

            currentGameScreen = nextGameScreen;
            if (currentGameScreen != null)
               currentGameScreen.InitUI ();

            nextGameScreen = null;
         }

         if (currentGameScreen != null) 
         {
            currentGameScreen.Update (gameTime);
         }

         base.Update (gameTime);
         Performance.Pop();
      }

      /// <summary>
      /// This is called when the game should draw itself.
      /// </summary>
      /// <param name="gameTime">Provides a snapshot of timing values.</param>
      protected override void Draw (GameTime gameTime)
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

         base.Draw (gameTime);

         MouseCursor.Render (gameTime);
         Performance.Pop();
      }

      internal void PointerPressed (Microsoft.Xna.Framework.Vector2 scaledPosition, PointerType pointerType)
      {
         if (currentGameScreen != null)
         {
            currentGameScreen.PointerDown (scaledPosition, pointerType);
         }
      }

      internal void PointerReleased (Microsoft.Xna.Framework.Vector2 scaledPosition, PointerType pointerType)
      {
         if (currentGameScreen != null)
         {
            currentGameScreen.PointerUp (scaledPosition, pointerType);
         }
      }

      internal void PointerMoved (Microsoft.Xna.Framework.Vector2 scaledPosition)
      {
         if (currentGameScreen != null)
         {
            currentGameScreen.PointerMoved (scaledPosition);
         }
      }
   }
}
