using Xunit;

namespace Great.EmvTags.Tests
{
    public class FindTests
    {
        private const string _validAsciiHexString = "6F1A840E315041592E5359532E4444463031A5088801025F2D02656E";

        [Fact]
        [Trait("Build", "Run")]
        public void FindFirst_ShouldLocate_RootTag()
        {
            var tlvs = EmvTlvList.Parse(_validAsciiHexString);
            var _6F = tlvs.FindFirst(0x6F);

            Assert.NotNull(_6F);
            Assert.True(_6F.Tag.Hex == "6F");
        }

        [Fact]
        [Trait("Build", "Run")]
        public void FindFirst_ShouldLocate_ConstructedTag()
        {
            var tlvs = EmvTlvList.Parse(_validAsciiHexString);
            var _tag = tlvs.FindFirst("A5");

            Assert.NotNull(_tag);
            Assert.True(_tag.Tag.Hex == "A5");
        }

        [Fact]
        [Trait("Build", "Run")]
        public void FindFirst_ShouldLocate_FirstLevelChildTag()
        {
            var tlvs = EmvTlvList.Parse(_validAsciiHexString);
            var _84 = tlvs.FindFirst("84");

            Assert.NotNull(_84);
            Assert.True(_84.Tag.Hex == "84");
        }

        [Fact]
        [Trait("Build", "Run")]
        public void FindFirst_ShouldLocate_SecondLevelChildTag()
        {
            var tlvs = EmvTlvList.Parse(_validAsciiHexString);
            var _tag = tlvs.FindFirst("88");

            Assert.NotNull(_tag);
            Assert.True(_tag.Tag.Hex == "88");
        }

        [Fact]
        [Trait("Build", "Run")]
        public void FindFirst_ShouldLocate_MultiByteTag()
        {
            var tlvs = EmvTlvList.Parse(_validAsciiHexString);
            var _tag = tlvs.FindFirst("5F2D");

            Assert.NotNull(_tag);
            Assert.True(_tag.Tag.Hex == "5F2D");
        }

        [Fact]
        [Trait("Build", "Run")]
        public void FindFirst_ShouldReturnNull_OnNoMatch()
        {
            var tlvs = EmvTlvList.Parse(_validAsciiHexString);
            var _tag = tlvs.FindFirst("5F20");

            Assert.Null(_tag);
        }

        [Fact]
        [Trait("Build", "Run")]
        public void FindFirst_ShouldReturnNull_OnPartialMatchWithMultiByteTag()
        {
            var tlvs = EmvTlvList.Parse(_validAsciiHexString);
            var _tag = tlvs.FindFirst("5F");

            Assert.Null(_tag);
        }

        [Fact]
        [Trait("Build", "Run")]
        public void FindAll_ShouldLocate_RootTags()
        {
            var tlvs = EmvTlvList.Parse(_validAsciiHexString);
            tlvs.AddRange(tlvs);

            var _tags = tlvs.FindAll("A5");

            Assert.NotNull(_tags);
            Assert.True(_tags.Count == 2);
        }

        [Fact]
        [Trait("Build", "Run")]
        public void FindAll_ShouldLocate_FirstLevelChildTags()
        {
            var tlvs = EmvTlvList.Parse(_validAsciiHexString);
            tlvs.AddRange(tlvs);

            var _tags = tlvs.FindAll(0x84);

            Assert.NotNull(_tags);
            Assert.True(_tags.Count == 2);
        }

        [Fact]
        [Trait("Build", "Run")]
        public void FindAll_ShouldLocate_SecondLevelChildTags()
        {
            var tlvs = EmvTlvList.Parse(_validAsciiHexString);
            tlvs.AddRange(tlvs);

            var _tags = tlvs.FindAll(0x88);

            Assert.NotNull(_tags);
            Assert.True(_tags.Count == 2);
        }

        [Fact]
        [Trait("Build", "Run")]
        public void FindAll_ShouldReturnEmptyList_OnNoMatch()
        {
            var tlvs = EmvTlvList.Parse(_validAsciiHexString);
            var _tag = tlvs.FindAll("5F20");

            Assert.Empty(_tag);
        }

        [Fact]
        [Trait("Build", "Run")]
        public void FindAll_ShouldReturnEmptyList_OnPartialMatchWithMultiByteTag()
        {
            var tlvs = EmvTlvList.Parse(_validAsciiHexString);
            var _tag = tlvs.FindAll("5F");

            Assert.Empty(_tag);
        }
    }
}
