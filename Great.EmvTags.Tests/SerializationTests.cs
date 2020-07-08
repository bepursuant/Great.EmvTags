using System;
using System.Diagnostics;
using Xunit;

namespace Great.EmvTags.Tests
{
    public class SerializationTests
    {
        private const string _primitiveMultiByteTag = "5F340112";
        private const string _constructedTag = "A5088801025F2D02656E";
        private const string _constructedNestedTag = "6F1A840E315041592E5359532E4444463031A5088801025F2D02656E";
        private const string _constructedNestedTagWithPadding = "00006F1D840E315041592E5359532E4444463031000000A5088801025F2D02656E000000";


        [Fact]
        [Trait("Build", "Run")]
        public void Serialize_ShouldHandle_PrimitiveTag()
        {
            EmvTlv tlv = EmvTlv.Parse(_primitiveMultiByteTag);

            string a = tlv.ObjectToXmlString();

        }

        [Fact]
        [Trait("Build", "Run")]
        public void Serialize_ShouldHandle_ConstructedNestedTag()
        {
            var tlvs = EmvTlvList.Parse(_constructedNestedTag);

            string a = tlvs.ObjectToXmlString();
        }


    }
}
