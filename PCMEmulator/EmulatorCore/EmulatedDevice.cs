using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PCMEmulator
{
    public abstract class EmulatedDevice
    {
        internal Emulator Emulator { get; set; }

        public abstract void Init();

        public abstract sbyte ReadOpByte(uint address);

        public abstract sbyte ReadByte(uint address);

        public abstract short ReadOpWord(uint address);

        public abstract short ReadWord(uint address);

        public abstract int ReadOpLong(uint address);

        public abstract int ReadLong(uint address);

        public abstract void WriteByte(uint address, sbyte value);

        public abstract void WriteWord(uint address, short value);

        public abstract void WriteLong(uint address, uint value);
    }
}
