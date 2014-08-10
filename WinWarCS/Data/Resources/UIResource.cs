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

      private int ReadResourceIndex(int index, WarResource res)
      {
         int offset = ReadInt(index, res.data);

         int resIndex = ReadInt(offset, res.data);
         if (resIndex == 0)
            return 0;

         return resIndex - 2;
      }

      private string ReadString(int index, WarResource res)
      {
         StringBuilder result = new StringBuilder();

         int offset = ReadInt(index, res.data);

         byte b = res.data[offset++];
         // Nullterminated string
         while (b != 0x00)
         {
            result.Append((char)b);
            b = res.data[offset++];
         }

         return result.ToString();
      }

      private void ReadBackgroundImage(WarResource res)
      {
         BackgroundImageResourceIndex = ReadResourceIndex(0x12, res);
      }

      private void ReadSubHeader(WarResource res)
      {
         // Unknown int ==> Always 0x2A
         // "Enter" button index
         // "Escape" button index

         int index = 0x1E;
         int offset = ReadInt(index, res.data);

         EnterButtonIndex = ReadUShort(offset + 4, res.data);
         EscapeButtonIndex = ReadUShort(offset + 6, res.data);
         ushort unk1 = ReadUShort(offset + 8, res.data);
         ushort unk2 = ReadUShort(offset + 10, res.data);
      }

      private void ReadTitle(WarResource res)
      {
         int index = 0x22;
         int offset = ReadInt(index, res.data);

         ushort firstVal = ReadUShort(offset, res.data);
         if (firstVal == 0xFFFF)
            // Means => No title
            return;

         Title = new MenuEntry();
         Title.X = ReadUShort(offset + 2, res.data);
         Title.Y = ReadUShort(offset + 4, res.data);
         Title.Text = ReadString(offset + 6, res);
      }

      private void ReadButtons(WarResource res)
      {
         int index = 0x26;
         int offset = ReadInt(index, res.data);

         // Length of one button => 0x1C
         while (offset < res.data.Length)
         {
            ushort firstVal = ReadUShort(offset, res.data);
            if (firstVal == 0xFFFF)
               break;

            MenuEntry me = new MenuEntry();
            me.X = ReadUShort(offset + 4, res.data);
            me.Y = ReadUShort(offset + 6, res.data);
            me.ButtonReleasedResourceIndex = ReadResourceIndex(offset + 10, res);
            me.ButtonPressedResourceIndex = ReadResourceIndex(offset + 14, res);
            me.Text = ReadString(offset + 18, res);
            me.ButtonIndex = ReadInt(offset + 22, res.data);
            me.HotKey = ReadUShort(offset + 26, res.data);
            Texts.Add(me);

            offset += 0x1C;
         }
      }
      #endregion

		#region Init
		private void Init(WarResource data)
      {
         Title = null;
         Texts = new List<MenuEntry>();

         ReadBackgroundImage(data);
         ReadSubHeader(data);
         ReadTitle(data);
         ReadButtons(data);
      }
		#endregion
	}
}
