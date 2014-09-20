#region Using directives
using System;
using System.Collections.Generic;
using System.Text;
using WinWarCS.Data;
using WinWarCS.Data.Resources;
#endregion

namespace WinWarCS.Gui
{
   internal class UIWindow : UIBaseComponent
   {
      #region Constructor
      internal UIWindow()
      {
         X = Y = 0;
         Width = MainGame.OriginalAppWidth;
         Height = MainGame.OriginalAppHeight;

         UIWindowManager.AddWindow(this);
      }
      #endregion

      #region Close
      internal void Close()
      {
         UIWindowManager.RemoveWindow(this);
      }

      internal virtual void DidRemove()
      {
      }
      #endregion

      #region Render
      internal override void Draw()
      {
         base.Draw();
      }
      #endregion

      #region Unit testing
      internal static void TestWindow()
      {
         throw new NotImplementedException();
         /*UIWindow wnd = null;

         TestGame.Start("TestWindow",
             delegate
             {
                 UIResource tr = new UIResource("Main Menu Text");
                 wnd = Window.FromUIResource(tr);
             },
             delegate
             {
                 wnd.Render();
             });*/
      }
      #endregion
   }
}
