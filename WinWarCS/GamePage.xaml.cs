using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using MonoGame.Framework;
using Windows.UI.ApplicationSettings;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.Foundation;
using Windows.UI.Xaml.Media;
using Windows.UI;


namespace WinWarRT
{
    /// <summary>
    /// The root page used to display the game.
    /// </summary>
    internal sealed partial class GamePage : SwapChainBackgroundPanel
    {
        readonly MainGame _game;

        internal GamePage(string launchArguments)
        {
            this.InitializeComponent();

            SettingsPane.GetForCurrentView().CommandsRequested += GamePage_CommandsRequested;

            // Create the game.
            _game = XamlGame<MainGame>.Create(launchArguments, Window.Current.CoreWindow, this);
        }

        void GamePage_CommandsRequested(SettingsPane sender, SettingsPaneCommandsRequestedEventArgs args)
        {
            args.Request.ApplicationCommands.Add(new SettingsCommand("Game Settings", "Game Settings", 
                delegate
                {
                    SettingsFlyout settings = new SettingsFlyout();
                    settings.Width = 100;//SettingsFlyout..SettingsFlyoutWidth.Narrow;
                    //settings.HeaderText = "Game Settings";
                    settings.HeaderBackground = new SolidColorBrush(Color.FromArgb(0xFF, 0x7F, 0x00, 0x00));
                    settings.Content = new GameSettingsFlyout();
                    //settings.IsOpen = true;
                }));
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
