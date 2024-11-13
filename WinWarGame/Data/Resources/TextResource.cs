#region Using directives
using System;
using System.Collections.Generic;
using System.Text;
#endregion

namespace WinWarGame.Data.Resources
{
   public class TextResource : BasicResource
   {
      /// <summary>
      /// Gets or sets the text of this TextResource
      /// </summary>
      public string Text { get; set; }

      #region Constructor
      internal TextResource(WarResource data)
         : base(data)
      {
         Type = ContentFileType.FileText;

         if (data == null)
            throw new ArgumentNullException(nameof(data));

         Init(data);
      }
      #endregion

      #region Init
      private void Init(WarResource data)
      {
         Text = Encoding.ASCII.GetString(data.data);
      }
      #endregion
   }
}