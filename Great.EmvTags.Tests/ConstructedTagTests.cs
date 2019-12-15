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
            var tlvs = EmvTlvList.Parse(_constructedTag);
            AssertConstructedTag(tlvs);
        }

        [Fact]
        [Trait("Build", "Run")]
        public void Parse_ShouldHandle_ConstructedNestedTag()
        {
            var tlvs = EmvTlvList.Parse(_constructedNestedTag);
            AssertConstructedNestedTag(tlvs);
        }

        [Fact]
        [Trait("Build", "Run")]
        public void Parse_ShouldHandle_ConstructedNestedTagWithPadding()
        {
            var tlvs = EmvTlvList.Parse(_constructedNestedTagWithPadding);
            AssertConstructedNestedTagWithPadding(tlvs);
        }

        private static void AssertConstructedTag(EmvTlvList tlvs)
        {
            Assert.NotNull(tlvs);
            Assert.True(tlvs.Count == 1);
            Assert.True(tlvs[0].Tag.Hex == "A5");
            Assert.True(tlvs[0].Length == 8);
            Assert.True(tlvs[0].Value.Hex == "8801025F2D02656E");

            Assert.NotNull(tlvs[0].Children);
            Assert.True(tlvs[0].Children.Count == 2);
            Assert.True(tlvs[0].Children[0].Tag.Hex == "88");
            Assert.True(tlvs[0].Children[0].Length == 1);
            Assert.True(tlvs[0].Children[0].Value.Hex == "02");

            Assert.True(tlvs[0].Children[1].Tag.Hex == "5F2D");
            Assert.True(tlvs[0].Children[1].Length == 2);
            Assert.True(tlvs[0].Children[1].Value.Hex == "656E");
        }

        private void AssertConstructedNestedTag(EmvTlvList tlvs)
        {
            Assert.NotNull(tlvs);
            Assert.True(tlvs.Count == 1);

            Assert.True(tlvs[0].Tag.Hex == "6F");
            Assert.True(tlvs[0].Length == 26);
            Assert.True(tlvs[0].Value.Hex == "840E315041592E5359532E4444463031A5088801025F2D02656E");

            Assert.NotNull(tlvs[0].Children);
            Assert.True(tlvs[0].Children.Count == 2);
            Assert.True(tlvs[0].Children[0].Tag.Hex == "84");
            Assert.True(tlvs[0].Children[0].Length == 14);
            Assert.True(tlvs[0].Children[0].Value.Hex == "315041592E5359532E4444463031");

            tlvs[0].Children.RemoveAt(0);
            AssertConstructedTag(tlvs[0].Children);
        }

        private void AssertConstructedNestedTagWithPadding(EmvTlvList tlvs)
        {
            Assert.NotNull(tlvs);
            Assert.True(tlvs.Count == 1);

            Assert.True(tlvs[0].Tag.Hex == "6F");
            Assert.True(tlvs[0].Length == 29);
            Assert.True(tlvs[0].Value.Hex == "840E315041592E5359532E4444463031000000A5088801025F2D02656E");

            Assert.NotNull(tlvs[0].Children);
            Assert.True(tlvs[0].Children.Count == 2);
            Assert.True(tlvs[0].Children[0].Tag.Hex == "84");
            Assert.True(tlvs[0].Children[0].Length == 14);
            Assert.True(tlvs[0].Children[0].Value.Hex == "315041592E5359532E4444463031");

            tlvs[0].Children.RemoveAt(0);
            AssertConstructedTag(tlvs[0].Children);
        }
    }
}
