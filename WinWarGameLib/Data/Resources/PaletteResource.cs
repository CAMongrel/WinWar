using System;

namespace WinWarCS.Data.Resources
{
   public class PaletteResource : BasicResource
   {
      internal byte[] Colors;

      internal PaletteResource(WarResource setData)
      {
         Type = ContentFileType.FilePalette;

         Colors = setData.data;
      }
   }
}

