using System.Collections.Generic;
using System.Xml.Serialization;

namespace Great.EmvTags
{
    [XmlRoot(ElementName = "EmvTlvList")]
    public class EmvTlvList : List<EmvTlv>
    {

        public static EmvTlvList Parse(ExtendedByteArray data)
        {
            return EmvTagParser.ParseTlvList(data);
        }

        public ExtendedByteArray Tlv { get => GetTlv(); }

        private ExtendedByteArray GetTlv()
        {
            string tlvHex = "";

            foreach (EmvTlv tlv in this)
            {
                tlvHex += tlv.Tlv.Hex;
            }

            return tlvHex;
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
