using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Great.EmvTags.Tests
{
    public class FindTests
    {
        private const string _validAsciiHexString = "6F1A840E315041592E5359532E4444463031A5088801025F2D02656E";

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

    }
}
