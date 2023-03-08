using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinWarGame.Data.Game;
using WinWarGame.Graphics;
using WinWarGame.Gui.Input;

namespace WinWarGame.Gui
{
   class UIMinimapControl : UIBaseComponent
   {
      internal UIMapControl MapControl { get; private set; }

      private WWTexture minimapTex;

      private bool isLeftPressed;
      private bool isRightPressed;

      internal Map CurrentMap {
         get {
            return (MapControl != null ? MapControl.CurrentMap : null);
         }
      }

      internal UIMinimapControl (UIMapControl setUIMapControl)
      {
         isLeftPressed = false;
         isRightPressed = false;
         MapControl = setUIMapControl;
      }

      public override void Dispose ()
      {
         if (minimapTex != null) 
         {
            minimapTex.Dispose ();
            minimapTex = null;
         }

         base.Dispose ();
      }

      internal void Init ()
      {
         minimapTex = WWTexture.FromRawData (Width, Height, null);
      }

      private void CenterOnPosition (Microsoft.Xna.Framework.Vector2 position)
      {
         Vector2 localCoords = ConvertGlobalToLocal (position);

         int tileX = (int)localCoords.X;
         int tileY = (int)localCoords.Y;

         tileX -= (MapControl.WidthInTiles / 2);
         tileY -= (MapControl.HeightInTiles / 2);

         if (tileX < 0)
            tileX = 0;
         if (tileY < 0)
            tileY = 0;
         if (tileX >= CurrentMap.MapWidth - MapControl.WidthInTiles)
            tileX = CurrentMap.MapWidth - MapControl.WidthInTiles;
         if (tileY > CurrentMap.MapHeight - MapControl.HeightInTiles)
            tileY = CurrentMap.MapHeight - MapControl.HeightInTiles;

         MapControl.SetCameraOffset ((float)tileX * MapControl.TileWidth, (float)tileY * MapControl.TileHeight);
      }

      internal override bool PointerDown (Microsoft.Xna.Framework.Vector2 position, PointerType pointerType)
      {
         if (pointerType == PointerType.LeftMouse) 
         {
            isLeftPressed = true;
         }
         if (pointerType == PointerType.RightMouse) 
         {
            isRightPressed = true;
         }

         bool shouldCenter = isLeftPressed;
         if (MapControl.InputHandler.InputMode == InputMode.Classic)
            shouldCenter |= isRightPressed;

         if (shouldCenter) 
         {
            CenterOnPosition (position);
         }

         return base.PointerDown (position, pointerType);
      }

      internal override bool PointerMoved (Microsoft.Xna.Framework.Vector2 position)
      {
         bool shouldCenter = isLeftPressed;
         if (MapControl.InputHandler.InputMode == InputMode.Classic)
            shouldCenter |= isRightPressed;

         if (shouldCenter) 
         {
            CenterOnPosition (position);
         }

         return base.PointerMoved (position);
      }

      internal override bool PointerUp (Microsoft.Xna.Framework.Vector2 position, PointerType pointerType)
      {
         if (pointerType == PointerType.LeftMouse)
            isLeftPressed = false;
         if (pointerType == PointerType.RightMouse)
            isRightPressed = false;

         if (pointerType == PointerType.RightMouse && 
            MapControl.InputHandler.InputMode == InputMode.EnhancedMouse) 
         {
            Vector2 localCoords = ConvertGlobalToLocal (position);

            int tileX = (int)localCoords.X;
            int tileY = (int)localCoords.Y;

            // Perform order if applicable
            UIMapControlInputHandlerEnhancedMouse mouseControl = (UIMapControlInputHandlerEnhancedMouse)MapControl.InputHandler;
            mouseControl.PerformRightClick (tileX, tileY);
         }

         return base.PointerUp (position, pointerType);
      }

      internal override void Update (Microsoft.Xna.Framework.GameTime gameTime)
      {
         base.Update (gameTime);

         if (CurrentMap != null) 
         {
            // Get terrain colors and entities
            Color[] colors = CurrentMap.GetMinimap ();

            // Overlay camera rectangle
            int width = MapControl.Width / MapControl.TileWidth;
            int height = MapControl.Height / MapControl.TileHeight;

            if (MapControl.CameraTileY + height >= MapControl.MapHeight)
               height = MapControl.MapHeight - MapControl.CameraTileY - 1;

            Color camRectColor = Color.LightGray;
            for (int x = 0; x < width; x++) {
               colors [x + MapControl.CameraTileX + MapControl.CameraTileY * MapControl.MapWidth] = camRectColor;
               colors [x + MapControl.CameraTileX + (MapControl.CameraTileY + height) * MapControl.MapWidth] = camRectColor;
            }

            for (int y = 1; y < height; y++) {
               colors [MapControl.CameraTileX + ((y + MapControl.CameraTileY) * MapControl.MapWidth)] = camRectColor;
               colors [MapControl.CameraTileX + (width - 1) + ((y + MapControl.CameraTileY) * MapControl.MapWidth)] = camRectColor;
            }

            // Apply to texture
            minimapTex.SetData(colors);
         }
      }

      internal override void Draw()
      {
         base.Draw();

         minimapTex.RenderOnScreen (X, Y);
      }
   }
}
