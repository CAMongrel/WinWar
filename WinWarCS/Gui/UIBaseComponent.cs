#region Using directives
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using WinWarCS.Util;
#endregion

namespace WinWarCS.Gui
{
   internal delegate void OnPointerDownInside(Vector2 position);
   internal delegate void OnPointerUpInside(Vector2 position);
   internal delegate void OnPointerUpOutside(Vector2 position);

   internal abstract class UIBaseComponent : IDisposable
   {
      internal int X;
      internal int Y;
      internal int Width;
      internal int Height;

      internal float Alpha;

      internal bool UserInteractionEnabled { get; set; }
      internal bool Visible { get; set; }

      internal float CompositeAlpha
      {
         get
         {
            if (ParentComponent == null)
               return Alpha;

            return Alpha * ParentComponent.CompositeAlpha;
         }
      }

      internal Vector2 ScreenPosition
      {
         get
         {
            if (ParentComponent == null)
               return new Vector2(X, Y);

            Vector2 parentScreenPos = ParentComponent.ScreenPosition;
            return new Vector2(X + parentScreenPos.X, Y + parentScreenPos.Y);
         }
      }

      internal UIBaseComponentList Components;
      private List<UIBaseComponent> components;

      internal UIBaseComponent ParentComponent { get; private set; }

      internal UIBaseComponent()
      {
         UserInteractionEnabled = true;
         Visible = true;
         Alpha = 1.0f;
         ParentComponent = null;
         components = new List<UIBaseComponent>();
         Components = new UIBaseComponentList(components);
      }

      internal void AddComponent(UIBaseComponent newComp)
      {
         if (newComp == null)
            return;

         if (newComp.ParentComponent != null)
            newComp.ParentComponent.RemoveComponent(newComp);

         newComp.ParentComponent = this;
         components.Add(newComp);
      }

      internal void RemoveComponent(UIBaseComponent comp)
      {
         if (comp == null)
            return;

         if (components.Contains(comp))
            components.Remove(comp);
         comp.ParentComponent = null;
      }

      internal void ClearComponents()
      {
         components.Clear();
      }

      internal void CenterOnScreen()
      {
         X = MainGame.OriginalAppWidth / 2 - Width / 2;
         Y = MainGame.OriginalAppHeight / 2 - Height / 2;
      }

      internal void CenterInParent()
      {
         if (ParentComponent == null)
            return;

         X = ParentComponent.Width / 2 - Width / 2;
         Y = ParentComponent.Height / 2 - Height / 2;
      }

      internal void CenterXInParent()
      {
         if (ParentComponent == null)
            return;

         X = ParentComponent.Width / 2 - Width / 2;
      }

      internal void CenterYInParent()
      {
         if (ParentComponent == null)
            return;

         Y = ParentComponent.Height / 2 - Height / 2;
      }

      internal Vector2 ConvertGlobalToLocal(Vector2 globalCoords)
      {
         Vector2 result = globalCoords;
         UIBaseComponent comp = this;

         while (comp != null)
         {
            result.X -= comp.X;
            result.Y -= comp.Y;

            comp = comp.ParentComponent;
         }

         return result;
      }

      internal Vector2 ConvertLocalToGlobal(Vector2 localCoords)
      {
         Vector2 result = localCoords;
         UIBaseComponent comp = this;

         while (comp != null)
         {
            result.X += comp.X;
            result.Y += comp.Y;

            comp = comp.ParentComponent;
         }

         return result;
      }

      internal virtual void Update(GameTime gameTime)
      {
         for (int i = 0; i < components.Count; i++)
         {
            components[i].Update(gameTime);
         }
      }

      internal virtual void Render()
      {
         for (int i = 0; i < components.Count; i++)
         {
            if (components[i].Visible == false)
               continue;

            components[i].Render();
         }
      }

      internal virtual bool PointerDown(Microsoft.Xna.Framework.Vector2 position)
      {
         if (UserInteractionEnabled == false)
            return false;

         Vector2 relPosition = new Vector2(position.X - X, position.Y - Y);
         for (int i = components.Count - 1; i >= 0; i--)
         {
            Vector2 screenPos = components[i].ScreenPosition;
            if (!WinWarCS.Util.MathHelper.InsideRect(position, new Rectangle((int)screenPos.X, (int)screenPos.Y, components[i].Width, components[i].Height)))
               continue;

            if (components[i].PointerDown(relPosition))
               return true;
         }

         return true;
      }

      internal virtual bool PointerUp(Microsoft.Xna.Framework.Vector2 position)
      {
         if (UserInteractionEnabled == false)
            return false;

         Vector2 relPosition = new Vector2(position.X - X, position.Y - Y);
         for (int i = components.Count - 1; i >= 0; i--)
         {
            Vector2 screenPos = components[i].ScreenPosition;
            if (!WinWarCS.Util.MathHelper.InsideRect(position, new Rectangle((int)screenPos.X, (int)screenPos.Y, components[i].Width, components[i].Height)))
               continue;

            if (components[i].PointerUp(relPosition))
               return true;
         }

         return true;
      }

      internal virtual bool PointerMoved(Microsoft.Xna.Framework.Vector2 position)
      {
         if (UserInteractionEnabled == false)
            return false;

         Vector2 relPosition = new Vector2(position.X - X, position.Y - Y);
         for (int i = components.Count - 1; i >= 0; i--)
         {
            Vector2 screenPos = components[i].ScreenPosition;
            if (!WinWarCS.Util.MathHelper.InsideRect(position, new Rectangle((int)screenPos.X, (int)screenPos.Y, components[i].Width, components[i].Height)))
               continue;

            if (components[i].PointerMoved(relPosition))
               return true;
         }

         return true;
      }

      public virtual void Dispose()
      {
         //
      }
   }
}
