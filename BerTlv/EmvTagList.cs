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

        private static void Parse(byte[] rawTlv, EmvTagList result)
        {
            for (int i = 0, start = 0; i < rawTlv.Length; start = i)
            {
                // 0x00 and 0xFF can be used as padding before, between, and after tags
                if (rawTlv[i] == 0x00)
                {
                    i++;
                    continue;
                }

                // bit6 being 1 indicates that the tag is 'constructed' (contains more tags within it)
                bool constructedTlv = (rawTlv[i] & 0x20) != 0;

                // bit1 through 5 being 1 indicates that the tag is more than one byte long
                // recognize this and keep looking until we find the last byte of the tag, indicated by bit8 being 0
                bool multiByteTag = (rawTlv[i] & 0x1F) == 0x1F;
                while (multiByteTag && (rawTlv[++i] & 0x80) != 0) ;

                // i is on the last byte of the tag, so move it forward one and 
                // retreve the tag value from the raw tlv
                i++;
                int tag = GetInt(rawTlv, start, i - start);

                // bit8 being 1 indicates that the length is multiple bytes long
                bool multiByteLength = (rawTlv[i] & 0x80) != 0;

                int length = multiByteLength ? GetInt(rawTlv, i + 1, rawTlv[i] & 0x1F) : rawTlv[i];
                i = multiByteLength ? i + (rawTlv[i] & 0x1F) + 1 : i + 1;

                // i is on the last byte of the length, so move it forward by the length we found
                i += length;

                // now that we know the start position and length of the data, retrieve it
                // then create the Tlv object and add it to the list
                byte[] rawData = new byte[i - start];
                Array.Copy(rawTlv, start, rawData, 0, i - start);
                var tlv = new EmvTag(tag, length, rawData.Length - length, rawData);
                result.Add(tlv);

                // if this was a constructed tag, parse its value into individual Tlv children as well
                if (constructedTlv)
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
