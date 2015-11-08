using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using WinWarCS.Gui;
using UIKit;

namespace WinWarCS.Platform
{
   public static class Input
   {
      private static Vector2 prevMousePos;
      private static TouchLocationState prevMouseState;

      static Input()
      {
         prevMousePos = new Vector2 (-1, -1);
         prevMouseState = TouchLocationState.Released;
      }

      public static void UpdateInput(GameTime gameTime)
      {
         Microsoft.Xna.Framework.Input.Touch.TouchPanel.DisplayWidth = (int)((float)MainGame.AppWidth / (float)UIScreen.MainScreen.Scale);
         Microsoft.Xna.Framework.Input.Touch.TouchPanel.DisplayHeight = (int)((float)MainGame.AppHeight / (float)UIScreen.MainScreen.Scale);

         var coll = Microsoft.Xna.Framework.Input.Touch.TouchPanel.GetState();
         if (coll.Count == 0)
            return;

         var touch = coll [0];

         Vector2 mousePos = new Vector2(touch.Position.X, touch.Position.Y);
     
         mousePos.X *= (float)UIScreen.MainScreen.Scale;
         mousePos.Y *= (float)UIScreen.MainScreen.Scale;

         Vector2 scaledPosition = new Microsoft.Xna.Framework.Vector2 (
            (mousePos.X - MainGame.ScaledOffsetX) / MainGame.ScaleX, 
            (mousePos.Y - MainGame.ScaledOffsetY) / MainGame.ScaleY);

         MouseCursor.Position = scaledPosition;

         if (mousePos != prevMousePos)
         {
            MainGame.WinWarGame.PointerMoved(scaledPosition);
            prevMousePos = mousePos;
         }

         // Left mouse button
         if (touch.State == TouchLocationState.Pressed && prevMouseState == TouchLocationState.Released)
         {
            MainGame.WinWarGame.PointerPressed (scaledPosition, PointerType.LeftMouse);
            prevMouseState = touch.State;
         }
         if (touch.State == TouchLocationState.Released && prevMouseState == TouchLocationState.Pressed)
         {
            MainGame.WinWarGame.PointerReleased (scaledPosition, PointerType.LeftMouse);
            prevMouseState = touch.State;
         }
      }
   }
}

