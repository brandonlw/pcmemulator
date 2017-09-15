using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PCMEmulator
{
    public class ValueChangedEventArgs : EventArgs
    {
        /// <summary>
        /// Creates a ValueChangedEventArgs object.
        /// </summary>
        /// <param name="address"></param>
        /// <param name="accessType"></param>
        /// <param name="value"></param>
        public ValueChangedEventArgs(uint address, ReadWriteAccessType accessType, uint value)
        {
            Address = address;
            AccessType = accessType;
            Value = value;
        }

        /// <summary>
        /// Address
        /// </summary>
        public uint Address { get; set; }

        /// <summary>
        /// AccessType
        /// </summary>
        public ReadWriteAccessType AccessType { get; set; }

        /// <summary>
        /// Value
        /// </summary>
        public uint Value { get; set; }
    }
}
