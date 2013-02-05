using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinWarRT.Gui
{
    public static class UIWindowManager
    {
        private static List<UIWindow> windows;

        static UIWindowManager()
        {
            windows = new List<UIWindow>();
        }

        public static void AddWindow(UIWindow window)
        {
            windows.Add(window);
        }

        public static void RemoveWindow(UIWindow window)
        {
            if (windows.Contains(window))
            {
                windows.Remove(window);
            }
        }

        public static void Clear()
        {
            windows.Clear();
        }

        public static void Update(GameTime gameTime)
        {
            for (int i = 0; i < windows.Count; i++)
            {
                windows[i].Update(gameTime);
            }
        }

        public static void Render()
        {
            for (int i = 0; i < windows.Count; i++)
            {
                windows[i].Render();
            }
        }

        public static bool PointerDown(Microsoft.Xna.Framework.Vector2 position)
        {
            for (int i = windows.Count - 1; i >= 0; i--)
            {
                if (windows[i].PointerDown(position))
                    return true;
            }

            return false;
        }

        public static bool PointerUp(Microsoft.Xna.Framework.Vector2 position)
        {
            for (int i = windows.Count - 1; i >= 0; i--)
            {
                if (windows[i].PointerUp(position))
                    return true;
            }

            return false;
        }

        public static bool PointerMoved(Microsoft.Xna.Framework.Vector2 position)
        {
            for (int i = windows.Count - 1; i >= 0; i--)
            {
                if (windows[i].PointerMoved(position))
                    return true;
            }

            return false;
        }
    }
}
