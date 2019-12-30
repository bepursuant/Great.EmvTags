using Xunit;

namespace Great.EmvTags.Tests
{

    public class PrimitiveTagTests
    {
        private const string _primitiveTag = "9A03191122";
        private const string _primitiveMultiByteTag = "5F340112";
        private const string _primitiveExtendedTag = "9081C01DE9604518D266B23721DBDDBB71CFE21003EFD717324E3B02749EDCA5901CBBFBB1A834594BDA2F3597A0345385E7587051D8350B4F7EFC2913609B855F00F2FEFB161D91A981D65609BE043F36F753011AD7B39956B7FAC1B154787A713F76C289DBDCACE3E3A3643AD7799F391D93124FD89F3D5CB325140B85FB6158315F91AE65C259C3AD19AAEB851270167E078D99A8E364ED12E4B4C7D4F30E38DF6D19CC8756472D12EBC38B6446EF544C626347B15D41DD88A7F61E3970CF6CDE5F";
        private const string _primitiveMultiByteExtendedTag = "5F502B68747470733A2F2F6769746875622E636F6D2F62657075727375616E742F47726561742E456D7654616773";

        [Fact]
        [Trait("Build", "Run")]
        public void Parse_ShouldHandle_PrimitiveTag()
        {
            var tlvs = EmvTlvList.Parse(_primitiveTag);
            AssertPrimitiveTag(tlvs);
            Assert.Equal(_primitiveTag, tlvs.Tlv.Hex);
        }

        [Fact]
        [Trait("Build", "Run")]
        public void Parse_ShouldHandle_PrimitiveTag_WithPadding()
        {
            var tlvs = EmvTlvList.Parse($"0000{_primitiveTag}0000");
            AssertPrimitiveTag(tlvs);
            Assert.Equal(_primitiveTag, tlvs.Tlv.Hex);
        }

        [Fact]
        [Trait("Build", "Run")]
        public void Parse_ShouldHandle_PrimitiveMultiByteTag()
        {
            var tlvs = EmvTlvList.Parse(_primitiveMultiByteTag);
            AssertPrimitiveMultiByteTag(tlvs);
            Assert.Equal(_primitiveMultiByteTag, tlvs.Tlv.Hex);
        }

        [Fact]
        [Trait("Build", "Run")]
        public void Parse_ShouldHandle_PrimitiveMultiByteTag_WithPadding()
        {
            var tlvs = EmvTlvList.Parse($"0000{_primitiveMultiByteTag}0000");
            AssertPrimitiveMultiByteTag(tlvs);
            Assert.Equal(_primitiveMultiByteTag, tlvs.Tlv.Hex);
        }

        [Fact]
        [Trait("Build", "Run")]
        public void Parse_ShouldHandle_PrimitiveExtendedTag()
        {
            var tlvs = EmvTlvList.Parse(_primitiveExtendedTag);
            AssertPrimitiveExtendedTag(tlvs);
            Assert.Equal(_primitiveExtendedTag, tlvs.Tlv.Hex);
        }

        [Fact]
        [Trait("Build", "Run")]
        public void Parse_ShouldHandle_PrimitiveExtendedTag_WithPadding()
        {
            var tlvs = EmvTlvList.Parse($"0000{_primitiveExtendedTag}0000");
            AssertPrimitiveExtendedTag(tlvs);
            Assert.Equal(_primitiveExtendedTag, tlvs.Tlv.Hex);
        }

        [Fact]
        [Trait("Build", "Run")]
        public void Parse_ShouldHandle_PrimitiveMultiByteExtendedTag()
        {
            var tlvs = EmvTlvList.Parse(_primitiveMultiByteExtendedTag);
            AssertPrimitiveMultiByteExtendedTag(tlvs);
            Assert.Equal(_primitiveMultiByteExtendedTag, tlvs.Tlv.Hex);
        }

        [Fact]
        [Trait("Build", "Run")]
        public void Parse_ShouldHandle_PrimitiveMultiByteExtendedTag_WithPadding()
        {
            var tlvs = EmvTlvList.Parse($"0000{_primitiveMultiByteExtendedTag}0000");
            AssertPrimitiveMultiByteExtendedTag(tlvs);
            Assert.Equal(_primitiveMultiByteExtendedTag, tlvs.Tlv.Hex);
        }


        // Validation Methods
        private static void AssertPrimitiveTag(EmvTlvList tlvs)
        {
            Assert.NotNull(tlvs);
            Assert.True(tlvs.Count == 1);
            Assert.True(tlvs[0].Tag.Hex == "9A");
            Assert.True(tlvs[0].Length == 3);
            Assert.True(tlvs[0].Value.Hex == "191122");
        }

        private static void AssertPrimitiveMultiByteTag(EmvTlvList tlvs)
        {
            Assert.NotNull(tlvs);
            Assert.True(tlvs.Count == 1);
            Assert.True(tlvs[0].Tag.Hex == "5F34");
            Assert.True(tlvs[0].Length == 1);
            Assert.True(tlvs[0].Value.Hex == "12");
        }

        private static void AssertPrimitiveExtendedTag(EmvTlvList tlvs)
        {
            Assert.NotNull(tlvs);
            Assert.True(tlvs.Count == 1);
            Assert.True(tlvs[0].Tag.Hex == "90");
            Assert.True(tlvs[0].Length == 192);
            Assert.True(tlvs[0].Value.Hex == "1DE9604518D266B23721DBDDBB71CFE21003EFD717324E3B02749EDCA5901CBBFBB1A834594BDA2F3597A0345385E7587051D8350B4F7EFC2913609B855F00F2FEFB161D91A981D65609BE043F36F753011AD7B39956B7FAC1B154787A713F76C289DBDCACE3E3A3643AD7799F391D93124FD89F3D5CB325140B85FB6158315F91AE65C259C3AD19AAEB851270167E078D99A8E364ED12E4B4C7D4F30E38DF6D19CC8756472D12EBC38B6446EF544C626347B15D41DD88A7F61E3970CF6CDE5F");
        }

        private static void AssertPrimitiveMultiByteExtendedTag(EmvTlvList tlvs)
        {
            Assert.NotNull(tlvs);
            Assert.True(tlvs.Count == 1);
            Assert.True(tlvs[0].Tag.Hex == "5F50");
            Assert.True(tlvs[0].Length == 43);
            Assert.True(tlvs[0].Value.Hex == "68747470733A2F2F6769746875622E636F6D2F62657075727375616E742F47726561742E456D7654616773");
        }

    }
}
