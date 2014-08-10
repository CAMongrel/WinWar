using System;

namespace WinWarCS.Data.Resources
{
   internal class UnknownResource : BasicResource
   {
      internal WarResource Resource;

      internal UnknownResource(WarResource setResource)
      {
         Type = ContentFileType.FileUnknown;

         Resource = setResource;
      }
   }
}

