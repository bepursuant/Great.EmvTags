using System;
using System.Collections.Generic;
using System.Linq;

namespace Great.EmvTags
{
    public class EmvTag
    {
        private byte[] tagBytes;
        private byte[] valueBytes;

        public EmvTagList Children { get; set; }



        public EmvTag()
        {
            tagBytes = new byte[] { };
            valueBytes = new byte[] { };
            Children = new EmvTagList();
        }

        public EmvTag(byte[] tag, byte[] value)
        {
            tagBytes = tag;
            valueBytes = value;
            Children = new EmvTagList();
        }


        public byte[] TagBytes 
        { 
            get => tagBytes; 
            set => SetTag(value); 
        }

        public byte[] LengthBytes 
        { 
            get => GetLengthBytes(); 
            set => SetLength(value); 
        }

        public byte[] ValueBytes 
        { 
            get => valueBytes; 
            set => SetValue(value);
        }


        public string TagHex
        {
            get => TagBytes.ByteArrayToHexString();
            set => SetTag(value);
        }

        public string LengthHex
        {
            get => LengthBytes.ByteArrayToHexString();
            set => SetLength(value);
        }

        public string ValueHex 
        { 
            get => ValueBytes.ByteArrayToHexString(); 
            set => SetValue(value); 
        }


        public int LengthInt
        {
            get => ValueBytes.Length;
            set => SetLength(value);
        }

        public string ValueAscii
        {
            get => ValueBytes.ByteArrayToAsciiString();
        }



        public void SetTag(string tag) => SetTag(tag.HexStringToByteArray());
        public void SetTag(byte tag) => SetTag(new byte[] { tag });
        public void SetTag(byte[] tag) => tagBytes = tag;

        public void SetLength(string length) => SetLength(length.HexStringToByteArray()); // WONT WORK!
        public void SetLength(byte length) => SetLength(new byte[] { length });
        public void SetLength(byte[] length)
        {
            //SetLength(length.ByteArrayToInt()); // WONT WORK!

            // short length
            if (length.Length == 1)
            {
                SetLength((int)length[0]);
                return;
            }
            
            // multi byte length
            if((length[0] & 0x80) != 0)
            {
                int lengthOfLength = length[0] - 0x80;

                // make sure we have all the bytes we need
                if (length.Length != lengthOfLength + 1)
                    throw new Exception($"Length specified {lengthOfLength} bytes but actually had {length.Length + 1}");

                byte[] l = new byte[lengthOfLength];
                Array.Copy(length, 1, l, 0, lengthOfLength);
                SetLength(l.ByteArrayToInt());
                return;
            }

            throw new Exception($"Invalid Length Specification: {length.ByteArrayToHexString()}");
            
        }
        public void SetLength(int length) => Array.Resize(ref valueBytes, length);

        public void SetValue(string value) => SetValue(value.HexStringToByteArray());
        public void SetValue(byte value) => SetValue(new byte[] { value });
        public void SetValue(byte[] value) => valueBytes = value;



        public EmvTag FindFirst(string tag)
        {
            return FindFirst(tag.HexStringToByteArray());
        }

        public EmvTag FindFirst(byte[] tag)
        {
            if (tag == null || tag.Length == 0)
                throw new ArgumentException("tag");

            if (TagBytes.SequenceEqual(tag))
                return this;

            if (Children.Any())
            {
                foreach (EmvTag t in Children)
                {
                    var found = t.FindFirst(tag);
                    if (found != null)
                        return found;
                }
            }

            return null;
        }

        public EmvTagList FindAll(string tag)
        {
            return FindAll(tag.HexStringToByteArray());
        }

        public EmvTagList FindAll(byte[] tag)
        {
            if (tag == null || tag.Length == 0)
                throw new ArgumentException("tag");

            var result = new EmvTagList();

            if (TagBytes.SequenceEqual(tag))
                result.Add(this);

            if (Children.Any())
            {
                foreach (EmvTag t in Children)
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

        public static EmvTag Parse(byte[] data)
        {
            var tl = EmvTagList.Parse(data);

            if (tl.Count > 0)
                return tl.First();
            else
                return default;
        }
    }
}
