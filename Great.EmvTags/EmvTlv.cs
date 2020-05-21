using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace Great.EmvTags
{
    [Serializable]
    public class EmvTlv : ISerializable
    {

        [XmlAttribute]
        public ExtendedByteArray Tag { get; }

        [XmlAttribute]
        public int Length { get => Value.Length; }

        [XmlText]
        public ExtendedByteArray Value { get; }
        public EmvTlvList Children { get; }

        public ExtendedByteArray Tlv { get => GetTlv(); }


        public EmvTlv()
        {
            Tag = 0;
            Value = 0;
        }

        public EmvTlv(ExtendedByteArray tag, int length)
        {
            Tag = tag;
            Value = new ExtendedByteArray(new byte[length]);
            Children = new EmvTlvList();
        }

        public EmvTlv(ExtendedByteArray tag, ExtendedByteArray value) {
            Tag = tag;
            Value = value;
            Children = new EmvTlvList();
        }


        public EmvTlv FindFirst(ExtendedByteArray findTag)
        {
            if (findTag == Tag)
                return this;

            if (Children.Any())
            {
                foreach (EmvTlv t in Children)
                {
                    var found = t.FindFirst(findTag);
                    if (found != null)
                        return found;
                }
            }

            return null;
        }

        public EmvTlvList FindAll(ExtendedByteArray findTag)
        {
            var result = new EmvTlvList();

            if (findTag == Tag)
                result.Add(this);

            if (Children.Any())
            {
                foreach (EmvTlv t in Children)
                {
                    var found = t.FindAll(findTag);
                    if (found != null)
                        result.AddRange(found);
                }
            }

            return result;
        }

        private ExtendedByteArray GetTlv()
        {
            return Tag.Bytes.Concat(GetLengthBytes().Bytes).Concat(Value.Bytes).ToArray();
        }

        public ExtendedByteArray GetLengthBytes()
        {
            int l = Length;

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

        public static EmvTlv Parse(ExtendedByteArray data)
        {
            return EmvTagParser.ParseTlv(data);
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("Tag", Tag.Hex);
            info.AddValue("Length", Length);
            info.AddValue("Value", Value.Hex);
        }
    }
}
