using System;
using System.Text;

namespace PCMEmulator
{
    partial class Emulator
    {
        void MOVE()
        {
            int size = ((Op >> 12) & 0x03);
            int dstMode = ((Op >> 6) & 0x07);
            int dstReg = ((Op >> 9) & 0x07);
            int srcMode = ((Op >> 3) & 0x07);
            int srcReg = (Op & 0x07);

            uint value = 0;
            switch (size)
            {
                case 1: // Byte
                    value = (uint)ReadValueB(srcMode, srcReg);
                    WriteValueB(dstMode, dstReg, (sbyte)value);
                    PendingCycles -= MoveCyclesBW[srcMode + (srcMode == 7 ? srcReg : 0), dstMode + (dstMode == 7 ? dstReg : 0)];
                    N = (value & 0x80) != 0;
                    break;
                case 3: // Word
                    value = (uint)ReadValueW(srcMode, srcReg);
                    WriteValueW(dstMode, dstReg, (short)value);
                    PendingCycles -= MoveCyclesBW[srcMode + (srcMode == 7 ? srcReg : 0), dstMode + (dstMode == 7 ? dstReg : 0)];
                    N = (value & 0x8000) != 0;
                    break;
                case 2: // Long
                    value = (uint)ReadValueL(srcMode, srcReg);
                    WriteValueL(dstMode, dstReg, value);
                    PendingCycles -= MoveCyclesL[srcMode + (srcMode == 7 ? srcReg : 0), dstMode + (dstMode == 7 ? dstReg : 0)];
                    N = (value & 0x80000000) != 0;
                    break;
            }

            V = false;
            C = false;
            Z = (value == 0);
        }

        void MOVE_Disasm(DisassemblyInfo info)
        {
            var pc = info.PC + 2;
            int size = ((Op >> 12) & 0x03);
            int dstMode = ((Op >> 6) & 0x07);
            int dstReg = ((Op >> 9) & 0x07);
            int srcMode = ((Op >> 3) & 0x07);
            int srcReg = (Op & 0x07);

            switch (size)
            {
                case 1:
                    info.Mnemonic = "move.b";
                    info.Args = DisassembleValue(srcMode, srcReg, 1, ref pc) + ", ";
                    info.Args += DisassembleValue(dstMode, dstReg, 1, ref pc);
                    break;
                case 3:
                    info.Mnemonic = "move.w";
                    info.Args = DisassembleValue(srcMode, srcReg, 2, ref pc) + ", ";
                    info.Args += DisassembleValue(dstMode, dstReg, 2, ref pc);
                    break;
                case 2:
                    info.Mnemonic = "move.l";
                    info.Args = DisassembleValue(srcMode, srcReg, 4, ref pc) + ", ";
                    info.Args += DisassembleValue(dstMode, dstReg, 4, ref pc);
                    break;
            }

            info.Length = pc - info.PC;
        }

        void MOVEA()
        {
            int size = ((Op >> 12) & 0x03);
            int dstReg = ((Op >> 9) & 0x07);
            int srcMode = ((Op >> 3) & 0x07);
            int srcReg = (Op & 0x07);

            if (size == 3) // Word
            {
                A[dstReg].S32 = ReadValueW(srcMode, srcReg);
                switch (srcMode)
                {
                    case 0: PendingCycles -= 4; break;
                    case 1: PendingCycles -= 4; break;
                    case 2: PendingCycles -= 8; break;
                    case 3: PendingCycles -= 8; break;
                    case 4: PendingCycles -= 10; break;
                    case 5: PendingCycles -= 12; break;
                    case 6: PendingCycles -= 14; break;
                    case 7:
                        switch (srcReg)
                        {
                            case 0: PendingCycles -= 12; break;
                            case 1: PendingCycles -= 16; break;
                            case 2: PendingCycles -= 12; break;
                            case 3: PendingCycles -= 14; break;
                            case 4: PendingCycles -= 8; break;
                            default: throw new InvalidOperationException();
                        }
                        break;
                }
            }
            else
            { // Long
                A[dstReg].S32 = ReadValueL(srcMode, srcReg);
                switch (srcMode)
                {
                    case 0: PendingCycles -= 4; break;
                    case 1: PendingCycles -= 4; break;
                    case 2: PendingCycles -= 12; break;
                    case 3: PendingCycles -= 12; break;
                    case 4: PendingCycles -= 14; break;
                    case 5: PendingCycles -= 16; break;
                    case 6: PendingCycles -= 18; break;
                    case 7:
                        switch (srcReg)
                        {
                            case 0: PendingCycles -= 16; break;
                            case 1: PendingCycles -= 20; break;
                            case 2: PendingCycles -= 16; break;
                            case 3: PendingCycles -= 18; break;
                            case 4: PendingCycles -= 12; break;
                            default: throw new InvalidOperationException();
                        }
                        break;
                }
            }
        }

        void MOVEA_Disasm(DisassemblyInfo info)
        {
            var pc = info.PC + 2;
            int size = ((Op >> 12) & 0x03);
            int dstReg = ((Op >> 9) & 0x07);
            int srcMode = ((Op >> 3) & 0x07);
            int srcReg = (Op & 0x07);

            if (size == 3)
            {
                info.Mnemonic = "movea.w";
                info.Args = DisassembleValue(srcMode, srcReg, 2, ref pc) + ", A" + dstReg;
            }
            else
            {
                info.Mnemonic = "movea.l";
                info.Args = DisassembleValue(srcMode, srcReg, 4, ref pc) + ", A" + dstReg;
            }
            info.Length = pc - info.PC;
        }

        void MOVEP()
        {
            int dReg = ((Op >> 9) & 0x07);
            int dir = ((Op >> 7) & 0x01);
            int size = ((Op >> 6) & 0x01);
            int aReg = (Op & 0x07);
            if (dir == 0 && size == 0)
            {
                int ea;
                ea = A[aReg].S32 + _ReadOpWord(PC); PC += 2;
                D[dReg].U32 = (D[dReg].U32 & 0xffff0000) | (ushort)((_ReadByte(ea) << 8) + _ReadByte(ea + 2));
                PendingCycles -= 16;
            }
            else if (dir == 0 && size == 1)
            {
                int ea;
                ea = A[aReg].S32 + _ReadOpWord(PC); PC += 2;
                D[dReg].S32 = (_ReadByte(ea) << 24) + (_ReadByte(ea + 2) << 16) + (_ReadByte(ea + 4) << 8) + _ReadByte(ea + 6);
                PendingCycles -= 24;
            }
            else if (dir == 1 && size == 0)
            {
                uint src;
                uint ea;
                ea = (uint)(A[aReg].U32 + _ReadOpWord(PC)); PC += 2;
                src = D[dReg].U32;
                _WriteByte(ea, (sbyte)((src >> 8) & 0xff));
                _WriteByte(ea + 2, (sbyte)(src & 0xff));
                PendingCycles -= 16;
            }
            else if (dir == 1 && size == 1)
            {
                uint src;
                uint ea;
                ea = (uint)(A[aReg].U32 + _ReadOpWord(PC)); PC += 2;
                src = D[dReg].U32;
                _WriteByte(ea, (sbyte)((src >> 24) & 0xff));
                _WriteByte(ea + 2, (sbyte)((src >> 16) & 0xff));
                _WriteByte(ea + 4, (sbyte)((src >> 8) & 0xff));
                _WriteByte(ea + 6, (sbyte)(src & 0xff));
                PendingCycles -= 24;
            }
        }

        void MOVEP_Disasm(DisassemblyInfo info)
        {
            var pc = info.PC + 2;
            int dReg = ((Op >> 9) & 0x07);
            int dir = ((Op >> 7) & 0x01);
            int size = ((Op >> 6) & 0x01);
            int aReg = (Op & 0x07);
            if (size == 0)
            {
                info.Mnemonic = "movep.w";
                if (dir == 0)
                {
                    info.Args = DisassembleValue(5, aReg, 2, ref pc) + ", D" + dReg;
                }
                else if (dir == 1)
                {
                    info.Args = "D" + dReg + ", " + DisassembleValue(5, aReg, 2, ref pc);
                }
            }
            else if (size == 1)
            {
                info.Mnemonic = "movep.l";
                if (dir == 0)
                {
                    info.Args = DisassembleValue(5, aReg, 4, ref pc) + ", D" + dReg;
                }
                else if (dir == 1)
                {
                    info.Args = "D" + dReg + ", " + DisassembleValue(5, aReg, 4, ref pc);
                }
            }
            info.Length = pc - info.PC;
        }

        void MOVEQ()
        {
            int value = (sbyte)Op; // 8-bit data payload is sign-extended to 32-bits.
            N = (value & 0x80) != 0;
            Z = (value == 0);
            V = false;
            C = false;
            D[(Op >> 9) & 7].S32 = value;
            PendingCycles -= 4;
        }

        void MOVEQ_Disasm(DisassemblyInfo info)
        {
            info.Mnemonic = "moveq";
            info.Args = String.Format("{0}, D{1}", (sbyte)Op, (Op >> 9) & 7);
        }

        static string DisassembleRegisterList0(int mode, ushort registers)
        {
            var str = new StringBuilder();
            int count = 0;
            if (mode == 4)
            {
                for (int i = 0; i < 8; i++)
                {
                    if ((registers & 0x8000) != 0)
                    {
                        if (count > 0) str.Append(",");
                        str.Append("D" + i);
                        count++;
                    }
                    registers <<= 1;
                }
                for (int i = 0; i < 8; i++)
                {
                    if ((registers & 0x8000) != 0)
                    {
                        if (count > 0) str.Append(",");
                        str.Append("A" + i);
                        count++;
                    }
                    registers <<= 1;
                }
            }
            else
            {
                for (int i = 0; i < 8; i++)
                {
                    if ((registers & 1) != 0)
                    {
                        if (count > 0) str.Append(",");
                        str.Append("D" + i);
                        count++;
                    }
                    registers >>= 1;
                }
                for (int i = 0; i < 8; i++)
                {
                    if ((registers & 1) != 0)
                    {
                        if (count > 0) str.Append(",");
                        str.Append("A" + i);
                        count++;
                    }
                    registers >>= 1;
                }
            }
            return str.ToString();
        }

        static string DisassembleRegisterList1(ushort registers)
        {
            var str = new StringBuilder();
            int count = 0;
            for (int i = 0; i < 8; i++)
            {
                if ((registers & 1) != 0)
                {
                    if (count > 0) str.Append(",");
                    str.Append("D" + i);
                    count++;
                }
                registers >>= 1;
            }
            for (int i = 0; i < 8; i++)
            {
                if ((registers & 1) != 0)
                {
                    if (count > 0) str.Append(",");
                    str.Append("A" + i);
                    count++;
                }
                registers >>= 1;
            }
            return str.ToString();
        }

        void MOVEM0()
        {
            // Move register to memory
            int size = (Op >> 6) & 1;
            int dstMode = (Op >> 3) & 7;
            int dstReg = (Op >> 0) & 7;

            ushort registers = (ushort)_ReadOpWord(PC); PC += 2;
            var address = ReadAddress(dstMode, dstReg);
            int regCount = 0;

            if (size == 0)
            {
                // word-assign
                if (dstMode == 4) // decrement address
                {
                    for (int i = 7; i >= 0; i--)
                    {
                        if ((registers & 1) == 1)
                        {
                            address -= 2;
                            _WriteWord(address, A[i].S16);
                            regCount++;
                        }
                        registers >>= 1;
                    }
                    for (int i = 7; i >= 0; i--)
                    {
                        if ((registers & 1) == 1)
                        {
                            address -= 2;
                            _WriteWord(address, D[i].S16);
                            regCount++;
                        }
                        registers >>= 1;
                    }
                    A[dstReg].U32 = address;
                }
                else
                { // increment address
                    for (int i = 0; i <= 7; i++)
                    {
                        if ((registers & 1) == 1)
                        {
                            _WriteWord(address, D[i].S16);
                            address += 2;
                            regCount++;
                        }
                        registers >>= 1;
                    }
                    for (int i = 0; i <= 7; i++)
                    {
                        if ((registers & 1) == 1)
                        {
                            _WriteWord(address, A[i].S16);
                            address += 2;
                            regCount++;
                        }
                        registers >>= 1;
                    }
                }
                PendingCycles -= regCount * 4;
            }
            else
            {
                // long-assign
                if (dstMode == 4) // decrement address
                {
                    for (int i = 7; i >= 0; i--)
                    {
                        if ((registers & 1) == 1)
                        {
                            address -= 4;
                            _WriteLong(address, A[i].U32);
                            regCount++;
                        }
                        registers >>= 1;
                    }
                    for (int i = 7; i >= 0; i--)
                    {
                        if ((registers & 1) == 1)
                        {
                            address -= 4;
                            _WriteLong(address, D[i].U32);
                            regCount++;
                        }
                        registers >>= 1;
                    }
                    A[dstReg].U32 = address;
                }
                else
                { // increment address
                    for (int i = 0; i <= 7; i++)
                    {
                        if ((registers & 1) == 1)
                        {
                            _WriteLong(address, D[i].U32);
                            address += 4;
                            regCount++;
                        }
                        registers >>= 1;
                    }
                    for (int i = 0; i <= 7; i++)
                    {
                        if ((registers & 1) == 1)
                        {
                            _WriteLong(address, A[i].U32);
                            address += 4;
                            regCount++;
                        }
                        registers >>= 1;
                    }
                }
                PendingCycles -= regCount * 8;
            }

            switch (dstMode)
            {
                case 2: PendingCycles -= 8; break;
                case 3: PendingCycles -= 8; break;
                case 4: PendingCycles -= 8; break;
                case 5: PendingCycles -= 12; break;
                case 6: PendingCycles -= 14; break;
                case 7:
                    switch (dstReg)
                    {
                        case 0: PendingCycles -= 12; break;
                        case 1: PendingCycles -= 16; break;
                    }
                    break;
            }
        }

        void MOVEM0_Disasm(DisassemblyInfo info)
        {
            var pc = info.PC + 2;
            int size = (Op >> 6) & 1;
            int mode = (Op >> 3) & 7;
            int reg = (Op >> 0) & 7;

            ushort registers = (ushort)_ReadOpWord(pc); pc += 2;
            string address = DisassembleAddress(mode, reg, ref pc);

            info.Mnemonic = size == 0 ? "movem.w" : "movem.l";
            info.Args = DisassembleRegisterList0(mode, registers) + ", " + address;
            info.Length = pc - info.PC;
        }

        void MOVEM1()
        {
            // Move memory to register
            int size = (Op >> 6) & 1;
            int srcMode = (Op >> 3) & 7;
            int srcReg = (Op >> 0) & 7;

            ushort registers = (ushort)_ReadOpWord(PC); PC += 2;
            var address = ReadAddress(srcMode, srcReg);
            int regCount = 0;

            if (size == 0)
            {
                // word-assign
                for (int i = 0; i < 8; i++)
                {
                    if ((registers & 1) == 1)
                    {
                        D[i].S32 = _ReadWord(address);
                        address += 2;
                        regCount++;
                    }
                    registers >>= 1;
                }
                for (int i = 0; i < 8; i++)
                {
                    if ((registers & 1) == 1)
                    {
                        A[i].S32 = _ReadWord(address);
                        address += 2;
                        regCount++;
                    }
                    registers >>= 1;
                }
                PendingCycles -= regCount * 4;
                if (srcMode == 3)
                    A[srcReg].U32 = address;
            }
            else
            {
                // long-assign
                for (int i = 0; i < 8; i++)
                {
                    if ((registers & 1) == 1)
                    {
                        D[i].S32 = _ReadLong(address);
                        address += 4;
                        regCount++;
                    }
                    registers >>= 1;
                }
                for (int i = 0; i < 8; i++)
                {
                    if ((registers & 1) == 1)
                    {
                        A[i].S32 = _ReadLong(address);
                        address += 4;
                        regCount++;
                    }
                    registers >>= 1;
                }
                PendingCycles -= regCount * 8;
                if (srcMode == 3)
                    A[srcReg].U32 = address;
            }

            switch (srcMode)
            {
                case 2: PendingCycles -= 12; break;
                case 3: PendingCycles -= 12; break;
                case 4: PendingCycles -= 12; break;
                case 5: PendingCycles -= 16; break;
                case 6: PendingCycles -= 18; break;
                case 7:
                    switch (srcReg)
                    {
                        case 0: PendingCycles -= 16; break;
                        case 1: PendingCycles -= 20; break;
                        case 2: PendingCycles -= 16; break;
                        case 3: PendingCycles -= 18; break;
                    }
                    break;
            }
        }

        void MOVEM1_Disasm(DisassemblyInfo info)
        {
            var pc = info.PC + 2;
            int size = (Op >> 6) & 1;
            int mode = (Op >> 3) & 7;
            int reg = (Op >> 0) & 7;

            ushort registers = (ushort)_ReadOpWord(pc); pc += 2;
            string address = DisassembleAddress(mode, reg, ref pc);

            info.Mnemonic = size == 0 ? "movem.w" : "movem.l";
            info.Args = address + ", " + DisassembleRegisterList1(registers);
            info.Length = pc - info.PC;
        }

        void LEA()
        {
            int mode = (Op >> 3) & 7;
            int sReg = (Op >> 0) & 7;
            int dReg = (Op >> 9) & 7;

            A[dReg].U32 = (uint)ReadAddress(mode, sReg);
            switch (mode)
            {
                case 2: PendingCycles -= 4; break;
                case 5: PendingCycles -= 8; break;
                case 6: PendingCycles -= 12; break;
                case 7:
                    switch (sReg)
                    {
                        case 0: PendingCycles -= 8; break;
                        case 1: PendingCycles -= 12; break;
                        case 2: PendingCycles -= 8; break;
                        case 3: PendingCycles -= 12; break;
                    }
                    break;
            }
        }

        void LEA_Disasm(DisassemblyInfo info)
        {
            var pc = info.PC + 2;
            int mode = (Op >> 3) & 7;
            int sReg = (Op >> 0) & 7;
            int dReg = (Op >> 9) & 7;

            info.Mnemonic = "lea";
            info.Args = DisassembleAddress(mode, sReg, ref pc);
            info.Args += ", A" + dReg;

            info.Length = pc - info.PC;
        }

        void CLR()
        {
            int size = (Op >> 6) & 3;
            int mode = (Op >> 3) & 7;
            int reg = (Op >> 0) & 7;

            switch (size)
            {
                case 0: WriteValueB(mode, reg, 0); PendingCycles -= mode == 0 ? 4 : 8 + EACyclesBW[mode, reg]; break;
                case 1:
                    WriteValueW(mode, reg, 0);
                    PendingCycles -= mode == 0 ? 4 : 8 + EACyclesBW[mode, reg]; break;
                case 2: WriteValueL(mode, reg, 0); PendingCycles -= mode == 0 ? 6 : 12 + EACyclesL[mode, reg]; break;
            }

            N = V = C = false;
            Z = true;
        }

        void CLR_Disasm(DisassemblyInfo info)
        {
            var pc = info.PC + 2;
            int size = (Op >> 6) & 3;
            int mode = (Op >> 3) & 7;
            int reg = (Op >> 0) & 7;

            switch (size)
            {
                case 0: info.Mnemonic = "clr.b"; info.Args = DisassembleValue(mode, reg, 1, ref pc); break;
                case 1: info.Mnemonic = "clr.w"; info.Args = DisassembleValue(mode, reg, 2, ref pc); break;
                case 2: info.Mnemonic = "clr.l"; info.Args = DisassembleValue(mode, reg, 4, ref pc); break;
            }
            info.Length = pc - info.PC;
        }

        void EXT()
        {
            int size = (Op >> 6) & 1;
            int reg = Op & 7;

            switch (size)
            {
                case 0: // ext.w
                    D[reg].S16 = D[reg].S8;
                    N = (D[reg].S16 & 0x8000) != 0;
                    Z = (D[reg].S16 == 0);
                    break;
                case 1: // ext.l
                    D[reg].S32 = D[reg].S16;
                    N = (D[reg].S32 & 0x80000000) != 0;
                    Z = (D[reg].S32 == 0);
                    break;
            }

            V = false;
            C = false;
            PendingCycles -= 4;
        }

        void EXT_Disasm(DisassemblyInfo info)
        {
            int size = (Op >> 6) & 1;
            int reg = Op & 7;

            switch (size)
            {
                case 0: info.Mnemonic = "ext.w"; info.Args = "D" + reg; break;
                case 1: info.Mnemonic = "ext.l"; info.Args = "D" + reg; break;
            }
        }

        void PEA()
        {
            int mode = (Op >> 3) & 7;
            int reg = (Op >> 0) & 7;
            uint ea = ReadAddress(mode, reg);

            A[7].U32 -= 4;
            _WriteLong(A[7].U32, ea);

            switch (mode)
            {
                case 2: PendingCycles -= 12; break;
                case 5: PendingCycles -= 16; break;
                case 6: PendingCycles -= 20; break;
                case 7:
                    switch (reg)
                    {
                        case 0: PendingCycles -= 16; break;
                        case 1: PendingCycles -= 20; break;
                        case 2: PendingCycles -= 16; break;
                        case 3: PendingCycles -= 20; break;
                    }
                    break;
            }
        }

        void PEA_Disasm(DisassemblyInfo info)
        {
            var pc = info.PC + 2;
            int mode = (Op >> 3) & 7;
            int reg = (Op >> 0) & 7;

            info.Mnemonic = "pea";
            info.Args = DisassembleAddress(mode, reg, ref pc);
            info.Length = pc - info.PC;
        }
    }
}
