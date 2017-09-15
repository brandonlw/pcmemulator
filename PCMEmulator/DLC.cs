using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PCMEmulator
{
    public class DLC
    {
        private PCMDevice _device;
        private bool _receivingData = false;
        private bool _dataReceiveComplete = false;
        private List<byte> _receivedData;

        public DLC(PCMDevice device)
        {
            _device = device;
        }

        public void Init()
        {
            _device.Emulator.InsideInterruptChanged -= Emulator_InsideInterruptChanged;
            _device.Emulator.InsideInterruptChanged += Emulator_InsideInterruptChanged;

            if (Frames == null)
            {
                Frames = new Queue<List<byte>>();
            }

            if (Possibilities == null)
            {
                Possibilities = new Queue<List<byte>>();
                //_BuildPossibilities();
            }

            if (_device.Emulator.SuccessiveResets > 2)
            {
                _device.Emulator.SuccessiveResets = 0;
                if (Possibilities.Count > 0)
                {
                    Possibilities.Dequeue(); //this one failed, so skip it
                }
            }

            _receivedData = null;
            _receivingData = false;
            _dataReceiveComplete = false;
        }

        public byte Status
        {
            get
            {
                byte ret = 0x00;

                if (Frames.Count > 0)
                {
                    if (Frames.Peek().Count > 1)
                    {
                        ret |= (0x02 << 5);
                    }
                    else if (Frames.Peek().Count == 1)
                    {
                        ret |= (0x07 << 5);
                    }
                    else
                    {
                        Frames.Dequeue();
                        //Frames.Enqueue(new List<byte>(new byte[] { 0x68, 0x6A, 0xF1, 0x01, 0x01, 0x00, 0x30 }));
                    }
                }

                return ret;
            }
        }

        public byte Command
        {
            set
            {
                if (value == 0x0C)
                {
                    if (_receivedData != null && _receivedData.Count >= 2 && _receivedData[1] == 0x6B)
                    {
                        _dataReceiveComplete = true;

                        var sb = new StringBuilder();
                        foreach (var b in _receivedData)
                        {
                            sb.Append(string.Format("{0} ", b.ToString("X02")));
                        }

                        Console.WriteLine("PCM transmitted bytes: " + sb.ToString().Trim());
                        _device.Emulator.LastPCLogString = "PCM transmitted bytes: " + sb.ToString().Trim();
                        _device.Emulator.LastSuccessfulTest = DateTime.Now;
                        if (Possibilities.Count > 0)
                        {
                            Possibilities.Dequeue();
                        }
                    }
                    else
                    {
                        _receivedData = new List<byte>();
                    }

                    ReceivingData = false;
                }
                else
                {
                    ReceivingData = true;
                }
            }
        }

        public byte Data
        {
            set
            {
                _dataReceiveComplete = false;
                if (!_receivingData)
                {
                    _receivedData = new List<byte>();
                }

                _receivedData.Add(value);
            }
        }

        public bool ReceivingData
        {
            get
            {
                return _receivingData;
            }

            set
            {
                var changed = (_receivingData != value);

                _receivingData = value;

                if (changed)
                {
                    Emulator_InsideInterruptChanged(this, EventArgs.Empty);
                }
            }
        }

        public Queue<List<byte>> Frames { get; set; }

        public Queue<List<byte>> Possibilities { get; set; }

        private void Emulator_InsideInterruptChanged(object sender, EventArgs e)
        {
            if (!_device.Emulator.InsideInterrupt && Frames.Count == 0 && Possibilities.Count > 0 &&
                !_receivingData && (_receivedData == null || _dataReceiveComplete))
            {
                Task.Factory.StartNew(() =>
                {
                    _dataReceiveComplete = false;
                    var bytes = Possibilities.Peek();

                    var sb = new StringBuilder();
                    foreach (var b in bytes)
                    {
                        sb.Append(string.Format("{0} ", b.ToString("X02")));
                    }

                    Console.WriteLine("PCM receiving bytes: " + sb.ToString().Trim());

                    if (_receivedData == null)
                    {
                        _receivedData = new List<byte>();
                    }

                    Thread.Sleep(2000);
                    var clone = new List<byte>();
                    foreach (var b in bytes)
                    {
                        clone.Add(b);
                    }

                    Frames.Enqueue(clone);
                    _device.Emulator.Interrupt = (0x1C0 / 4) - 0x18;
                    _device.Emulator.InterruptMaskLevel = _device.Emulator.Interrupt - 1;
                });
            }
        }

        private void _BuildPossibilities()
        {
            Possibilities.Clear();
            for (int i = 0x36; i < 0x100; i++)
            {
                //for (byte j = 0; j <= 0xFF; j++)
                //{
                    //for (byte k = 0; k <= 0xFF; k++)
                    //{
                        Possibilities.Enqueue(new List<byte>(new byte[] { 0x68, 0x6A, 0xF1, (byte)i, 0x01, 0x20 }));
                    //}
                //}
            }
        }
    }
}
