using Great.EmvTags;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Great.EmvTags
{
    public class EmvTlvList : List<EmvTlv>
    {

        public static EmvTlvList Parse(ExtendedByteArray data)
        {
            return EmvTags.ParseTlvList(data);
        }

        public EmvTlv FindFirst(ExtendedByteArray findTag)
        {
            foreach(EmvTlv t in this)
            {
                var found = t.FindFirst(findTag);
                if (found != null)
                    return found;
            }

            return null;
        }

        public EmvTlvList FindAll(ExtendedByteArray findTag)
        {
            var result = new EmvTlvList();

            foreach (EmvTlv t in this)
            {
                var found = t.FindAll(findTag);
                if (found != null)
                    result.AddRange(found);
            }

            return result;
        }
    }
}
