﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Great.EmvTags
{
    public static class EmvTlvParser
    {

        public static EmvTlv ParseTlv(string tlv)
        {
            return Parse(tlv.HexStringToByteArray()).Item2;
        }

        public static EmvTlv ParseTlv(byte[] rawTlv)
        {
            return Parse(rawTlv).Item2;
        }

        public static EmvTlvList ParseTlvList(string tlv)
        {
            return ParseTlvList(tlv.HexStringToByteArray());
        }

        public static EmvTlvList ParseTlvList(byte[] rawTlv)
        {
            EmvTlvList result = new EmvTlvList();
            int i = 0;
            while (i < rawTlv.Length && i != -1)
            {
                Tuple<int, EmvTlv> t = Parse(rawTlv, i);
                if (t.Item2 != null)
                    result.Add(t.Item2);

                i = t.Item1;
            }
            return result;
        }

        private static Tuple<int, EmvTlv> Parse(byte[] rawTlv, int offset = 0)
        {
            EmvTlv tlv = null;

            // `start` and `i` represent cursors along our TLV data. We will move start
            // along until we find the start byte of our tag, length, or value. then
            // we will move i along until we find the end of it. Using their positions
            // we then extract that data, and push them forward one step to look for our
            // next tag, length, or value.

            // rawTlv: PPTTLLLVVVVVVVVPP
            //  start:     ^
            //      i:       ^

            for (int i = offset, start = offset; i < rawTlv.Length; start = i)
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
                tlv = new EmvTlv(tag, value);

                // if this was a constructed tag, parse its value into individual Tlv children as well
                if (tag[0].IsConstructedTag())
                {
                    tlv.Children = ParseTlvList(tlv.ValueBytes);
                }

                return new Tuple<int, EmvTlv>(start, tlv);
            }

            return new Tuple<int, EmvTlv>(-1, null);
        }
    }
}
