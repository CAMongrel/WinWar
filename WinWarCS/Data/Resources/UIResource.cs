#region Using directives
using System;
using System.Collections.Generic;
using System.Text;
#endregion

namespace WinWarCS.Data.Resources
{
	internal class UIResource : BasicResource
	{
		#region Subclass MenuEntry
		internal class MenuEntry
		{
			internal string Text;
			internal ushort X;
			internal ushort Y;

			internal int ButtonReleasedResourceIndex;
         internal int ButtonPressedResourceIndex;
         internal int ButtonIndex;
			internal ushort Unknown;

         public MenuEntry()
         {
            Text = string.Empty;
            X = 0;
            Y = 0;
         }
		}
		#endregion

		#region Variables
      internal int BackgroundImageResourceIndex;
      internal MenuEntry Title;
		internal List<MenuEntry> Texts;
		#endregion

		#region Constructor
		internal UIResource(string name)
		{
			WarResource res = WarFile.GetResourceByName(name);

			if (res == null)
				throw new ArgumentNullException("name");

			Init(res);
		}

		internal UIResource(WarResource data)
		{
			if (data == null)
				throw new ArgumentNullException("data");

			Init(data);
		}
		#endregion

      #region ReadData
      /*
      // Offset of resource tables starts at 0x12 (uint)
      // Offset of string table starts at 0x16 (uint)
      // Offset of unknown* starts at 0x1A (uint)
      // Offset of unknown* starts at 0x1E (uint)
      // Offset of title info starts at 0x22 (uint)
      // Offset of button list starts at 0x26 (uint)

      // Resource table
      // -2 resource index of background image
      // -2 resource index of button
      // -2 resource index of pressed button
      */

      private int ReadResourceIndex(int index)
      {
         int offset = data.data[index + 0] + (data.data[index + 1] << 8) + (data.data[index + 2] << 16) + (data.data[index + 3] << 24);

         int resIndex = data.data[offset + 0] + (data.data[offset + 1] << 8) + (data.data[offset + 2] << 16) + (data.data[offset + 3] << 24);
         if (resIndex == 0)
            return 0;

         return resIndex - 2;
      }

      private string ReadString(int index)
      {
         StringBuilder result = new StringBuilder();

         int offset = data.data[index + 0] + (data.data[index + 1] << 8) + (data.data[index + 2] << 16) + (data.data[index + 3] << 24);

         byte b = data.data[offset++];
         // Nullterminated string
         while (b != 0x00)
         {
            result.Append((char)b);
            b = data.data[offset++];
         }

         return result.ToString();
      }

      private void ReadBackgroundImage()
      {
         BackgroundImageResourceIndex = ReadResourceIndex(0x12);
      }

      private void ReadTitle()
      {
         int index = 0x22;
         int offset = data.data[index + 0] + (data.data[index + 1] << 8) + (data.data[index + 2] << 16) + (data.data[index + 3] << 24);

         ushort firstVal = (ushort)(data.data[offset + 0] + (data.data[offset + 1] << 8));
         if (firstVal == 0xFFFF)
            // Means => No title
            return;

         Title = new MenuEntry();
         Title.X = (ushort)(data.data[offset + 2] + (data.data[offset + 3] << 8));
         Title.Y = (ushort)(data.data[offset + 4] + (data.data[offset + 5] << 8));
         Title.Text = ReadString(offset + 6);
      }

      private void ReadButtons()
      {
         int index = 0x26;
         int offset = data.data[index + 0] + (data.data[index + 1] << 8) + (data.data[index + 2] << 16) + (data.data[index + 3] << 24);

         // Length of one button => 0x1C
         while (offset < data.data.Length)
         {
            ushort firstVal = (ushort)(data.data[offset + 0] + (data.data[offset + 1] << 8));
            if (firstVal == 0xFFFF)
               break;

            MenuEntry me = new MenuEntry();
            me.X = (ushort)(data.data[offset + 4] + (data.data[offset + 5] << 8));
            me.Y = (ushort)(data.data[offset + 6] + (data.data[offset + 7] << 8));
            me.ButtonReleasedResourceIndex = ReadResourceIndex(offset + 10);
            me.ButtonPressedResourceIndex = ReadResourceIndex(offset + 14);
            me.Text = ReadString(offset + 18);
            me.ButtonIndex = data.data[offset + 22] + (data.data[offset + 23] << 8) + (data.data[offset + 24] << 16) + (data.data[offset + 25] << 24);
            me.Unknown = (ushort)(data.data[offset + 26] + (data.data[offset + 27] << 8));
            Texts.Add(me);

            offset += 0x1C;
         }
      }
      #endregion

		#region Init
		private void Init(WarResource data)
      {
         this.data = data;

         Title = null;
         Texts = new List<MenuEntry>();

         ReadBackgroundImage();
         ReadTitle();
         ReadButtons();
      }
		#endregion
	}
}
