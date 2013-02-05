using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using MonoGame.Framework;


namespace WinWarRT
{
    /// <summary>
    /// The root page used to display the game.
    /// </summary>
    public sealed partial class GamePage : SwapChainBackgroundPanel
    {
        readonly MainGame _game;

        public GamePage(string launchArguments)
        {
            this.InitializeComponent();

            // Create the game.
            _game = XamlGame<MainGame>.Create(launchArguments, Window.Current.CoreWindow, this);
        }

        private void Grid_PointerPressed_1(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            Windows.UI.Input.PointerPoint pp = e.GetCurrentPoint(null);

            _game.PointerPressed(new Microsoft.Xna.Framework.Vector2((float)pp.Position.X, (float)pp.Position.Y));
        }

        private void Grid_PointerReleased_1(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            Windows.UI.Input.PointerPoint pp = e.GetCurrentPoint(null);

            _game.PointerReleased(new Microsoft.Xna.Framework.Vector2((float)pp.Position.X, (float)pp.Position.Y));
        }

        private void SwapChainBackgroundPanel_PointerMoved_1(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            Windows.UI.Input.PointerPoint pp = e.GetCurrentPoint(null);

            _game.PointerMoved(new Microsoft.Xna.Framework.Vector2((float)pp.Position.X, (float)pp.Position.Y));
        }
    }
}
