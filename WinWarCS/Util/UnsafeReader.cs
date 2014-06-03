using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace WinWarCS.Util
{
	internal unsafe class UnsafeReader : IDisposable
	{
		GCHandle handle;

		internal UnsafeReader(byte[] data)
		{
			handle = GCHandle.Alloc(data);			
		}

      public void Dispose()
		{
			if (handle != null && handle.IsAllocated)
				handle.Free();
		}
	}
}
