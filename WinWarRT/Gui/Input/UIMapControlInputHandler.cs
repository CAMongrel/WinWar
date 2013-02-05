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

    public delegate void MapDidScroll(float setMapOffsetX, float setMapOffsetY);

    class UIMapControlInputHandler
    {
        public InputMode InputMode { get; private set; }
        public UIMapControl MapControl { get; private set; }

        public event MapDidScroll OnMapDidScroll;

        protected UIMapControlInputHandler(InputMode setInputMode, UIMapControl setUIMapControl)
        {
            MapControl = setUIMapControl;
            InputMode = setInputMode;
        }

        public virtual void SetCameraOffset(float setCamOffsetX, float setCamOffsetY)
        {
            //
        }

        public virtual bool PointerDown(Microsoft.Xna.Framework.Vector2 position)
        {
            return true;
        }

        public virtual bool PointerUp(Microsoft.Xna.Framework.Vector2 position)
        {
            return true;
        }

        public virtual bool PointerMoved(Microsoft.Xna.Framework.Vector2 position)
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
