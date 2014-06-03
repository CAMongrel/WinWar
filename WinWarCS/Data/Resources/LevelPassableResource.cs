using System;
using System.Collections.Generic;
using System.Text;

namespace WinWarCS.Data.Resources
{
	internal class LevelPassableResource : BasicResource
	{
		short[] passableData;

        internal LevelPassableResource(string res_name)
        {
            this.data = WarFile.GetResourceByName(res_name);

            CreatePassableData();
        }
	
		/// <summary>
		/// Create level visual resource
		/// </summary>
		internal LevelPassableResource(WarResource setData)
		{
			this.data = setData;
			
			CreatePassableData();
		} // LevelVisualResource(setData, setOffset)
		
		/// <summary>
		/// Create visual data
		/// </summary>
		void CreatePassableData()
		{
			// Levels are always 64 x 64 in size
			passableData = new short[64 * 64];
			
			if (data.data != null)
			{
				unsafe 
				{
					fixed (byte* org_ptr = &data.data[0])
					{
						short* ptr = (short*)org_ptr;
						
						for (int y = 0; y < 64; y++)
                        {
                        	for (int x = 0; x < 64; x++)
                            {
								passableData[x + y * 64] = *ptr;
                            	ptr++;
                            }
                        }
					} // fixed
				} // fixed
			} // if
		} // CreateVisualData()
		
		/// <summary>
		/// Destroy visual data
		/// </summary>
		internal void DestroyPassableData()
		{
		} // DestroyVisualData()
	}
}
