using System;
using System.Collections.Generic;
using System.Text;

namespace WinWarCS.Data.Game
{
   internal class AStarNode : IMapPathNode
	{
      public int X { get; set; }
      public int Y { get; set; }

      internal int G;
      internal int H;

      internal int F
		{
			get { return G + H; }
		}

      internal AStarNode parent;
	}

	internal class AStar2D
	{
		private static byte[] SQRT = new byte[] { 0, 10, 14 };

      private short[,] field = null;
      private int fieldWidth;
      private int fieldHeight;

      internal bool UseHeuristic;

		internal AStar2D()
		{
         UseHeuristic = true;
			fieldWidth = 0;
			fieldHeight = 0;
			field = null;
		}

      #region Field setup
		public void SetField(short[,] field, int width, int height)
		{
			this.field = field;
			this.fieldWidth = width;
			this.fieldHeight = height;
		}

		public void SetEmptyField(int width, int height)
		{
         this.field = new short[width, height];
			this.fieldWidth = width;
			this.fieldHeight = height;
		}
      #endregion

      private void SetFieldsValue(int x, int y, int width, int height, short value)
      {
         if (x >= fieldWidth || x < 0 ||
            y >= fieldHeight || y < 0 ||
            width < 1 || height < 1)
         {
            // Invalid placement
            return;
         }

         if (x + width >= fieldWidth)
         {
            width = fieldWidth - x;
         }
         if (y + height >= fieldHeight)
         {
            height = fieldHeight - y;
         }

         for (int valY = y; valY < y + height; valY++)
         {
            for (int valX = x; valX < x + width; valX++)
            {
               field[valX, valY] = value;
            }
         }
      }

      internal void SetFieldsBlocked(int x, int y, int width, int height)
      {
         SetFieldsValue(x, y, width, height, 64);
      }

      internal void SetFieldsFree(int x, int y, int width, int height)
      {
         SetFieldsValue(x, y, width, height, 0);
      }

#if BLA //!NETFX_CORE
		public void PrintFieldFile(string filename)
		{
			if (field == null)
				return;

			System.IO.StreamWriter sw = System.IO.File.CreateText(filename);

			int x, y;

			for (y = 0; y < height; y++)
			{
				for (x = 0; x < width; x++)
				{
					if (x == startX && y == startY)
						sw.Write("S");
					else
					if (x == endX && y == endY)
						sw.Write("E");
					else
						sw.Write(field[y, x] == 1 ? "X" : ".");
				}
				sw.WriteLine();
			}

			sw.WriteLine();

			for (y = 0; y < height; y++)
			{
				for (x = 0; x < width; x++)
				{
					bool bFound = false;
					foreach (Node n in Path)
					{
						if (x == n.X && y == n.Y)
						{
							sw.Write("P");
							bFound = true;
							break;
						}
					}
					if (bFound)
						continue;
					foreach (Node n in Closed)
					{
						if (n == null)
							continue;

						if (x == n.X && y == n.Y)
						{
							sw.Write("C");
							bFound = true;
							break;
						}
					}
					if (bFound)
						continue;
					foreach (Node n in OpenHeap)
					//foreach (Node n in Open)
					{
						if (x == n.X && y == n.Y)
						{
							sw.Write("O");
							bFound = true;
							break;
						}
					}
					if (bFound)
						continue;
					sw.Write(" ");
				}
				sw.WriteLine();
			}
			
			sw.Close();
		}
#endif

      #region Pathfinding
      private AStarNode GetClosedNode(int nodeX, int nodeY, List<AStarNode> Closed)
		{
         for (int i = 0; i < Closed.Count; i++) 
         {
            AStarNode n = Closed[i];
            if (n.X == nodeX && n.Y == nodeY)
               return n;
         }

			return null;
		}

      private AStarNode GetOpenHeapNode(int nodeX, int nodeY, BinaryHeap<AStarNode> OpenHeap, out int index)
		{
			index = -1;
			int i;
			for (i = 0; i < OpenHeap.Count; i++)
			{
				AStarNode n = OpenHeap[i];

				if (n == null)
					continue;

				if (n.X == nodeX && n.Y == nodeY)
				{
					index = i;
					return n;
				}
			}

			return null;
		}

      private sbyte ProcessLowestNode(int endX, int endY, List<AStarNode> Closed, BinaryHeap<AStarNode> OpenHeap)
		{
			AStarNode node = OpenHeap.Remove();
			Closed.Add(node);

			if (node == null)
				return -1;

			if (node.X == endX && node.Y == endY)
				return 1;

			int x, y;

			for (y = -1; y <= 1; y++)
			{
				for (x = -1; x <= 1; x++)
				{
					if (x == 0 && y == 0)
						continue;

					int newX = node.X + x;
					int newY = node.Y + y;

					if (newX < 0 || newX >= fieldWidth || newY < 0 || newY >= fieldHeight)
						continue;

               if (field[newX, newY] > 0 && newX == endX && newY == endY)
               {
                  return 2;
               }

               if (field[newX, newY] > 0 || GetClosedNode(newX, newY, Closed) != null)
						continue;

					int index = 0;
               AStarNode o_node = GetOpenHeapNode(newX, newY, OpenHeap, out index);
					if (o_node != null)
					{
						// Node is already in the open list
						int G = node.G + SQRT[x * x + y * y];

						if (G < o_node.G)
						{
							o_node.parent = node;
							o_node.G = G;

							OpenHeap.ChangeItem(index, o_node.F, o_node);
						}
					}
					else
					{
						// Node is NOT in the open list

						AStarNode n = new AStarNode();
						n.X = node.X + x;
						n.Y = node.Y + y;
						n.parent = node;

						n.G = node.G + SQRT[x * x + y * y];
						int offX = Math.Abs(n.X - endX);
						int offY = Math.Abs(n.Y - endY);
                  if (UseHeuristic == false)
                  {
                     n.H = 0;
                  }
                  else
                  {
                     if (offX > offY)
                        n.H = 14 * offY + 10 * (offX - offY);
                     else
                        n.H = 14 * offX + 10 * (offY - offX);
                     n.H = (Math.Abs(n.X - endX) + Math.Abs(n.Y - endY)) * 10;
                  }

						OpenHeap.Add(n.F, n);
					}
				}
			}

			return 0;
		}

      public MapPath FindPath(int startX, int startY, int endX, int endY, bool useHeuristic = true)
      {
			if (field == null)
				return null;

         UseHeuristic = useHeuristic;

         List<AStarNode> Closed = new List<AStarNode>();
         BinaryHeap<AStarNode> OpenHeap = new BinaryHeap<AStarNode>(fieldWidth * fieldHeight);
         AStarNode Root;

			Root = new AStarNode();
			Root.parent = null;
			Root.X = startX;
			Root.Y = startY;
			Root.G = 0;
			int offX = Math.Abs(Root.X - endX);
			int offY = Math.Abs(Root.Y - endY);
         if (UseHeuristic == false)
         {
            Root.H = 0;
         }
         else
         {
            if (offX > offY)
               Root.H = 14 * offY + 10 * (offX - offY);
            else
               Root.H = 14 * offX + 10 * (offY - offX);
         }
			OpenHeap.Add(Root.F, Root);

         int res = ProcessLowestNode (endX, endY, Closed, OpenHeap);
			while (res == 0)
			{
				res = ProcessLowestNode(endX, endY, Closed, OpenHeap);
			}

			if (res == -1)
				return null;

         AStarNode node = null;
         if (res == 1)
            node = GetClosedNode(endX, endY, Closed);
         else if (res == 2)
            node = Closed[Closed.Count - 1];

         MapPath result = new MapPath ();
         result.BuildFromFinalAStarNode (node);

         return result;
		}
      #endregion

      #region Properties
      public short this[int x, int y]
		{
			get { return field[x, y]; }
			set { field[x, y] = value; }
		}
		public int Width
		{
			get { return fieldWidth; }
		}
		public int Height
		{
			get { return fieldHeight; }
		}
      #endregion
	}
}
