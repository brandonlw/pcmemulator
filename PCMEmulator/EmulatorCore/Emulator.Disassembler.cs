using System.Text;

namespace PCMEmulator
{
    partial class Emulator
    {
        public DisassemblyInfo Disassemble(uint pc)
        {
            var info = new DisassemblyInfo { Mnemonic = "UNKNOWN", PC = pc, Length = 2 };
            Op = (ushort)_ReadOpWord(pc);

            if (Opcodes[Op] == MOVE) MOVE_Disasm(info);
            else if (Opcodes[Op] == MOVEA) MOVEA_Disasm(info);
            else if (Opcodes[Op] == MOVEQ) MOVEQ_Disasm(info);
            else if (Opcodes[Op] == MOVEM0) MOVEM0_Disasm(info);
            else if (Opcodes[Op] == MOVEM1) MOVEM1_Disasm(info);
            else if (Opcodes[Op] == LEA) LEA_Disasm(info);
            else if (Opcodes[Op] == CLR) CLR_Disasm(info);
            else if (Opcodes[Op] == EXT) EXT_Disasm(info);
            else if (Opcodes[Op] == PEA) PEA_Disasm(info);
            else if (Opcodes[Op] == ANDI) ANDI_Disasm(info);
            else if (Opcodes[Op] == ANDI_CCR) ANDI_CCR_Disasm(info);
            else if (Opcodes[Op] == EORI) EORI_Disasm(info);
            else if (Opcodes[Op] == EORI_CCR) EORI_CCR_Disasm(info);
            else if (Opcodes[Op] == ORI) ORI_Disasm(info);
            else if (Opcodes[Op] == ORI_CCR) ORI_CCR_Disasm(info);
            else if (Opcodes[Op] == ASLd) ASLd_Disasm(info);
            else if (Opcodes[Op] == ASRd) ASRd_Disasm(info);
            else if (Opcodes[Op] == LSLd) LSLd_Disasm(info);
            else if (Opcodes[Op] == LSRd) LSRd_Disasm(info);
            else if (Opcodes[Op] == ROXLd) ROXLd_Disasm(info);
            else if (Opcodes[Op] == ROXRd) ROXRd_Disasm(info);
            else if (Opcodes[Op] == ROLd) ROLd_Disasm(info);
            else if (Opcodes[Op] == RORd) RORd_Disasm(info);
            else if (Opcodes[Op] == ASLd0) ASLd0_Disasm(info);
            else if (Opcodes[Op] == ASRd0) ASRd0_Disasm(info);
            else if (Opcodes[Op] == LSLd0) LSLd0_Disasm(info);
            else if (Opcodes[Op] == LSRd0) LSRd0_Disasm(info);
            else if (Opcodes[Op] == ROXLd0) ROXLd0_Disasm(info);
            else if (Opcodes[Op] == ROXRd0) ROXRd0_Disasm(info);
            else if (Opcodes[Op] == ROLd0) ROLd0_Disasm(info);
            else if (Opcodes[Op] == RORd0) RORd0_Disasm(info);
            else if (Opcodes[Op] == SWAP) SWAP_Disasm(info);
            else if (Opcodes[Op] == AND0) AND0_Disasm(info);
            else if (Opcodes[Op] == AND1) AND1_Disasm(info);
            else if (Opcodes[Op] == EOR) EOR_Disasm(info);
            else if (Opcodes[Op] == OR0) OR0_Disasm(info);
            else if (Opcodes[Op] == OR1) OR1_Disasm(info);
            else if (Opcodes[Op] == NOT) NOT_Disasm(info);
            else if (Opcodes[Op] == NEG) NEG_Disasm(info);
            else if (Opcodes[Op] == JMP) JMP_Disasm(info);
            else if (Opcodes[Op] == JSR) JSR_Disasm(info);
            else if (Opcodes[Op] == Bcc) Bcc_Disasm(info);
            else if (Opcodes[Op] == BRA) BRA_Disasm(info);
            else if (Opcodes[Op] == BSR) BSR_Disasm(info);
            else if (Opcodes[Op] == DBcc) DBcc_Disasm(info);
            else if (Opcodes[Op] == Scc) Scc_Disasm(info);
            else if (Opcodes[Op] == RTE) RTE_Disasm(info);
            else if (Opcodes[Op] == RTS) RTS_Disasm(info);
            else if (Opcodes[Op] == RTR) RTR_Disasm(info);
            else if (Opcodes[Op] == TST) TST_Disasm(info);
            else if (Opcodes[Op] == BTSTi) BTSTi_Disasm(info);
            else if (Opcodes[Op] == BTSTr) BTSTr_Disasm(info);
            else if (Opcodes[Op] == BCHGi) BCHGi_Disasm(info);
            else if (Opcodes[Op] == BCHGr) BCHGr_Disasm(info);
            else if (Opcodes[Op] == BCLRi) BCLRi_Disasm(info);
            else if (Opcodes[Op] == BCLRr) BCLRr_Disasm(info);
            else if (Opcodes[Op] == BSETi) BSETi_Disasm(info);
            else if (Opcodes[Op] == BSETr) BSETr_Disasm(info);
            else if (Opcodes[Op] == LINK) LINK_Disasm(info);
            else if (Opcodes[Op] == UNLK) UNLK_Disasm(info);
            else if (Opcodes[Op] == RESET) RESET_Disasm(info);
            else if (Opcodes[Op] == NOP) NOP_Disasm(info);
            else if (Opcodes[Op] == ADD0) ADD_Disasm(info);
            else if (Opcodes[Op] == ADD1) ADD_Disasm(info);
            else if (Opcodes[Op] == ADDA) ADDA_Disasm(info);
            else if (Opcodes[Op] == ADDI) ADDI_Disasm(info);
            else if (Opcodes[Op] == ADDQ) ADDQ_Disasm(info);
            else if (Opcodes[Op] == SUB0) SUB_Disasm(info);
            else if (Opcodes[Op] == SUB1) SUB_Disasm(info);
            else if (Opcodes[Op] == SUBA) SUBA_Disasm(info);
            else if (Opcodes[Op] == SUBI) SUBI_Disasm(info);
            else if (Opcodes[Op] == SUBQ) SUBQ_Disasm(info);
            else if (Opcodes[Op] == CMP) CMP_Disasm(info);
            else if (Opcodes[Op] == CMPM) CMPM_Disasm(info);
            else if (Opcodes[Op] == CMPA) CMPA_Disasm(info);
            else if (Opcodes[Op] == CMPI) CMPI_Disasm(info);
            else if (Opcodes[Op] == MULU_WORD) MULU_Disasm(info);
            else if (Opcodes[Op] == MULS) MULS_Disasm(info);
            else if (Opcodes[Op] == DIVU_WORD) DIVU_Disasm(info);
            else if (Opcodes[Op] == DIVS) DIVS_Disasm(info);
            else if (Opcodes[Op] == MOVEtSR) MOVEtSR_Disasm(info);
            else if (Opcodes[Op] == MOVEfSR) MOVEfSR_Disasm(info);
            else if (Opcodes[Op] == MOVEUSP) MOVEUSP_Disasm(info);
            else if (Opcodes[Op] == ANDI_SR) ANDI_SR_Disasm(info);
            else if (Opcodes[Op] == EORI_SR) EORI_SR_Disasm(info);
            else if (Opcodes[Op] == ORI_SR) ORI_SR_Disasm(info);
            else if (Opcodes[Op] == MOVECCR) MOVECCR_Disasm(info);
            else if (Opcodes[Op] == TRAP) TRAP_Disasm(info);
            else if (Opcodes[Op] == NBCD) NBCD_Disasm(info);
            else if (Opcodes[Op] == ILLEGAL) ILLEGAL_Disasm(info);
            else if (Opcodes[Op] == STOP) STOP_Disasm(info);
            else if (Opcodes[Op] == TRAPV) TRAPV_Disasm(info);
            else if (Opcodes[Op] == CHK) CHK_Disasm(info);
            else if (Opcodes[Op] == NEGX) NEGX_Disasm(info);
            else if (Opcodes[Op] == SBCD0) SBCD0_Disasm(info);
            else if (Opcodes[Op] == SBCD1) SBCD1_Disasm(info);
            else if (Opcodes[Op] == ABCD0) ABCD0_Disasm(info);
            else if (Opcodes[Op] == ABCD1) ABCD1_Disasm(info);
            else if (Opcodes[Op] == EXGdd) EXGdd_Disasm(info);
            else if (Opcodes[Op] == EXGaa) EXGaa_Disasm(info);
            else if (Opcodes[Op] == EXGda) EXGda_Disasm(info);
            else if (Opcodes[Op] == TAS) TAS_Disasm(info);
            else if (Opcodes[Op] == MOVEP) MOVEP_Disasm(info);
            else if (Opcodes[Op] == ADDX0) ADDX0_Disasm(info);
            else if (Opcodes[Op] == ADDX1) ADDX1_Disasm(info);
            else if (Opcodes[Op] == SUBX0) SUBX0_Disasm(info);
            else if (Opcodes[Op] == SUBX1) SUBX1_Disasm(info);
            else if (Opcodes[Op] == ILL) ILL_Disasm(info);

            var sb = new StringBuilder();
            for (var p = info.PC; p < info.PC + info.Length; p += 2)
            {
                sb.AppendFormat("{0:X4} ", _ReadOpWord(p));
            }

            info.RawBytes = sb.ToString();
            return info;
        }
    }
}