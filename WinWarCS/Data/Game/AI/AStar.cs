using System;
using System.Collections.Generic;
using System.Text;

namespace WinWarCS.Data.Game
{
	internal class Node
	{
		public int X;
		public int Y;

		public int G;
		public int H;

		public int F
		{
			get { return G + H; }
		}

		public Node parent;
	}

	internal class AStar2D
	{
		static byte[] SQRT = new byte[] { 0, 10, 14 };

		short[,] field = null;
		int startX;
		int startY;
		int endX;
		int endY;
		int width;
		int height;
		int steps;

		//List<Node> Open;
		List<Node> Closed;
		List<Node> Path;
		BinaryHeap<Node> OpenHeap;

		Node Root;

		internal AStar2D()
		{
			//Open = null;
			Closed = null;
			Path = null;
			OpenHeap = null;

			steps = 0;
			width = 0;
			height = 0;
			field = null;
			startX = 0;
			startY = 0;
			endX = 0;
			endY = 0;
		}

		public void SetField(short[,] field, int width, int height)
		{
			this.field = field;
			this.width = width;
			this.height = height;

			//Open = new List<Node>();
			Closed = new List<Node>();
			Path = new List<Node>();
			OpenHeap = new BinaryHeap<Node>(width * height);
		}

		public void SetEmptyField(int width, int height)
		{
         this.field = new short[height, width];
			this.width = width;
			this.height = height;

			//Open = new List<Node>();
			Closed = new List<Node>();
			Path = new List<Node>();
			OpenHeap = new BinaryHeap<Node>(width * height);
		}

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

		Node GetClosedNode(int nodeX, int nodeY)
		{
			foreach (Node n in Closed)
				if (n.X == nodeX && n.Y == nodeY)
					return n;

			return null;
		}

		Node GetOpenHeapNode(int nodeX, int nodeY, out int index)
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

		sbyte ProcessLowestNode()
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

					if (field[newY, newX] == 1 || GetClosedNode(newX, newY) != null)
						continue;

					int index = 0;
					Node o_node = GetOpenHeapNode(newX, newY, out index);
					if (o_node != null)
					{
						// Node ist bereits in der offenen Liste
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
						// Node ist NICHT in der offenen Liste

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

		public bool FindPath()
		{
			if (field == null)
				return false;

			//Open.Clear();
			Closed.Clear();
			Path.Clear();
			OpenHeap.Clear();

			steps = 0;

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
			//Root.H = Math.Abs(Root.X - endX) + Math.Abs(Root.Y - endY);
			OpenHeap.Add(Root.F, Root);
			//Open.Add(Root);
			//ProcessNode(Root);

			int res = ProcessLowestNode();
			//int res = ProcessLowestF();
			while (res == 0)
			{
				steps++;
				//res = ProcessLowestF();
				res = ProcessLowestNode();
			}

			if (res == -1)
				return false;

			Node node = GetClosedNode(endX, endY);
			Path.Add(node);
			while (node.parent != null)
			{
				Path.Add(node);
				node = node.parent;
			}
			Path.Reverse();

			//OpenHeap.Clear();
			//Closed.Clear();

			return true;
		}

		public Node GetPathNode(int index)
		{
			if (index < 0 || index >= PathNodeCount)
				return null;

			return Path[index];
		}

      public short this[int x, int y]
		{
			get { return field[y, x]; }
			set { field[y, x] = value; }
		}
		public int Steps
		{
			get { return steps; }
		}
		public int Width
		{
			get { return width; }
		}
		public int Height
		{
			get { return height; }
		}
		public int StartX
		{
			get { return startX; }
			set { startX = value; }
		}
		public int StartY
		{
			get { return startY; }
			set { startY = value; }
		}
		public int EndX
		{
			get { return endX; }
			set { endX = value; }
		}
		public int EndY
		{
			get { return endY; }
			set { endY = value; }
		}
		public int PathNodeCount
		{
			get { return Path.Count; }
		}
	}
}

#region Old code
/*		static Node GetOpenNode(int nodeX, int nodeY)
		{
			foreach (Node n in Open)
				if (n.X == nodeX && n.Y == nodeY)
					return n;

			return null;
		}*/

/*		static sbyte ProcessNode(Node node)
		{
			Open.Remove(node);
			Closed.Add(node);

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

					if (newX < 0 || newX >= Width || newY < 0 || newY >= Height)
						continue;

					if (field[newY, newX] == 1 || GetClosedNode(newX, newY) != null)
						continue;

					Node o_node = GetOpenNode(newX, newY);
					if (o_node != null)
					{
						// Node ist bereits in der offenen Liste
//						int G = node.G + SQRT[(Math.Abs(x) + Math.Abs(y))];
						int G = node.G + SQRT[x * x + y * y];						

						if (G < o_node.G)
						{
							o_node.parent = node;
							o_node.G = G;
						}
					}
					else
					{
						// Node ist NICHT in der offenen Liste

						Node n = new Node();
						n.X = node.X + x;
						n.Y = node.Y + y;
						n.parent = node;

//						n.G = node.G + SQRT[(Math.Abs(x) + Math.Abs(y))];
						n.G = node.G + SQRT[x * x + y * y];
						n.H = (Math.Abs(n.X - endX) + Math.Abs(n.Y - endY)) * 10;

						Open.Add(n);
					}
				}
			}

			return 0;
		}

		static sbyte ProcessLowestF()
		{
			int i, lowest;
			Node CurNode = null;
			lowest = int.MaxValue;
			for (i = 0; i < Open.Count; i++)
			{
				if (Open[i].F < lowest)
				{
					CurNode = Open[i];
					lowest = Open[i].F;
				}
			}
			if (CurNode == null)
				return -1;

			Steps++;
			return ProcessNode(CurNode);
		}*/

/*public void PrintField()
{
	if (field == null)
		return;

	int x, y;

	Console.Clear();
	Console.SetCursorPosition(0, 0);

	for (y = 0; y < height; y++)
	{
		for (x = 0; x < width; x++)
		{
			Console.Write(field[y, x] == 1 ? "X" : ".");
		}
		Console.WriteLine();
	}

	Console.SetCursorPosition(startX, startY);
	Console.Write("S");
	Console.SetCursorPosition(endX, endY);
	Console.Write("E");
	Console.SetCursorPosition(0, height + 1);

	foreach (Node n in OpenHeap)
	{
		Console.SetCursorPosition(n.X, n.Y + height + 1);
		Console.Write("O");
	}
	foreach (Node n in Closed)
	{
		if (n == null)
			continue;

		Console.SetCursorPosition(n.X, n.Y + height + 1);
		Console.Write("C");
	}
	foreach (Node n in Path)
	{
		Console.SetCursorPosition(n.X, n.Y + height + 1);
		Console.Write("P");
	}
}*/
#endregion