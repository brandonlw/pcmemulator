using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Forms;

namespace PCMEmulator
{
    /// <summary>
    /// Main
    /// </summary>
    public partial class Main : Form
    {
        private Emulator _emulator;

        /// <summary>
        /// Default constructor
        /// </summary>
        public Main()
        {
            InitializeComponent();
        }

        private void Main_Load(object sender, EventArgs e)
        {
            uctDebugger.Parent = this;
        }

        private void Main_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (_emulator != null)
            {
                _emulator.Stop();
            }
        }

        private void Exit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void About_Click(object sender, EventArgs e)
        {
            var asm = System.Reflection.Assembly.GetExecutingAssembly();
            var name = asm != null ? asm.GetName() : null;
            var message = string.Format("{0} v{1}", name.Name, name.Version.ToString(3));

            MessageBox.Show(message + "\n\nBrandon Wilson\nbrandonlw@gmail.com", message);
        }

        private void Load_Click(object sender, EventArgs e)
        {
            var ofd = new OpenFileDialog();
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                if (_emulator != null)
                {
                    _emulator.Stop();
                    _emulator = null;
                }

                _emulator = new Emulator(new PCMDevice(), ofd.FileName, 0x10000);
                _emulator.EmulatorStateChanged += _emulator_EmulatorStateChanged;
                _emulator.ValueRead += _emulator_ValueRead;
                _emulator.ValueWritten += _emulator_ValueWritten;
                uctDebugger.Emulator = _emulator;
                //_emulator.LogPC = true;

                //_emulator.BreakpointAddresses.Add(0x286B8);
                //_emulator.BreakpointAddresses.Add(0x7B1BE);
                //_emulator.BreakpointAddresses.Add(0x7B2D4);
                //_emulator.BreakpointAddresses.Add(0x286D2);
                //_emulator.BreakpointAddresses.Add(0x2A20E);
                //_emulator.BreakpointAddresses.Add(0x054C);
                //_emulator.BreakpointAddresses.Add(0x286E2);
                //_emulator.BreakpointAddresses.Add(0x2A21C);
                //_emulator.BreakpointAddresses.Add(0x7B30E);
                //_emulator.AddressRangeWatchpoints.Add(new Tuple<uint, uint, MemoryAccess>(0xFFFFB0D2, 0xFFFFB0D3, MemoryAccess.Any));
                //_emulator.BreakpointAddresses.Add(0x7B2BE);
                //_emulator.AddressRangeWatchpoints.Add(new Tuple<uint, uint, MemoryAccess>(0xFFFF99B9, 0xFFFF99BA, MemoryAccess.Write));
                //_emulator.BreakpointAddresses.Add(0x7B35E);
                //_emulator.AddressRangeWatchpoints.Add(new Tuple<uint, uint, MemoryAccess>(0x1F64, 0x190B, MemoryAccess.Read));
                //_emulator.BreakpointAddresses.Add(0x40C60);
                //_emulator.BreakpointAddresses.Add(0x2D8A4);

                //_emulator.BreakpointAddresses.Add(0x40C60);
                //_emulator.BreakpointAddresses.Add(0x3BB5E);
                //_emulator.BreakpointAddresses.Add(0x3BD2E);
                //_emulator.BreakpointAddresses.Add(0x3E67C);
                //_emulator.BreakpointAddresses.Add(0x3E6AE);
                //_emulator.BreakpointAddresses.Add(0x3E622);
                //_emulator.BreakpointAddresses.Add(0x3E636);
                //_emulator.BreakpointAddresses.Add(0x3E64A);
                //_emulator.BreakpointAddresses.Add(0x44AD8);
                //_emulator.BreakpointAddresses.Add(0x3BE0A);
                //_emulator.AddressRangeWatchpoints.Add(new Tuple<uint, uint, MemoryAccess>(0x3A7E, 0x3A81, MemoryAccess.Read));
                //_emulator.BreakpointAddresses.Add(0x3E84E);
                //_emulator.BreakpointAddresses.Add(0x3E8EC);

                //_emulator.BreakpointAddresses.Add(0x3A91C); //Handle_Received_DLC_Data_0
                //_emulator.BreakpointAddresses.Add(0x20CA4);
                _emulator.BreakpointAddresses.Add(0x2B26C);
                _emulator.BreakpointAddresses.Add(0x2D8A4);

                _emulator.Start();
            }
        }

        private void _emulator_ValueWritten(object sender, ValueChangedEventArgs e)
        {
            var address = e.Address & 0xFFFFFF;
            if (address >= 0xFFF600 && address < 0xFFF640)
            {
                Console.WriteLine(string.Format("Value Written ({0})\tAddress {1}\tValue: {2}",
                   e.AccessType.ToString(), e.Address.ToString("X08"), e.Value.ToString("X08")));
                var spBytes = new byte[16];
                Array.Copy(_emulator.Ram, (_emulator.A[7].U32 & 0xFFFF) - spBytes.Length, spBytes, 0, spBytes.Length);
                var sp = string.Empty;
                foreach (var b in spBytes)
                {
                    sp += b.ToString("X02");
                }
                //Console.WriteLine(string.Format("  PC: {0}  SP: {1}", _emulator.PC.ToString("X08"), sp));
            }
        }

        private void _emulator_ValueRead(object sender, ValueChangedEventArgs e)
        {
            var address = e.Address & 0xFFFFFF;
            if (address >= 0xFFF600 && address < 0xFFF640)
            {
                Console.WriteLine(string.Format("Value Read ({0})\tAddress {1}\tValue: {2}",
                    e.AccessType.ToString(), e.Address.ToString("X08"), e.Value.ToString("X08")));
                var spBytes = new byte[16];
                Array.Copy(_emulator.Ram, (_emulator.A[7].U32 & 0xFFFF) - spBytes.Length, spBytes, 0, spBytes.Length);
                var sp = string.Empty;
                foreach (var b in spBytes)
                {
                    sp += b.ToString("X02");
                }
                //Console.WriteLine(string.Format("  PC: {0}  SP: {1}", _emulator.PC.ToString("X08"), sp));
            }
        }

        private void _emulator_EmulatorStateChanged(object sender, EventArgs e)
        {
            tsslStatus.Text = "State: " + _emulator.State.ToString();
        }

        private void breakpointsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var old = _emulator.State;
            _emulator.State = EmulatorState.Stopped;

            var frm = new Breakpoints(_emulator);
            frm.ShowDialog();

            _emulator.State = old;
        }
    }
}
