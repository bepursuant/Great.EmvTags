using System;
using System.Collections.Generic;
using System.Linq;

namespace Great.EmvTags
{
    public class EmvTlv
    {
        public EmvTlvList Children { get; set; }


        public EmvTlv()
        {
            TagBytes = new byte[] { };
            ValueBytes = new byte[] { };
            Children = new EmvTlvList();
        }

        public EmvTlv(byte tag, int length) : this(new byte[] { tag }, length) { }

        public EmvTlv(byte[] tag, int length)
        {
            TagBytes = tag;
            ValueBytes = new byte[length];
            Children = new EmvTlvList();
        }

        public EmvTlv(byte tag, byte[] value) : this(new byte[] { tag }, value) { }

        public EmvTlv(byte[] tag, byte[] value)
        {
            TagBytes = tag;
            ValueBytes = value;
            Children = new EmvTlvList();
        }


        public byte[] TagBytes { get; private set; }

        public byte[] LengthBytes 
        { 
            get => GetLengthBytes(); 
        }

        public byte[] ValueBytes { get; private set; }

        public ExtendedByteArray Tag { get; }
        public ExtendedByteArray Length { get => new ExtendedByteArray(GetLengthBytes()); }
        public ExtendedByteArray Value { get; }



        public string TagHex
        {
            get => TagBytes.ByteArrayToHexString();
        }

        public string LengthHex
        {
            get => LengthBytes.ByteArrayToHexString();
        }

        public string ValueHex 
        { 
            get => ValueBytes.ByteArrayToHexString(); 
        }


        public int LengthInt
        {
            get => ValueBytes.Length;
        }

        public string ValueAscii
        {
            get => ValueBytes.ByteArrayToAsciiString();
        }


        public EmvTlv FindFirst(string tag)
        {
            return FindFirst(tag.HexStringToByteArray());
        }

        public EmvTlv FindFirst(byte tag)
        {
            return FindFirst(new byte[] { tag });
        }

        public EmvTlv FindFirst(byte[] tag)
        {
            if (tag == null || tag.Length == 0)
                throw new ArgumentException("tag");

            if (TagBytes.SequenceEqual(tag))
                return this;

            if (Children.Any())
            {
                foreach (EmvTlv t in Children)
                {
                    var found = t.FindFirst(tag);
                    if (found != null)
                        return found;
                }
            }

            return null;
        }

        public EmvTlvList FindAll(string tag)
        {
            return FindAll(tag.HexStringToByteArray());
        }

        public EmvTlvList FindAll(byte tag)
        {
            return FindAll(new byte[] { tag });
        }

        public EmvTlvList FindAll(byte[] tag)
        {
            if (tag == null || tag.Length == 0)
                throw new ArgumentException("tag");

            var result = new EmvTlvList();

            if (TagBytes.SequenceEqual(tag))
                result.Add(this);

            if (Children.Any())
            {
                foreach (EmvTlv t in Children)
                {
                    var found = t.FindAll(tag);
                    if (found != null)
                        result.AddRange(found);
                }
            }

            return result;
        }

        

        private byte[] GetLengthBytes()
        {
            int l = LengthInt;

            // ensure length is in encodable range
            if (l < 0 || l > 0xffffffff)
                throw new Exception(string.Format("Invalid length value: {0}", l));

            // use short form if possible
            if (l <= 0x7f)
                return new byte[] { checked((byte)l) };

            byte lengthofLen;
            List<byte> b = new List<byte>();

            // use minimum number of octets
            if (l <= 0xff)
                lengthofLen = 1;
            else if (l <= 0xffff)
                lengthofLen = 2;
            else if (l <= 0xffffff)
                lengthofLen = 3;
            else if (l <= 0xffffffff)
                lengthofLen = 4;
            else
                throw new Exception(string.Format("Length value too big: {0}", l));

            // initial byte indicating length
            b.Add((byte)(lengthofLen | 0x80));

            // shift out the bytes
            for (var i = lengthofLen - 1; i >= 0; i--)
            {
                var data = (byte)(l >> (8 * i));
                b.Add(data);
            }

            return b.ToArray();
        }

        public static EmvTlv Parse(byte[] data)
        {
            return EmvTags.ParseTlv(data);
        }
    }
}
