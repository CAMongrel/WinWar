#if WINFX_CORE
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinWarCS.Platform.WindowsRT
{
    struct RectangleF
    {
       public static RectangleF Empty = new RectangleF(0, 0, 0, 0);

        public float X;
        public float Y;
        public float Width;
        public float Height;

        public RectangleF(float setX, float setY, float setWidth, float setHeight)
        {
            X = setX;
            Y = setY;
            Width = setWidth;
            Height = setHeight;
        }
    }
}
#endif