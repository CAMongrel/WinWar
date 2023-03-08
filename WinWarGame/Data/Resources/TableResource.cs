using System;

namespace WinWarGame.Data.Resources
{
   public class TableResource : BasicResource
   {
      internal ushort Rows { get; private set; }
      internal ushort Columns { get; private set; }

      private ushort[,] tableData;

      internal ushort this[int col, int row]
      {
         get
         {
            if (col < 0 || col >= Columns)
               return 0;
            if (row < 0 || row >= Rows)
               return 0;

            return tableData[col, row];
         }
      }

      #region Constructor
      internal TableResource(WarResource data)
      {
         Type = ContentFileType.FileTable;

         if (data == null)
            throw new ArgumentNullException("data");

         Init(data);
      }
      #endregion

      #region Init
      private void Init(WarResource data)
      {
         Columns = (ushort)(data.data[0] + (data.data[1] << 8));
         Rows = (ushort)(data.data[2] + (data.data[3] << 8));

         tableData = new ushort[Columns, Rows];

         int offset = 4;
         for (int y = 0; y < Rows; y++)
         {
            for (int x = 0; x < Columns; x++)
            {
               tableData[x, y] = (ushort)(data.data[offset] + (data.data[offset + 1] << 8));

               offset += 2;
            }
         }
      }
      #endregion
   }
}

