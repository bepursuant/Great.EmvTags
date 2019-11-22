using System;
using System.Linq;
using System.Text;

namespace Great.EmvTags
{
    public class EmvTag
    {

        public EmvTag(byte[] tag, byte[] length, byte[] value)
        {
            Tag = tag;
            Length = length;
            Value = value;
            Children = new EmvTagList();
        }

        public byte[] Tag { get; private set; }

        public string HexTag { get { return GetHexString(Tag); } }

        public byte[] Length { get; private set; }

        public string HexLength { get { return GetHexString(Length); } }

        public byte[] Value { get; private set; }

        public string HexValue { get { return GetHexString(Value); } }

        public EmvTagList Children { get; set; }

        public EmvTag FindFirst(byte[] tag)
        {
            if (tag == null || tag.Length == 0)
                throw new ArgumentException("tag");

            if (Tag.SequenceEqual(tag))
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


        private static string GetHexString(byte[] arr)
        {
            var sb = new StringBuilder(arr.Length * 2);
            foreach (byte b in arr)
            {
                sb.AppendFormat("{0:X2}", b);
            }

            return sb.ToString();
        }
    }
}
