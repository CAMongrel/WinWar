#region Using directives
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using WinWarRT.Util;
#endregion

namespace WinWarRT.Gui
{
    public delegate void OnMouseDownInside(Vector2 position);
    public delegate void OnMouseUpInside(Vector2 position);
    public delegate void OnMouseUpOutside(Vector2 position);

	public abstract class UIBaseComponent
	{
		public int X;
		public int Y;
		public int Width;
		public int Height;

		public List<UIBaseComponent> Components;

		public UIBaseComponent()
		{
			Components = new List<UIBaseComponent>();
		}

		public virtual void Render()
		{
			for (int i = 0; i < Components.Count; i++)
			{
				Components[i].Render();
			}
		}

        public virtual bool MouseDown(Microsoft.Xna.Framework.Vector2 position)
        {
            if (!WinWarRT.Util.MathHelper.InsideRect(position, new Rectangle(X, Y, Width, Height)))
                return false;

            for (int i = 0; i < Components.Count; i++)
            {
                if (Components[i].MouseDown(position))
                    return true;
            }

            return true;
        }

		public virtual bool MouseUp(Microsoft.Xna.Framework.Vector2 position)
		{
			if (!WinWarRT.Util.MathHelper.InsideRect(position, new Rectangle(X, Y, Width, Height)))
				return false;

			for (int i = 0; i < Components.Count; i++)
			{
                if (Components[i].MouseUp(position))
					return true;
			}

			return true;
		}
	}
}
