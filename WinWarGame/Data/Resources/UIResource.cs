#region Using directives
using System;
using System.Collections.Generic;
using System.Text;
#endregion

namespace WinWarCS.Data.Resources
{
   public class UIResource : BasicResource
   {
      #region Subclass MenuEntry
      internal enum UIEntryType
      {
         Button = 0,
         LeftArrow = 1,
         RightArrow = 2,
         ValueList = 3,
         ListBox = 5,
         ListBoxSelectButton = 6,
         TextFieldButton = 7,
         TextFieldHidden = 8,
         TextFieldSelectable = 9,
         Label = 255,      // Not used in DATA.WAR
      }

      internal enum UIEntryTextAlignment
      {
         Center,
         Left,
         Right,
      }

      internal class UIEntry
      {
         internal UIEntryType Type;
         internal string Text;
         internal ushort X;
         internal ushort Y;
         internal UIEntryTextAlignment Alignment;

         internal int ButtonReleasedResourceIndex;
         internal int ButtonPressedResourceIndex;
         internal int ButtonIndex;
         internal ushort HotKey;
         internal int ValueCount;
         internal List<string> Values;

         public UIEntry()
         {
            Alignment = UIEntryTextAlignment.Center;
            Text = string.Empty;
            X = 0;
            Y = 0;
            Values = new List<string>();
         }

         public override string ToString()
         {
            string result = Type + " -- " + X + "," + Y;
            switch (Type)
            {
               case UIEntryType.Button:
                  result += " -- '" + Text + "'";
                  result += " -- '" + (char)HotKey + "'";
                  break;

               case UIEntryType.LeftArrow:
               case UIEntryType.RightArrow:
                  result += " -- " + ValueCount + " values";
                  break;

               case UIEntryType.ValueList:
                  result += " -- " + Values.Count + " values";
                  break;
            }
            return result;
         }
      }
      #endregion

      #region Variables
      internal int BackgroundImageResourceIndex;
      internal List<UIEntry> Labels;
      internal List<UIEntry> Elements;

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

         return ReadResourceIndexDirectInt(offset, res);
      }

      private string ReadString(int index, WarResource res)
      {
         int offset = ReadInt(index, res.data);
         if (offset == 0)
            return string.Empty;

         return ReadStringDirect(offset, res);
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
         // In some cases, there's more data in this section

         int index = 0x1E;
         int offset = ReadInt(index, res.data);

         EnterButtonIndex = ReadUShort(offset + 4, res.data);
         EscapeButtonIndex = ReadUShort(offset + 6, res.data);
         ushort unk1 = ReadUShort(offset + 8, res.data);
         ushort unk2 = ReadUShort(offset + 10, res.data);
      }

      private List<string> ReadMultiselectValuesRowAtOffset(int offset, WarResource res)
      {
         List<string> result = new List<string>();

         while (offset < res.data.Length)
         {
            int val = ReadInt(offset, res.data);
            if (val == unchecked((int)0xFFFFFFFF) ||
               val == 0)
               break;

            result.Add(ReadStringDirect(val, res));

            offset += 4;
         }

         return result;
      }

      private string[] ReadMultiselectValues(int buttonIndex, WarResource res)
      {
         int index = 0x1A;

         int offset = ReadInt(index, res.data);

         int counter = 1;
         while (offset < res.data.Length)
         {
            List<string> list = ReadMultiselectValuesRowAtOffset(offset, res);
            if (list.Count == 0)
               return new string[0];

            if (counter == buttonIndex)
               return list.ToArray();

            counter++;
            offset += list.Count * 4 + 4;
         }

         return new string[0];
      }

      private void ReadTitle(WarResource res)
      {
         int index = 0x22;
         int offset = ReadInt(index, res.data);

         while (offset < res.data.Length)
         {
            ushort firstVal = ReadUShort(offset, res.data);
            if (firstVal == 0xFFFF)
               // Means => end of title section
               return;

            UIEntry Label = new UIEntry();
            Label.Type = UIEntryType.Label;
            Label.Alignment = (UIEntryTextAlignment)firstVal;
            Label.X = ReadUShort(offset + 2, res.data);
            Label.Y = ReadUShort(offset + 4, res.data);
            Label.Text = ReadString(offset + 6, res);
            Labels.Add(Label);

            offset += 0x0A;
         }
      }

      private void ReadUIElements(WarResource res)
      {
         int index = 0x26;
         int offset = ReadInt(index, res.data);

         // Length of one button => 0x1C
         while (offset < res.data.Length)
         {
            ushort firstVal = ReadUShort(offset, res.data);
            if (firstVal == 0xFFFF)
               break;

            UIEntry me = new UIEntry();
            me.Type = (UIEntryType)ReadUShort(offset, res.data);
            me.Alignment = (UIEntryTextAlignment)ReadUShort(offset + 2, res.data);      // This is just a guess based on the label section
            me.X = ReadUShort(offset + 4, res.data);
            me.Y = ReadUShort(offset + 6, res.data);
            ushort unk = ReadUShort(offset + 8, res.data);
            me.ButtonReleasedResourceIndex = ReadResourceIndex(offset + 10, res);
            me.ButtonPressedResourceIndex = ReadResourceIndex(offset + 14, res);
            me.Text = string.Empty;
            if (me.Type == UIEntryType.Button || me.Type == UIEntryType.TextFieldSelectable ||
               me.Type == UIEntryType.ListBoxSelectButton || me.Type == UIEntryType.TextFieldButton ||
               me.Type == UIEntryType.TextFieldHidden)
            {
               me.Text = ReadString(offset + 18, res);
            }
            else if (me.Type == UIEntryType.ValueList || me.Type == UIEntryType.ListBox)
            {
               // Read values
               int valuesOffset = ReadInt(offset + 18, res.data);
               while (valuesOffset < res.data.Length)
               {
                  int valueOff = ReadInt(valuesOffset, res.data);
                  if (valueOff == unchecked((int)0xFFFFFFFF) ||
                     valueOff == 0)
                     break;

                  string val = ReadStringDirect(valueOff, res);
                  me.Values.Add(val);

                  valuesOffset += 4;
               }
            }
            else
            {
               me.ValueCount = ReadInt(offset + 18, res.data);
            }

            me.ButtonIndex = ReadInt(offset + 22, res.data);
            me.HotKey = ReadUShort(offset + 26, res.data);
            Elements.Add(me);

            if (me.Type == UIEntryType.ListBoxSelectButton)
            {
               // Fill values which will be pasted into the listbox
               me.Values = new List<string>(ReadMultiselectValues(me.ButtonIndex, res));
            }

            offset += 0x1C;
         }
      }
      #endregion

      #region Init
      private void Init(WarResource data)
      {
         Labels = new List<UIEntry>();
         Elements = new List<UIEntry>();

         ReadBackgroundImage(data);
         ReadSubHeader(data);
         ReadTitle(data);
         ReadUIElements(data);
      }
      #endregion
   }
}
