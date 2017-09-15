using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PCMEmulator
{
    /// <summary>
    /// PCMDevice
    /// </summary>
    public class PCMDevice : EmulatedDevice
    {
        private static DLC _dlc;

        public PCMDevice()
        {
            if (_dlc == null)
            {
                _dlc = new DLC(this);
            }
        }

        public DLC DLC
        {
            get
            {
                return _dlc;
            }
        }

        public override void Init()
        {
            _dlc.Init();
        }

        /// <summary>
        /// ReadOpByte
        /// </summary>
        /// <param name="address"></param>
        /// <returns></returns>
        public override sbyte ReadOpByte(uint address)
        {
            var addr = (address & 0xFFFFFF);
            if (addr >= 0xFF0000)
            {
                return (sbyte)(Emulator.Ram[addr & 0xFFFF]);
            }
            else if (address < Emulator.Rom.Length)
            {
                return (sbyte)(Emulator.Rom[address]);
            }
            else
            {
                return 0;
            }
        }

        /// <summary>
        /// ReadOpWord
        /// </summary>
        /// <param name="address"></param>
        /// <returns></returns>
        public override short ReadOpWord(uint address)
        {
            var addr = (address & 0xFFFFFF);
            if (addr + 1 >= 0xFF0000)
            {
                var a = addr & 0xFFFF;
                return (short)((Emulator.Ram[a] * 0x100) + Emulator.Ram[a + 1]);
            }
            else if (address + 1 < Emulator.Rom.Length)
            {
                return (short)((Emulator.Rom[address] * 0x100) + Emulator.Rom[address + 1]);
            }
            else
            {
                return 0;
            }
        }

        /// <summary>
        /// ReadOpLong
        /// </summary>
        /// <param name="address"></param>
        /// <returns></returns>
        public override int ReadOpLong(uint address)
        {
            var addr = (address & 0xFFFFFF);
            if (addr + 3 >= 0xFF0000)
            {
                var a = addr & 0xFFFF;
                return (int)((Emulator.Ram[a] * 0x1000000) + (Emulator.Ram[a + 1] * 0x10000) +
                    (Emulator.Ram[a + 2] * 0x100) + Emulator.Ram[a + 3]);
            }
            else if (address + 3 < Emulator.Rom.Length)
            {
                return (int)((Emulator.Rom[address] * 0x1000000) + (Emulator.Rom[address + 1] * 0x10000) +
                    (Emulator.Rom[address + 2] * 0x100) + Emulator.Rom[address + 3]);
            }
            else
            {
                return 0;
            }
        }

        /// <summary>
        /// ReadByte
        /// </summary>
        /// <param name="address"></param>
        /// <returns></returns>
        public override sbyte ReadByte(uint address)
        {
            address &= 0xffffff;
            if (address < Emulator.Rom.Length)
            {
                return (sbyte)Emulator.Rom[address];
            }
            else if (address == 0xFFA9EA)
            {
                return -1;
            }
            else if (address == 0xFFF70F)
            {
                return 0x02 | 0x01;
            }
            else if (address == 0xFFFC1F)
            {
                return -1; //0xFF
            }
            else if (address == 0xFFF60E)
            {
                return (sbyte)_dlc.Status;
            }
            else if (address >= 0xff0000 && address <= 0xffffff)
            {
                return (sbyte)Emulator.Ram[address & 0xffff];
            }

            return 0;
        }

        /// <summary>
        /// ReadWord
        /// </summary>
        /// <param name="address"></param>
        /// <returns></returns>
        private short _ff30 = 0;
        private short _e1e4 = 0;
        private short _funkyRangeW = 0;
        public override short ReadWord(uint address)
        {
            address &= 0xffffff;
            if (address + 1 < Emulator.Rom.Length)
            {
                return (short)((Emulator.Rom[address] * 0x100) + Emulator.Rom[address + 1]);
            }
            else if (address >= 0xFFE200 && address < 0xFFE300)
            {
                return _funkyRangeW++;
            }
            else if (address == 0xFFE0E0)
            {
                return 0xA3;
            }
            else if (address == 0xFFE1E4)
            {
                return _e1e4++;
            }
            else if (address == 0xFFFE18)
            {
                return 0x0000;
            }
            else if (address == 0xFFFE1A)
            {
                return 0x0000;
            }
            else if (address == 0xFFFF30)
            {
                return _ff30++;
            }
            else if (address == 0xFFF60E)
            {
                var status = _dlc.Status;
                byte b = 0x00;
                if (_dlc.Frames.Count > 0)
                {
                    var data = _dlc.Frames.Peek();
                    if (data.Count > 0)
                    {
                        b = data[0];
                        data.RemoveAt(0);
                    }
                }
                
                return (short)((status << 8) | b);
            }
            else if (address >= 0xff0000 && address + 1 <= 0xffffff)
            {
                return (short)((Emulator.Ram[(address & 0xffff)] * 0x100) + Emulator.Ram[(address & 0xffff) + 1]);
            }

            return 0;
        }

        /// <summary>
        /// ReadLong
        /// </summary>
        /// <param name="address"></param>
        /// <returns></returns>
        public override int ReadLong(uint address)
        {
            address &= 0xffffff;
            if (address + 3 < Emulator.Rom.Length)
            {
                return (int)((Emulator.Rom[address] * 0x1000000) + (Emulator.Rom[address + 1] * 0x10000) +
                    (Emulator.Rom[address + 2] * 0x100) + Emulator.Rom[address + 3]);
            }
            else if (address == 0xFFFE18)
            {
                return 0x0000;
            }
            else if (address >= 0xff0000 && address + 3 <= 0xffffff)
            {
                return (int)((Emulator.Ram[(address & 0xffff)] * 0x1000000) + (Emulator.Ram[(address & 0xffff) + 1] * 0x10000) +
                    (Emulator.Ram[(address & 0xffff) + 2] * 0x100) + Emulator.Ram[(address & 0xffff) + 3]);
            }

            return 0;
        }

        /// <summary>
        /// WriteByte
        /// </summary>
        /// <param name="address"></param>
        /// <param name="value"></param>
        public override void WriteByte(uint address, sbyte value)
        {
            address &= 0xffffff;
            if (address >= 0xff0000 && address <= 0xffffff)
            {
                switch (address)
                {
                    case 0xFFF60D:
                        {
                            _dlc.Data = (byte)(value & 0xFF);

                            break;
                        }

                    default:
                        {
                            Emulator.Ram[(address & 0xffff)] = (byte)value;

                            break;
                        }
                }
            }
        }

        /// <summary>
        /// WriteWord
        /// </summary>
        /// <param name="address"></param>
        /// <param name="value"></param>
        public override void WriteWord(uint address, short value)
        {
            address &= 0xffffff;
            if (address >= 0xff0000 && address + 1 <= 0xffffff)
            {
                switch (address)
                {
                    case 0xFFF60C:
                        {
                            _dlc.Data = (byte)(value & 0xFF);
                            _dlc.Command = (byte)((value >> 8) & 0xFF);

                            break;
                        }

                    default:
                        {
                            Emulator.Ram[(address & 0xffff)] = (byte)(value >> 8);
                            Emulator.Ram[(address & 0xffff) + 1] = (byte)(value);

                            break;
                        }
                }
            }
        }

        /// <summary>
        /// WriteLong
        /// </summary>
        /// <param name="address"></param>
        /// <param name="value"></param>
        public override void WriteLong(uint address, uint value)
        {
            address &= 0xffffff;
            if (address >= 0xff0000 && address + 3 <= 0xffffff)
            {
                Emulator.Ram[(address & 0xffff)] = (byte)(value >> 24);
                Emulator.Ram[(address & 0xffff) + 1] = (byte)(value >> 16);
                Emulator.Ram[(address & 0xffff) + 2] = (byte)(value >> 8);
                Emulator.Ram[(address & 0xffff) + 3] = (byte)(value);
            }
        }
    }
}
