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
            Assert.True(tlvs[0].TagHex == "A5");
            Assert.True(tlvs[0].LengthInt == 8);
            Assert.True(tlvs[0].ValueHex == "8801025F2D02656E");

            Assert.NotNull(tlvs[0].Children);
            Assert.True(tlvs[0].Children.Count == 2);
            Assert.True(tlvs[0].Children[0].TagHex == "88");
            Assert.True(tlvs[0].Children[0].LengthInt == 1);
            Assert.True(tlvs[0].Children[0].ValueHex == "02");

            Assert.True(tlvs[0].Children[1].TagHex == "5F2D");
            Assert.True(tlvs[0].Children[1].LengthInt == 2);
            Assert.True(tlvs[0].Children[1].ValueHex == "656E");
        }

        private void AssertConstructedNestedTag(EmvTlvList tlvs)
        {
            Assert.NotNull(tlvs);
            Assert.True(tlvs.Count == 1);

            Assert.True(tlvs[0].TagHex == "6F");
            Assert.True(tlvs[0].LengthInt == 26);
            Assert.True(tlvs[0].ValueHex == "840E315041592E5359532E4444463031A5088801025F2D02656E");

            Assert.NotNull(tlvs[0].Children);
            Assert.True(tlvs[0].Children.Count == 2);
            Assert.True(tlvs[0].Children[0].TagHex == "84");
            Assert.True(tlvs[0].Children[0].LengthInt == 14);
            Assert.True(tlvs[0].Children[0].ValueHex == "315041592E5359532E4444463031");

            tlvs[0].Children.RemoveAt(0);
            AssertConstructedTag(tlvs[0].Children);
        }

        private void AssertConstructedNestedTagWithPadding(EmvTlvList tlvs)
        {
            Assert.NotNull(tlvs);
            Assert.True(tlvs.Count == 1);

            Assert.True(tlvs[0].TagHex == "6F");
            Assert.True(tlvs[0].LengthInt == 29);
            Assert.True(tlvs[0].ValueHex == "840E315041592E5359532E4444463031000000A5088801025F2D02656E");

            Assert.NotNull(tlvs[0].Children);
            Assert.True(tlvs[0].Children.Count == 2);
            Assert.True(tlvs[0].Children[0].TagHex == "84");
            Assert.True(tlvs[0].Children[0].LengthInt == 14);
            Assert.True(tlvs[0].Children[0].ValueHex == "315041592E5359532E4444463031");

            tlvs[0].Children.RemoveAt(0);
            AssertConstructedTag(tlvs[0].Children);
        }
    }
}
