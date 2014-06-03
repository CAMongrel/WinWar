using System;
using System.Collections.Generic;
using System.Text;

namespace WinWarCS.Data.Resources
{
	internal abstract class BasicResource
	{
		internal WarResource data;

		protected BasicResource()
		{
			this.data = null;
		}
	}
}
