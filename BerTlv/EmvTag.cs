using System;
using System.Linq;
using System.Text;

namespace Great.EmvTags
{
    public class EmvTag
    {
        private readonly int _valueOffset;

        public EmvTag(int tag, int length, int valueOffset, byte[] data)
        {
            Tag = tag;
            Length = length;
            Data = data;
            Children = new EmvTagList();

            _valueOffset = valueOffset;
        }

        public byte[] Data { get; private set; }

        public string HexData { get { return GetHexString(Data); } }

        public int Tag { get; private set; }

        public string HexTag { get { return Tag.ToString("X"); } }


        public int Length { get; private set; }

        public string HexLength { get { return Length.ToString("X"); } }

        public byte[] Value
        {
            get
            {
                byte[] result = new byte[Length];
                Array.Copy(Data, _valueOffset, result, 0, Length);
                return result;
            }
        }

        public string HexValue { get { return GetHexString(Value); } }

        public EmvTagList Children { get; set; }


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
