using System;
using System.Collections.Generic;
using System.Text;

namespace Great.EmvTags
{
    public class ExtendedByteArray
    {
        private byte[] _value;

        public byte[] Bytes { get => _value; set => _value = value; }
        public string Hex { get => _value.ByteArrayToHexString(); set => _value = value.HexStringToByteArray(); }
        public string Ascii { get => _value.ByteArrayToAsciiString(); set => _value = value.AsciiStringToByteArray(); }

        public override string ToString() => Hex;

        public ExtendedByteArray(string val) => Hex = val;
        public ExtendedByteArray(byte val) => Bytes = new byte[] { val };
        public ExtendedByteArray(byte[] val) => Bytes = val;

        public static implicit operator ExtendedByteArray(string val) => new ExtendedByteArray(val);
        public static implicit operator ExtendedByteArray(byte val) => new ExtendedByteArray(val);
        public static implicit operator ExtendedByteArray(byte[] val) => new ExtendedByteArray(val);
    }
}
