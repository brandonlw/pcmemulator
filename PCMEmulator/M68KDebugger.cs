using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Forms;

namespace PCMEmulator
{
    /// <summary>
    /// M68KDebugger
    /// </summary>
    public partial class M68KDebugger : UserControl
    {
        private uint _ppc;
        private bool _step = false;
        private bool _stepTill = false;
        private Emulator _emulator;

        /// <summary>
        /// Default constructor
        /// </summary>
        public M68KDebugger()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Emulator
        /// </summary>
        public Emulator Emulator
        {
            get
            {
                return _emulator;
            }

            set
            {
                if (value == null)
                {
                    if (_emulator != null)
                    {
                        _emulator.PreInstructionCallback = null;
                        _emulator.PostInstructionCallback = null;
                    }
                }

                _emulator = value;

                if (_emulator != null)
                {
                    _emulator.PreInstructionCallback = _PreCallback;
                    _emulator.PostInstructionCallback = _PostCallback;
                }

                this.Enabled = (_emulator != null);
            }
        }

        private void Get_Click(object sender, EventArgs e)
        {
            var old = Emulator.State;
            Emulator.State = EmulatorState.Stopped;
            _GetData();
            Emulator.State = old;
        }

        private void _GetData()
        {
            txtPPC.Text = Emulator.PPC.ToString("X6");
            txtOp.Text = Emulator.Op.ToString("X4");
            cbS.Checked = Emulator.S;
            cbM.Checked = Emulator.M;
            cbX.Checked = Emulator.X;
            cbN.Checked = Emulator.N;
            cbZ.Checked = Emulator.Z;
            cbV.Checked = Emulator.V;
            cbC.Checked = Emulator.C;
            txtIntMaskLevel.Text = Emulator.InterruptMaskLevel.ToString();
            txtUSP.Text = Emulator.USP.ToString("X8");
            txtSSP.Text = Emulator.SSP.ToString("X8");
            txtTotalCycles.Text = Emulator.TotalExecutedCycles.ToString("X16");
            txtPC.Text = Emulator.PC.ToString("X6");
            txtD0.Text = Emulator.D[0].U32.ToString("X08");
            txtD1.Text = Emulator.D[1].U32.ToString("X08");
            txtD2.Text = Emulator.D[2].U32.ToString("X08");
            txtD3.Text = Emulator.D[3].U32.ToString("X08");
            txtD4.Text = Emulator.D[4].U32.ToString("X08");
            txtD5.Text = Emulator.D[5].U32.ToString("X08");
            txtD6.Text = Emulator.D[6].U32.ToString("X08");
            txtD7.Text = Emulator.D[7].U32.ToString("X08");
            txtA0.Text = Emulator.A[0].U32.ToString("X08");
            txtA1.Text = Emulator.A[1].U32.ToString("X08");
            txtA2.Text = Emulator.A[2].U32.ToString("X08");
            txtA3.Text = Emulator.A[3].U32.ToString("X08");
            txtA4.Text = Emulator.A[4].U32.ToString("X08");
            txtA5.Text = Emulator.A[5].U32.ToString("X08");
            txtA6.Text = Emulator.A[6].U32.ToString("X08");
            txtA7.Text = Emulator.A[7].U32.ToString("X08");

            tbResult.AppendText(Emulator.Disassemble(Emulator.PPC).ToString() + "\r\n");
        }

        private void Run_Click(object sender, EventArgs e)
        {
            if (Emulator != null)
            {
                Emulator.State = EmulatorState.Running;
            }
        }

        private void Step_Click(object sender, EventArgs e)
        {
            if (Emulator != null)
            {
                _step = true;
                Emulator.State = EmulatorState.Running;
            }
        }

        private void btnStep2_Click(object sender, EventArgs e)
        {
            _stepTill = true;
            Emulator.State = EmulatorState.Running;
        }

        private void _PreCallback(Emulator emulator)
        {
            _ppc = Emulator.PPC;
            if (_stepTill)
            {
                int? ppc = null;
                try
                {
                    ppc = Convert.ToInt32(txtPPCTill.Text, 16);
                }
                catch
                {
                    //Don't care
                }

                if (ppc.HasValue)
                {
                    if (Emulator.PPC == ppc.Value)
                    {
                        Emulator.State = EmulatorState.Stopped;
                        _stepTill = false;
                    }
                }
            }
        }

        private void _PostCallback(Emulator emulator)
        {
            if (_step)
            {
                Emulator.State = EmulatorState.Stopped;
                _step = false;
            }

            if (Emulator.State == EmulatorState.Stopped)
            {
                _GetData();
            }

            if (/*Emulator.PC == 0x69A ||*/ Emulator.PC < 0x400)
            {
                Emulator.LogPC = true;
                Debugger.Break();
            }
        }

        private void txtReadByte_Click(object sender, EventArgs e)
        {
            txtValue.Text = Convert.ToString(_Read(1), 16);
        }

        private void btnReadWord_Click(object sender, EventArgs e)
        {
            txtValue.Text = Convert.ToString(_Read(2), 16);
        }

        private void btnReadLong_Click(object sender, EventArgs e)
        {
            txtValue.Text = Convert.ToString(_Read(4), 16);
        }

        private void btnWriteByte_Click(object sender, EventArgs e)
        {
            _Write(1);
        }

        private void btnWriteWord_Click(object sender, EventArgs e)
        {
            _Write(2);
        }

        private void btnWriteLong_Click(object sender, EventArgs e)
        {
            _Write(4);
        }

        private int _Read(int bytes)
        {
            var address = Convert.ToInt32(txtAddress.Text, 16);
            var ret = 0;
            
            for (int i = 0; i < bytes; i++)
            {
                var idx = (bytes - i - 1);
                if (idx < 0) idx = 0;
                ret |= Emulator.Ram[(address & 0xFFFF) + i] << (8 * idx);
            }

            return ret;
        }

        private void _Write(int bytes)
        {
            var address = Convert.ToInt32(txtAddress.Text, 16);
            var value = Convert.ToInt32(txtValue.Text, 16);

            for (int i = 0; i < bytes; i++)
            {
                var idx = (bytes - i - 1);
                if (idx < 0) idx = 0;
                Emulator.Ram[(address & 0xFFFF) + i] = (byte)(value >> (8 * idx));
            }
        }

        private void btnTriggerInterrupt_Click(object sender, EventArgs e)
        {
            var address = Convert.ToInt32(txtInterrupt.Text, 16);

            Emulator.Interrupt = (address / 4) - 0x18;
            Emulator.InterruptMaskLevel = Emulator.Interrupt - 1;
        }

        private void btnLogPC_Click(object sender, EventArgs e)
        {
            Emulator.LogPC = !Emulator.LogPC;
            if (Emulator.LogPC)
            {
                btnLogPC.Text = "Stop PC Log";
            }
            else
            {
                btnLogPC.Text = "Start PC Log";
            }
        }

        private void btnSavePCLog_Click(object sender, EventArgs e)
        {
            var sb = new System.Text.StringBuilder();
            lock (Emulator.PCLog)
            {
                foreach (var pc in Emulator.PCLog)
                {
                    sb.AppendLine(pc.Key.ToString("X08") + " " + pc.Value);
                }

                System.IO.File.WriteAllText("pc.txt", sb.ToString());
            }
        }

        private void btnSendPacket_Click(object sender, EventArgs e)
        {
            var bytes = txtPacket.Text.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            var list = new List<byte>();
            foreach (var b in bytes)
            {
                list.Add(Convert.ToByte(b, 16));
            }

            var device = _emulator.Device as PCMDevice;
            device.DLC.Frames.Enqueue(list);
        }
    }
}
