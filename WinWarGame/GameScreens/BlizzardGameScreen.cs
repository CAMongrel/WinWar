using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinWarGame.Graphics;
using WinWarGame.Gui;

namespace WinWarGame.GameScreens
{
   class BlizzardGameScreen : BaseGameScreen
   {
      private double elapsedTime;
      private UIImage img;

      internal BlizzardGameScreen()
      {
         elapsedTime = 0;
      }

      internal override Microsoft.Xna.Framework.Color BackgroundColor
      {
         get
         {
            return Microsoft.Xna.Framework.Color.Black;
         }
      }

      internal override void InitUI()
      {
         base.InitUI();

         MouseCursor.State = MouseCursorState.None;

         UIWindow wnd = new UIWindow();

         img = new UIImage(WWTexture.FromImageResource("Background 'Blizzard'"));
         wnd.AddComponent(img);

         elapsedTime = 0;

         MainGame.SoundManager.PlaySound(472);
      }

      internal override void Update(Microsoft.Xna.Framework.GameTime gameTime)
      {
         base.Update(gameTime);

         elapsedTime += gameTime.ElapsedGameTime.TotalSeconds;

         img.Alpha = 1.0f;
         if (elapsedTime <= 0.5)
         {
            float alpha = 1.0f - (float)((0.5 - elapsedTime) / 0.5);
            img.Alpha = alpha;
         }

         if (elapsedTime >= 2.0)
         {
            float alpha = (float)((2.5 - elapsedTime) / 0.5);
            img.Alpha = alpha;
         }

         if (elapsedTime > 2.5)
            MainGame.WinWarGame.SetNextGameScreen(new MenuGameScreen(true));
      }

      internal override void PointerUp(Microsoft.Xna.Framework.Vector2 position, PointerType pointerType)
      {
         base.PointerUp(position, pointerType);

         MainGame.WinWarGame.SetNextGameScreen(new MenuGameScreen(false));
      }

      internal override void Draw(Microsoft.Xna.Framework.GameTime gameTime)
      {
         base.Draw(gameTime);

         UIWindowManager.Render();
      }
   }
}
