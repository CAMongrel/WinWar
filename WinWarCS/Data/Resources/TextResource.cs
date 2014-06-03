#region Using directives
using System;
using System.Collections.Generic;
using System.Text;
#endregion

namespace WinWarCS.Data.Resources
{
	internal class TextResource : BasicResource
	{
		#region Subclass MenuEntry
		internal class MenuEntry
		{
			internal string Text;
			internal ushort X;
			internal ushort Y;
			internal int ButtonID;

			internal int unknown1;
			internal int unknown3;
			internal int unknown4;
			internal int unknown5;
		}
		#endregion

		#region Variables
		internal List<MenuEntry> Texts;
		#endregion

		#region Constructor
		internal TextResource(string name)
		{
			WarResource res = WarFile.GetResourceByName(name);

			if (res == null)
				throw new ArgumentNullException("name");

			Init(res);
		}

		internal TextResource(WarResource data)
		{
			if (data == null)
				throw new ArgumentNullException("data");

			Init(data);
		}
		#endregion

		#region Init
		private void Init(WarResource data)
		{
			this.data = data;

			MenuEntry me = null;
			Texts = new List<MenuEntry>();

			int offset = data.data[22] + (data.data[23] >> 8) + (data.data[23] >> 16) + (data.data[23] >> 24);

			int off2 = offset;

			while (true)
			{
				while (data.data[off2] != 0x00)
					off2++;

				if (off2 == offset)
					break;

				StringBuilder sb = new StringBuilder(off2 - offset);
				for (int i = offset; i < off2; i++)
					sb.Append((char)data.data[i]);

				me = new MenuEntry();
				me.Text = sb.ToString();
				Texts.Add(me);

				off2++;
				offset = off2;
			}

			// Bytes überspringen bis wir zu den Koordinaten kommen
			while (data.data[offset] != 0xff)
				offset++;

			while (data.data[offset] == 0xff)
				offset++;

			while (data.data[offset] != 0xff)
				offset++;

			offset += 16;

			bool hasTitle = false;

			if (data.data[offset] == 0xff)
			{
				offset += 2;
			}
			else
			{
				hasTitle = true;

				offset += 2;

				me = Texts[0];
				me.X = (ushort)(data.data[offset] + (data.data[offset + 1] >> 8));
				offset += 2;
				me.Y = (ushort)(data.data[offset] + (data.data[offset + 1] >> 8));
				offset += 2;
				me.ButtonID = data.data[offset] + (data.data[offset + 1] >> 8)
								 + (data.data[offset + 2] >> 16) + (data.data[offset + 3] >> 24);
				offset += 4;

				me.unknown1 = 0;
				me.unknown3 = 0;
				me.unknown4 = 0;
				me.unknown5 = 0;

				offset += 2;
			}

			offset += 4;

			for (int i = 0; i < Texts.Count; i++)
			{
				if ((hasTitle) && (i == 0))
					continue;

				me = Texts[i];
				me.X = (ushort)(data.data[offset] + (data.data[offset + 1] >> 8));
				offset += 2;
				me.Y = (ushort)(data.data[offset] + (data.data[offset + 1] >> 8));
				offset += 2;

				offset += 2;
				me.unknown4 = data.data[offset] + (data.data[offset + 1] >> 8)
								 + (data.data[offset + 2] >> 16) + (data.data[offset + 3] >> 24);
				offset += 4;
				me.unknown5 = data.data[offset] + (data.data[offset + 1] >> 8)
								 + (data.data[offset + 2] >> 16) + (data.data[offset + 3] >> 24);
				offset += 4;

				me.unknown1 = data.data[offset] + (data.data[offset + 1] >> 8)
								 + (data.data[offset + 2] >> 16) + (data.data[offset + 3] >> 24);
				offset += 4;
				me.ButtonID = data.data[offset] + (data.data[offset + 1] >> 8)
								 + (data.data[offset + 2] >> 16) + (data.data[offset + 3] >> 24);
				offset += 4;
				me.unknown3 = data.data[offset] + (data.data[offset + 1] >> 8);
				offset += 4;
				offset += 2;
			}
		}
		#endregion
	}
}
