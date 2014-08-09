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
			internal ushort HotKey;

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

      internal ushort EnterButtonIndex;
      internal ushort EscapeButtonIndex;
		#endregion

		#region Constructor
		internal UIResource(WarResource data)
		{
         Type = ContentFileType.FileUI;

			if (data == null)
				throw new ArgumentNullException("data");

			Init(data);
		}
		#endregion

      #region ReadData
      /*
      // Offset of resource tables starts at 0x12 (uint)
      // Offset of string table starts at 0x16 (uint)
      // Offset of multiselect values starts at 0x1A (uint)
      // Offset of sub header starts at 0x1E (uint)
      // Offset of title info starts at 0x22 (uint)
      // Offset of button list starts at 0x26 (uint)

      // Resource table
      // -2 resource index of background image
      // -2 resource index of button
      // -2 resource index of pressed button
      */

      private int ReadResourceIndex(int index)
      {
         int offset = ReadInt(index);

         int resIndex = ReadInt(offset);
         if (resIndex == 0)
            return 0;

         return resIndex - 2;
      }

      private string ReadString(int index)
      {
         StringBuilder result = new StringBuilder();

         int offset = ReadInt(index);

         byte b = Resource.data[offset++];
         // Nullterminated string
         while (b != 0x00)
         {
            result.Append((char)b);
            b = Resource.data[offset++];
         }

         return result.ToString();
      }

      private void ReadBackgroundImage()
      {
         BackgroundImageResourceIndex = ReadResourceIndex(0x12);
      }

      private void ReadSubHeader()
      {
         // Unknown int ==> Always 0x2A
         // "Enter" button index
         // "Escape" button index

         int index = 0x1E;
         int offset = ReadInt(index);

         EnterButtonIndex = ReadUShort(offset + 4);
         EscapeButtonIndex = ReadUShort(offset + 6);
         ushort unk1 = ReadUShort(offset + 8);
         ushort unk2 = ReadUShort(offset + 10);
      }

      private void ReadTitle()
      {
         int index = 0x22;
         int offset = ReadInt(index);

         ushort firstVal = ReadUShort(offset);
         if (firstVal == 0xFFFF)
            // Means => No title
            return;

         Title = new MenuEntry();
         Title.X = ReadUShort(offset + 2);
         Title.Y = ReadUShort(offset + 4);
         Title.Text = ReadString(offset + 6);
      }

      private void ReadButtons()
      {
         int index = 0x26;
         int offset = ReadInt(index);

         // Length of one button => 0x1C
         while (offset < Resource.data.Length)
         {
            ushort firstVal = ReadUShort(offset);
            if (firstVal == 0xFFFF)
               break;

            MenuEntry me = new MenuEntry();
            me.X = ReadUShort(offset + 4);
            me.Y = ReadUShort(offset + 6);
            me.ButtonReleasedResourceIndex = ReadResourceIndex(offset + 10);
            me.ButtonPressedResourceIndex = ReadResourceIndex(offset + 14);
            me.Text = ReadString(offset + 18);
            me.ButtonIndex = ReadInt(offset + 22);
            me.HotKey = ReadUShort(offset + 26);
            Texts.Add(me);

            offset += 0x1C;
         }
      }
      #endregion

		#region Init
		private void Init(WarResource data)
      {
         this.Resource = data;

         Title = null;
         Texts = new List<MenuEntry>();

         ReadBackgroundImage();
         ReadSubHeader();
         ReadTitle();
         ReadButtons();
      }
		#endregion
	}
}
