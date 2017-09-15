using System;

namespace PCMEmulator
{
    public partial class Emulator
    {
        static readonly int[,] MoveCyclesBW = new int[12, 9]
        {
            { 4, 4, 8, 8, 8, 12, 14, 12, 16 },
			{ 4, 4, 8, 8, 8, 12, 14, 12, 16 },
			{ 8, 8, 12, 12, 12, 16, 18, 16, 20 },
			{ 8, 8, 12, 12, 12, 16, 18, 16, 20 },
			{ 10, 10, 14, 14, 14, 18, 20, 18, 22 },
			{ 12, 12, 16, 16, 16, 20, 22, 20, 24 },
			{ 14, 14, 18, 18, 18, 22, 24, 22, 26 },
			{ 12, 12, 16, 16, 16, 20, 22, 20, 24 },
			{ 16, 16, 20, 20, 20, 24, 26, 24, 28 },
			{ 12, 12, 16, 16, 16, 20, 22, 20, 24 },
			{ 14, 14, 18, 18, 18, 22, 24, 22, 26 },
			{ 8, 8, 12, 12, 12, 16, 18, 16, 20 }
        };

        static readonly int[,] MoveCyclesL = new int[12, 9]
        {
			{ 4, 4, 12, 12, 12, 16, 18, 16, 20 },
			{ 4, 4, 12, 12, 12, 16, 18, 16, 20 },
			{ 12, 12, 20, 20, 20, 24, 26, 24, 28 },
			{ 12, 12, 20, 20, 20, 24, 26, 24, 28 },
			{ 14, 14, 22, 22, 22, 26, 28, 26, 30 },
			{ 16, 16, 24, 24, 24, 28, 30, 28, 32 },
			{ 18, 18, 26, 26, 26, 30, 32, 30, 34 },
			{ 16, 16, 24, 24, 24, 28, 30, 28, 32 },
			{ 20, 20, 28, 28, 28, 32, 34, 32, 36 },
			{ 16, 16, 24, 24, 24, 28, 30, 28, 32 },
			{ 18, 18, 26, 26, 26, 30, 32, 30, 34 },
			{ 12, 12, 20, 20, 20, 24, 26, 24, 28 }
        };

        static readonly int[,] EACyclesBW = new int[8, 8]
        {
            { 0, 0, 0, 0, 0, 0, 0, 0 },
            { 0, 0, 0, 0, 0, 0, 0, 0 },
            { 4, 4, 4, 4, 4, 4, 4, 4 },
            { 4, 4, 4, 4, 4, 4, 4, 4 },
            { 6, 6, 6, 6, 6, 6, 6, 6 },
            { 8, 8, 8, 8, 8, 8, 8, 8 },
            { 10, 10, 10, 10, 10, 10, 10, 10 },
            { 8, 12, 8, 10, 4, 99, 99, 99 }
        };

        static readonly int[,] EACyclesL = new int[8, 8]
        {
            { 0, 0, 0, 0, 0, 0, 0, 0 },
            { 0, 0, 0, 0, 0, 0, 0, 0 },
            { 8, 8, 8, 8, 8, 8, 8, 8 },
            { 8, 8, 8, 8, 8, 8, 8, 8 },
            { 10, 10, 10, 10, 10, 10, 10, 10 },
            { 12, 12, 12, 12, 12, 12, 12, 12 },
            { 14, 14, 14, 14, 14, 14, 14, 14 },
            { 12, 16, 12, 14, 8, 99, 99, 99 }
        };

        static readonly int[] CyclesException = new int[0x30]
        {
            0x04, 0x04, 0x32, 0x32, 0x22, 0x26, 0x28, 0x22,
            0x22, 0x22, 0x04, 0x04, 0x04, 0x04, 0x04, 0x2C,
            0x04, 0x04, 0x04, 0x04, 0x04, 0x04, 0x04, 0x04,
            0x2C, 0x2C, 0x2C, 0x2C, 0x2C, 0x2C, 0x2C, 0x2C,
            0x22, 0x22, 0x22, 0x22, 0x22, 0x22, 0x22, 0x22,
            0x22, 0x22, 0x22, 0x22, 0x22, 0x22, 0x22, 0x22
        };

        void EXTB()
        {
            var opMode = (Op >> 6) & 0x7;
            var reg = Op & 0x7;

            if (opMode == 0x2)
            {
                if (D[reg].S8 < 0)
                {
                    D[reg].U16 |= 0xFF00;
                }
                else
                {
                    D[reg].U16 &= 0xFF;
                }
            }
            else if (opMode == 0x3)
            {
                if (D[reg].S16 < 0)
                {
                    D[reg].U32 |= 0xFFFF0000;
                }
                else
                {
                    D[reg].U32 &= 0xFFFF;
                }
            }
            else if (opMode == 0x7)
            {
                if (D[reg].S8 < 0)
                {
                    D[reg].U32 |= 0xFFFFFF00;
                }
                else
                {
                    D[reg].U32 &= 0xFF;
                }
            }
        }

        void TBLU()
        {
            var mode = (Op >> 3) & 0x7;
            var reg = Op & 0x7;
            var next = _ReadOpWord(PC); PC += 2;
            var dreg = (next >> 12) & 0x7;
            var r = (next & 0x400) > 0;
            var flag = (next & 0x100) > 0;
            var size = (next >> 6) & 0x3;
            var dyn = next & 0x7;

            long val1 = 0, val2 = 0;
            if (dyn == 0)
            {
                //Get the table address
                var addr = ReadAddress(mode, reg);
                var offset = (D[dreg].U8 >> 8) & 0xFF;

                //Get the two values
                switch (size)
                {
                    case 0x0:
                        {
                            val1 = _ReadByte((uint)(addr + offset));
                            val2 = _ReadByte((uint)(addr + offset + 1));
                            break;
                        }

                    case 0x1:
                        {
                            val1 = _ReadWord(addr + offset);
                            val2 = _ReadWord(addr + (2 * (offset + 1)));
                            break;
                        }

                    case 0x2:
                        {
                            val1 = _ReadLong(addr + offset);
                            val2 = _ReadLong(addr + (4 * (offset + 1)));
                            break;
                        }

                    default:
                        {
                            throw new InvalidOperationException("Not supported yet");
                        }
                }
            }
            else
            {
                switch (size)
                {
                    case 0x0:
                        {
                            val1 = D[dreg].U8;
                            val2 = D[dyn].U8;
                            break;
                        }

                    case 0x1:
                        {
                            val1 = D[dreg].U16;
                            val2 = D[dyn].U16;
                            break;
                        }

                    case 0x2:
                        {
                            val1 = D[dreg].U32;
                            val2 = D[dyn].U32;
                            break;
                        }

                    default:
                        {
                            throw new InvalidOperationException("Invalid size");
                        }
                }
            }

            switch (size)
            {
                case 0x0:
                    {
                        var ret = val1 + ((val2 - val1) * ((D[dreg].U8 & 0xFF) / 256.0));
                        if (r)
                        {
                            D[dreg].U8 = (byte)Math.Floor(ret);
                        }
                        else
                        {
                            D[dreg].U8 = (byte)Math.Round(ret);
                        }

                        break;
                    }

                case 0x1:
                    {
                        var ret = val1 + ((val2 - val1) * ((D[dreg].U8 & 0xFF) / 256.0));
                        if (r)
                        {
                            D[dreg].U16 = (byte)Math.Floor(ret);
                        }
                        else
                        {
                            D[dreg].U16 = (byte)Math.Round(ret);
                        }

                        break;
                    }

                case 0x2:
                    {
                        var ret = val1 + ((val2 - val1) * ((D[dreg].U8 & 0xFF) / 256.0));
                        if (r)
                        {
                            D[dreg].U32 = (byte)Math.Floor(ret);
                        }
                        else
                        {
                            D[dreg].U32 = (byte)Math.Round(ret);
                        }

                        break;
                    }

                default:
                    {
                        throw new InvalidOperationException("Invalid size");
                    }
            }
        }
    }
}
