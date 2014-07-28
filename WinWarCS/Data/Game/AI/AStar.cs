using System;
using System.Collections.Generic;
using System.Text;

namespace WinWarCS.Data.Game
{
	internal class Node
	{
      internal int X;
      internal int Y;

      internal int G;
      internal int H;

      internal int F
		{
			get { return G + H; }
		}

      internal Node parent;
	}

	internal class AStar2D
	{
		private static byte[] SQRT = new byte[] { 0, 10, 14 };

      private short[,] field = null;
      private int width;
      private int height;

		internal AStar2D()
		{
			width = 0;
			height = 0;
			field = null;
		}

      #region Field setup
		public void SetField(short[,] field, int width, int height)
		{
			this.field = field;
			this.width = width;
			this.height = height;
		}

		public void SetEmptyField(int width, int height)
		{
         this.field = new short[height, width];
			this.width = width;
			this.height = height;
		}
      #endregion

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
      private Node GetClosedNode(int nodeX, int nodeY, List<Node> Closed)
		{
         for (int i = 0; i < Closed.Count; i++) 
         {
            Node n = Closed[i];
            if (n.X == nodeX && n.Y == nodeY)
               return n;
         }

			return null;
		}

      private Node GetOpenHeapNode(int nodeX, int nodeY, BinaryHeap<Node> OpenHeap, out int index)
		{
			index = -1;
			int i;
			for (i = 0; i < OpenHeap.Count; i++)
			{
				Node n = OpenHeap[i];

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

      private sbyte ProcessLowestNode(int endX, int endY, List<Node> Closed, BinaryHeap<Node> OpenHeap)
		{
			Node node = OpenHeap.Remove();
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

					if (newX < 0 || newX >= width || newY < 0 || newY >= height)
						continue;

               if (field[newX, newY] > 0 || GetClosedNode(newX, newY, Closed) != null)
						continue;

					int index = 0;
               Node o_node = GetOpenHeapNode(newX, newY, OpenHeap, out index);
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

						Node n = new Node();
						n.X = node.X + x;
						n.Y = node.Y + y;
						n.parent = node;

						n.G = node.G + SQRT[x * x + y * y];
						int offX = Math.Abs(n.X - endX);
						int offY = Math.Abs(n.Y - endY);
						if (offX > offY)
							n.H = 14 * offY + 10 * (offX - offY);
						else
							n.H = 14 * offX + 10 * (offY - offX);
						n.H = (Math.Abs(n.X - endX) + Math.Abs(n.Y - endY)) * 10;

						OpenHeap.Add(n.F, n);
					}
				}
			}

			return 0;
		}

      public MapPath FindPath(int startX, int startY, int endX, int endY)
		{
			if (field == null)
				return null;

         List<Node> Closed = new List<Node>();
         BinaryHeap<Node> OpenHeap = new BinaryHeap<Node>(width * height);
         Node Root;

			Root = new Node();
			Root.parent = null;
			Root.X = startX;
			Root.Y = startY;
			Root.G = 0;
			int offX = Math.Abs(Root.X - endX);
			int offY = Math.Abs(Root.Y - endY);
			if (offX > offY)
				Root.H = 14 * offY + 10 * (offX - offY);
			else
				Root.H = 14 * offX + 10 * (offY - offX);
			OpenHeap.Add(Root.F, Root);

         int res = ProcessLowestNode (endX, endY, Closed, OpenHeap);
			while (res == 0)
			{
				res = ProcessLowestNode(endX, endY, Closed, OpenHeap);
			}

			if (res == -1)
				return null;

         Node node = GetClosedNode(endX, endY, Closed);
         MapPath result = new MapPath ();
         result.BuildFromFinalNode (node);

         return result;
		}
      #endregion

      #region Properties
      public short this[int x, int y]
		{
			get { return field[y, x]; }
			set { field[y, x] = value; }
		}
		public int Width
		{
			get { return width; }
		}
		public int Height
		{
			get { return height; }
		}
      #endregion
	}
}
