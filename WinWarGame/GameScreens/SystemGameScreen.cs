using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinWarGame.GameScreens;
using WinWarGame.Gui;

namespace WinWarGame.GameScreens
{
    class SystemGameScreen : BaseGameScreen
    {
        public bool IsActive { get; set; }

        public SystemGameScreen()
        {
            IsActive = false;
        }

        internal override void InitUI()
        {
            MouseCursor.State = MouseCursorState.Pointer;
        }

        internal override void Close()
        {
            UIWindowManager.Clear();
        }

        internal override void Draw(Microsoft.Xna.Framework.GameTime gameTime)
        {
            UIWindowManager.Render();
        }

        internal override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            base.Update(gameTime);
        }

        internal override void PointerDown(Microsoft.Xna.Framework.Vector2 position, PointerType pointerType)
        {
            UIWindowManager.PointerDown(position, pointerType);
        }

        internal override void PointerUp(Microsoft.Xna.Framework.Vector2 position, PointerType pointerType)
        {
            UIWindowManager.PointerUp(position, pointerType);
        }

        internal override void PointerMoved(Microsoft.Xna.Framework.Vector2 position)
        {
            UIWindowManager.PointerMoved(position);
        }
    }
}
