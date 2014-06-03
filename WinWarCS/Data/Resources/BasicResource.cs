using System;
using System.Collections.Generic;
using System.Text;

namespace WinWarRT.Data.Resources
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
