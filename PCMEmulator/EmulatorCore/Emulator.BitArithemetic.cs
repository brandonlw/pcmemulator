using System;

namespace PCMEmulator
{
    /// <summary>
    /// Emulator
    /// </summary>
    public partial class Emulator
    {
        internal void AND0() // AND <ea>, Dn
        {
            int dstReg = (Op >> 9) & 0x07;
            int size = (Op >> 6) & 0x03;
            int srcMode = (Op >> 3) & 0x07;
            int srcReg = Op & 0x07;

            V = false;
            C = false;

            switch (size)
            {
                case 0: // Byte
                    D[dstReg].S8 &= ReadValueB(srcMode, srcReg);
                    PendingCycles -= (srcMode == 0) ? 4 : 4 + EACyclesBW[srcMode, srcReg];
                    N = (D[dstReg].S8 & 0x80) != 0;
                    Z = (D[dstReg].S8 == 0);
                    return;
                case 1: // Word
                    D[dstReg].S16 &= ReadValueW(srcMode, srcReg);
                    PendingCycles -= (srcMode == 0) ? 4 : 4 + EACyclesBW[srcMode, srcReg];
                    N = (D[dstReg].S16 & 0x8000) != 0;
                    Z = (D[dstReg].S16 == 0);
                    return;
                case 2: // Long
                    D[dstReg].S32 &= ReadValueL(srcMode, srcReg);
                    if (srcMode == 0 || (srcMode == 7 && srcReg == 4))
                    {
                        PendingCycles -= 8 + EACyclesL[srcMode, srcReg];
                    }
                    else
                    {
                        PendingCycles -= 6 + EACyclesL[srcMode, srcReg];
                    }

                    N = (D[dstReg].S32 & 0x80000000) != 0;
                    Z = (D[dstReg].S32 == 0);
                    return;
            }
        }

        internal void AND0_Disasm(DisassemblyInfo info)
        {
            int dstReg = (Op >> 9) & 0x07;
            int size = (Op >> 6) & 0x03;
            int srcMode = (Op >> 3) & 0x07;
            int srcReg = Op & 0x07;

            var pc = info.PC + 2;

            switch (size)
            {
                case 0: // Byte
                    info.Mnemonic = "and.b";
                    info.Args = string.Format("{0}, D{1}", DisassembleValue(srcMode, srcReg, 1, ref pc), dstReg);
                    break;
                case 1: // Word
                    info.Mnemonic = "and.w";
                    info.Args = string.Format("{0}, D{1}", DisassembleValue(srcMode, srcReg, 2, ref pc), dstReg);
                    break;
                case 2: // Long
                    info.Mnemonic = "and.l";
                    info.Args = string.Format("{0}, D{1}", DisassembleValue(srcMode, srcReg, 4, ref pc), dstReg);
                    break;
            }

            info.Length = pc - info.PC;
        }

        internal void AND1() // AND Dn, <ea>
        {
            int srcReg = (Op >> 9) & 0x07;
            int size = (Op >> 6) & 0x03;
            int dstMode = (Op >> 3) & 0x07;
            int dstReg = Op & 0x07;

            V = false;
            C = false;

            switch (size)
            {
                case 0: // Byte
                    {
                        sbyte dest = PeekValueB(dstMode, dstReg);
                        sbyte value = (sbyte)(dest & D[srcReg].S8);
                        WriteValueB(dstMode, dstReg, value);
                        PendingCycles -= (dstMode == 0) ? 4 : 8 + EACyclesBW[dstMode, dstReg];
                        N = (value & 0x80) != 0;
                        Z = (value == 0);
                        return;
                    }

                case 1: // Word
                    {
                        short dest = PeekValueW(dstMode, dstReg);
                        short value = (short)(dest & D[srcReg].S16);
                        WriteValueW(dstMode, dstReg, value);
                        PendingCycles -= (dstMode == 0) ? 4 : 8 + EACyclesBW[dstMode, dstReg];
                        N = (value & 0x8000) != 0;
                        Z = (value == 0);
                        return;
                    }

                case 2: // Long
                    {
                        int dest = PeekValueL(dstMode, dstReg);
                        uint value = (uint)(dest & D[srcReg].U32);
                        WriteValueL(dstMode, dstReg, value);
                        PendingCycles -= (dstMode == 0) ? 8 : 12 + EACyclesL[dstMode, dstReg];
                        N = (value & 0x80000000) != 0;
                        Z = (value == 0);
                        return;
                    }
            }
        }

        internal void AND1_Disasm(DisassemblyInfo info)
        {
            int srcReg = (Op >> 9) & 0x07;
            int size = (Op >> 6) & 0x03;
            int dstMode = (Op >> 3) & 0x07;
            int dstReg = Op & 0x07;

            var pc = info.PC + 2;

            switch (size)
            {
                case 0: // Byte
                    info.Mnemonic = "and.b";
                    info.Args = string.Format("D{0}, {1}", srcReg, DisassembleValue(dstMode, dstReg, 1, ref pc));
                    break;
                case 1: // Word
                    info.Mnemonic = "and.w";
                    info.Args = string.Format("D{0}, {1}", srcReg, DisassembleValue(dstMode, dstReg, 2, ref pc));
                    break;
                case 2: // Long
                    info.Mnemonic = "and.l";
                    info.Args = string.Format("D{0}, {1}", srcReg, DisassembleValue(dstMode, dstReg, 4, ref pc));
                    break;
            }

            info.Length = pc - info.PC;
        }

        internal void ANDI() // ANDI #<data>, <ea>
        {
            int size = (Op >> 6) & 0x03;
            int dstMode = (Op >> 3) & 0x07;
            int dstReg = Op & 0x07;

            V = false;
            C = false;

            switch (size)
            {
                case 0: // Byte
                    {
                        sbyte imm = (sbyte)_ReadOpWord(PC);
                        PC += 2;

                        sbyte arg = PeekValueB(dstMode, dstReg);
                        sbyte result = (sbyte)(imm & arg);
                        WriteValueB(dstMode, dstReg, result);
                        PendingCycles -= (dstMode == 0) ? 8 : 12 + EACyclesBW[dstMode, dstReg];
                        N = (result & 0x80) != 0;
                        Z = (result == 0);
                        return;
                    }

                case 1: // Word
                    {
                        short imm = _ReadOpWord(PC);
                        PC += 2;

                        short arg = PeekValueW(dstMode, dstReg);
                        short result = (short)(imm & arg);
                        WriteValueW(dstMode, dstReg, result);
                        PendingCycles -= (dstMode == 0) ? 8 : 12 + EACyclesBW[dstMode, dstReg];
                        N = (result & 0x8000) != 0;
                        Z = (result == 0);
                        return;
                    }

                case 2: // Long
                    {
                        int imm = _ReadOpLong(PC);
                        PC += 4;

                        int arg = PeekValueL(dstMode, dstReg);
                        uint result = (uint)(imm & arg);
                        WriteValueL(dstMode, dstReg, result);
                        PendingCycles -= (dstMode == 0) ? 14 : 20 + EACyclesL[dstMode, dstReg];
                        N = (result & 0x80000000) != 0;
                        Z = (result == 0);
                        return;
                    }
            }
        }

        internal void ANDI_Disasm(DisassemblyInfo info)
        {
            int size = ((Op >> 6) & 0x03);
            int dstMode = ((Op >> 3) & 0x07);
            int dstReg = (Op & 0x07);
            var pc = info.PC + 2;

            switch (size)
            {
                case 0: // Byte
                    {
                        info.Mnemonic = "andi.b";
                        sbyte imm = (sbyte)_ReadOpWord(pc);
                        pc += 2;

                        info.Args = string.Format("${0:X}, ", imm);
                        info.Args += DisassembleValue(dstMode, dstReg, 1, ref pc);
                        break;
                    }

                case 1: // Word
                    {
                        info.Mnemonic = "andi.w";
                        short imm = _ReadOpWord(pc);
                        pc += 2;

                        info.Args = string.Format("${0:X}, ", imm);
                        info.Args += DisassembleValue(dstMode, dstReg, 2, ref pc);
                        break;
                    }

                case 2: // Long
                    {
                        info.Mnemonic = "andi.l";
                        int imm = _ReadOpLong(pc);
                        pc += 4;

                        info.Args = string.Format("${0:X}, ", imm);
                        info.Args += DisassembleValue(dstMode, dstReg, 4, ref pc);
                        break;
                    }
            }

            info.Length = pc - info.PC;
        }

        internal void ANDI_CCR() //m68k_op_andi_16_toc         , 0xffff, 0x023c, { 20}
        {
            int value;
            value = _ReadOpWord(PC);
            PC += 2;

            X &= ((value & 0x10) != 0);
            N &= ((value & 0x08) != 0);
            Z &= ((value & 0x04) != 0);
            V &= ((value & 0x02) != 0);
            C &= ((value & 0x01) != 0);
            PendingCycles -= 20;
        }

        internal void ANDI_CCR_Disasm(DisassemblyInfo info)
        {
            var pc = info.PC + 2;
            info.Mnemonic = "andi";
            info.Args = DisassembleImmediate(1, ref pc) + ", CCR";
            info.Length = pc - info.PC;
        }

        internal void EOR() // EOR Dn, <ea>
        {
            int srcReg = (Op >> 9) & 0x07;
            int size = (Op >> 6) & 0x03;
            int dstMode = (Op >> 3) & 0x07;
            int dstReg = Op & 0x07;

            V = false;
            C = false;

            switch (size)
            {
                case 0: // Byte
                    {
                        sbyte dest = PeekValueB(dstMode, dstReg);
                        sbyte value = (sbyte)(dest ^ D[srcReg].S8);
                        WriteValueB(dstMode, dstReg, value);
                        PendingCycles -= (dstMode == 0) ? 4 : 8 + EACyclesBW[dstMode, dstReg];
                        N = (value & 0x80) != 0;
                        Z = (value == 0);
                        return;
                    }

                case 1: // Word
                    {
                        short dest = PeekValueW(dstMode, dstReg);
                        short value = (short)(dest ^ D[srcReg].S16);
                        WriteValueW(dstMode, dstReg, value);
                        PendingCycles -= (dstMode == 0) ? 4 : 8 + EACyclesBW[dstMode, dstReg];
                        N = (value & 0x8000) != 0;
                        Z = (value == 0);
                        return;
                    }

                case 2: // Long
                    {
                        int dest = PeekValueL(dstMode, dstReg);
                        uint value = (uint)(dest ^ D[srcReg].U32);
                        WriteValueL(dstMode, dstReg, value);
                        PendingCycles -= (dstMode == 0) ? 8 : 12 + EACyclesL[dstMode, dstReg];
                        N = (value & 0x80000000) != 0;
                        Z = (value == 0);
                        return;
                    }
            }
        }

        internal void EOR_Disasm(DisassemblyInfo info)
        {
            int srcReg = (Op >> 9) & 0x07;
            int size = (Op >> 6) & 0x03;
            int dstMode = (Op >> 3) & 0x07;
            int dstReg = Op & 0x07;

            var pc = info.PC + 2;

            switch (size)
            {
                case 0: // Byte
                    info.Mnemonic = "eor.b";
                    info.Args = string.Format("D{0}, {1}", srcReg, DisassembleValue(dstMode, dstReg, 1, ref pc));
                    break;

                case 1: // Word
                    info.Mnemonic = "eor.w";
                    info.Args = string.Format("D{0}, {1}", srcReg, DisassembleValue(dstMode, dstReg, 2, ref pc));
                    break;

                case 2: // Long
                    info.Mnemonic = "eor.l";
                    info.Args = string.Format("D{0}, {1}", srcReg, DisassembleValue(dstMode, dstReg, 4, ref pc));
                    break;
            }

            info.Length = pc - info.PC;
        }

        internal void EORI()
        {
            int size = (Op >> 6) & 3;
            int mode = (Op >> 3) & 7;
            int reg = (Op >> 0) & 7;

            V = false;
            C = false;

            switch (size)
            {
                case 0: // byte
                    {
                        sbyte immed = (sbyte)_ReadOpWord(PC);
                        PC += 2;

                        sbyte value = (sbyte)(PeekValueB(mode, reg) ^ immed);
                        WriteValueB(mode, reg, value);
                        N = (value & 0x80) != 0;
                        Z = value == 0;
                        PendingCycles -= mode == 0 ? 8 : 12 + EACyclesBW[mode, reg];
                        return;
                    }

                case 1: // word
                    {
                        short immed = _ReadOpWord(PC);
                        PC += 2;

                        short value = (short)(PeekValueW(mode, reg) ^ immed);
                        WriteValueW(mode, reg, value);
                        N = (value & 0x8000) != 0;
                        Z = value == 0;
                        PendingCycles -= mode == 0 ? 8 : 12 + EACyclesBW[mode, reg];
                        return;
                    }

                case 2: // long
                    {
                        uint immed = (uint)_ReadOpLong(PC);
                        PC += 4;

                        uint value = (uint)PeekValueL(mode, reg) ^ immed;
                        WriteValueL(mode, reg, value);
                        N = (value & 0x80000000) != 0;
                        Z = value == 0;
                        PendingCycles -= mode == 0 ? 16 : 20 + EACyclesL[mode, reg];
                        return;
                    }
            }
        }

        internal void EORI_Disasm(DisassemblyInfo info)
        {
            var pc = info.PC + 2;
            int size = (Op >> 6) & 3;
            int mode = (Op >> 3) & 7;
            int reg = (Op >> 0) & 7;

            switch (size)
            {
                case 0: // byte
                    {
                        info.Mnemonic = "eori.b";
                        sbyte immed = (sbyte)_ReadOpWord(pc);
                        pc += 2;

                        info.Args = string.Format("${0:X}, {1}", immed, DisassembleValue(mode, reg, 1, ref pc));
                        break;
                    }

                case 1: // word
                    {
                        info.Mnemonic = "eori.w";
                        short immed = _ReadOpWord(pc);
                        pc += 2;

                        info.Args = string.Format("${0:X}, {1}", immed, DisassembleValue(mode, reg, 2, ref pc));
                        break;
                    }

                case 2: // long
                    {
                        info.Mnemonic = "eori.l";
                        int immed = _ReadOpLong(pc);
                        pc += 4;

                        info.Args = string.Format("${0:X}, {1}", immed, DisassembleValue(mode, reg, 4, ref pc));
                        break;
                    }
            }

            info.Length = pc - info.PC;
        }

        internal void EORI_CCR() //m68k_op_eori_16_toc         , 0xffff, 0x0a3c, { 20}
        {
            //m68ki_set_ccr(m68ki_get_ccr() ^ m68ki_read_imm_16());
            int value;
            value = _ReadOpWord(PC);
            PC += 2;

            X ^= ((value & 0x10) != 0);
            N ^= ((value & 0x08) != 0);
            Z ^= ((value & 0x04) != 0);
            V ^= ((value & 0x02) != 0);
            C ^= ((value & 0x01) != 0);
            PendingCycles -= 20;
        }

        internal void EORI_CCR_Disasm(DisassemblyInfo info)
        {
            var pc = info.PC + 2;
            info.Mnemonic = "eori";
            info.Args = DisassembleImmediate(1, ref pc) + ", CCR";
            info.Length = pc - info.PC;
        }

        internal void OR0() // OR <ea>, Dn
        {
            int dstReg = (Op >> 9) & 0x07;
            int size = (Op >> 6) & 0x03;
            int srcMode = (Op >> 3) & 0x07;
            int srcReg = Op & 0x07;

            V = false;
            C = false;

            switch (size)
            {
                case 0: // Byte
                    D[dstReg].S8 |= ReadValueB(srcMode, srcReg);
                    PendingCycles -= (srcMode == 0) ? 4 : 4 + EACyclesBW[srcMode, srcReg];
                    N = (D[dstReg].S8 & 0x80) != 0;
                    Z = (D[dstReg].S8 == 0);
                    return;

                case 1: // Word
                    D[dstReg].S16 |= ReadValueW(srcMode, srcReg);
                    PendingCycles -= (srcMode == 0) ? 4 : 4 + EACyclesBW[srcMode, srcReg];
                    N = (D[dstReg].S16 & 0x8000) != 0;
                    Z = (D[dstReg].S16 == 0);
                    return;

                case 2: // Long
                    D[dstReg].S32 |= ReadValueL(srcMode, srcReg);
                    if (srcMode == 0 || (srcMode == 7 && srcReg == 4))
                    {
                        PendingCycles -= 8 + EACyclesL[srcMode, srcReg];
                    }
                    else
                    {
                        PendingCycles -= 6 + EACyclesL[srcMode, srcReg];
                    }

                    N = (D[dstReg].S32 & 0x80000000) != 0;
                    Z = (D[dstReg].S32 == 0);
                    return;
            }
        }

        internal void OR0_Disasm(DisassemblyInfo info)
        {
            int dstReg = (Op >> 9) & 0x07;
            int size = (Op >> 6) & 0x03;
            int srcMode = (Op >> 3) & 0x07;
            int srcReg = Op & 0x07;

            var pc = info.PC + 2;

            switch (size)
            {
                case 0: // Byte
                    info.Mnemonic = "or.b";
                    info.Args = string.Format("{0}, D{1}", DisassembleValue(srcMode, srcReg, 1, ref pc), dstReg);
                    break;
                case 1: // Word
                    info.Mnemonic = "or.w";
                    info.Args = string.Format("{0}, D{1}", DisassembleValue(srcMode, srcReg, 2, ref pc), dstReg);
                    break;
                case 2: // Long
                    info.Mnemonic = "or.l";
                    info.Args = string.Format("{0}, D{1}", DisassembleValue(srcMode, srcReg, 4, ref pc), dstReg);
                    break;
            }

            info.Length = pc - info.PC;
        }

        internal void OR1() // OR Dn, <ea>
        {
            int srcReg = (Op >> 9) & 0x07;
            int size = (Op >> 6) & 0x03;
            int dstMode = (Op >> 3) & 0x07;
            int dstReg = Op & 0x07;

            V = false;
            C = false;

            switch (size)
            {
                case 0: // Byte
                    {
                        sbyte dest = PeekValueB(dstMode, dstReg);
                        sbyte value = (sbyte)(dest | D[srcReg].S8);
                        WriteValueB(dstMode, dstReg, value);
                        PendingCycles -= (dstMode == 0) ? 4 : 8 + EACyclesBW[dstMode, dstReg];
                        N = (value & 0x80) != 0;
                        Z = (value == 0);
                        return;
                    }

                case 1: // Word
                    {
                        short dest = PeekValueW(dstMode, dstReg);
                        short value = (short)(dest | D[srcReg].S16);
                        WriteValueW(dstMode, dstReg, value);
                        PendingCycles -= (dstMode == 0) ? 4 : 8 + EACyclesBW[dstMode, dstReg];
                        N = (value & 0x8000) != 0;
                        Z = (value == 0);
                        return;
                    }

                case 2: // Long
                    {
                        uint dest = (uint)PeekValueL(dstMode, dstReg);
                        uint value = (uint)(dest | D[srcReg].U32);
                        WriteValueL(dstMode, dstReg, value);
                        PendingCycles -= (dstMode == 0) ? 8 : 12 + EACyclesL[dstMode, dstReg];
                        N = (value & 0x80000000) != 0;
                        Z = (value == 0);
                        return;
                    }
            }
        }

        internal void OR1_Disasm(DisassemblyInfo info)
        {
            int srcReg = (Op >> 9) & 0x07;
            int size = (Op >> 6) & 0x03;
            int dstMode = (Op >> 3) & 0x07;
            int dstReg = Op & 0x07;

            var pc = info.PC + 2;

            switch (size)
            {
                case 0: // Byte
                    info.Mnemonic = "or.b";
                    info.Args = string.Format("D{0}, {1}", srcReg, DisassembleValue(dstMode, dstReg, 1, ref pc));
                    break;
                case 1: // Word
                    info.Mnemonic = "or.w";
                    info.Args = string.Format("D{0}, {1}", srcReg, DisassembleValue(dstMode, dstReg, 2, ref pc));
                    break;
                case 2: // Long
                    info.Mnemonic = "or.l";
                    info.Args = string.Format("D{0}, {1}", srcReg, DisassembleValue(dstMode, dstReg, 4, ref pc));
                    break;
            }

            info.Length = pc - info.PC;
        }        

        internal void ORI()
        {
            int size = (Op >> 6) & 3;
            int mode = (Op >> 3) & 7;
            int reg = (Op >> 0) & 7;

            V = false;
            C = false;

            switch (size)
            {
                case 0: // byte
                    {
                        sbyte immed = (sbyte)_ReadOpWord(PC);
                        PC += 2;
                        sbyte value = (sbyte)(PeekValueB(mode, reg) | immed);
                        WriteValueB(mode, reg, value);
                        N = (value & 0x80) != 0;
                        Z = value == 0;
                        PendingCycles -= mode == 0 ? 8 : 12 + EACyclesBW[mode, reg];
                        return;
                    }

                case 1: // word
                    {
                        short immed = _ReadOpWord(PC);
                        PC += 2;
                        short value = (short)(PeekValueW(mode, reg) | immed);
                        WriteValueW(mode, reg, value);
                        N = (value & 0x8000) != 0;
                        Z = value == 0;
                        PendingCycles -= mode == 0 ? 8 : 12 + EACyclesBW[mode, reg];
                        return;
                    }

                case 2: // long
                    {
                        int immed = _ReadOpLong(PC);
                        PC += 4;
                        var value = (uint)PeekValueL(mode, reg) | (uint)immed;
                        WriteValueL(mode, reg, value);
                        N = (value & 0x80000000) != 0;
                        Z = value == 0;
                        PendingCycles -= mode == 0 ? 16 : 20 + EACyclesL[mode, reg];
                        return;
                    }
            }
        }

        void ORI_Disasm(DisassemblyInfo info)
        {
            var pc = info.PC + 2;
            int size = (Op >> 6) & 3;
            int mode = (Op >> 3) & 7;
            int reg = (Op >> 0) & 7;

            switch (size)
            {
                case 0: // byte
                    {
                        info.Mnemonic = "ori.b";
                        sbyte immed = (sbyte)_ReadOpWord(pc);
                        pc += 2;
                        info.Args = String.Format("${0:X}, {1}", immed, DisassembleValue(mode, reg, 1, ref pc));
                        break;
                    }

                case 1: // word
                    {
                        info.Mnemonic = "ori.w";
                        short immed = _ReadOpWord(pc);
                        pc += 2;
                        info.Args = String.Format("${0:X}, {1}", immed, DisassembleValue(mode, reg, 2, ref pc));
                        break;
                    }

                case 2: // long
                    {
                        info.Mnemonic = "ori.l";
                        int immed = _ReadOpLong(pc);
                        pc += 4;
                        info.Args = String.Format("${0:X}, {1}", immed, DisassembleValue(mode, reg, 4, ref pc));
                        break;
                    }
            }

            info.Length = pc - info.PC;
        }

        internal void ORI_CCR() //m68k_op_ori_16_toc          , 0xffff, 0x003c, { 20}
        {
            int value;
            value = _ReadOpWord(PC);
            PC += 2;
            X |= ((value & 0x10) != 0);
            N |= ((value & 0x08) != 0);
            Z |= ((value & 0x04) != 0);
            V |= ((value & 0x02) != 0);
            C |= ((value & 0x01) != 0);
            PendingCycles -= 20;
        }

        void ORI_CCR_Disasm(DisassemblyInfo info)
        {
            var pc = info.PC + 2;
            info.Mnemonic = "ori";
            info.Args = DisassembleImmediate(1, ref pc) + ", CCR";
            info.Length = pc - info.PC;
        }

        void NOT()
        {
            int size = (Op >> 6) & 0x03;
            int mode = (Op >> 3) & 0x07;
            int reg = Op & 0x07;

            V = false;
            C = false;

            switch (size)
            {
                case 0: // Byte
                    {
                        sbyte value = PeekValueB(mode, reg);
                        value = (sbyte)~value;
                        WriteValueB(mode, reg, value);
                        PendingCycles -= (mode == 0) ? 4 : 8 + EACyclesBW[mode, reg];
                        N = (value & 0x80) != 0;
                        Z = (value == 0);
                        return;
                    }
                case 1: // Word
                    {
                        short value = PeekValueW(mode, reg);
                        value = (short)~value;
                        WriteValueW(mode, reg, value);
                        PendingCycles -= (mode == 0) ? 4 : 8 + EACyclesBW[mode, reg];
                        N = (value & 0x8000) != 0;
                        Z = (value == 0);
                        return;
                    }
                case 2: // Long
                    {
                        var value = (uint)PeekValueL(mode, reg);
                        value = ~value;
                        WriteValueL(mode, reg, value);
                        PendingCycles -= (mode == 0) ? 6 : 12 + EACyclesL[mode, reg]; //8:12
                        N = (value & 0x80000000) != 0;
                        Z = (value == 0);
                        return;
                    }
            }
        }

        void NOT_Disasm(DisassemblyInfo info)
        {
            int size = (Op >> 6) & 0x03;
            int mode = (Op >> 3) & 0x07;
            int reg = Op & 0x07;

            var pc = info.PC + 2;

            switch (size)
            {
                case 0: // Byte
                    info.Mnemonic = "not.b";
                    info.Args = DisassembleValue(mode, reg, 1, ref pc);
                    break;
                case 1: // Word
                    info.Mnemonic = "not.w";
                    info.Args = DisassembleValue(mode, reg, 2, ref pc);
                    break;
                case 2: // Long
                    info.Mnemonic = "not.l";
                    info.Args = DisassembleValue(mode, reg, 4, ref pc);
                    break;
            }

            info.Length = pc - info.PC;
        }

        void LSLd()
        {
            int rot = (Op >> 9) & 7;
            int size = (Op >> 6) & 3;
            int m = (Op >> 5) & 1;
            int reg = Op & 7;

            if (m == 0 && rot == 0) rot = 8;
            else if (m == 1) rot = D[rot].S32 & 63;

            V = false;
            C = false;

            switch (size)
            {
                case 0: // byte
                    for (int i = 0; i < rot; i++)
                    {
                        C = X = (D[reg].U8 & 0x80) != 0;
                        D[reg].U8 <<= 1;
                    }
                    N = (D[reg].S8 & 0x80) != 0;
                    Z = D[reg].U8 == 0;
                    PendingCycles -= 6 + (rot * 2);
                    return;
                case 1: // word
                    for (int i = 0; i < rot; i++)
                    {
                        C = X = (D[reg].U16 & 0x8000) != 0;
                        D[reg].U16 <<= 1;
                    }
                    N = (D[reg].S16 & 0x8000) != 0;
                    Z = D[reg].U16 == 0;
                    PendingCycles -= 6 + (rot * 2);
                    return;
                case 2: // long
                    for (int i = 0; i < rot; i++)
                    {
                        C = X = (D[reg].U32 & 0x80000000) != 0;
                        D[reg].U32 <<= 1;
                    }
                    N = (D[reg].S32 & 0x80000000) != 0;
                    Z = D[reg].U32 == 0;
                    PendingCycles -= 8 + (rot * 2);
                    return;
            }
        }

        void LSLd_Disasm(DisassemblyInfo info)
        {
            var pc = info.PC + 2;
            int rot = (Op >> 9) & 7;
            int size = (Op >> 6) & 3;
            int m = (Op >> 5) & 1;
            int reg = Op & 7;

            if (m == 0 && rot == 0) rot = 8;

            switch (size)
            {
                case 0: info.Mnemonic = "lsl.b";
                    break;

                case 1: info.Mnemonic = "lsl.w";
                    break;

                case 2: info.Mnemonic = "lsl.l";
                    break;
            }

            if (m == 0)
                info.Args = rot + ", D" + reg;
            else
                info.Args = "D" + rot + ", D" + reg;

            info.Length = pc - info.PC;
        }

        void LSLd0()
        {
            //m68k_op_lsl_16_ai           , 0xfff8, 0xe3d0, { 12}
            //m68k_op_lsl_16_pi           , 0xfff8, 0xe3d8, { 12}
            //m68k_op_lsl_16_pd           , 0xfff8, 0xe3e0, { 14}
            //m68k_op_lsl_16_di           , 0xfff8, 0xe3e8, { 16}
            //m68k_op_lsl_16_ix           , 0xfff8, 0xe3f0, { 18}
            //m68k_op_lsl_16_aw           , 0xffff, 0xe3f8, { 16}
            //m68k_op_lsl_16_al           , 0xffff, 0xe3f9, { 20}
            int mode = (Op >> 3) & 0x07;
            int reg = Op & 0x07;
            int src;
            ushort res;
            src = PeekValueW(mode, reg);
            res = (ushort)(src << 1);
            WriteValueW(mode, reg, (short)res);
            N = ((res & 0x8000) != 0);
            Z = (res == 0);
            X = C = ((res & 0x8000) != 0);
            V = false;
            PendingCycles -= 8 + EACyclesBW[mode, reg];
        }

        void LSLd0_Disasm(DisassemblyInfo info)
        {
            var pc = info.PC + 2;
            int mode = (Op >> 3) & 0x07;
            int reg = Op & 0x07;
            info.Mnemonic = "lsl";
            info.Args = DisassembleValue(mode, reg, 1, ref pc);
            info.Length = pc - info.PC;
        }

        void LSRd()
        {
            int rot = (Op >> 9) & 7;
            int size = (Op >> 6) & 3;
            int m = (Op >> 5) & 1;
            int reg = Op & 7;

            if (m == 0 && rot == 0) rot = 8;
            else if (m == 1) rot = D[rot].S32 & 63;

            V = false;
            C = false;

            switch (size)
            {
                case 0: // byte
                    for (int i = 0; i < rot; i++)
                    {
                        C = X = (D[reg].U8 & 1) != 0;
                        D[reg].U8 >>= 1;
                    }
                    N = (D[reg].S8 & 0x80) != 0;
                    Z = D[reg].U8 == 0;
                    PendingCycles -= 6 + (rot * 2);
                    return;
                case 1: // word
                    for (int i = 0; i < rot; i++)
                    {
                        C = X = (D[reg].U16 & 1) != 0;
                        D[reg].U16 >>= 1;
                    }
                    N = (D[reg].S16 & 0x8000) != 0;
                    Z = D[reg].U16 == 0;
                    PendingCycles -= 6 + (rot * 2);
                    return;
                case 2: // long
                    for (int i = 0; i < rot; i++)
                    {
                        C = X = (D[reg].U32 & 1) != 0;
                        D[reg].U32 >>= 1;
                    }
                    N = (D[reg].S32 & 0x80000000) != 0;
                    Z = D[reg].U32 == 0;
                    PendingCycles -= 8 + (rot * 2);
                    return;
            }
        }

        void LSRd_Disasm(DisassemblyInfo info)
        {
            var pc = info.PC + 2;
            int rot = (Op >> 9) & 7;
            int size = (Op >> 6) & 3;
            int m = (Op >> 5) & 1;
            int reg = Op & 7;

            if (m == 0 && rot == 0) rot = 8;

            switch (size)
            {
                case 0: info.Mnemonic = "lsr.b";
                    break;

                case 1: info.Mnemonic = "lsr.w";
                    break;

                case 2: info.Mnemonic = "lsr.l";
                    break;
            }
            if (m == 0) info.Args = rot + ", D" + reg;
            else info.Args = "D" + rot + ", D" + reg;

            info.Length = pc - info.PC;
        }

        void LSRd0()
        {
            //m68k_op_lsr_16_ai           , 0xfff8, 0xe2d0, { 12}
            //m68k_op_lsr_16_pi           , 0xfff8, 0xe2d8, { 12}
            //m68k_op_lsr_16_pd           , 0xfff8, 0xe2e0, { 14}
            //m68k_op_lsr_16_di           , 0xfff8, 0xe2e8, { 16}
            //m68k_op_lsr_16_ix           , 0xfff8, 0xe2f0, { 18}
            //m68k_op_lsr_16_aw           , 0xffff, 0xe2f8, { 16}
            //m68k_op_lsr_16_al           , 0xffff, 0xe2f9, { 20}
            int mode = (Op >> 3) & 0x07;
            int reg = Op & 0x07;
            int src;
            ushort res;
            src = PeekValueW(mode, reg);
            res = (ushort)(src >> 1);
            WriteValueW(mode, reg, (short)res);
            N = false;
            Z = (res == 0);
            C = X = ((src & 0x10000) != 0);
            V = false;
            PendingCycles -= 8 + EACyclesBW[mode, reg];
        }

        void LSRd0_Disasm(DisassemblyInfo info)
        {
            var pc = info.PC + 2;
            int mode = (Op >> 3) & 0x07;
            int reg = Op & 0x07;
            info.Mnemonic = "lsr";
            info.Args = DisassembleValue(mode, reg, 1, ref pc);
            info.Length = pc - info.PC;
        }

        void ASLd()
        {
            int rot = (Op >> 9) & 7;
            int size = (Op >> 6) & 3;
            int m = (Op >> 5) & 1;
            int reg = Op & 7;

            if (m == 0 && rot == 0) rot = 8;
            else if (m == 1) rot = D[rot].S32 & 63;

            V = false;
            C = false;

            switch (size)
            {
                case 0: // byte
                    for (int i = 0; i < rot; i++)
                    {
                        bool msb = D[reg].S8 < 0;
                        C = X = (D[reg].U8 & 0x80) != 0;
                        D[reg].S8 <<= 1;
                        V |= (D[reg].S8 < 0) != msb;
                    }
                    N = (D[reg].S8 & 0x80) != 0;
                    Z = D[reg].U8 == 0;
                    PendingCycles -= 6 + (rot * 2);
                    return;
                case 1: // word
                    for (int i = 0; i < rot; i++)
                    {
                        bool msb = D[reg].S16 < 0;
                        C = X = (D[reg].U16 & 0x8000) != 0;
                        D[reg].S16 <<= 1;
                        V |= (D[reg].S16 < 0) != msb;
                    }
                    N = (D[reg].S16 & 0x8000) != 0;
                    Z = D[reg].U16 == 0;
                    PendingCycles -= 6 + (rot * 2);
                    return;
                case 2: // long
                    for (int i = 0; i < rot; i++)
                    {
                        bool msb = D[reg].S32 < 0;
                        C = X = (D[reg].U32 & 0x80000000) != 0;
                        D[reg].S32 <<= 1;
                        V |= (D[reg].S32 < 0) != msb;
                    }
                    N = (D[reg].S32 & 0x80000000) != 0;
                    Z = D[reg].U32 == 0;
                    PendingCycles -= 8 + (rot * 2);
                    return;
            }
        }

        void ASLd_Disasm(DisassemblyInfo info)
        {
            var pc = info.PC + 2;
            int rot = (Op >> 9) & 7;
            int size = (Op >> 6) & 3;
            int m = (Op >> 5) & 1;
            int reg = Op & 7;

            if (m == 0 && rot == 0) rot = 8;

            switch (size)
            {
                case 0: info.Mnemonic = "asl.b";
                    break;

                case 1: info.Mnemonic = "asl.w";
                    break;

                case 2: info.Mnemonic = "asl.l";
                    break;
            }
            if (m == 0) info.Args = rot + ", D" + reg;
            else info.Args = "D" + rot + ", D" + reg;

            info.Length = pc - info.PC;
        }

        void ASLd0()
        {
            //m68k_op_asl_16_ai           , 0xfff8, 0xe1d0, { 12}
            //m68k_op_asl_16_pi           , 0xfff8, 0xe1d8, { 12}
            //m68k_op_asl_16_pd           , 0xfff8, 0xe1e0, { 14}
            //m68k_op_asl_16_di           , 0xfff8, 0xe1e8, { 16}
            //m68k_op_asl_16_ix           , 0xfff8, 0xe1f0, { 18}
            //m68k_op_asl_16_aw           , 0xffff, 0xe1f8, { 16}
            //m68k_op_asl_16_al           , 0xffff, 0xe1f9, { 20}
            int mode = (Op >> 3) & 0x07;
            int reg = Op & 0x07;
            int src;
            ushort res;
            src = PeekValueW(mode, reg);
            res = (ushort)(src << 1);
            WriteValueW(mode, reg, (short)res);
            N = ((res & 0x8000) != 0);
            Z = (res == 0);
            X = C = ((src & 0x8000) != 0);
            src &= 0xc000;
            V = !(src == 0 || src == 0xc000);
            PendingCycles -= 8 + EACyclesBW[mode, reg];
        }

        void ASLd0_Disasm(DisassemblyInfo info)
        {
            var pc = info.PC + 2;
            int mode = (Op >> 3) & 0x07;
            int reg = Op & 0x07;
            info.Mnemonic = "asl";
            info.Args = DisassembleValue(mode, reg, 1, ref pc);
            info.Length = pc - info.PC;
        }
        
        void ASRd()
        {
            int rot = (Op >> 9) & 7;
            int size = (Op >> 6) & 3;
            int m = (Op >> 5) & 1;
            int reg = Op & 7;

            if (m == 0 && rot == 0) rot = 8;
            else if (m == 1) rot = D[rot].S32 & 63;

            V = false;
            C = false;

            switch (size)
            {
                case 0: // byte
                    for (int i = 0; i < rot; i++)
                    {
                        bool msb = D[reg].S8 < 0;
                        C = X = (D[reg].U8 & 1) != 0;
                        D[reg].S8 >>= 1;
                        V |= (D[reg].S8 < 0) != msb;
                    }
                    N = (D[reg].S8 & 0x80) != 0;
                    Z = D[reg].U8 == 0;
                    PendingCycles -= 6 + (rot * 2);
                    return;
                case 1: // word
                    for (int i = 0; i < rot; i++)
                    {
                        bool msb = D[reg].S16 < 0;
                        C = X = (D[reg].U16 & 1) != 0;
                        D[reg].S16 >>= 1;
                        V |= (D[reg].S16 < 0) != msb;
                    }
                    N = (D[reg].S16 & 0x8000) != 0;
                    Z = D[reg].U16 == 0;
                    PendingCycles -= 6 + (rot * 2);
                    return;
                case 2: // long
                    for (int i = 0; i < rot; i++)
                    {
                        bool msb = D[reg].S32 < 0;
                        C = X = (D[reg].U32 & 1) != 0;
                        D[reg].S32 >>= 1;
                        V |= (D[reg].S32 < 0) != msb;
                    }
                    N = (D[reg].S32 & 0x80000000) != 0;
                    Z = D[reg].U32 == 0;
                    PendingCycles -= 8 + (rot * 2);
                    return;
            }
        }

        void ASRd_Disasm(DisassemblyInfo info)
        {
            var pc = info.PC + 2;
            int rot = (Op >> 9) & 7;
            int size = (Op >> 6) & 3;
            int m = (Op >> 5) & 1;
            int reg = Op & 7;

            if (m == 0 && rot == 0) rot = 8;

            switch (size)
            {
                case 0: info.Mnemonic = "asr.b";
                    break;

                case 1: info.Mnemonic = "asr.w";
                    break;

                case 2: info.Mnemonic = "asr.l";
                    break;
            }

            if (m == 0)
                info.Args = rot + ", D" + reg;
            else
                info.Args = "D" + rot + ", D" + reg;

            info.Length = pc - info.PC;
        }

        void ASRd0()
        {
            //m68k_op_asr_16_ai           , 0xfff8, 0xe0d0, { 12}
            //m68k_op_asr_16_pi           , 0xfff8, 0xe0d8, { 12}
            //m68k_op_asr_16_pd           , 0xfff8, 0xe0e0, { 14}
            //m68k_op_asr_16_di           , 0xfff8, 0xe0e8, { 16}
            //m68k_op_asr_16_ix           , 0xfff8, 0xe0f0, { 18}
            //m68k_op_asr_16_aw           , 0xffff, 0xe0f8, { 16}
            //m68k_op_asr_16_al           , 0xffff, 0xe0f9, { 20}
            int mode = (Op >> 3) & 0x07;
            int reg = Op & 0x07;
            int src;
            ushort res;
            src = PeekValueW(mode, reg);
            res = (ushort)(src >> 1);
            if ((src & 0x8000) != 0)
            {
                res |= 0x8000;
            }
            WriteValueW(mode, reg, (short)res);
            N = ((res & 0x8000) != 0);
            Z = (res == 0);
            V = false;
            C = X = ((src & 0x01) != 0);
            PendingCycles -= 8 + EACyclesBW[mode, reg];
        }

        void ASRd0_Disasm(DisassemblyInfo info)
        {
            var pc = info.PC + 2;
            int mode = (Op >> 3) & 0x07;
            int reg = Op & 0x07;
            info.Mnemonic = "asr";
            info.Args = DisassembleValue(mode, reg, 1, ref pc);
            info.Length = pc - info.PC;
        }

        void ROLd()
        {
            int rot = (Op >> 9) & 7;
            int size = (Op >> 6) & 3;
            int m = (Op >> 5) & 1;
            int reg = Op & 7;

            if (m == 0 && rot == 0) rot = 8;
            else if (m == 1) rot = D[rot].S32 & 63;

            V = false;
            C = false;

            switch (size)
            {
                case 0: // byte
                    for (int i = 0; i < rot; i++)
                    {
                        C = (D[reg].U8 & 0x80) != 0;
                        D[reg].U8 = (byte)((D[reg].U8 << 1) | (D[reg].U8 >> 7));
                    }
                    N = (D[reg].S8 & 0x80) != 0;
                    Z = D[reg].U8 == 0;
                    PendingCycles -= 6 + (rot * 2);
                    return;
                case 1: // word
                    for (int i = 0; i < rot; i++)
                    {
                        C = (D[reg].U16 & 0x8000) != 0;
                        D[reg].U16 = (ushort)((D[reg].U16 << 1) | (D[reg].U16 >> 15));
                    }
                    N = (D[reg].S16 & 0x8000) != 0;
                    Z = D[reg].U16 == 0;
                    PendingCycles -= 6 + (rot * 2);
                    return;
                case 2: // long
                    for (int i = 0; i < rot; i++)
                    {
                        C = (D[reg].U32 & 0x80000000) != 0;
                        D[reg].U32 = ((D[reg].U32 << 1) | (D[reg].U32 >> 31));
                    }
                    N = (D[reg].S32 & 0x80000000) != 0;
                    Z = D[reg].U32 == 0;
                    PendingCycles -= 8 + (rot * 2);
                    return;
            }
        }

        void ROLd_Disasm(DisassemblyInfo info)
        {
            var pc = info.PC + 2;
            int rot = (Op >> 9) & 7;
            int size = (Op >> 6) & 3;
            int m = (Op >> 5) & 1;
            int reg = Op & 7;

            if (m == 0 && rot == 0) rot = 8;

            switch (size)
            {
                case 0: info.Mnemonic = "rol.b";
                    break;

                case 1: info.Mnemonic = "rol.w";
                    break;

                case 2: info.Mnemonic = "rol.l";
                    break;
            }

            if (m == 0)
                info.Args = rot + ", D" + reg;
            else
                info.Args = "D" + rot + ", D" + reg;

            info.Length = pc - info.PC;
        }

        void ROLd0()
        {
            //m68k_op_rol_16_ai           , 0xfff8, 0xe7d0, { 12}
            //m68k_op_rol_16_pi           , 0xfff8, 0xe7d8, { 12}
            //m68k_op_rol_16_pd           , 0xfff8, 0xe7e0, { 14}
            //m68k_op_rol_16_di           , 0xfff8, 0xe7e8, { 16}
            //m68k_op_rol_16_ix           , 0xfff8, 0xe7f0, { 18}
            //m68k_op_rol_16_aw           , 0xffff, 0xe7f8, { 16}
            //m68k_op_rol_16_al           , 0xffff, 0xe7f9, { 20}
            int mode = (Op >> 3) & 0x07;
            int reg = Op & 0x07;
            int src;
            uint res;
            src = PeekValueW(mode, reg);
            res = (uint)(((src << 1) | (src >> 15)) & 0xffff);
            WriteValueW(mode, reg, (short)res);
            N = ((res & 0x8000) != 0);
            Z = (res == 0);
            C = ((res & 0x8000) != 0);
            V = false;
            PendingCycles -= 8 + EACyclesBW[mode, reg];
        }

        void ROLd0_Disasm(DisassemblyInfo info)
        {
            var pc = info.PC + 2;
            int mode = (Op >> 3) & 0x07;
            int reg = Op & 0x07;
            info.Mnemonic = "rol";
            info.Args = DisassembleValue(mode, reg, 1, ref pc);
            info.Length = pc - info.PC;
        }

        void RORd()
        {
            int rot = (Op >> 9) & 7;
            int size = (Op >> 6) & 3;
            int m = (Op >> 5) & 1;
            int reg = Op & 7;

            if (m == 0 && rot == 0) rot = 8;
            else if (m == 1) rot = D[rot].S32 & 63;

            V = false;
            C = false;

            switch (size)
            {
                case 0: // byte
                    for (int i = 0; i < rot; i++)
                    {
                        C = (D[reg].U8 & 1) != 0;
                        D[reg].U8 = (byte)((D[reg].U8 >> 1) | (D[reg].U8 << 7));
                    }
                    N = (D[reg].S8 & 0x80) != 0;
                    Z = D[reg].U8 == 0;
                    PendingCycles -= 6 + (rot * 2);
                    return;
                case 1: // word
                    for (int i = 0; i < rot; i++)
                    {
                        C = (D[reg].U16 & 1) != 0;
                        D[reg].U16 = (ushort)((D[reg].U16 >> 1) | (D[reg].U16 << 15));
                    }
                    N = (D[reg].S16 & 0x8000) != 0;
                    Z = D[reg].U16 == 0;
                    PendingCycles -= 6 + (rot * 2);
                    return;
                case 2: // long
                    for (int i = 0; i < rot; i++)
                    {
                        C = (D[reg].U32 & 1) != 0;
                        D[reg].U32 = ((D[reg].U32 >> 1) | (D[reg].U32 << 31));
                    }
                    N = (D[reg].S32 & 0x80000000) != 0;
                    Z = D[reg].U32 == 0;
                    PendingCycles -= 8 + (rot * 2);
                    return;
            }
        }

        void RORd_Disasm(DisassemblyInfo info)
        {
            var pc = info.PC + 2;
            int rot = (Op >> 9) & 7;
            int size = (Op >> 6) & 3;
            int m = (Op >> 5) & 1;
            int reg = Op & 7;

            if (m == 0 && rot == 0) rot = 8;

            switch (size)
            {
                case 0: info.Mnemonic = "ror.b";
                    break;

                case 1: info.Mnemonic = "ror.w";
                    break;

                case 2: info.Mnemonic = "ror.l";
                    break;
            }

            if (m == 0)
                info.Args = rot + ", D" + reg;
            else
                info.Args = "D" + rot + ", D" + reg;

            info.Length = pc - info.PC;
        }

        void RORd0()
        {
            //m68k_op_ror_16_ai           , 0xfff8, 0xe6d0, { 12}
            //m68k_op_ror_16_pi           , 0xfff8, 0xe6d8, { 12}
            //m68k_op_ror_16_pd           , 0xfff8, 0xe6e0, { 14}
            //m68k_op_ror_16_di           , 0xfff8, 0xe6e8, { 16}
            //m68k_op_ror_16_ix           , 0xfff8, 0xe6f0, { 18}
            //m68k_op_ror_16_aw           , 0xffff, 0xe6f8, { 16}
            //m68k_op_ror_16_al           , 0xffff, 0xe6f9, { 20}
            int mode = (Op >> 3) & 0x07;
            int reg = Op & 0x07;
            int src;
            uint res;
            src = PeekValueW(mode, reg);
            res = (uint)(((src >> 1) | (src << 15)) & 0xffff);
            WriteValueW(mode, reg, (short)res);
            N = ((res & 0x8000) != 0);
            Z = (res == 0);
            C = ((res & 0x01) != 0);
            V = false;
            PendingCycles -= 8 + EACyclesBW[mode, reg];
        }

        void RORd0_Disasm(DisassemblyInfo info)
        {
            var pc = info.PC + 2;
            int mode = (Op >> 3) & 0x07;
            int reg = Op & 0x07;
            info.Mnemonic = "ror";
            info.Args = DisassembleValue(mode, reg, 1, ref pc);
            info.Length = pc - info.PC;
        }

        void ROXLd()
        {
            int rot = (Op >> 9) & 7;
            int size = (Op >> 6) & 3;
            int m = (Op >> 5) & 1;
            int reg = Op & 7;

            if (m == 0 && rot == 0) rot = 8;
            else if (m == 1) rot = D[rot].S32 & 63;

            C = X;
            V = false;

            switch (size)
            {
                case 0: // byte
                    for (int i = 0; i < rot; i++)
                    {
                        C = (D[reg].U8 & 0x80) != 0;
                        D[reg].U8 = (byte)((D[reg].U8 << 1) | (X ? 1 : 0));
                        X = C;
                    }
                    N = (D[reg].S8 & 0x80) != 0;
                    Z = D[reg].S8 == 0;
                    PendingCycles -= 6 + (rot * 2);
                    return;
                case 1: // word
                    for (int i = 0; i < rot; i++)
                    {
                        C = (D[reg].U16 & 0x8000) != 0;
                        D[reg].U16 = (ushort)((D[reg].U16 << 1) | (X ? 1 : 0));
                        X = C;
                    }
                    N = (D[reg].S16 & 0x8000) != 0;
                    Z = D[reg].S16 == 0;
                    PendingCycles -= 6 + (rot * 2);
                    return;
                case 2: // long
                    for (int i = 0; i < rot; i++)
                    {
                        C = (D[reg].S32 & 0x80000000) != 0;
                        D[reg].S32 = ((D[reg].S32 << 1) | (X ? 1 : 0));
                        X = C;
                    }
                    N = (D[reg].S32 & 0x80000000) != 0;
                    Z = D[reg].S32 == 0;
                    PendingCycles -= 8 + (rot * 2);
                    return;
            }
        }

        void ROXLd_Disasm(DisassemblyInfo info)
        {
            var pc = info.PC + 2;
            int rot = (Op >> 9) & 7;
            int size = (Op >> 6) & 3;
            int m = (Op >> 5) & 1;
            int reg = Op & 7;

            if (m == 0 && rot == 0) rot = 8;

            switch (size)
            {
                case 0: info.Mnemonic = "roxl.b";
                    break;
                    
                case 1: info.Mnemonic = "roxl.w";
                    break;

                case 2: info.Mnemonic = "roxl.l";
                    break;
            }
            if (m == 0) info.Args = rot + ", D" + reg;
            else info.Args = "D" + rot + ", D" + reg;

            info.Length = pc - info.PC;
        }

        void ROXLd0()
        {
            //m68k_op_roxl_16_ai          , 0xfff8, 0xe5d0, { 12}
            //m68k_op_roxl_16_pi          , 0xfff8, 0xe5d8, { 12}
            //m68k_op_roxl_16_pd          , 0xfff8, 0xe5e0, { 14}
            //m68k_op_roxl_16_di          , 0xfff8, 0xe5e8, { 16}
            //m68k_op_roxl_16_ix          , 0xfff8, 0xe5f0, { 18}
            //m68k_op_roxl_16_aw          , 0xffff, 0xe5f8, { 16}
            //m68k_op_roxl_16_al          , 0xffff, 0xe5f9, { 20}
            int mode = (Op >> 3) & 0x07;
            int reg = Op & 0x07;
            ushort src;
            uint src2;
            uint res;
            src = (ushort)PeekValueW(mode, reg);
            src2 = (uint)(src | (X ? 0x10000 : 0));
            res = ((src2 << 1) | (src2 >> 16));
            C = X = (((res >> 8) & 0x100) != 0);
            WriteValueW(mode, reg, (short)res);
            N = ((res & 0x8000) != 0);
            Z = (res == 0);
            V = false;
            PendingCycles -= 8 + EACyclesBW[mode, reg];
        }

        void ROXLd0_Disasm(DisassemblyInfo info)
        {
            var pc = info.PC + 2;
            int mode = (Op >> 3) & 0x07;
            int reg = Op & 0x07;
            info.Mnemonic = "roxl";
            info.Args = DisassembleValue(mode, reg, 1, ref pc);
            info.Length = pc - info.PC;
        }

        void ROXRd()
        {
            int rot = (Op >> 9) & 7;
            int size = (Op >> 6) & 3;
            int m = (Op >> 5) & 1;
            int reg = Op & 7;

            if (m == 0 && rot == 0) rot = 8;
            else if (m == 1) rot = D[rot].S32 & 63;

            C = X;
            V = false;

            switch (size)
            {
                case 0: // byte
                    for (int i = 0; i < rot; i++)
                    {
                        C = (D[reg].U8 & 1) != 0;
                        D[reg].U8 = (byte)((D[reg].U8 >> 1) | (X ? 0x80 : 0));
                        X = C;
                    }
                    N = (D[reg].S8 & 0x80) != 0;
                    Z = D[reg].S8 == 0;
                    PendingCycles -= 6 + (rot * 2);
                    return;
                case 1: // word
                    for (int i = 0; i < rot; i++)
                    {
                        C = (D[reg].U16 & 1) != 0;
                        D[reg].U16 = (ushort)((D[reg].U16 >> 1) | (X ? 0x8000 : 0));
                        X = C;
                    }
                    N = (D[reg].S16 & 0x8000) != 0;
                    Z = D[reg].S16 == 0;
                    PendingCycles -= 6 + (rot * 2);
                    return;
                case 2: // long
                    for (int i = 0; i < rot; i++)
                    {
                        C = (D[reg].S32 & 1) != 0;
                        D[reg].U32 = ((D[reg].U32 >> 1) | (X ? 0x80000000 : 0));
                        X = C;
                    }
                    N = (D[reg].S32 & 0x80000000) != 0;
                    Z = D[reg].S32 == 0;
                    PendingCycles -= 8 + (rot * 2);
                    return;
            }
        }

        void ROXRd_Disasm(DisassemblyInfo info)
        {
            var pc = info.PC + 2;
            int rot = (Op >> 9) & 7;
            int size = (Op >> 6) & 3;
            int m = (Op >> 5) & 1;
            int reg = Op & 7;

            if (m == 0 && rot == 0) rot = 8;

            switch (size)
            {
                case 0: info.Mnemonic = "roxr.b"; break;
                case 1: info.Mnemonic = "roxr.w"; break;
                case 2: info.Mnemonic = "roxr.l"; break;
            }
            if (m == 0) info.Args = rot + ", D" + reg;
            else info.Args = "D" + rot + ", D" + reg;

            info.Length = pc - info.PC;
        }

        void ROXRd0()
        {
            //m68k_op_roxr_16_ai          , 0xfff8, 0xe4d0, { 12}
            //m68k_op_roxr_16_pi          , 0xfff8, 0xe4d8, { 12}
            //m68k_op_roxr_16_pd          , 0xfff8, 0xe4e0, { 14}
            //m68k_op_roxr_16_di          , 0xfff8, 0xe4e8, { 16}
            //m68k_op_roxr_16_ix          , 0xfff8, 0xe4f0, { 18}
            //m68k_op_roxr_16_aw          , 0xffff, 0xe4f8, { 16}
            //m68k_op_roxr_16_al          , 0xffff, 0xe4f9, { 20}
            int mode = (Op >> 3) & 0x07;
            int reg = Op & 0x07;
            ushort src;
            uint src2;
            uint res;
            src = (ushort)PeekValueW(mode, reg);
            src2 = ((uint)src | (uint)(X ? 0x10000 : 0));
            res = ((src2 >> 1) | (src2 << 16));
            C = X = ((res & 0x10000) != 0);
            res = res & 0xffff;
            WriteValueW(mode, reg, (short)res);
            N = ((res & 0x8000) != 0);
            Z = (res == 0);
            V = false;
            PendingCycles -= 8 + EACyclesBW[mode, reg];
        }

        void ROXRd0_Disasm(DisassemblyInfo info)
        {
            var pc = info.PC + 2;
            int mode = (Op >> 3) & 0x07;
            int reg = Op & 0x07;
            info.Mnemonic = "roxr";
            info.Args = DisassembleValue(mode, reg, 1, ref pc);
            info.Length = pc - info.PC;
        }

        void SWAP()
        {
            int reg = Op & 7;
            D[reg].U32 = (D[reg].U32 << 16) | (D[reg].U32 >> 16);
            V = C = false;
            Z = D[reg].U32 == 0;
            N = (D[reg].S32 & 0x80000000) != 0;
            PendingCycles -= 4;
        }

        void SWAP_Disasm(DisassemblyInfo info)
        {
            int reg = Op & 7;
            info.Mnemonic = "swap";
            info.Args = "D" + reg;
        }
    }
}
