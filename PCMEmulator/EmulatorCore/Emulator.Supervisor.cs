using System;

namespace PCMEmulator
{
    public partial class Emulator
    {
        void MOVEtSR()
        {
            int mode = (Op >> 3) & 7;
            int reg = (Op >> 0) & 7;
            if (S == false)
            {
                //throw new Exception("Write to SR when not in supervisor mode. supposed to trap or something...");
                TrapVector2(8);
            }
            else
            {
                SR = ReadValueW(mode, reg);
            }

            PendingCycles -= 12 + EACyclesBW[mode, reg];
        }

        void MOVEtSR_Disasm(DisassemblyInfo info)
        {
            var pc = info.PC + 2;
            int mode = (Op >> 3) & 7;
            int reg = (Op >> 0) & 7;
            info.Mnemonic = "move";
            info.Args = DisassembleValue(mode, reg, 2, ref pc) + ", SR";
            info.Length = pc - info.PC;
        }

        void MOVEfSR()
        {
            int mode = (Op >> 3) & 7;
            int reg = (Op >> 0) & 7;
            WriteValueW(mode, reg, (short)SR);
            PendingCycles -= (mode == 0) ? 6 : 8 + EACyclesBW[mode, reg];
        }

        void MOVEfSR_Disasm(DisassemblyInfo info)
        {
            var pc = info.PC + 2;
            int mode = (Op >> 3) & 7;
            int reg = (Op >> 0) & 7;
            info.Mnemonic = "move";
            info.Args = "SR, " + DisassembleValue(mode, reg, 2, ref pc);
            info.Length = pc - info.PC;
        }

        void MOVEUSP()
        {
            int dir = (Op >> 3) & 1;
            int reg = Op & 7;
            if (S == false)
            {
                //throw new Exception("MOVE to USP when not supervisor. needs to trap");
                TrapVector2(8);
            }
            else
            {
                if (dir == 0)
                {
                    USP = A[reg].S32;
                }
                else
                {
                    A[reg].S32 = USP;
                }
            }
            PendingCycles -= 4;
        }

        void MOVEUSP_Disasm(DisassemblyInfo info)
        {
            var pc = info.PC + 2;
            int dir = (Op >> 3) & 1;
            int reg = Op & 7;
            info.Mnemonic = "move";
            info.Args = (dir == 0) ? ("A" + reg + ", USP") : ("USP, A" + reg);
            info.Length = pc - info.PC;
        }

        void ANDI_SR()
        {
            if (S == false)
                throw new Exception("trap!");
            SR &= _ReadOpWord(PC); PC += 2;
            PendingCycles -= 20;
        }

        void ANDI_SR_Disasm(DisassemblyInfo info)
        {
            var pc = info.PC + 2;
            info.Mnemonic = "andi";
            info.Args = DisassembleImmediate(2, ref pc) + ", SR";
            info.Length = pc - info.PC;
        }

        void EORI_SR()
        {
            if (S == false)
                throw new Exception("trap!");
            SR ^= _ReadOpWord(PC); PC += 2;
            PendingCycles -= 20;
        }

        void EORI_SR_Disasm(DisassemblyInfo info)
        {
            var pc = info.PC + 2;
            info.Mnemonic = "eori";
            info.Args = DisassembleImmediate(2, ref pc) + ", SR";
            info.Length = pc - info.PC;
        }

        void ORI_SR()
        {
            if (S == false)
                throw new Exception("trap!");
            SR |= _ReadOpWord(PC); PC += 2;
            PendingCycles -= 20;
        }

        void ORI_SR_Disasm(DisassemblyInfo info)
        {
            var pc = info.PC + 2;
            info.Mnemonic = "ori";
            info.Args = DisassembleImmediate(2, ref pc) + ", SR";
            info.Length = pc - info.PC;
        }

        void MOVECCR()
        {
            int mode = (Op >> 3) & 7;
            int reg = (Op >> 0) & 7;

            ushort sr = (ushort)(SR & 0xFF00);
            sr |= (byte)ReadValueB(mode, reg);
            SR = (short)sr;
            PendingCycles -= 12 + EACyclesBW[mode, reg];
        }

        void MOVEC()
        {
            bool to_control = (Op & 1) > 0;
            var extra = _ReadOpWord(PC); PC += 2;
            bool addrreg = (extra & 0x8000) > 0;
            int reg = (extra >> 12) & 7;
            int ctrl = extra & 0xFFF;

            if (to_control)
            {
                switch (ctrl)
                {
                    case 0x801:
                        {
                            //Vector base register
                            if (addrreg)
                            {
                                //VBR = A[reg].U32;
                            }
                            else
                            {
                                //VBR = D[reg].U32;
                            }

                            break;
                        }
                    default:
                        {
                            TrapVector2(4);
                            break;
                        }
                }
            }
            else
            {

            }
        }

        void MOVECCR_Disasm(DisassemblyInfo info)
        {
            var pc = info.PC + 2;
            int mode = (Op >> 3) & 7;
            int reg = (Op >> 0) & 7;
            info.Mnemonic = "move";
            info.Args = DisassembleValue(mode, reg, 2, ref pc) + ", CCR";
            info.Length = pc - info.PC;
        }

        void TRAP()
        {
            int vector = 32 + (Op & 0x0F);
            TrapVector(vector);
            PendingCycles -= 4;
        }

        void TRAP_Disasm(DisassemblyInfo info)
        {
            info.Mnemonic = "trap";
            info.Args = string.Format("${0:X}", Op & 0xF);
        }

        void TrapVector(int vector)
        {
            short sr = (short)SR;        // capture current SR.
            S = true;                    // switch to supervisor mode, if not already in it.
            A[7].S32 -= 4;               // Push PC on stack
            _WriteLong(A[7].U32, PC);
            A[7].S32 -= 2;               // Push SR on stack
            _WriteWord(A[7].U32, sr);
            PC = (uint)_ReadLong(vector * 4);   // Jump to vector
            PendingCycles -= CyclesException[vector];
        }

        void TrapVector2(int vector)
        {
            short sr = (short)SR;        // capture current SR.
            S = true;                    // switch to supervisor mode, if not already in it.
            A[7].S32 -= 4;               // Push PPC on stack
            _WriteLong(A[7].U32, PPC);
            A[7].S32 -= 2;               // Push SR on stack
            _WriteWord(A[7].U32, sr);
            PC = (uint)_ReadLong(vector * 4);   // Jump to vector
            PendingCycles -= CyclesException[vector];
        }
    }
}
