using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace PCMEmulator
{
    public partial class Breakpoints : Form
    {
        private Emulator _emulator;

        public Breakpoints(Emulator emulator)
        {
            InitializeComponent();

            _emulator = emulator;
            foreach (var breakpoint in _emulator.BreakpointAddresses)
            {
                chkBreakpoints.Items.Add(breakpoint.ToString("X08"));
            }
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            var items = chkBreakpoints.SelectedItems.OfType<string>().ToList();
            foreach (string item in items)
            {
                var addr = Convert.ToUInt32(item, 16);
                _emulator.BreakpointAddresses.Remove(addr);
                chkBreakpoints.Items.Remove(item);
            }
        }

        private void Breakpoints_FormClosing(object sender, FormClosingEventArgs e)
        {
            _emulator.BreakpointAddresses.Clear();
            foreach (string item in chkBreakpoints.Items)
            {
                var addr = Convert.ToUInt32(item, 16);
                _emulator.BreakpointAddresses.Add(addr);
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            var addr = Convert.ToUInt32(txtAddress.Text, 16);
            chkBreakpoints.Items.Add(addr.ToString("X08"));
        }
    }
}
