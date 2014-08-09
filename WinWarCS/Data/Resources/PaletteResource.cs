using System;

namespace WinWarCS.Data.Resources
{
   internal class PaletteResource : BasicResource
   {
      internal byte[] Colors;

      internal PaletteResource(WarResource setData)
         : base(setData)
      {
         Type = ContentFileType.FilePalette;

         Colors = setData.data;
      }
   }
}

