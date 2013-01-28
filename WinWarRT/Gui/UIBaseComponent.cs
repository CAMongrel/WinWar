#region Using directives
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using WinWarRT.Util;
#endregion

namespace WinWarRT.Gui
{
    public delegate void OnPointerDownInside(Vector2 position);
    public delegate void OnPointerUpInside(Vector2 position);
    public delegate void OnPointerUpOutside(Vector2 position);

	public abstract class UIBaseComponent
	{
		public int X;
		public int Y;
		public int Width;
		public int Height;

        public Vector2 ScreenPosition
        {
            get
            {
                if (ParentComponent == null)
                    return new Vector2(X, Y);

                Vector2 parentScreenPos = ParentComponent.ScreenPosition;
                return new Vector2(X + parentScreenPos.X, Y + parentScreenPos.Y); 
            }
        }

        public UIBaseComponentList Components;
        private List<UIBaseComponent> components;

        public UIBaseComponent ParentComponent { get; private set; }

		public UIBaseComponent()
		{
            ParentComponent = null;
            components = new List<UIBaseComponent>();
            Components = new UIBaseComponentList(components);
		}

        public void AddComponent(UIBaseComponent newComp)
        {
            if (newComp == null)
                return;

            if (newComp.ParentComponent != null)
                newComp.ParentComponent.RemoveComponent(newComp);

            newComp.ParentComponent = this;
            components.Add(newComp);
        }

        public void RemoveComponent(UIBaseComponent comp)
        {
            if (comp == null)
                return;

            if (components.Contains(comp))
                components.Remove(comp);
            comp.ParentComponent = null;
        }

        public void ClearComponents()
        {
            components.Clear();
        }

        public void CenterOnScreen()
        {
            X = MainGame.OriginalAppWidth / 2 - Width / 2;
            Y = MainGame.OriginalAppHeight / 2 - Height / 2;
        }

        public void CenterInParent()
        {
            if (ParentComponent == null)
                return;

            X = ParentComponent.Width / 2 - Width / 2;
            Y = ParentComponent.Height / 2 - Height / 2;
        }

        public void CenterXInParent()
        {
            if (ParentComponent == null)
                return;

            X = ParentComponent.Width / 2 - Width / 2;
        }

        public void CenterYInParent()
        {
            if (ParentComponent == null)
                return;

            Y = ParentComponent.Height / 2 - Height / 2;
        }

		public virtual void Render()
		{
            for (int i = 0; i < components.Count; i++)
			{
                components[i].Render();
			}
		}

        public virtual bool PointerDown(Microsoft.Xna.Framework.Vector2 position)
        {
            if (!WinWarRT.Util.MathHelper.InsideRect(position, new Rectangle(X, Y, Width, Height)))
                return false;

            Vector2 relPosition = new Vector2(position.X - X, position.Y - Y);
            for (int i = components.Count - 1; i >= 0; i--)
            {
                if (components[i].PointerDown(relPosition))
                    return true;
            }

            return true;
        }

		public virtual bool PointerUp(Microsoft.Xna.Framework.Vector2 position)
		{
			if (!WinWarRT.Util.MathHelper.InsideRect(position, new Rectangle(X, Y, Width, Height)))
				return false;

            Vector2 relPosition = new Vector2(position.X - X, position.Y - Y);
            for (int i = components.Count - 1; i >= 0; i--)
            {
                if (components[i].PointerUp(relPosition))
					return true;
			}

			return true;
		}
	}
}
