using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PCMEmulator
{
    public enum MemoryAccess
    {
        Read = 1,
        Write = 2,
        Any = Read | Write
    }
}
