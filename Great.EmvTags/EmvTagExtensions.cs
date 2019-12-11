using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Great.EmvTags
{
    public static class EmvTagExtensions
    {
        public static byte[] HexStringToByteArray(this string hexString)
        {
            return Enumerable
                .Range(0, hexString.Length)
                .Where(x => x % 2 == 0)
                .Select(x => Convert.ToByte(hexString.Substring(x, 2), 16))
                .ToArray();
        }

        public static string ByteArrayToHexString(this byte[] arr)
        {
            var sb = new StringBuilder(arr.Length * 2);
            foreach (byte b in arr)
            {
                sb.AppendFormat("{0:X2}", b);
            }

            return sb.ToString();
        }

        public static string ByteArrayToAsciiString(this byte[] arr)
        {
            return Encoding.ASCII.GetString(arr);
        }

        public static int ByteArrayToInt(this byte[] data)
        {
            var result = 0;
            for (var i = 0; i < data.Length; i++)
            {
                result = (result << 8) | data[i];
            }

            return result;
        }

        public static bool IsMultiByteLength(this byte v) => (v & 0x80) != 0;

        public static bool IsLastTagByte(this byte v) => (v & 0x80) == 0;

        public static bool IsMultiByteTag(this byte v) => (v & 0x1F) == 0x1F;

        public static bool IsConstructedTag(this byte v) => (v & 0x20) != 0;

        public static bool IsNullByte(this byte v) => v == 0x00;
    }
}
