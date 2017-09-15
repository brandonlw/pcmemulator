using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PCMEmulator
{
    internal class Timer
    {
        /// <summary>
        /// Callback
        /// </summary>
        public TimerFiredDelegate Callback { get; set; }

        /// <summary>
        /// Func
        /// </summary>
        public string Func { get; set; }

        /// <summary>
        /// Enabled
        /// </summary>
        public bool Enabled { get; set; }

        /// <summary>
        /// Temporary
        /// </summary>
        public bool Temporary { get; set; }

        /// <summary>
        /// Period
        /// </summary>
        internal AttoTime Period { get; set; }

        /// <summary>
        /// Start
        /// </summary>
        internal AttoTime Start { get; set; }

        /// <summary>
        /// Expire
        /// </summary>
        internal AttoTime Expire { get; set; }
    }
}
