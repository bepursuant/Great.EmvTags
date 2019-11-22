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


        public EmvTag FindFirst(string tag)
        {
            if (string.IsNullOrWhiteSpace(tag))
            {
                throw new ArgumentException("tag");
            }

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

        private static void Parse(byte[] rawTlv, EmvTagList result)
        {
            for (int i = 0, start = 0; i < rawTlv.Length; start = i)
            {

                //// rawTlv: PPTTLLVVVVVVVVV
                ////      i: ^
                ////  start: ^

                // 0x00 and 0xFF can be used as padding before, between, and after tags
                if (rawTlv[i] == 0x00)
                {
                    i++;
                    continue;
                }

                //// rawTlv: PPTTLLVVVVVVVVV
                ////      i:   ^
                ////  start:   ^

                // get tag meta-data (type of tag, and if it is multi-byte)
                // if the tag is multiByte, advance i until we get to the last byte of the tag
                bool constructedTag = (rawTlv[i] & 0x20) != 0;
                bool multiByteTag = (rawTlv[i] & 0x1F) == 0x1F;
                while (multiByteTag && (rawTlv[++i] & 0x80) != 0) ;
                int lengthOfTag = (i + 1) - start;

                //// rawTlv: PPTTLLVVVVVVVVV
                ////      i:    ^
                ////  start:   ^


                // RETRIEVE TAG
                byte[] tag = new byte[lengthOfTag];
                Array.Copy(rawTlv, start, tag, 0, lengthOfTag);

                // move past tag bytes
                i += 1;
                start = i;

                //// rawTlv: PPTTLLVVVVVVVVV
                ////      i:     ^
                ////  start:     ^

                // bit8 being 1 indicates that the length is multiple bytes long
                bool multiByteLength = (rawTlv[i] & 0x80) != 0;
                int lengthOfLength = multiByteLength ? rawTlv[i] & 0x1F : 1;

                // RETRIEVE LENGTH
                byte[] length = new byte[lengthOfLength];
                Array.Copy(rawTlv, start, length, 0, lengthOfLength);

                // move past length bytes
                i += lengthOfLength;
                start = i;

                //// rawTlv: PPTTLLVVVVVVVVV
                ////      i:       ^
                ////  start:       ^

                // now that we know the start position and length of the data, retrieve it
                // then create the Tlv object and add it to the list
                int lengthOfValue = GetInt(length, 0, length.Length);

                // RETRIEVE VALUE
                byte[] value = new byte[lengthOfValue];
                Array.Copy(rawTlv, start, value, 0, lengthOfValue);

                // move past value bytes
                i += lengthOfValue;
                start = i;

                //// rawTlv: PPTTLLVVVVVVVVV
                ////      i:                ^
                ////  start:                ^


                // build the tag!
                var tlv = new EmvTag(tag, length, value);
                result.Add(tlv);

                // if this was a constructed tag, parse its value into individual Tlv children as well
                if (constructedTag)
                {
                    Parse(tlv.Value, tlv.Children);
                }
            }
        }

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
