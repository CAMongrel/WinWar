using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace WinWarCS.Util
{
	class MathHelper
	{
		internal static bool InsideRect(int x, int y, Rectangle rect)
		{
			return (x >= rect.X && x <= rect.X + rect.Width &&
					y >= rect.Y && y <= rect.Y + rect.Height);
		}

        internal static bool InsideRect(Vector2 pos, Rectangle rect)
        {
            return (pos.X >= rect.X && pos.X <= rect.X + rect.Width &&
                    pos.Y >= rect.Y && pos.Y <= rect.Y + rect.Height);
        }
	}
}
