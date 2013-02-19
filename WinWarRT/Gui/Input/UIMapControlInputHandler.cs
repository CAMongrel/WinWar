using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinWarRT.Gui
{
    enum InputMode
    {
        Classic,
        EnhancedMouse,
        EnhancedTouch
    }

    internal delegate void MapDidScroll(float setMapOffsetX, float setMapOffsetY);

    class UIMapControlInputHandler
    {
        internal InputMode InputMode { get; private set; }
        internal UIMapControl MapControl { get; private set; }

        internal event MapDidScroll OnMapDidScroll;

        protected UIMapControlInputHandler(InputMode setInputMode, UIMapControl setUIMapControl)
        {
            MapControl = setUIMapControl;
            InputMode = setInputMode;
        }

        internal virtual void SetCameraOffset(float setCamOffsetX, float setCamOffsetY)
        {
            //
        }

        internal virtual bool PointerDown(Microsoft.Xna.Framework.Vector2 position)
        {
            return true;
        }

        internal virtual bool PointerUp(Microsoft.Xna.Framework.Vector2 position)
        {
            return true;
        }

        internal virtual bool PointerMoved(Microsoft.Xna.Framework.Vector2 position)
        {
            return true;
        }

        protected void InvokeOnMapDidScroll(float setMapOffsetX, float setMapOffsetY)
        {
            if (OnMapDidScroll != null)
                OnMapDidScroll(setMapOffsetX, setMapOffsetY);
        }
    }
}
