using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace PCMEmulator
{
    /// <summary>
    /// Program
    /// </summary>
    public static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        public static void Main()
        {
            /*var file = new System.IO.FileStream("C:\\Users\\Brandon\\Desktop\\ecm1_swapped.bin", System.IO.FileMode.Open);
            var data = new byte[file.Length - 4];
            file.Read(data, 0, data.Length);
            file.Close();
            ushort checksum = 0;
            for (int i = 0; i < data.Length; i++)
            {
                if (i < 0x4000 || i >= 0x8010)
                {
                    checksum = (ushort)((checksum + data[i]) & 0xFFFF);
                }
            }
            return;*/
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            //Not sure if we need this anymore...
            Control.CheckForIllegalCrossThreadCalls = false;

            Application.Run(new Main());
        }
    }
}
