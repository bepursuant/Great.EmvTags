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
            return Parse(tlv.HexStringToByteArray());
        }

        public static EmvTlvList Parse(byte[] tlv)
        {
            return EmvTags.ParseTlvList(tlv);
        }

        public EmvTlv FindFirst(byte tag)
        {
            return FindFirst(new byte[] { tag });
        }

        public EmvTlv FindFirst(string tag)
        {
            return FindFirst(tag.HexStringToByteArray());
        }

        public EmvTlv FindFirst(byte[] tag)
        {
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
            return FindAll(tag.HexStringToByteArray());
        }

        public EmvTlvList FindAll(byte[] tag)
        {
            var result = new EmvTlvList();

            foreach (EmvTlv t in this)
            {
                var found = t.FindAll(tag);
                if (found != null)
                    result.AddRange(found);
            }

            return result;
        }
    }
}
