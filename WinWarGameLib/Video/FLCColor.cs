using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace FLCLib
{
    [StructLayout(LayoutKind.Explicit)]
    public struct FLCColor
    {
        [FieldOffset(0)]
        public byte R;
        [FieldOffset(1)]
        public byte G;
        [FieldOffset(2)]
        public byte B;
        [FieldOffset(3)]
        public byte A;

        public FLCColor(byte setR, byte setG, byte setB, byte setA)
        {
            R = setR;
            G = setG;
            B = setB;
            A = setA;
        }

        public override string ToString()
        {
            return string.Format("R: {0}, G: {1}, B: {2}, A: {3}", R, G, B, A);
        }
    }
}
