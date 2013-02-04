#region Using directives
using System;
using System.Collections.Generic;
using System.Text;
using WinWarRT.Data;
using WinWarRT.Data.Resources;
#endregion

namespace WinWarRT.Gui
{
	public class UIWindow : UIBaseComponent
    {
        #region Constructor
        protected UIWindow()
		{
			X = Y = 0;
			Width = MainGame.AppWidth;
            Height = MainGame.AppHeight;

            UIWindowManager.AddWindow(this); ;
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
        public static UIWindow FromTextResource(string name)
        {
            int idx = KnowledgeBase.IndexByName(name);
			if (idx == -1)
				return null;

			TextResource tr = WarFile.GetTextResource(idx);
			return FromTextResource(tr);
        }

		public static UIWindow FromTextResource(TextResource resource)
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
        public void Close()
        {
            UIWindowManager.RemoveWindow(this);
        }
        #endregion

        #region Render
        public override void Render()
		{
			base.Render();
		}
		#endregion

		#region Unit testing
		public static void TestWindow()
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
