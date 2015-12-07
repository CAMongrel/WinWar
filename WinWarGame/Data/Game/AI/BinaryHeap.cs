using System;
using System.Collections.Generic;
using System.Text;

namespace WinWarCS.Data.Game
{
	internal class BinaryHeap<T> : IEnumerable<T>
	{
		int[] values;
		T[] objects;
		int cItems;
		int size;

		internal BinaryHeap(int size)
		{
			this.size = size;
			cItems = 0;
			values = new int[size];
			objects = new T[size];
			int i;
			for (i = 0; i < size; i++)
			{
				values[i] = -1;
				objects[i] = default(T);
			}
		}

		public void Clear()
		{
			int i;
			for (i = 0; i <= cItems; i++)
			{
				values[i] = -1;
				if (objects[i] is IDisposable)
					((IDisposable)objects[i]).Dispose();
				objects[i] = default(T);
			}
			cItems = 0;
		}

		public int Count
		{
			get { return cItems; }
		}
		public T this[int index]
		{
			get
			{
				if (index < 0 || index >= cItems)
					return default(T);

				return objects[index + 1];
			}
		}

		public void Add(int value, T data)
		{
			if (cItems == 0)
			{
				values[1] = value;
				objects[1] = data;
				cItems++;
			}
			else
			{
				//				values[cItems] = value;

				int curIdx = cItems + 1;
				int parent = (cItems + 1) / 2;
				while (parent != 0)
				{
					if (values[parent] > value)
					{
						values[curIdx] = values[parent];
						objects[curIdx] = objects[parent];
						values[parent] = value;
						objects[parent] = data;
					}
					else
					{
						values[curIdx] = value;
						objects[curIdx] = data;
						break;
					}

					curIdx = parent;
					parent = parent / 2;
				}

				cItems++;
			}
		}

		private void ResortDownFrom(int startIdx)
		{
			int curIdx = startIdx;
			int swapChild = startIdx;

			while (values[curIdx] != -1 && curIdx * 2 < size)
			{
				int curVal = values[curIdx];

				if (curIdx * 2 + 1 <= cItems)
				{
					int child1 = values[curIdx * 2];
					int child2 = values[curIdx * 2 + 1];

					if (curVal >= child1)
						swapChild = curIdx * 2;
					if (values[swapChild] >= child2)
						swapChild = curIdx * 2 + 1;
				}
				else
				{
					if (curIdx * 2 <= cItems)
					{
						int child1 = values[curIdx * 2];
						if (curVal >= child1)
							swapChild = curIdx * 2;
					}
				}

				if (curIdx == swapChild)
					break;

				int tmp = values[swapChild];
				T tmp2 = objects[swapChild];
				values[swapChild] = values[curIdx];
				objects[swapChild] = objects[curIdx];
				values[curIdx] = tmp;
				objects[curIdx] = tmp2;

				curIdx = swapChild;
			}
		}

		private void ResortUpFrom(int startIdx)
		{
			int curIdx = startIdx;
			int parent = startIdx / 2;

			int value = values[startIdx];
			T data = objects[startIdx];

			while (parent != 0)
			{
				if (values[parent] > value)
				{
					values[curIdx] = values[parent];
					objects[curIdx] = objects[parent];
					values[parent] = value;
					objects[parent] = data;
				}
				else
					break;

				curIdx = parent;
				parent = parent / 2;
			}
		}

		public T Remove()
		{
			T res = objects[1];

			values[1] = values[cItems];
			objects[1] = objects[cItems];
			values[cItems] = -1;
			objects[cItems] = default(T);
			cItems--;

			ResortDownFrom(1);

			return res;
		}

		public void ChangeItem(int idx, int newValue, T data)
		{
			values[idx] = newValue;
			objects[idx] = data;

			ResortUpFrom(idx);
		}

#if !NETFX_CORE && !IOS
		public void Print()
		{
			int[] line_pos = new int[20];
			int i;
			for (i = 1; i <= cItems; i++)
			{
				int line_no = 0;
				int tmp = i / 2;
				while (tmp >= 1)
				{
					line_no++;
					tmp = tmp / 2;
				}
				Console.SetCursorPosition(line_pos[line_no] * 2, line_no);
				line_pos[line_no] += values[i].ToString().Length;
				Console.Write(values[i]);
				Console.Write(" ");
			}
			Console.WriteLine();
		}
#endif

        public IEnumerator<T> GetEnumerator()
		{
			for (int i = cItems; --i >= 1; )
			{
				yield return objects[i];
			}
		}

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		// Iterate from top to bottom.
		public IEnumerable<T> TopToBottom
		{
			get
			{
				// Since we implement IEnumerable<T>
				// and the default iteration is top to bottom,
				// just return the object.
				return this;
			}
		}

		// Iterate from bottom to top.
		public IEnumerable<T> BottomToTop
		{
			get
			{
				for (int i = 1; i <= cItems; i++)
				{
					yield return objects[i];
				}
			}
		}
	}
}
