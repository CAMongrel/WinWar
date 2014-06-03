using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinWarRT.Gui.Input
{
    class UIMapControlInputHandlerClassic : UIMapControlInputHandler
    {
        internal UIMapControlInputHandlerClassic(UIMapControl setUIMapControl)
            : base(InputMode.Classic, setUIMapControl)
        {
        }

        internal override bool PointerDown(Microsoft.Xna.Framework.Vector2 position)
        {
            return true;
        }

        internal override bool PointerUp(Microsoft.Xna.Framework.Vector2 position)
        {
            return true;
        }

        internal override bool PointerMoved(Microsoft.Xna.Framework.Vector2 position)
        {
            return true;
        }
    }
}
