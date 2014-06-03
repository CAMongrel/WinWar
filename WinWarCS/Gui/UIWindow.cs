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

      #region InitWithTextResource
      protected void InitWithTextResource(string name)
      {
         int idx = KnowledgeBase.IndexByName(name);
         if (idx == -1)
            return;

         TextResource tr = WarFile.GetTextResource(idx);
         InitWithTextResource(tr);
      }

      protected void InitWithTextResource(TextResource resource)
      {
         ClearComponents();

         for (int i = 0; i < resource.Texts.Count; i++)
         {
            if (resource.Texts[i].unknown1 == 0)
            {
               UILabel lbl = new UILabel(resource.Texts[i].Text);
               lbl.X = (int)(resource.Texts[i].X);
               lbl.Y = (int)(70 + resource.Texts[i].Y);
               AddComponent(lbl);
            }
            else
            {
               UIButton.ButtonType type = UIButton.ButtonType.MediumButton;
               if (resource.Texts[i].unknown4 == 66)
                  type = UIButton.ButtonType.SmallButton;

               UIButton btn = new UIButton(resource.Texts[i].Text, type);
               btn.X = (int)(resource.Texts[i].X);
               btn.Y = (int)(70 + resource.Texts[i].Y);
               AddComponent(btn);
            }
         }
      }
      #endregion

      #region FromTextResource
      internal static UIWindow FromTextResource(string name)
      {
         int idx = KnowledgeBase.IndexByName(name);
         if (idx == -1)
            return null;

         TextResource tr = WarFile.GetTextResource(idx);
         return FromTextResource(tr);
      }

      internal static UIWindow FromTextResource(TextResource resource)
      {
         UIWindow wnd = new UIWindow();

         for (int i = 0; i < resource.Texts.Count; i++)
         {
            UIButton btn = new UIButton(resource.Texts[i].Text, UIButton.ButtonType.MediumButton);
            btn.X = (int)(resource.Texts[i].X);
            btn.Y = (int)(70 + resource.Texts[i].Y);
            wnd.AddComponent(btn);
         }

         return wnd;
      }
      #endregion

      #region Close
      internal void Close()
      {
         UIWindowManager.RemoveWindow(this);
      }
      #endregion

      #region Render
      internal override void Render()
      {
         base.Render();
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
                 TextResource tr = new TextResource("Main Menu Text");
                 wnd = Window.FromTextResource(tr);
             },
             delegate
             {
                 wnd.Render();
             });*/
      }
      #endregion
   }
}
