using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinWarGame.GameScreens.Windows;
using WinWarGame.Graphics;
using WinWarGame.Gui;
using WinWarGame.Gui.Rendering;

namespace WinWarGame.GameScreens
{
    class MenuGameScreen : BaseGameScreen
    {
        private double elapsedTime;
        internal static MenuGameScreen Menu { get; private set; }

        private MenuBackgroundWindow backgroundWindow;
        private MainMenuWindow mainMenuWindow;
        private UIWindow textWindow;

        private bool shouldFadeIn;

        internal MenuGameScreen(bool setShouldFadeIn)
        {
            Menu = this;
            elapsedTime = 0;

            shouldFadeIn = setShouldFadeIn;

            MainGame.SoundManager.StopAll();
        }

        internal void ResetFade()
        {
            elapsedTime = 0;
        }

        internal override void InitUI()
        {
            elapsedTime = 0;

            MouseCursor.State = MouseCursorState.Pointer;

            backgroundWindow = new MenuBackgroundWindow();
            mainMenuWindow = new MainMenuWindow();

            textWindow = new UIWindow();
            UIImage img = new UIImage(WWTexture.FromImageResource("Text 'WarCraft'"));
            textWindow.AddComponent(img);
            textWindow.Height = img.Height;
        }

        internal override Microsoft.Xna.Framework.Color BackgroundColor
        {
            get
            {
                return Microsoft.Xna.Framework.Color.Black;
            }
        }

        internal override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            elapsedTime += gameTime.ElapsedGameTime.TotalSeconds;

            if (shouldFadeIn && elapsedTime < 10.5)
            {
                float alpha = (float)(elapsedTime / 10.5);
                if (alpha > 1.0f)
                    alpha = 1.0f;

                textWindow.Alpha = 1.0f;
                backgroundWindow.Alpha = alpha;
                mainMenuWindow.Alpha = alpha;
            }
            else
            {
                backgroundWindow.Alpha = 1.0f;
                mainMenuWindow.Alpha = 1.0f;
                textWindow.Alpha = 0.0f;
            }
        }

        internal override void Draw(GameTime gameTime)
        {
            UIWindowManager.Render();

            FontRenderer.DrawStringDirect(MainGame.DefaultFont, "Version: " + MainGame.Version, 270, 194, Color.White, 0.5f);
        }

        internal override void PointerDown(Microsoft.Xna.Framework.Vector2 position, PointerType pointerType)
        {
            UIWindowManager.PointerDown(position, pointerType);
        }

        internal override void PointerUp(Microsoft.Xna.Framework.Vector2 position, PointerType pointerType)
        {
            UIWindowManager.PointerUp(position, pointerType);
        }

        internal override void PointerMoved(Vector2 position)
        {
            UIWindowManager.PointerMoved(position);
        }
    }
}
