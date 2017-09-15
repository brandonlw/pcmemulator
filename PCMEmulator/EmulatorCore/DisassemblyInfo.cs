using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PCMEmulator
{
    /// <summary>
    /// DisassemblyInfo
    /// </summary>
    public sealed class DisassemblyInfo
    {
        /// <summary>
        /// PC
        /// </summary>
        public uint PC { get; set; }

        /// <summary>
        /// Mnemonic
        /// </summary>
        public string Mnemonic { get; set; }

        /// <summary>
        /// Args
        /// </summary>
        public string Args { get; set; }

        /// <summary>
        /// RawBytes
        /// </summary>
        public string RawBytes { get; set; }

        /// <summary>
        /// Length
        /// </summary>
        public uint Length { get; set; }

        /// <summary>
        /// ToString
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("{0:X6}: {3,-20}  {1,-8} {2}", PC, Mnemonic, Args, RawBytes);
        }
    }
}
