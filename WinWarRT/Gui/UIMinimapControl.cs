using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinWarRT.Data.Game;
using WinWarRT.Graphics;

namespace WinWarRT.Gui
{
    class UIMinimapControl : UIBaseComponent
    {
        internal UIMapControl MapControl { get; private set; }

        private Texture2D minimapTexInternal;
        private WWTexture minimapTex;

        private bool isPressed;

        internal Map CurrentMap
        {
            get
            {
                return (MapControl != null ? MapControl.CurrentMap : null);
            }
        }

        internal UIMinimapControl(UIMapControl setUIMapControl)
        {
            isPressed = false;
            MapControl = setUIMapControl;
        }

        public override void Dispose()
        {
            if (minimapTexInternal != null)
            {
                minimapTexInternal.Dispose();
                minimapTexInternal = null;
            }

            base.Dispose();
        }

        internal void Init()
        {
            minimapTexInternal = new Texture2D(MainGame.Device, Width, Height, false, SurfaceFormat.Color);

            minimapTex = WWTexture.FromDXTexture(minimapTexInternal);
        }

        private void CenterOnPosition(Microsoft.Xna.Framework.Vector2 position)
        {
            Vector2 localCoords = ConvertGlobalToLocal(position);

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
            if (tileY >= CurrentMap.MapHeight - MapControl.HeightInTiles)
                tileY = CurrentMap.MapHeight - 1 - MapControl.HeightInTiles;

            MapControl.SetCameraOffset((float)tileX * MapControl.TileWidth, (float)tileY * MapControl.TileHeight);
        }

        internal override bool PointerDown(Microsoft.Xna.Framework.Vector2 position)
        {
            CenterOnPosition(position);

            isPressed = true;

            return base.PointerDown(position);
        }

        internal override bool PointerMoved(Microsoft.Xna.Framework.Vector2 position)
        {
            if (isPressed)
            {
                CenterOnPosition(position);
            }
            return base.PointerMoved(position);
        }

        internal override bool PointerUp(Microsoft.Xna.Framework.Vector2 position)
        {
            isPressed = false;

            return base.PointerUp(position);
        }

        internal override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            base.Update(gameTime);

            if (CurrentMap != null)
            {
                // Get terrain colors
                Color[] colors = CurrentMap.GetMinimap();

                // Overlay units and buildings
                // TODO ...

                // Overlay camera rectangle
                int width = MapControl.Width / MapControl.TileWidth;
                int height = MapControl.Height / MapControl.TileHeight;

                for (int x = 0; x < width; x++)
                {
                    colors[x + MapControl.CameraTileX + MapControl.CameraTileY * MapControl.MapWidth] = Color.Yellow;
                    colors[x + MapControl.CameraTileX + (MapControl.CameraTileY + height) * MapControl.MapWidth] = Color.Yellow;
                }

                for (int y = 1; y < height; y++)
                {
                    colors[MapControl.CameraTileX + ((y + MapControl.CameraTileY) * MapControl.MapWidth)] = Color.Yellow;
                    colors[MapControl.CameraTileX + (width - 1) + ((y + MapControl.CameraTileY) * MapControl.MapWidth)] = Color.Yellow;
                }

                // Apply to texture
                minimapTexInternal.SetData<Color>(colors);
            }
        }

        internal override void Render()
        {
            base.Render();

            minimapTex.RenderOnScreen(X, Y);
        }
    }
}
