using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinWarRT.Gui
{
   internal static class UIWindowManager
   {
      private static List<UIWindow> windows;

      static UIWindowManager()
      {
         windows = new List<UIWindow>();
      }

      internal static void AddWindow(UIWindow window)
      {
         windows.Add(window);
      }

      internal static void RemoveWindow(UIWindow window)
      {
         if (windows.Contains(window))
         {
            windows.Remove(window);
         }
      }

      internal static void Clear()
      {
         windows.Clear();
      }

      internal static void Update(GameTime gameTime)
      {
         for (int i = 0; i < windows.Count; i++)
         {
            windows[i].Update(gameTime);
         }
      }

      internal static void Render()
      {
         for (int i = 0; i < windows.Count; i++)
         {
            if (windows[i].Visible == false)
               continue;
            windows[i].Render();
         }
      }

      internal static bool PointerDown(Microsoft.Xna.Framework.Vector2 position)
      {
         for (int i = windows.Count - 1; i >= 0; i--)
         {
            if (windows[i].UserInteractionEnabled == false)
               continue;

            if (!WinWarRT.Util.MathHelper.InsideRect(position, new Rectangle((int)windows[i].X, (int)windows[i].Y, windows[i].Width, windows[i].Height)))
               continue;

            if (windows[i].PointerDown(position))
               return true;
         }

         return false;
      }

      internal static bool PointerUp(Microsoft.Xna.Framework.Vector2 position)
      {
         for (int i = windows.Count - 1; i >= 0; i--)
         {
            if (windows[i].UserInteractionEnabled == false)
               continue;

            if (!WinWarRT.Util.MathHelper.InsideRect(position, new Rectangle((int)windows[i].X, (int)windows[i].Y, windows[i].Width, windows[i].Height)))
               continue;

            if (windows[i].PointerUp(position))
               return true;
         }

         return false;
      }

      internal static bool PointerMoved(Microsoft.Xna.Framework.Vector2 position)
      {
         for (int i = windows.Count - 1; i >= 0; i--)
         {
            if (windows[i].UserInteractionEnabled == false)
               continue;

            if (!WinWarRT.Util.MathHelper.InsideRect(position, new Rectangle((int)windows[i].X, (int)windows[i].Y, windows[i].Width, windows[i].Height)))
               continue;

            if (windows[i].PointerMoved(position))
               return true;
         }

         return false;
      }
   }
}
