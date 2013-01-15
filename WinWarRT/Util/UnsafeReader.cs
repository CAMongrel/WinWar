using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace WinWarRT.Util
{
	public unsafe class UnsafeReader : IDisposable
	{
		GCHandle handle;

		public UnsafeReader(byte[] data)
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
