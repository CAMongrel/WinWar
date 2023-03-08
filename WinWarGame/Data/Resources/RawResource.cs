using System;

namespace WinWarGame.Data.Resources
{
   public class RawResource : BasicResource
   {
      internal WarResource Resource;

      internal RawResource(WarResource setResource, ContentFileType fileType)
      {
         Type = fileType;

         Resource = setResource;
      }

      internal override void WriteToStream(System.IO.BinaryWriter writer)
      {
         base.WriteToStream(writer);

         writer.Write(Resource.data);
      }
   }
}

