using Xunit;

namespace Great.EmvTags.Tests
{
    public class ConstructedTagTests
    {
        private const string _constructedTag = "A5088801025F2D02656E";
        private const string _constructedNestedTag = "6F1A840E315041592E5359532E4444463031A5088801025F2D02656E";
        private const string _constructedNestedTagWithPadding = "00006F1D840E315041592E5359532E4444463031000000A5088801025F2D02656E000000";


        [Fact]
        [Trait("Build", "Run")]
        public void Parse_ShouldHandle_ConstructedTag()
        {
            var tlvs = EmvTagList.Parse(_constructedTag);
            AssertConstructedTag(tlvs);
        }

        [Fact]
        [Trait("Build", "Run")]
        public void Parse_ShouldHandle_ConstructedNestedTag()
        {
            var tlvs = EmvTagList.Parse(_constructedNestedTag);
            AssertConstructedNestedTag(tlvs);
        }

        [Fact]
        [Trait("Build", "Run")]
        public void Parse_ShouldHandle_ConstructedNestedTagWithPadding()
        {
            var tlvs = EmvTagList.Parse(_constructedNestedTagWithPadding);
            AssertConstructedNestedTagWithPadding(tlvs);
        }

        private static void AssertConstructedTag(EmvTagList tlvs)
        {
            Assert.NotNull(tlvs);
            Assert.True(tlvs.Count == 1);
            Assert.True(tlvs[0].HexTag == "A5");
            Assert.True(tlvs[0].HexLength == "08");
            Assert.True(tlvs[0].HexValue == "8801025F2D02656E");

            Assert.NotNull(tlvs[0].Children);
            Assert.True(tlvs[0].Children.Count == 2);
            Assert.True(tlvs[0].Children[0].HexTag == "88");
            Assert.True(tlvs[0].Children[0].HexLength == "01");
            Assert.True(tlvs[0].Children[0].HexValue == "02");

            Assert.True(tlvs[0].Children[1].HexTag == "5F2D");
            Assert.True(tlvs[0].Children[1].HexLength == "02");
            Assert.True(tlvs[0].Children[1].HexValue == "656E");
        }

        private void AssertConstructedNestedTag(EmvTagList tlvs)
        {
            Assert.NotNull(tlvs);
            Assert.True(tlvs.Count == 1);

            Assert.True(tlvs[0].HexTag == "6F");
            Assert.True(tlvs[0].HexLength == "1A");
            Assert.True(tlvs[0].HexValue == "840E315041592E5359532E4444463031A5088801025F2D02656E");

            Assert.NotNull(tlvs[0].Children);
            Assert.True(tlvs[0].Children.Count == 2);
            Assert.True(tlvs[0].Children[0].HexTag == "84");
            Assert.True(tlvs[0].Children[0].HexLength == "0E");
            Assert.True(tlvs[0].Children[0].HexValue == "315041592E5359532E4444463031");

            tlvs[0].Children.RemoveAt(0);
            AssertConstructedTag(tlvs[0].Children);
        }

        private void AssertConstructedNestedTagWithPadding(EmvTagList tlvs)
        {
            Assert.NotNull(tlvs);
            Assert.True(tlvs.Count == 1);

            Assert.True(tlvs[0].HexTag == "6F");
            Assert.True(tlvs[0].HexLength == "1D");
            Assert.True(tlvs[0].HexValue == "840E315041592E5359532E4444463031000000A5088801025F2D02656E");

            Assert.NotNull(tlvs[0].Children);
            Assert.True(tlvs[0].Children.Count == 2);
            Assert.True(tlvs[0].Children[0].HexTag == "84");
            Assert.True(tlvs[0].Children[0].HexLength == "0E");
            Assert.True(tlvs[0].Children[0].HexValue == "315041592E5359532E4444463031");

            tlvs[0].Children.RemoveAt(0);
            AssertConstructedTag(tlvs[0].Children);
        }
    }
}
