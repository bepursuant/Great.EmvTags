using Great.EmvTags;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Great.EmvTags
{
    public class EmvTlvList : List<EmvTlv>
    {

        public static EmvTlvList Parse(string tlv)
        {
            if (string.IsNullOrWhiteSpace(tlv))
            {
                throw new ArgumentException("tlv");
            }

            return Parse(tlv.HexStringToByteArray());
        }

        public static EmvTlvList Parse(byte[] tlv)
        {
            if (tlv == null || tlv.Length == 0)
            {
                throw new ArgumentException("tlv");
            }

            var result = new EmvTlvList();
            Parse(tlv, result);

            return result;
        }

        public EmvTlv FindFirst(byte tag)
        {
            return FindFirst(new byte[] { tag });
        }

        public EmvTlv FindFirst(string tag)
        {
            if (string.IsNullOrWhiteSpace(tag))
                throw new ArgumentException("tag");

            return FindFirst(tag.HexStringToByteArray());
        }

        public EmvTlv FindFirst(byte[] tag)
        {
            if (tag == null || tag.Length == 0)
                throw new ArgumentException("tag");

            foreach(EmvTlv t in this)
            {
                var found = t.FindFirst(tag);
                if (found != null)
                    return found;
            }

            return null;
        }


        public EmvTlvList FindAll(byte tag)
        {
            return FindAll(new byte[] { tag });
        }

        public EmvTlvList FindAll(string tag)
        {
            if (string.IsNullOrWhiteSpace(tag))
                throw new ArgumentException("tag");

            return FindAll(tag.HexStringToByteArray());
        }

        public EmvTlvList FindAll(byte[] tag)
        {
            if (tag == null || tag.Length == 0)
                throw new ArgumentException("tag");

            var result = new EmvTlvList();

            foreach (EmvTlv t in this)
            {
                var found = t.FindAll(tag);
                if (found != null)
                    result.AddRange(found);
            }

            return result;
        }

        private static void Parse(byte[] rawTlv, EmvTlvList result)
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
                if (rawTlv[i].IsNullByte())
                {
                    i++;
                    continue;
                }


                // RETRIEVE TAG
                if (rawTlv[i].IsMultiByteTag())
                {
                    while (!rawTlv[++i].IsLastTagByte()) ;
                }

                int lengthOfTag = (i - start) + 1;
                byte[] tag = new byte[lengthOfTag];
                Array.Copy(rawTlv, start, tag, 0, lengthOfTag);
                start = ++i;


                // RETRIEVE LENGTH
                if (rawTlv[i].IsMultiByteLength())
                {
                    start++;
                    i += rawTlv[i] - 0x80;
                }

                int lengthOfLength = (i - start) + 1;
                byte[] length = new byte[lengthOfLength];
                Array.Copy(rawTlv, start, length, 0, lengthOfLength);
                start = ++i;


                // RETRIEVE VALUE
                int lengthOfValue = length.ByteArrayToInt();
                byte[] value = new byte[lengthOfValue];
                Array.Copy(rawTlv, start, value, 0, lengthOfValue);
                start = (i += lengthOfValue);


                // build the tag!
                var tlv = new EmvTlv(tag, value);
                result.Add(tlv);

                // if this was a constructed tag, parse its value into individual Tlv children as well
                if (tag[0].IsConstructedTag())
                {
                    Parse(tlv.ValueBytes, tlv.Children);
                }
            }
        }
    }
}
