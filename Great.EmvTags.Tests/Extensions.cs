using System.IO;
using System.Xml.Serialization;

namespace Great.EmvTags.Tests
{
    public static class Extensions
    {
        public static string ObjectToXmlString(this object input)
        {
            var xmlNs = new XmlSerializerNamespaces();
            xmlNs.Add("", "");
            var xmlStr = new StringWriter();
            var xmlSer = new XmlSerializer(input.GetType(), "");
            xmlSer.Serialize(xmlStr, input, xmlNs);
            return xmlStr.ToString();
        }


        public static T XmlStringToObject<T>(this string input)
        {
            StringReader xmlStr = new StringReader(input);
            XmlSerializer xmlSer = new XmlSerializer(typeof(T), "");
            return (T)xmlSer.Deserialize(xmlStr);
        }
    }
}
