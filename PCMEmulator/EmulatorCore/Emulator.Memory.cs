using System;

namespace PCMEmulator
{
    partial class Emulator
    {
        sbyte ReadValueB(int mode, int reg)
        {
            sbyte value;
            switch (mode)
            {
                case 0: // Dn
                    return D[reg].S8;
                case 1: // An
                    return A[reg].S8;
                case 2: // (An)
                    return _ReadByte(A[reg].U32);
                case 3: // (An)+
                    value = _ReadByte(A[reg].U32);
                    A[reg].S32 += reg == 7 ? 2 : 1;
                    return value;
                case 4: // -(An)
                    A[reg].S32 -= reg == 7 ? 2 : 1;
                    return _ReadByte(A[reg].U32);
                case 5: // (d16,An)
                    value = _ReadByte((uint)(A[reg].U32 + _ReadOpWord(PC))); PC += 2;
                    return value;
                case 6: // (d8,An,Xn)
                    return _ReadByte((uint)(GetIndex(A[reg].U32)));
                case 7:
                    switch (reg)
                    {
                        case 0: // (imm).W
                            value = _ReadByte(_ReadOpWord(PC)); PC += 2;
                            return value;
                        case 1: // (imm).L
                            value = _ReadByte(_ReadOpLong(PC)); PC += 4;
                            return value;
                        case 2: // (d16,PC)
                            value = _ReadOpByte(PC + (uint)_ReadOpWord(PC)); PC += 2;
                            return value;
                        case 3: // (d8,PC,Xn)
                            value = _ReadOpByte((uint)(GetIndex(PC)));
                            return value;
                        case 4: // immediate
                            value = (sbyte)_ReadOpWord(PC); PC += 2;
                            return value;
                        default:
                            throw new Exception("Invalid addressing mode!");
                    }
            }
            throw new Exception("Invalid addressing mode!");
        }

        short ReadValueW(int mode, int reg)
        {
            short value;
            switch (mode)
            {
                case 0: // Dn
                    return D[reg].S16;
                case 1: // An
                    return A[reg].S16;
                case 2: // (An)
                    return _ReadWord(A[reg].U32);
                case 3: // (An)+
                    value = _ReadWord(A[reg].U32);
                    A[reg].S32 += 2;
                    return value;
                case 4: // -(An)
                    A[reg].S32 -= 2;
                    return _ReadWord(A[reg].U32);
                case 5: // (d16,An)
                    value = _ReadWord((uint)(A[reg].U32 + _ReadOpWord(PC))); PC += 2;
                    return value;
                case 6: // (d8,An,Xn)
                    return _ReadWord((uint)(GetIndex(A[reg].U32)));
                case 7:
                    switch (reg)
                    {
                        case 0: // (imm).W
                            value = _ReadWord(_ReadOpWord(PC)); PC += 2;
                            return value;
                        case 1: // (imm).L
                            value = _ReadWord(_ReadOpLong(PC)); PC += 4;
                            return value;
                        case 2: // (d16,PC)
                            value = _ReadOpWord(PC + _ReadOpWord(PC)); PC += 2;
                            return value;
                        case 3: // (d8,PC,Xn)
                            value = _ReadOpWord((GetIndex(PC)));
                            return value;
                        case 4: // immediate
                            value = _ReadOpWord(PC); PC += 2;
                            return value;
                        default:
                            throw new Exception("Invalid addressing mode!");
                    }
            }

            throw new Exception("Invalid addressing mode!");
        }

        int ReadValueL(int mode, int reg)
        {
            int value;
            switch (mode)
            {
                case 0: // Dn
                    return D[reg].S32;
                case 1: // An
                    return A[reg].S32;
                case 2: // (An)
                    return _ReadLong(A[reg].U32);
                case 3: // (An)+
                    value = _ReadLong(A[reg].U32);
                    A[reg].S32 += 4;
                    return value;
                case 4: // -(An)
                    A[reg].S32 -= 4;
                    return _ReadLong(A[reg].U32);
                case 5: // (d16,An)
                    value = _ReadLong((A[reg].U32 + _ReadOpWord(PC))); PC += 2;
                    return value;
                case 6: // (d8,An,Xn)
                    return _ReadLong(GetIndex(A[reg].U32));
                case 7:
                    switch (reg)
                    {
                        case 0: // (imm).W
                            value = _ReadLong(_ReadOpWord(PC)); PC += 2;
                            return value;
                        case 1: // (imm).L
                            value = _ReadLong(_ReadOpLong(PC)); PC += 4;
                            return value;
                        case 2: // (d16,PC)
                            value = _ReadOpLong(PC + _ReadOpWord(PC)); PC += 2;
                            return value;
                        case 3: // (d8,PC,Xn)
                            value = _ReadOpLong((GetIndex(PC)));
                            return value;
                        case 4: // immediate
                            value = _ReadOpLong(PC); PC += 4;
                            return value;
                        default:
                            throw new Exception("Invalid addressing mode!");
                    }
            }

            throw new Exception("Invalid addressing mode!");
        }

        sbyte PeekValueB(int mode, int reg)
        {
            sbyte value;
            switch (mode)
            {
                case 0: // Dn
                    return D[reg].S8;
                case 1: // An
                    return A[reg].S8;
                case 2: // (An)
                    return _ReadByte(A[reg].U32);
                case 3: // (An)+
                    value = _ReadByte(A[reg].U32);
                    return value;
                case 4: // -(An)
                    value = _ReadByte(A[reg].U32 - (reg == 7 ? 2 : 1));
                    return value;
                case 5: // (d16,An)
                    value = _ReadByte((A[reg].U32 + _ReadOpWord(PC)));
                    return value;
                case 6: // (d8,An,Xn)
                    return _ReadByte(/*A[reg].S32 +*/ PeekIndex());
                case 7:
                    switch (reg)
                    {
                        case 0: // (imm).W
                            value = _ReadByte(_ReadOpWord(PC));
                            return value;
                        case 1: // (imm).L
                            value = _ReadByte(_ReadOpLong(PC));
                            return value;
                        case 2: // (d16,PC)
                            value = _ReadByte(PC + _ReadOpWord(PC));
                            return value;
                        case 3: // (d8,PC,Xn)
                            value = _ReadByte((PC + PeekIndex()));
                            return value;
                        case 4: // immediate
                            return (sbyte)_ReadOpWord(PC);
                        default:
                            throw new Exception("Invalid addressing mode!");
                    }
            }

            throw new Exception("Invalid addressing mode!");
        }

        short PeekValueW(int mode, int reg)
        {
            short value;
            switch (mode)
            {
                case 0: // Dn
                    return D[reg].S16;
                case 1: // An
                    return A[reg].S16;
                case 2: // (An)
                    return _ReadWord(A[reg].S32);
                case 3: // (An)+
                    value = _ReadWord(A[reg].S32);
                    return value;
                case 4: // -(An)
                    value = _ReadWord(A[reg].S32 - 2);
                    return value;
                case 5: // (d16,An)
                    value = _ReadWord((A[reg].S32 + _ReadOpWord(PC)));
                    return value;
                case 6: // (d8,An,Xn)
                    return _ReadWord(/*A[reg].S32 +*/ PeekIndex());
                case 7:
                    switch (reg)
                    {
                        case 0: // (imm).W
                            value = _ReadWord(_ReadOpWord(PC));
                            return value;
                        case 1: // (imm).L
                            value = _ReadWord(_ReadOpLong(PC));
                            return value;
                        case 2: // (d16,PC)
                            value = _ReadWord(PC + _ReadOpWord(PC));
                            return value;
                        case 3: // (d8,PC,Xn)
                            value = _ReadWord((PC + PeekIndex()));
                            return value;
                        case 4: // immediate
                            return _ReadOpWord(PC);
                        default:
                            throw new Exception("Invalid addressing mode!");
                    }
            }

            throw new Exception("Invalid addressing mode!");
        }

        int PeekValueL(int mode, int reg)
        {
            int value;
            switch (mode)
            {
                case 0: // Dn
                    return D[reg].S32;
                case 1: // An
                    return A[reg].S32;
                case 2: // (An)
                    return _ReadLong(A[reg].S32);
                case 3: // (An)+
                    value = _ReadLong(A[reg].S32);
                    return value;
                case 4: // -(An)
                    value = _ReadLong(A[reg].S32 - 4);
                    return value;
                case 5: // (d16,An)
                    value = _ReadLong((A[reg].S32 + _ReadOpWord(PC)));
                    return value;
                case 6: // (d8,An,Xn)
                    return _ReadLong(/*A[reg].S32*/ + PeekIndex());
                case 7:
                    switch (reg)
                    {
                        case 0: // (imm).W
                            value = _ReadLong(_ReadOpWord(PC));
                            return value;
                        case 1: // (imm).L
                            value = _ReadLong(_ReadOpLong(PC));
                            return value;
                        case 2: // (d16,PC)
                            value = _ReadLong(PC + _ReadOpWord(PC));
                            return value;
                        case 3: // (d8,PC,Xn)
                            value = _ReadLong((PC + PeekIndex()));
                            return value;
                        case 4: // immediate
                            return _ReadOpLong(PC);
                        default:
                            throw new Exception("Invalid addressing mode!");
                    }
            }

            throw new Exception("Invalid addressing mode!");
        }

        uint ReadAddress(int mode, int reg)
        {
            uint addr;
            switch (mode)
            {
                case 0: throw new Exception("Invalid addressing mode!"); // Dn
                case 1: throw new Exception("Invalid addressing mode!"); // An
                case 2: return A[reg].U32; // (An)
                case 3: return A[reg].U32; // (An)+
                case 4: return A[reg].U32; // -(An)
                case 5: addr = A[reg].U32 + (uint)_ReadOpWord(PC); PC += 2; return addr; // (d16,An)
                case 6: return (uint)GetIndex(A[reg].U32); // (d8,An,Xn)
                case 7:
                    switch (reg)
                    {
                        case 0: addr = (uint)_ReadOpWord(PC); PC += 2; return addr; // (imm).w
                        case 1: addr = (uint)_ReadOpLong(PC); PC += 4; return addr; // (imm).l
                        case 2: addr = PC; addr += (uint)_ReadOpWord(PC); PC += 2; return addr; // (d16,PC)
                        case 3: addr = (uint)GetIndex(PC); return addr; // (d8,PC,Xn)
                        case 4: throw new Exception("Invalid addressing mode!"); // immediate
                    }
                    break;
            }

            throw new Exception("Invalid addressing mode!");
        }

        string DisassembleValue(int mode, int reg, int size, ref uint pc)
        {
            string value;
            int addr;
            switch (mode)
            {
                case 0: return "D" + reg;       // Dn
                case 1: return "A" + reg;       // An
                case 2: return "(A" + reg + ")";  // (An)
                case 3: return "(A" + reg + ")+"; // (An)+
                case 4: return "-(A" + reg + ")"; // -(An)
                case 5: value = string.Format("(${0:X},A{1})", _ReadOpWord(pc), reg); pc += 2; return value; // (d16,An)
                case 6: addr = _ReadOpWord(pc); pc += 2; return DisassembleIndex("A" + reg, (short)addr); // (d8,An,Xn)
                case 7:
                    switch (reg)
                    {
                        case 0: value = String.Format("(${0:X})", _ReadOpWord(pc)); pc += 2; return value; // (imm).W
                        case 1: value = String.Format("(${0:X})", _ReadOpLong(pc)); pc += 4; return value; // (imm).L
                        case 2: value = String.Format("(${0:X})", pc + _ReadOpWord(pc)); pc += 2; return value; // (d16,PC)
                        case 3: addr = _ReadOpWord(pc); pc += 2; return DisassembleIndex("PC", (short)addr); // (d8,PC,Xn)
                        case 4:
                            switch (size)
                            {
                                case 1: value = String.Format("${0:X}", (byte)_ReadOpWord(pc)); pc += 2; return value;
                                case 2: value = String.Format("${0:X}", _ReadOpWord(pc)); pc += 2; return value;
                                case 4: value = String.Format("${0:X}", _ReadOpLong(pc)); pc += 4; return value;
                            }
                            break;
                    }
                    break;
            }

            throw new Exception("Invalid addressing mode!");
        }

        string DisassembleImmediate(int size, ref uint pc)
        {
            int immed;
            switch (size)
            {
                case 1:
                    immed = (byte)_ReadOpWord(pc); pc += 2;
                    return String.Format("${0:X}", immed);
                case 2:
                    immed = (ushort)_ReadOpWord(pc); pc += 2;
                    return String.Format("${0:X}", immed);
                case 4:
                    immed = _ReadOpLong(pc); pc += 4;
                    return String.Format("${0:X}", immed);
            }

            throw new ArgumentException("Invalid size");
        }

        string DisassembleAddress(int mode, int reg, ref uint pc)
        {
            int addr;
            switch (mode)
            {
                case 0: return "INVALID"; // Dn
                case 1: return "INVALID"; // An
                case 2: return "(A" + reg + ")"; // (An)
                case 3: return "(A" + reg + ")+"; // (An)+
                case 4: return "-(A" + reg + ")"; // -(An)
                case 5: addr = _ReadOpWord(pc); pc += 2; return String.Format("({0},A{1})", addr, reg); // (d16,An)
                case 6: addr = _ReadOpWord(pc); pc += 2; return DisassembleIndex("A" + reg, (short)addr); // (d8,An,Xn)
                case 7:
                    switch (reg)
                    {
                        case 0: addr = _ReadOpWord(pc); pc += 2; return String.Format("${0:X}.w", addr); // (imm).w
                        case 1: addr = _ReadOpLong(pc); pc += 4; return String.Format("${0:X}.l", addr); // (imm).l
                        case 2: addr = _ReadOpWord(pc); pc += 2; return String.Format("(${0:X},PC)", addr); // (d16,PC)
                        case 3: addr = _ReadOpWord(pc); pc += 2; return DisassembleIndex("PC", (short)addr); // (d8,PC,Xn)
                        case 4: return "INVALID"; // immediate
                    }
                    break;
            }

            throw new Exception("Invalid addressing mode!");
        }

        void WriteValueB(int mode, int reg, sbyte value)
        {
            switch (mode)
            {
                case 0x00: // Dn
                    D[reg].S8 = value;
                    return;
                case 0x01: // An
                    A[reg].S32 = value;
                    return;
                case 0x02: // (An)
                    _WriteByte(A[reg].U32, value);
                    return;
                case 0x03: // (An)+
                    _WriteByte(A[reg].U32, value);
                    A[reg].S32 += reg == 7 ? 2 : 1;
                    return;
                case 0x04: // -(An)
                    A[reg].S32 -= reg == 7 ? 2 : 1;
                    _WriteByte(A[reg].U32, value);
                    return;
                case 0x05: // (d16,An)
                    _WriteByte(A[reg].U32 + (uint)_ReadOpWord(PC), value); PC += 2;
                    return;
                case 0x06: // (d8,An,Xn)
                    _WriteByte((uint)GetIndex(A[reg].U32), value);
                    return;
                case 0x07:
                    switch (reg)
                    {
                        case 0x00: // (imm).W
                            _WriteByte((uint)_ReadOpWord(PC), value); PC += 2;
                            return;
                        case 0x01: // (imm).L
                            _WriteByte((uint)_ReadOpLong(PC), value); PC += 4;
                            return;
                        case 0x02: // (d16,PC)
                            _WriteByte(PC + (uint)_ReadOpWord(PC), value); PC += 2;
                            return;
                        case 0x03: // (d8,PC,Xn)
                            _WriteByte(PC + (uint)PeekIndex(), value);
                            PC += 2;
                            return;
                        default: throw new Exception("Invalid addressing mode!");
                    }
            }
        }

        void WriteValueW(int mode, int reg, short value)
        {
            switch (mode)
            {
                case 0x00: // Dn
                    D[reg].S16 = value;
                    return;
                case 0x01: // An
                    A[reg].S32 = value;
                    return;
                case 0x02: // (An)
                    _WriteWord(A[reg].U32, value);
                    return;
                case 0x03: // (An)+
                    _WriteWord(A[reg].U32, value);
                    A[reg].U32 += 2;
                    return;
                case 0x04: // -(An)
                    A[reg].U32 -= 2;
                    _WriteWord(A[reg].U32, value);
                    return;
                case 0x05: // (d16,An)
                    _WriteWord(A[reg].U32 + (uint)_ReadOpWord(PC), value); PC += 2;
                    return;
                case 0x06: // (d8,An,Xn)
                    _WriteWord((uint)GetIndex(A[reg].U32), value);
                    return;
                case 0x07:
                    switch (reg)
                    {
                        case 0x00: // (imm).W
                            _WriteWord((uint)_ReadOpWord(PC), value); PC += 2;
                            return;
                        case 0x01: // (imm).L
                            _WriteWord((uint)_ReadOpLong(PC), value); PC += 4;
                            return;
                        case 0x02: // (d16,PC)
                            _WriteWord(PC + (uint)_ReadOpWord(PC), value); PC += 2;
                            return;
                        case 0x03: // (d8,PC,Xn)
                            _WriteWord(PC + (uint)PeekIndex(), value);
                            PC += 2;
                            return;
                        default:
                            throw new Exception("Invalid addressing mode!");
                    }
            }
        }

        void WriteValueL(int mode, int reg, uint value)
        {
            switch (mode)
            {
                case 0x00: // Dn
                    D[reg].U32 = value;
                    return;
                case 0x01: // An
                    A[reg].U32 = value;
                    return;
                case 0x02: // (An)
                    _WriteLong(A[reg].U32, value);
                    return;
                case 0x03: // (An)+
                    _WriteLong(A[reg].U32, value);
                    A[reg].S32 += 4;
                    return;
                case 0x04: // -(An)
                    A[reg].S32 -= 4;
                    _WriteLong(A[reg].U32, value);
                    return;
                case 0x05: // (d16,An)
                    _WriteLong(A[reg].U32 + (uint)_ReadOpWord(PC), value); PC += 2;
                    return;
                case 0x06: // (d8,An,Xn)
                    _WriteLong((uint)GetIndex(A[reg].U32), value);
                    return;
                case 0x07:
                    switch (reg)
                    {
                        case 0x00: // (imm).W
                            _WriteLong((uint)_ReadOpWord(PC), value); PC += 2;
                            return;
                        case 0x01: // (imm).L
                            _WriteLong((uint)_ReadOpLong(PC), value); PC += 4;
                            return;
                        case 0x02: // (d16,PC)
                            _WriteLong(PC + (uint)_ReadOpWord(PC), value); PC += 2;
                            return;
                        case 0x03: // (d8,PC,Xn)
                            _WriteLong(PC + (uint)PeekIndex(), value);
                            PC += 2;
                            return;
                        default:
                            throw new Exception("Invalid addressing mode!");
                    }
            }
        }

        int GetIndex(uint baseReg)
        {
            short extension = _ReadOpWord(PC); PC += 2;

            int da = (extension >> 15) & 0x1;
            int reg = (extension >> 12) & 0x7;
            int size = (extension >> 11) & 0x1;
            int scale = (extension >> 9) & 0x3;
            bool full = ((extension >> 8) & 0x1) > 0;
            bool bs = ((extension >> 7) & 0x1) > 0;
            bool idxs = ((extension >> 6) & 0x1) > 0;
            int displacement = 0;
            if (full)
            {
                var bd = (extension >> 4) & 0x3;
                if (bd == 0x2)
                {
                    displacement = _ReadOpWord(PC); PC += 2;
                }
                else if (bd == 0x3)
                {
                    displacement = _ReadOpLong(PC); PC += 4;
                }
            }
            else
            {
                displacement = extension & 0xFF;
            }

            int indexReg;
            switch (scale)
            {
                case 0: indexReg = 1; break;
                case 1: indexReg = 2; break;
                case 2: indexReg = 4; break;
                default: indexReg = 8; break;
            }
            if (da == 0)
                indexReg *= size == 0 ? D[reg].S16 : D[reg].S32;
            else
                indexReg *= size == 0 ? A[reg].S16 : A[reg].S32;

            return (int)((bs ? 0 : baseReg) + displacement + (idxs ? 0 : indexReg));
        }

        int PeekIndex()
        {
            //Console.WriteLine("IN INDEX PORTION - NOT VERIFIED!!!");

            short extension = _ReadOpWord(PC);

            int da = (extension >> 15) & 0x1;
            int reg = (extension >> 12) & 0x7;
            int size = (extension >> 11) & 0x1;
            int scale = (extension >> 9) & 0x3;
            bool full = ((extension >> 8) & 0x1) > 0;
            int displacement = 0;
            if (full)
            {
                var bd = (extension >> 4) & 0x3;
                if (bd == 0x2)
                {
                    displacement = _ReadOpWord(PC);
                }
                else if (bd == 0x3)
                {
                    displacement = _ReadOpLong(PC);
                }
            }
            else
            {
                displacement = extension;
            }

            int indexReg;
            switch (scale)
            {
                case 0: indexReg = 1; break;
                case 1: indexReg = 2; break;
                case 2: indexReg = 4; break;
                default: indexReg = 8; break;
            }
            if (da == 0)
                indexReg *= size == 0 ? D[reg].S16 : D[reg].S32;
            else
                indexReg *= size == 0 ? A[reg].S16 : A[reg].S32;

            return displacement + indexReg;
        }

        string DisassembleIndex(string baseRegister, short extension)
        {
            int d_a = (extension >> 15) & 0x1;
            int reg = (extension >> 12) & 0x7;
            int size = (extension >> 11) & 0x1;
            int scale = (extension >> 9) & 0x3;
            sbyte displacement = (sbyte)extension;

            string scaleFactor;
            switch (scale)
            {
                case 0: scaleFactor = ""; break;
                case 1: scaleFactor = "2"; break;
                case 2: scaleFactor = "4"; break;
                default: scaleFactor = "8"; break;
            }

            string offsetRegister = (d_a == 0) ? "D" : "A";
            string sizeStr = size == 0 ? ".w" : ".l";
            string displacementStr = displacement == 0 ? "" : ("," + displacement);
            return string.Format("({0},{1}{2}{3}{4}{5})", baseRegister, scaleFactor, offsetRegister, reg, sizeStr, displacementStr);
        }
    }
}
