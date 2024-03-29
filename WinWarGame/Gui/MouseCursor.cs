using System;
using Microsoft.Xna.Framework;
using WinWarGame.Data.Game;
using Microsoft.Xna.Framework.Graphics;
using WinWarGame.Data;
using WinWarGame.Data.Resources;
using WinWarGame.Graphics;

namespace WinWarGame.Gui
{
   public enum MouseCursorState
   {
      None,
      Pointer,
      NotAllowed,
      CrosshairOrange,
      CrosshairRed,
      CrosshairOrange2,
      Magnifier,
      CrosshairGreen,
      Loading,
      ScrollTop,
      ScrollTopright,
      ScrollRight,
      ScrollBottomright,
      ScrollBottom,
      ScrollBottomleft,
      ScrollLeft,
      ScrollTopleft
   }

   class Cursor
   {
      public ushort HotSpotX;
      public ushort HotSpotY;
      public WWTexture Texture;

      public Cursor(CursorResource res)
      {
         HotSpotX = res.HotSpotX;
         HotSpotY = res.HotSpotY;

         Texture = WWTexture.FromCursorResource(res);
      }
   }

   public static class MouseCursor
   {
      public static bool IsVisible;

      public static Vector2 Position;

      public static Vector2 Hotspot 
      {
         get
         {
            if (currentCursor == null)
               return Vector2.Zero;

            return new Vector2 (currentCursor.HotSpotX, currentCursor.HotSpotY);
         }
      }

      private static Cursor currentCursor;

      private static MouseCursorState state;
      public static MouseCursorState State
      { 
         get { return state; }
         set
         {
            state = value;
         }
      }

      private static bool didLoadResources;

      private static Cursor[] cursorResources;

      static MouseCursor()
      {
         Position = Vector2.Zero;
         IsVisible = true;
         state = MouseCursorState.None;
         didLoadResources = false;
         currentCursor = null;
         cursorResources = null;
      }

      private static void LoadResources()
      {
         if (didLoadResources || WarFile.AreResoucesLoaded == false)
            return;

         cursorResources = new Cursor[Enum.GetValues (typeof(MouseCursorState)).Length];

         cursorResources[(int)MouseCursorState.Pointer] = new Cursor(WarFile.GetCursorResource (WarFile.KnowledgeBase.IndexByName ("Normal Pointer")));
         cursorResources[(int)MouseCursorState.NotAllowed] = new Cursor(WarFile.GetCursorResource (WarFile.KnowledgeBase.IndexByName ("Not allowed")));
         cursorResources[(int)MouseCursorState.CrosshairOrange] = new Cursor(WarFile.GetCursorResource (WarFile.KnowledgeBase.IndexByName ("Crosshair Orange")));
         cursorResources[(int)MouseCursorState.CrosshairRed] = new Cursor(WarFile.GetCursorResource (WarFile.KnowledgeBase.IndexByName ("Crosshair Red")));
         cursorResources[(int)MouseCursorState.CrosshairOrange2] = new Cursor(WarFile.GetCursorResource (WarFile.KnowledgeBase.IndexByName ("Crosshair Orange 2")));
         cursorResources[(int)MouseCursorState.Magnifier] = new Cursor(WarFile.GetCursorResource (WarFile.KnowledgeBase.IndexByName ("Magnifier")));
         cursorResources[(int)MouseCursorState.CrosshairGreen] = new Cursor(WarFile.GetCursorResource (WarFile.KnowledgeBase.IndexByName ("Crosshair Green")));
         cursorResources[(int)MouseCursorState.Loading] = new Cursor(WarFile.GetCursorResource (WarFile.KnowledgeBase.IndexByName ("Loading...")));
         cursorResources[(int)MouseCursorState.ScrollTop] = new Cursor(WarFile.GetCursorResource (WarFile.KnowledgeBase.IndexByName ("Scroll Top")));
         cursorResources[(int)MouseCursorState.ScrollTopright] = new Cursor(WarFile.GetCursorResource (WarFile.KnowledgeBase.IndexByName ("Scroll Topright")));
         cursorResources[(int)MouseCursorState.ScrollRight] = new Cursor(WarFile.GetCursorResource (WarFile.KnowledgeBase.IndexByName ("Scroll Right")));
         cursorResources[(int)MouseCursorState.ScrollBottomright] = new Cursor(WarFile.GetCursorResource (WarFile.KnowledgeBase.IndexByName ("Scroll Bottomright")));
         cursorResources[(int)MouseCursorState.ScrollBottom] = new Cursor(WarFile.GetCursorResource (WarFile.KnowledgeBase.IndexByName ("Scroll Bottom")));
         cursorResources[(int)MouseCursorState.ScrollBottomleft] = new Cursor(WarFile.GetCursorResource (WarFile.KnowledgeBase.IndexByName ("Scroll Bottomleft")));
         cursorResources[(int)MouseCursorState.ScrollLeft] = new Cursor(WarFile.GetCursorResource (WarFile.KnowledgeBase.IndexByName ("Scroll Left")));
         cursorResources[(int)MouseCursorState.ScrollTopleft] = new Cursor(WarFile.GetCursorResource (WarFile.KnowledgeBase.IndexByName ("Scroll Topleft")));

         didLoadResources = true;
      }

      public static void Render(GameTime gameTime)
      {
         if (!IsVisible)
            return;

         if (didLoadResources == false)
            LoadResources ();

         // If loading failed, then try again later
         if (didLoadResources == false)
            return;

         currentCursor = cursorResources [(int)state];

         // "None" state will result in "null" for currentCursor
         if (currentCursor == null)
            return;

         Vector2 hotSpotOffset = Hotspot;
         currentCursor.Texture.RenderOnScreen (Position.X - hotSpotOffset.X, Position.Y - hotSpotOffset.Y);
      }
   }
}

