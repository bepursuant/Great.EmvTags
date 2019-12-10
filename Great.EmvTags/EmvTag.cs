﻿using System;
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

        public int IntLength { get { return Length.Length; } }

        public byte[] Value { get; private set; }

        public string HexValue { get { return GetHexString(Value); } }

        public EmvTagList Children { get; set; }

        public EmvTag FindFirst(string tag)
        {
            return FindFirst(GetBytes(tag));
        }

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

        public EmvTagList FindAll(string tag)
        {
            return FindAll(GetBytes(tag));
        }

        public EmvTagList FindAll(byte[] tag)
        {
            if (tag == null || tag.Length == 0)
                throw new ArgumentException("tag");

            var result = new EmvTagList();

            if (Tag.SequenceEqual(tag))
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

        private static byte[] GetBytes(string hexString)
        {
            return Enumerable
                .Range(0, hexString.Length)
                .Where(x => x % 2 == 0)
                .Select(x => Convert.ToByte(hexString.Substring(x, 2), 16))
                .ToArray();
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

        private static EmvTag Parse(byte[] data)
        {
            var tl = EmvTagList.Parse(data);

            if (tl.Count > 0)
                return tl.First();
            else
                return null;
        }
    }
}
