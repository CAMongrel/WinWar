using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinWarCS.Platform.WindowsRT
{
    struct RectangleF
    {
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
