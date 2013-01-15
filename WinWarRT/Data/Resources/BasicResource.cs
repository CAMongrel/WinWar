using System;
using System.Collections.Generic;
using System.Text;

namespace WinWarRT.Data.Resources
{
	public abstract class BasicResource
	{
		public WarResource data;

		protected BasicResource()
		{
			this.data = null;
		}
	}
}
