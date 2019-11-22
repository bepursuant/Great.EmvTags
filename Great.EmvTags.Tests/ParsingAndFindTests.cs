using System;
using System.Linq;
using Xunit;

namespace Great.EmvTags.Tests
{
    public class ParsingAndFindTests
    {
        // Verify: http://www.emvlab.org/tlvutils/?data=6F1A840E315041592E5359532E4444463031A5088801025F2D02656E
        private string _validAsciiHexString = "6F1A840E315041592E5359532E4444463031A5088801025F2D02656E";

        // Note: emvlab does not correctly parse inline and trailing padding (0x00, 0xFF), but the specification 
        // allows for it. The TLV string below should be parsed in the same way as the one above
        private string _validAsciiHexStringWithPadding = "00006F1D840E315041592E5359532E4444463031000000A5088801025F2D02656E000000";

        [Fact]
        [Trait("Build", "Run")]
        public void Parse_ShouldThrowException_OnEmptyString()
        {
            Assert.Throws<ArgumentException>(() => { var tlvs = EmvTagList.Parse(""); });
        }

        [Fact]
        [Trait("Build", "Run")]
        public void Parse_ShouldThrowException_OnEmptyHexArray()
        {
            Assert.Throws<ArgumentException>(() => { var tlvs = EmvTagList.Parse(new byte[] { }); });
        }

        [Fact]
        [Trait("Build", "Run")]
        public void Parse_ShouldReturnValidList_OnValidAsciiHexString()
        {
            var tlvs = EmvTagList.Parse(_validAsciiHexString);

            AssertTlv(tlvs);
        }

        [Fact]
        [Trait("Build", "Run")]
        public void Parse_ShouldReturnValidList_OnValidHexArray()
        {
            var validHexArray = Enumerable
                .Range(0, _validAsciiHexString.Length)
                .Where(x => x % 2 == 0)
                .Select(x => Convert.ToByte(_validAsciiHexString.Substring(x, 2), 16))
                .ToArray();

            var tlvs = EmvTagList.Parse(validHexArray);

            AssertTlv(tlvs);
        }

        [Fact]
        [Trait("Build", "Run")]
        public void Parse_ShouldReturnValidList_OnValidAsciiHexStringWithPadding()
        {
            var tlvs = EmvTagList.Parse(_validAsciiHexStringWithPadding);

            AssertTlv(tlvs);
        }

        [Fact]
        [Trait("Build", "Run")]
        public void Parse_SoundReturnValidList_OnValidHexArrayWithPadding()
        {
            var validHexArrayWithPadding = Enumerable
                .Range(0, _validAsciiHexStringWithPadding.Length)
                .Where(x => x % 2 == 0)
                .Select(x => Convert.ToByte(_validAsciiHexStringWithPadding.Substring(x, 2), 16))
                .ToArray();

            var tlvs = EmvTagList.Parse(validHexArrayWithPadding);

            AssertTlv(tlvs);
        }

        [Fact]
        [Trait("Build", "Run")]
        public void Find_ShouldReturnTag_OnMatchWithRootTag()
        {
            var tlvs = EmvTagList.Parse(_validAsciiHexString);
            var _6F = tlvs.FindFirst("6F");

            Assert.NotNull(_6F);
            Assert.True(_6F.HexTag == "6F");
        }

        [Fact]
        [Trait("Build", "Run")]
        public void Find_ShouldReturnTag_OnMatchWithFirstLevelChildTag()
        {
            var tlvs = EmvTagList.Parse(_validAsciiHexString);
            var _84 = tlvs.FindFirst("84");

            Assert.NotNull(_84);
            Assert.True(_84.HexTag == "84");
        }

        [Fact]
        [Trait("Build", "Run")]
        public void Find_ShouldReturnTag_OnMatchWithConstructedTag()
        {
            var tlvs = EmvTagList.Parse(_validAsciiHexString);
            var _tag = tlvs.FindFirst("A5");

            Assert.NotNull(_tag);
            Assert.True(_tag.HexTag == "A5");
        }

        [Fact]
        [Trait("Build", "Run")]
        public void Find_ShouldReturnTag_OnMatchWithSecondLevelChildTag()
        {
            var tlvs = EmvTagList.Parse(_validAsciiHexString);
            var _tag = tlvs.FindFirst("88");

            Assert.NotNull(_tag);
            Assert.True(_tag.HexTag == "88");
        }

        [Fact]
        [Trait("Build", "Run")]
        public void Find_ShouldReturnTag_OnMatchWithMultiByteTag()
        {
            var tlvs = EmvTagList.Parse(_validAsciiHexString);
            var _tag = tlvs.FindFirst("5F2D");

            Assert.NotNull(_tag);
            Assert.True(_tag.HexTag == "5F2D");
        }

        [Fact]
        [Trait("Build", "Run")]
        public void Find_ShouldReturnNull_OnNoMatch()
        {
            var tlvs = EmvTagList.Parse(_validAsciiHexString);
            var _tag = tlvs.FindFirst("5F20");

            Assert.Null(_tag);
        }

        [Fact]
        [Trait("Build", "Run")]
        public void Find_ShouldReturnNull_OnPartialMatchWithMultiByteTag()
        {
            var tlvs = EmvTagList.Parse(_validAsciiHexString);
            var _tag = tlvs.FindFirst("5F");

            Assert.Null(_tag);
        }

        private void AssertTlv(EmvTagList tlvs)
        {
            Assert.NotNull(tlvs);
            Assert.True(tlvs.Count == 1);

            var _6F = tlvs.SingleOrDefault(t => t.HexTag == "6F");
            Assert.NotNull(_6F);
            Assert.True(_6F.Children.Count == 2);

            var _6F84 = _6F.Children.SingleOrDefault(t => t.HexTag == "84");
            Assert.NotNull(_6F84);
            Assert.True(_6F84.Children.Count == 0);
            Assert.True(_6F84.HexValue == "315041592E5359532E4444463031");

            var _6FA5 = _6F.Children.SingleOrDefault(t => t.HexTag == "A5");
            Assert.NotNull(_6FA5);
            Assert.True(_6FA5.Children.Count == 2);

            var _6FA588 = _6FA5.Children.SingleOrDefault(t => t.HexTag == "88");
            Assert.NotNull(_6FA588);
            Assert.True(_6FA588.HexValue == "02");

            var _6FA55F2D = _6FA5.Children.SingleOrDefault(t => t.HexTag == "5F2D");
            Assert.NotNull(_6FA55F2D);
            Assert.True(_6FA55F2D.HexValue == "656E");
        }

    }
}
