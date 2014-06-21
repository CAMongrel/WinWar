using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Text;
using WinWarCS.GameScreens;
using Microsoft.Xna.Framework.Input;
using WinWarCS.Data;

namespace WinWarCS
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

		#endregion

		public MainGame ()
		{
			MainGame.WinWarGame = this;

			backgroundClearColor = new Color (0x7F, 0x00, 0x00);

			currentGameScreen = null;
			nextGameScreen = null;

			_graphics = new GraphicsDeviceManager (this);
         _graphics.PreferredBackBufferWidth = 320 * 3;
         _graphics.PreferredBackBufferHeight = 200 * 3;
         _graphics.ApplyChanges ();

			Content.RootDirectory = "Assets";
		}

		/// <summary>
		/// Allows the game to perform any initialization it needs to before starting to run.
		/// This is where it can query for any required services and load any non-graphic
		/// related content.  Calling base.Initialize will enumerate through any components
		/// and initialize them as well.
		/// </summary>
		protected override void Initialize ()
		{
			base.Initialize ();

			/*Windows.Storage.StorageFile warFile = await WinWarCS.Data.WarFile.GetDataWarFile();
         if (warFile == null)
         {
            /*Windows.UI.Popups.MessageDialog dlg = new Windows.UI.Popups.MessageDialog("DATA.WAR not found in local documents store.", "WinWarCS - WarCraft for Windows Modern UI");
            await dlg.ShowAsync();
            return;
         }*/

			Exception loadingException = null;

			try {
				WinWarCS.Data.WarFile.LoadResources ();
				WinWarCS.Data.Game.MapTileset.LoadAllTilesets ();
			} catch (Exception ex) {
				loadingException = ex;
			}

			if (loadingException != null) {
				/*Windows.UI.Popups.MessageDialog dlg = new Windows.UI.Popups.MessageDialog("An error occured during loading of DATA.WAR (" + loadingException + ").", 
               "WinWarCS - WarCraft for Windows Modern UI");
            await dlg.ShowAsync();*/
				return;
			}

			SetNextGameScreen (new IntroGameScreen (
				delegate(bool wasCancelled) {
					SetNextGameScreen (new MenuGameScreen (!wasCancelled));
				}));

			//SetNextGameScreen(new MenuGameScreen());
			/*MovieGameScreen.PlayMovie("TITLE.WAR", 
             delegate
             {
                 SetNextGameScreen(new MenuGameScreen(true));
             });*/
		}

		/*private bool HasData(byte[] blob, byte[] dataToSearch)
      {
         int counter = 0;
         for (int i = 0; i < blob.Length; i++)
         {
            if (blob[i] == dataToSearch[counter])
               counter++;
            else
               counter = 0;

            if (counter >= dataToSearch.Length)
               return true;
         }

         return false;
      }

      private void SearchForText(string text)
      {
         byte[] byteData = Encoding.UTF8.GetBytes(text);

         for (int i = 0; i < 485; i++)
         {
            WinWarCS.Data.WarResource res = WinWarCS.Data.WarFile.GetResource(i);

            bool hasData = HasData(res.data, byteData);
            if (hasData)
            {
               int hehe = 42;
            }
         }
      }*/

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
			Platform.Input.UpdateInput (gameTime);

			if (nextGameScreen != null) {
				if (currentGameScreen != null)
					currentGameScreen.Close ();

				currentGameScreen = nextGameScreen;
				if (currentGameScreen != null)
					currentGameScreen.InitUI ();

				nextGameScreen = null;
			}

			if (currentGameScreen != null) {
				currentGameScreen.Update (gameTime);
			}

			base.Update (gameTime);
		}

		/// <summary>
		/// This is called when the game should draw itself.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		protected override void Draw (GameTime gameTime)
		{
			if (currentGameScreen != null) {
				GraphicsDevice.Clear (currentGameScreen.BackgroundColor);

				currentGameScreen.Draw (gameTime);
			} else {
				GraphicsDevice.Clear (backgroundClearColor);
			}

			base.Draw (gameTime);
		}

		internal void PointerPressed (Microsoft.Xna.Framework.Vector2 position)
		{
			if (currentGameScreen != null) {
				currentGameScreen.PointerDown (new Microsoft.Xna.Framework.Vector2 ((position.X - MainGame.ScaledOffsetX) / ScaleX, (position.Y - MainGame.ScaledOffsetY) / ScaleY));
			}
		}

		internal void PointerReleased (Microsoft.Xna.Framework.Vector2 position)
		{
			if (currentGameScreen != null) {
				currentGameScreen.PointerUp (new Microsoft.Xna.Framework.Vector2 ((position.X - MainGame.ScaledOffsetX) / ScaleX, (position.Y - MainGame.ScaledOffsetY) / ScaleY));
			}
		}

		internal void PointerMoved (Microsoft.Xna.Framework.Vector2 position)
		{
			if (currentGameScreen != null) {
				currentGameScreen.PointerMoved (new Microsoft.Xna.Framework.Vector2 ((position.X - MainGame.ScaledOffsetX) / ScaleX, (position.Y - MainGame.ScaledOffsetY) / ScaleY));
			}
		}
	}
}
