using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace PCMEmulator
{
    /// <summary>
    /// Register
    /// </summary>
    [StructLayout(LayoutKind.Explicit)]
    public struct Register
    {
        [FieldOffset(0)]
        public uint U32;

        [FieldOffset(0)]
        public int S32;

        [FieldOffset(0)]
        public ushort U16;

        [FieldOffset(0)]
        public short S16;

        [FieldOffset(0)]
        public byte U8;

        [FieldOffset(0)]
        public sbyte S8;

        /// <summary>
        /// ToString
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("{0:X8}", U32);
        }
    }
}
