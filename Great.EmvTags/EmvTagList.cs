using Great.EmvTags;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Great.EmvTags
{
    public class EmvTagList : List<EmvTag>
    {

        public static EmvTagList Parse(string tlv)
        {
            if (string.IsNullOrWhiteSpace(tlv))
            {
                throw new ArgumentException("tlv");
            }

            return Parse(GetBytes(tlv));
        }

        public static EmvTagList Parse(byte[] tlv)
        {
            if (tlv == null || tlv.Length == 0)
            {
                throw new ArgumentException("tlv");
            }

            var result = new EmvTagList();
            Parse(tlv, result);

            return result;
        }

        public EmvTag FindFirst(byte tag)
        {
            return FindFirst(new byte[] { tag });
        }

        public EmvTag FindFirst(string tag)
        {
            if (string.IsNullOrWhiteSpace(tag))
                throw new ArgumentException("tag");

            return FindFirst(GetBytes(tag));
        }

        public EmvTag FindFirst(byte[] tag)
        {
            if (tag == null || tag.Length == 0)
                throw new ArgumentException("tag");

            foreach(EmvTag t in this)
            {
                var found = t.FindFirst(tag);
                if (found != null)
                    return found;
            }

            return null;
        }


        public EmvTagList FindAll(byte tag)
        {
            return FindAll(new byte[] { tag });
        }

        public EmvTagList FindAll(string tag)
        {
            if (string.IsNullOrWhiteSpace(tag))
                throw new ArgumentException("tag");

            return FindAll(GetBytes(tag));
        }

        public EmvTagList FindAll(byte[] tag)
        {
            if (tag == null || tag.Length == 0)
                throw new ArgumentException("tag");

            var result = new EmvTagList();

            foreach (EmvTag t in this)
            {
                var found = t.FindAll(tag);
                if (found != null)
                    result.AddRange(found);
            }

            return result;
        }

        private static void Parse(byte[] rawTlv, EmvTagList result)
        {
            // `start` and `i` represent cursors along our TLV data. We will move start
            // along until we find the start byte of our tag, length, or value. then
            // we will move i along until we find the end of it. Using their positions
            // we then extract that data, and push them forward one step to look for our
            // next tag, length, or value.

            // rawTlv: PPTTLLLVVVVVVVVPP
            //  start:     ^
            //      i:       ^

            for (int i = 0, start = 0; i < rawTlv.Length; start = i)
            {

                // 0x00 can be used as padding before, between, and after tags
                if (IsNullByte(rawTlv[i]))
                {
                    i++;
                    continue;
                }


                // RETRIEVE TAG
                if (IsMultiByteTag(rawTlv[i]))
                {
                    while (!IsLastTagByte(rawTlv[++i])) ;
                }

                int lengthOfTag = (i - start) + 1;
                byte[] tag = new byte[lengthOfTag];
                Array.Copy(rawTlv, start, tag, 0, lengthOfTag);
                start = ++i;


                // RETRIEVE LENGTH
                if (IsMultiByteLength(rawTlv[i]))
                {
                    start++;
                    i += rawTlv[i] - 0x80;
                }

                int lengthOfLength = (i - start) + 1;
                byte[] length = new byte[lengthOfLength];
                Array.Copy(rawTlv, start, length, 0, lengthOfLength);
                start = ++i;


                // RETRIEVE VALUE
                int lengthOfValue = GetInt(length, 0, length.Length);
                byte[] value = new byte[lengthOfValue];
                Array.Copy(rawTlv, start, value, 0, lengthOfValue);
                start = (i += lengthOfValue);


                // build the tag!
                var tlv = new EmvTag(tag, length, value);
                result.Add(tlv);

                // if this was a constructed tag, parse its value into individual Tlv children as well
                if (IsConstructedTag(tag[0]))
                {
                    Parse(tlv.Value, tlv.Children);
                }
            }
        }

        private static bool IsMultiByteLength(byte v) => (v & 0x80) != 0;

        private static bool IsLastTagByte(byte v) => (v & 0x80) == 0;

        private static bool IsMultiByteTag(byte v) => (v & 0x1F) == 0x1F;

        private static bool IsConstructedTag(byte v) => (v & 0x20) != 0;

        private static bool IsNullByte(byte v) => v == 0x00;

        private static byte[] GetBytes(string hexString)
        {
            return Enumerable
                .Range(0, hexString.Length)
                .Where(x => x % 2 == 0)
                .Select(x => Convert.ToByte(hexString.Substring(x, 2), 16))
                .ToArray();
        }

        private static int GetInt(byte[] data, int offset, int length)
        {
            var result = 0;
            for (var i = 0; i < length; i++)
            {
                result = (result << 8) | data[offset + i];
            }

            return result;
        }
    }
}
