using System;

namespace WinWarCS.Data.Resources
{
   public class RawResource : BasicResource
   {
      internal WarResource Resource;

      internal RawResource(WarResource setResource)
      {
         Type = ContentFileType.FileUnknown;

         Resource = setResource;
      }

      internal override void WriteToStream(System.IO.BinaryWriter writer)
      {
         base.WriteToStream(writer);

         writer.Write(Resource.data);
      }
   }
}

