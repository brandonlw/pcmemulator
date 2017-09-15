using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PCMEmulator
{
    /// <summary>
    /// TimerFiredDelegate
    /// </summary>
    public delegate void TimerFiredDelegate();
    
    /// <summary>
    /// Emulator
    /// </summary>
    public partial class Emulator
    {
        private bool _insideInterrupt = false;
        private long[] _attoseconds_per_cycle = new long[2];
        private Timer _timedint_timer;
        private List<Timer> _timers;
        private ulong _totalcycles;           /* total CPU cycles executed */
        private AttoTime _globalBaseTime;
        private AttoTime _timedIntPeriod;
        private AttoTime _localTime; /* local time, relative to the timer system's global time */
        private EmulatorState _state = EmulatorState.None;

        private Timer callback_timer;
        private AttoTime callback_timer_expire_time;
        private bool callback_timer_modified;
        private Thread _th;
        private bool _s;
        private int _ramBytes;

        /// <summary>
        /// Creates an Emulator object with the specified image and settings.
        /// </summary>
        /// <param name="path"></param>
        /// <param name="ramBytes"></param>
        public Emulator(EmulatedDevice device, string path, int ramBytes)
        {
            _ramBytes = ramBytes;
            Device = device;
            Device.Emulator = this;
            Device.Init();
            D = new Register[8];
            A = new Register[8];
            Rom = File.ReadAllBytes(path);
            BreakpointAddresses = new List<uint>();
            AddressRangeWatchpoints = new List<Tuple<uint, uint, MemoryAccess>>();

            _timers = new List<Timer>();

            Opcodes = new Action[0x10000];
            BuildOpcodeTable();
        }

        /// <summary>
        /// EmulatorStateChanged
        /// </summary>
        public event EventHandler<EventArgs> EmulatorStateChanged;

        /// <summary>
        /// ValueRead
        /// </summary>
        public event EventHandler<ValueChangedEventArgs> ValueRead;

        /// <summary>
        /// ValueWritten
        /// </summary>
        public event EventHandler<ValueChangedEventArgs> ValueWritten;

        /// <summary>
        /// InsideInterruptChanged
        /// </summary>
        public event EventHandler<EventArgs> InsideInterruptChanged;

        /// <summary>
        /// CyclesRunning
        /// </summary>
        public static int CyclesRunning { get; set; }

        /// <summary>
        /// CyclesStolen
        /// </summary>
        public static int CyclesStolen { get; set; }

        /// <summary>
        /// CyclesPerSecond
        /// </summary>
        public static int CyclesPerSecond { get; set; }

        /// <summary>
        /// D
        /// </summary>
        public Register[] D { get; set; }

        /// <summary>
        /// A
        /// </summary>
        public Register[] A { get; set; }

        /// <summary>
        /// Breakpoint addresses.
        /// </summary>
        public List<uint> BreakpointAddresses { get; private set; }

        /// <summary>
        /// Address watchpoints.
        /// </summary>
        public List<Tuple<uint, uint, MemoryAccess>> AddressRangeWatchpoints { get; private set; }

        /// <summary>
        /// PC
        /// </summary>
        private uint _pc;
        public uint PC
        {
            get
            {
                return _pc;
            }

            set
            {
                if (value == 0x20002)
                {
                    value = value;
                }

                var changed = (_pc != value);

                _pc = value;

                if (changed)
                {
                    changed = changed;
                }
            }
        }
        
        /// <summary>
        /// PPC
        /// </summary>
        public uint PPC { get; set; }

        /// <summary>
        /// InterruptLevel
        /// </summary>
        public int InterruptLevel { get; set; }

        /// <summary>
        /// TotalExecutedCycles
        /// </summary>
        public ulong TotalExecutedCycles { get; private set; }

        /// <summary>
        /// InterruptCycles
        /// </summary>
        public int InterruptCycles { get; private set; }

        /// <summary>
        /// InterruptMaskLevel
        /// </summary>
        private int _interruptMaskLevel;
        public int InterruptMaskLevel
        {
            get
            {
                return _interruptMaskLevel;
            }

            set
            {
                _interruptMaskLevel = value;
            }
        }

        /// <summary>
        /// USP
        /// </summary>
        public int USP { get; set; }
        
        /// <summary>
        /// SSP
        /// </summary>
        public int SSP { get; set; }

        /// <summary>
        /// Stopped
        /// </summary>
        public bool Stopped { get; set; }

        /// <summary>
        /// Opcodes
        /// </summary>
        public Action[] Opcodes { get; set; }

        /// <summary>Extend Flag</summary>
        public bool X { get; set; }

        /// <summary>Negative Flag</summary>
        public bool N { get; set; }

        /// <summary>Zero Flag</summary>
        public bool Z { get; set; }

        /// <summary>Overflow Flag</summary>
        public bool V { get; set; }

        /// <summary>Carry Flag</summary>
        public bool C { get; set; }

        /// <summary>
        /// Op
        /// </summary>
        public ushort Op { get; set; }

        /// <summary>
        /// IsExitPending
        /// </summary>
        public bool IsExitPending { get; set; }

        /// <summary>
        /// ROM
        /// </summary>
        public byte[] Rom { get; set; }

        /// <summary>
        /// RAM
        /// </summary>
        public byte[] Ram { get; set; }

        /// <summary>
        /// Interrupt.
        /// </summary>
        public int Interrupt { get; set; }

        /// <summary>
        /// Device.
        /// </summary>
        public EmulatedDevice Device { get; private set; }

        /// <summary>
        /// Emulator state.
        /// </summary>
        public EmulatorState State
        {
            get
            {
                return _state;
            }

            set
            {
                var old = _state;
                _state = value;

                Stopped = (_state == EmulatorState.Stopped);

                if (old != value)
                {
                    if (EmulatorStateChanged != null)
                    {
                        EmulatorStateChanged(this, EventArgs.Empty);
                    }
                }
            }
        }

        /// <summary>
        /// PreInstructionCallback
        /// </summary>
        public Action<Emulator> PreInstructionCallback { get; set; }

        /// <summary>
        /// PostInstructionCallback
        /// </summary>
        public Action<Emulator> PostInstructionCallback { get; set; }

        /// <summary>
        /// Indicates whether to log PC changes.
        /// </summary>
        public bool LogPC { get; set; }

        /// <summary>
        /// PendingCycles
        /// </summary>
        public int PendingCycles { get; set; }

        /// <summary>Machine/Interrupt mode</summary>
        public bool M { get; set; }

        /// <summary>Supervisor/User mode</summary>
        public bool S
        {
            get
            {
                return _s;
            }

            set
            {
                if (value == _s)
                {
                    return;
                }

                if (value)
                {
                    // entering supervisor mode
                    //Console.WriteLine("&^&^&^&^& ENTER SUPERVISOR MODE");
                    USP = A[7].S32;
                    A[7].S32 = SSP;
                    _s = true;
                }
                else
                { // exiting supervisor mode
                    //Console.WriteLine("&^&^&^&^& LEAVE SUPERVISOR MODE");
                    SSP = A[7].S32;
                    A[7].S32 = USP;
                    _s = false;
                }
            }
        }

        public bool InsideInterrupt
        {
            get
            {
                return _insideInterrupt;
            }

            set
            {
                var changed = (_insideInterrupt != value);

                _insideInterrupt = value;

                if (changed)
                {
                    if (InsideInterruptChanged != null)
                    {
                        InsideInterruptChanged(this, EventArgs.Empty);
                    }
                }
            }
        }

        /// <summary>Status Register</summary>
        public short SR
        {
            get
            {
                short value = 0;
                if (C)
                {
                    value |= 0x0001;
                }

                if (V)
                {
                    value |= 0x0002;
                }

                if (Z)
                {
                    value |= 0x0004;
                }

                if (N)
                {
                    value |= 0x0008;
                }

                if (X)
                {
                    value |= 0x0010;
                }

                if (M)
                {
                    value |= 0x1000;
                }

                if (S)
                {
                    value |= 0x2000;
                }

                value |= (short)((InterruptMaskLevel & 7) << 8);
                return value;
            }

            set
            {
                C = (value & 0x0001) != 0;
                V = (value & 0x0002) != 0;
                Z = (value & 0x0004) != 0;
                N = (value & 0x0008) != 0;
                X = (value & 0x0010) != 0;
                M = (value & 0x1000) != 0;
                S = (value & 0x2000) != 0;
                InterruptMaskLevel = (value >> 8) & 7;
            }
        }

        /// <summary>
        /// Sort
        /// </summary>
        public void Sort()
        {
            int i1, i2, n1;
            n1 = _timers.Count;
            for (i2 = 1; i2 < n1; i2++)
            {
                for (i1 = 0; i1 < i2; i1++)
                {
                    if (_timers[i1].Expire > _timers[i2].Expire)
                    {
                        var temp = _timers[i1];
                        _timers[i1] = _timers[i2];
                        _timers[i2] = temp;
                    }
                }
            }
        }

        /// <summary>
        /// PulseReset
        /// </summary>
        public void PulseReset()
        {
            if (File.Exists("pc.txt"))
            {
                File.Delete("pc.txt");
            }

            Stopped = false;
            S = true;
            M = false;
            InterruptMaskLevel = 2;
            Interrupt = 0;
            A[7].S32 = _ReadOpLong(0);
            PC = (uint)_ReadOpLong(4);
        }

        /// <summary>
        /// Start
        /// </summary>
        public void Start()
        {
            InitializeTimer();
            _localTime = AttoTime.TIME_ZERO;

            _th = new Thread(_Run);
            _th.IsBackground = true;
            _th.Start();

            Task.Factory.StartNew(() =>
            {
                bool running = true;
                Thread.Sleep(30000);
                while (running)
                {
                    if (!Stopped && !InsideInterrupt)
                    {
                        Interrupt = (0x108 / 4) - 0x18;
                        InterruptMaskLevel = Interrupt - 1;
                    }

                    Thread.Sleep(2000);
                    running = false;
                }
            });
        }

        /// <summary>
        /// Stop
        /// </summary>
        public void Stop()
        {
            IsExitPending = true;
        }

        /// <summary>
        /// Step
        /// </summary>
        public void Step()
        {
            Op = (ushort)_ReadOpWord(PC);
            PC += 2;
            Opcodes[Op]();
        }

        /// <summary>
        /// ExecuteCycles
        /// </summary>
        /// <param name="cycles"></param>
        /// <returns></returns>
        public List<KeyValuePair<uint, string>> PCLog = new List<KeyValuePair<uint, string>>();
        public string LastPCLogString = string.Empty;
        public int ExecuteCycles(int cycles)
        { 
            if (!Stopped)
            {
                PendingCycles = cycles;
                int ran;
                PendingCycles -= InterruptCycles;
                InterruptCycles = 0;
                do
                {
                    int prevCycles = PendingCycles;
                    if (PC == 0x23C2C || PC == 0x2B26C)
                    {
                        PC += 2;
                    }

                    PPC = PC;
                    Op = (ushort)_ReadOpWord(PC);
                    if (PreInstructionCallback != null)
                    {
                        PreInstructionCallback(this);
                    }

                    if (!Stopped)
                    {
                        if (BreakpointAddresses.Contains(PC))
                        {
                            LogPC = true;
                            Debugger.Break();
                        }

                        if (!InsideInterrupt)
                        {
                            InsideInterrupt = InsideInterrupt;
                        }

                        if (LogPC)
                        {
                            lock (PCLog)
                            {
                                PCLog.Add(new KeyValuePair<uint, string>(PC, LastPCLogString));
                                LastPCLogString = string.Empty;
                            }
                        }

                        PC += 2;
                        Opcodes[Op]();
                        CheckInterrupts();

                        if (PostInstructionCallback != null)
                        {
                            PostInstructionCallback(this);
                        }

                        int delta = prevCycles - PendingCycles;
                        TotalExecutedCycles = TotalExecutedCycles + (ulong)delta;
                    }
                }
                while (!Stopped && PendingCycles > 0);
                PendingCycles -= InterruptCycles;
                InterruptCycles = 0;
                ran = cycles - PendingCycles;
                return ran;
            }

            PendingCycles = 0;
            InterruptCycles = 0;

            return cycles;
        }

        /// <summary>
        /// CheckInterrupts
        /// </summary>
        public void CheckInterrupts()
        {
            if (Interrupt > 0 && (Interrupt > InterruptMaskLevel || Interrupt > 7))
            {
                InsideInterrupt = true;
                Stopped = false;
                short sr = (short)SR;                  // capture current SR.
                S = true;                               // switch to supervisor mode, if not already in it.
                A[7].S32 -= 4;                          // Push PC on stack
                _WriteLong(A[7].U32, PC);
                A[7].S32 -= 2;                          // Push SR on stack
                _WriteWord(A[7].U32, sr);
                PC = (uint)_ReadLong((0x18 + Interrupt) * 4);    // Jump to interrupt vector
                if (PC == 0x57A)
                {
                    Debugger.Break();
                }
                InterruptMaskLevel = Interrupt;         // Set interrupt mask to level currently being entered
                Interrupt = 0;                          // "ack" interrupt. Note: this is wrong.
                InterruptCycles += 0x2c;
            }
        }

        /// <summary>
        /// SetInterruptLevel
        /// </summary>
        /// <param name="interrupt"></param>
        public void SetInterruptLevel(int interrupt)
        {
            Interrupt = interrupt;
            CheckInterrupts();
        }

        /// <summary>
        /// Empty_Event_Queue002
        /// </summary>
        public void Empty_Event_Queue002()
        {
            SetInterruptLevel(3);
        }

        /// <summary>
        /// SetInputLineAndVector002
        /// </summary>
        public void SetInputLineAndVector002()
        {
            TimerSetInterval(Empty_Event_Queue002, "Empty_Event_Queue002");
        }

        /// <summary>
        /// InitializeTimer
        /// </summary>
        public void InitializeTimer()
        {
            _globalBaseTime = AttoTime.TIME_ZERO;
            _timers = new List<Timer>();
        }

        /// <summary>
        /// TimerSetInterval
        /// </summary>
        /// <param name="callback"></param>
        /// <param name="func"></param>
        public void TimerSetInterval(TimerFiredDelegate callback, string func)
        {
            Timer timer = TimerAllocateCommon(callback, func, true);
            TimerAdjustPeriodic(timer, AttoTime.TIME_ZERO, new AttoTime(1, 1));
        }

        /// <summary>
        /// TimerListInsert
        /// </summary>
        /// <param name="timer1"></param>
        internal void TimerListInsert(Timer timer1)
        {
            int i1 = -1;
            foreach (var et in _timers)
            {
                if (et.Func == timer1.Func)
                {
                    i1 = _timers.IndexOf(et);
                    break;
                }
            }

            if (i1 == -1)
            {
                _timers.Add(timer1);
            }
        }

        /// <summary>
        /// TimerAllocateCommon
        /// </summary>
        /// <param name="callback"></param>
        /// <param name="func"></param>
        /// <param name="temp"></param>
        /// <returns></returns>
        internal Timer TimerAllocateCommon(TimerFiredDelegate callback, string func, bool temp)
        {
            AttoTime time = GetCurrentTime();
            var timer = new Timer();
            timer.Callback = callback;
            timer.Enabled = false;
            timer.Temporary = temp;
            timer.Period = AttoTime.TIME_ZERO;
            timer.Func = func;
            timer.Start = time;
            timer.Expire = AttoTime.TIME_NEVER;
            TimerListInsert(timer);
            return timer;
        }

        /// <summary>
        /// AbortTimeSlice
        /// </summary>
        public void AbortTimeSlice()
        {
            int current_icount;
            current_icount = PendingCycles + 1;
            CyclesStolen += current_icount;
            CyclesRunning -= current_icount;
            PendingCycles -= current_icount;
        }

        internal void TimerSetGlobalTime(AttoTime newbase)
        {
            Timer timer;
            _globalBaseTime = newbase;
            return;
            while (_timers[0].Expire <= _globalBaseTime)
            {
                bool was_enabled = _timers[0].Enabled;
                timer = _timers[0];
                if (timer.Period == AttoTime.TIME_ZERO || timer.Period == AttoTime.TIME_NEVER)
                {
                    timer.Enabled = false;
                }

                callback_timer_modified = false;
                callback_timer = timer;
                callback_timer_expire_time = timer.Expire;
                if (was_enabled && timer.Callback != null)
                {
                    timer.Callback();
                }

                callback_timer = null;
                if (callback_timer_modified == false)
                {
                    if (timer.Temporary)
                    {
                        _TimerListRemove(timer);
                    }
                    else
                    {
                        timer.Start = timer.Expire;
                        timer.Expire = timer.Expire + timer.Period;
                        Sort();
                    }
                }
            }
        }

        internal AttoTime GetCurrentTime()
        {
            if (callback_timer != null)
            {
                return callback_timer_expire_time;
            }

            return GetLocalTime();
        }

        internal AttoTime GetLocalTime()
        {
            AttoTime result;
            result = _localTime;
            int cycles;
            cycles = CyclesRunning - PendingCycles;
            result = result + new AttoTime(cycles / CyclesPerSecond, cycles * _attoseconds_per_cycle[0]);
            return result;
        }

        internal void TimerAdjustPeriodic(Timer which, AttoTime start_delay, AttoTime period)
        {
            AttoTime time = GetCurrentTime();
            if (which == callback_timer)
            {
                callback_timer_modified = true;
            }

            which.Enabled = true;
            if (start_delay.Seconds < 0)
            {
                start_delay = AttoTime.TIME_ZERO;
            }

            which.Start = time;
            which.Expire = time + start_delay;
            which.Period = period;
            Sort();
            if (_timers.IndexOf(which) == 0)
            {
                AbortTimeSlice();
            }
        }

        private sbyte _ReadOpByte(uint address)
        {
            var ret = Device.ReadOpByte(address);
            //_NotifyRead(address, ReadWriteAccessType.Byte, ret);

            return ret;
        }

        private sbyte _ReadByte(long address)
        {
            return _ReadByte((uint)address);
        }

        private sbyte _ReadByte(short address)
        {
            return _ReadByte((uint)address);
        }

        private sbyte _ReadByte(int address)
        {
            return _ReadByte((uint)address);
        }

        private sbyte _ReadByte(uint address)
        {
            var ret = Device.ReadByte(address);
            _NotifyRead(address, ReadWriteAccessType.Byte, (uint)ret);

            return ret;
        }

        private short _ReadOpWord(long address)
        {
            return _ReadOpWord((uint)address);
        }

        private short _ReadOpWord(uint address)
        {
            var ret = Device.ReadOpWord(address);
            //_NotifyRead(address, ReadWriteAccessType.Word, ret);

            return ret;
        }

        private short _ReadWord(long address)
        {
            return _ReadWord((uint)address);
        }

        private short _ReadWord(int address)
        {
            return _ReadWord((uint)address);
        }

        private short _ReadWord(short address)
        {
            return _ReadWord((uint)address);
        }

        private short _ReadWord(uint address)
        {
            var ret = Device.ReadWord(address & 0xFFFFFF);
            _NotifyRead((uint)address, ReadWriteAccessType.Word, (uint)ret);

            return ret;
        }

        private int _ReadOpLong(long address)
        {
            return _ReadOpLong((uint)address);
        }

        private int _ReadOpLong(short address)
        {
            return _ReadOpLong((uint)address);
        }

        private int _ReadOpLong(uint address)
        {
            var ret = Device.ReadOpLong(address & 0xFFFFFF);
            //_NotifyRead(address, ReadWriteAccessType.Long, ret);

            return ret;
        }

        private int _ReadLong(long address)
        {
            return _ReadLong((uint)address);
        }

        private int _ReadLong(uint address)
        {
            var ret = Device.ReadLong(address);
            _NotifyRead(address, ReadWriteAccessType.Long, (uint)ret);

            return ret;
        }

        private void _WriteByte(uint address, sbyte value)
        {
            _NotifyWrite(address, ReadWriteAccessType.Byte, (uint)value);
            Device.WriteByte(address, value);
        }

        private void _WriteWord(uint address, short value)
        {
            _NotifyWrite(address, ReadWriteAccessType.Word, (uint)value);
            Device.WriteWord(address, value);
        }

        private void _WriteLong(uint address, uint value)
        {
            _NotifyWrite(address, ReadWriteAccessType.Long, value);
            Device.WriteLong(address, value);
        }

        private void _NotifyRead(uint address, ReadWriteAccessType access, uint value)
        {
            if (ValueRead != null)
            {
                ValueRead(this, new ValueChangedEventArgs(address, access, value));
            }

            var ranges = AddressRangeWatchpoints.Where(t => t.Item1 <= address && t.Item2 >= address);
            if (ranges.Any())
            {
                if ((ranges.Any(t => (t.Item3 & MemoryAccess.Read) > 0)))
                {
                    Debugger.Break();
                }
            }
        }

        private void _NotifyWrite(uint address, ReadWriteAccessType access, uint value)
        {
            if (ValueWritten != null)
            {
                ValueWritten(this, new ValueChangedEventArgs(address, access, value));
            }

            var ranges = AddressRangeWatchpoints.Where(t => t.Item1 <= address && t.Item2 >= address);
            if (ranges.Any())
            {
                if ((ranges.Any(t => (t.Item3 & MemoryAccess.Write) > 0)))
                {
                    Debugger.Break();
                }
            }
        }

        private void _ResetCPU()
        {
            Ram = new byte[_ramBytes];
            SuccessiveResets++;
            LastSuccessfulTest = DateTime.Now;
            Device.Init();
            CyclesPerSecond = 12000000;
            _attoseconds_per_cycle[0] = AttoTime.SECONDS_PER_SECOND / CyclesPerSecond;
            InitTimers();
            InterruptLevel = 0;
            TotalExecutedCycles = 0;
            PendingCycles = 0;
            PulseReset();
        }

        private void _ExecuteTimeslice()
        {
            AttoTime target = _timers[0].Expire;
            AttoTime tbase = _globalBaseTime;
            int ran0;
            AttoTime at0 = target - _localTime;
            CyclesRunning = (int)((at0.Seconds * CyclesPerSecond) + (at0.AttoSeconds / _attoseconds_per_cycle[0]));
            if (true)//CyclesRunning > 0)
            {
                CyclesStolen = 0;
                ran0 = ExecuteCycles(CyclesRunning);
                ran0 -= CyclesStolen;
                _totalcycles += (ulong)ran0;
                _localTime = _localTime + new AttoTime((int)(ran0 / CyclesPerSecond), ran0 * _attoseconds_per_cycle[0]);
                if (_localTime < target)
                {
                    if (_localTime > tbase)
                    {
                        target = _localTime;
                    }
                    else
                    {
                        target = tbase;
                    }
                }
            }

            TimerSetGlobalTime(target);
        }

        private void _TimerListRemove(Timer timer1)
        {
            foreach (var et in _timers)
            {
                if (et.Func == timer1.Func)
                {
                    _timers.Remove(et);
                    break;
                }
            }
        }

        public DateTime? LastSuccessfulTest { get; set; }

        public int SuccessiveResets { get; set; }

        private void _Run()
        {
            _ResetCPU();
            while (!IsExitPending)
            {
                if (State == EmulatorState.Running)
                {
                    /*if (LastSuccessfulTest.HasValue && DateTime.Now > LastSuccessfulTest.Value.AddSeconds(20))
                    {
                        Console.WriteLine("Test threshold exceeded, resetting CPU...");
                        _ResetCPU();
                    }*/

                    _ExecuteTimeslice();
                }
                else
                {
                    //LastSuccessfulTest = null;
                }
            }
        }

        private void InitTimers()
        {
            _timedIntPeriod = new AttoTime(0, (long)(1e18 / 250));
            _timedint_timer = TimerAllocateCommon(_DoNothing, "cpunum_set_input_line_and_vector1002", false);
            TimerAdjustPeriodic(_timedint_timer, _timedIntPeriod, _timedIntPeriod);
        }

        private void _DoNothing()
        {
            SetInputLineAndVector002();
        }
    }
}
