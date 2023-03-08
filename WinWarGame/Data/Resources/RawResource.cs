using System;

namespace WinWarGame.Data.Resources
{
   public class RawResource : BasicResource
   {
      internal RawResource(WarResource setResource, ContentFileType fileType)
         : base(setResource)
      {
         Type = fileType;
      }
   }
}

