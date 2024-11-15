using System;
using System.Collections.Generic;
using System.Text;
using WinWarGame.Data.Resources;

namespace WinWarGame.Data.Game
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

        private bool useHeuristic;

        internal AStar2D()
        {
            useHeuristic = true;
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

        #region Pathfinding

        private AStarNode GetClosedNode(int nodeX, int nodeY, List<AStarNode> Closed)
        {
            for (int i = 0; i < Closed.Count; i++)
            {
                AStarNode n = Closed[i];
                if (n.X == nodeX && n.Y == nodeY)
                {
                    return n;
                }
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
                {
                    continue;
                }

                if (n.X == nodeX && n.Y == nodeY)
                {
                    index = i;
                    return n;
                }
            }

            return null;
        }

        private sbyte ProcessLowestNode(int endX, int endY, List<AStarNode> closed, BinaryHeap<AStarNode> openHeap)
        {
            AStarNode node = openHeap.Remove();
            closed.Add(node);

            if (node == null)
            {
                return -1;
            }

            if (node.X == endX && node.Y == endY)
            {
                return 1;
            }

            int x, y;

            for (y = -1; y <= 1; y++)
            {
                for (x = -1; x <= 1; x++)
                {
                    if (x == 0 && y == 0)
                    {
                        continue;
                    }

                    int newX = node.X + x;
                    int newY = node.Y + y;

                    if (newX < 0 || newX >= fieldWidth || newY < 0 || newY >= fieldHeight)
                    {
                        continue;
                    }

                    if (field[newX, newY] > 0 && newX == endX && newY == endY)
                    {
                        return 2;
                    }

                    if (field[newX, newY] > 0 || GetClosedNode(newX, newY, closed) != null)
                    {
                        continue;
                    }

                    int index = 0;
                    AStarNode oNode = GetOpenHeapNode(newX, newY, openHeap, out index);
                    if (oNode != null)
                    {
                        // Node is already in the open list
                        int g = node.G + SQRT[x * x + y * y];

                        if (g < oNode.G)
                        {
                            oNode.parent = node;
                            oNode.G = g;

                            openHeap.ChangeItem(index, oNode.F, oNode);
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
                        if (useHeuristic == false)
                        {
                            n.H = 0;
                        }
                        else
                        {
                            if (offX > offY)
                            {
                                n.H = 14 * offY + 10 * (offX - offY);
                            }
                            else
                            {
                                n.H = 14 * offX + 10 * (offY - offX);
                            }

                            n.H = (Math.Abs(n.X - endX) + Math.Abs(n.Y - endY)) * 10;
                        }

                        openHeap.Add(n.F, n);
                    }
                }
            }

            return 0;
        }

        public MapPath FindPath(int startX, int startY, int endX, int endY, bool setUseHeuristic = true)
        {
            if (field == null)
            {
                return null;
            }

            useHeuristic = setUseHeuristic;

            List<AStarNode> closed = new List<AStarNode>();
            BinaryHeap<AStarNode> openHeap = new BinaryHeap<AStarNode>(fieldWidth * fieldHeight);

            AStarNode root = new AStarNode
            {
                parent = null,
                X = startX,
                Y = startY,
                G = 0
            };
            int offX = Math.Abs(root.X - endX);
            int offY = Math.Abs(root.Y - endY);
            if (useHeuristic == false)
            {
                root.H = 0;
            }
            else
            {
                if (offX > offY)
                {
                    root.H = 14 * offY + 10 * (offX - offY);
                }
                else
                {
                    root.H = 14 * offX + 10 * (offY - offX);
                }
            }

            openHeap.Add(root.F, root);

            int res = ProcessLowestNode(endX, endY, closed, openHeap);
            while (res == 0)
            {
                res = ProcessLowestNode(endX, endY, closed, openHeap);
            }

            if (res == -1)
            {
                return null;
            }

            AStarNode node = res switch
            {
                1 => GetClosedNode(endX, endY, closed),
                2 => closed[^1],
                _ => null
            };

            MapPath result = new MapPath();
            result.BuildFromFinalAStarNode(node);

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