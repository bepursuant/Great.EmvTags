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
            var tlvs = EmvTagList.Parse(_validAsciiHexString);
            var _6F = tlvs.FindFirst(0x6F);

            Assert.NotNull(_6F);
            Assert.True(_6F.HexTag == "6F");
        }

        [Fact]
        [Trait("Build", "Run")]
        public void FindFirst_ShouldLocate_ConstructedTag()
        {
            var tlvs = EmvTagList.Parse(_validAsciiHexString);
            var _tag = tlvs.FindFirst("A5");

            Assert.NotNull(_tag);
            Assert.True(_tag.HexTag == "A5");
        }

        [Fact]
        [Trait("Build", "Run")]
        public void FindFirst_ShouldLocate_FirstLevelChildTag()
        {
            var tlvs = EmvTagList.Parse(_validAsciiHexString);
            var _84 = tlvs.FindFirst("84");

            Assert.NotNull(_84);
            Assert.True(_84.HexTag == "84");
        }

        [Fact]
        [Trait("Build", "Run")]
        public void FindFirst_ShouldLocate_SecondLevelChildTag()
        {
            var tlvs = EmvTagList.Parse(_validAsciiHexString);
            var _tag = tlvs.FindFirst("88");

            Assert.NotNull(_tag);
            Assert.True(_tag.HexTag == "88");
        }

        [Fact]
        [Trait("Build", "Run")]
        public void FindFirst_ShouldLocate_MultiByteTag()
        {
            var tlvs = EmvTagList.Parse(_validAsciiHexString);
            var _tag = tlvs.FindFirst("5F2D");

            Assert.NotNull(_tag);
            Assert.True(_tag.HexTag == "5F2D");
        }

        [Fact]
        [Trait("Build", "Run")]
        public void FindFirst_ShouldReturnNull_OnNoMatch()
        {
            var tlvs = EmvTagList.Parse(_validAsciiHexString);
            var _tag = tlvs.FindFirst("5F20");

            Assert.Null(_tag);
        }

        [Fact]
        [Trait("Build", "Run")]
        public void FindFirst_ShouldReturnNull_OnPartialMatchWithMultiByteTag()
        {
            var tlvs = EmvTagList.Parse(_validAsciiHexString);
            var _tag = tlvs.FindFirst("5F");

            Assert.Null(_tag);
        }

        [Fact]
        [Trait("Build", "Run")]
        public void FindAll_ShouldLocate_RootTags()
        {
            var tlvs = EmvTagList.Parse(_validAsciiHexString);
            tlvs.AddRange(tlvs);

            var _tags = tlvs.FindAll("A5");

            Assert.NotNull(_tags);
            Assert.True(_tags.Count == 2);
        }

        [Fact]
        [Trait("Build", "Run")]
        public void FindAll_ShouldLocate_FirstLevelChildTags()
        {
            var tlvs = EmvTagList.Parse(_validAsciiHexString);
            tlvs.AddRange(tlvs);

            var _tags = tlvs.FindAll(0x84);

            Assert.NotNull(_tags);
            Assert.True(_tags.Count == 2);
        }

        [Fact]
        [Trait("Build", "Run")]
        public void FindAll_ShouldLocate_SecondLevelChildTags()
        {
            var tlvs = EmvTagList.Parse(_validAsciiHexString);
            tlvs.AddRange(tlvs);

            var _tags = tlvs.FindAll(0x88);

            Assert.NotNull(_tags);
            Assert.True(_tags.Count == 2);
        }

        [Fact]
        [Trait("Build", "Run")]
        public void FindAll_ShouldReturnEmptyList_OnNoMatch()
        {
            var tlvs = EmvTagList.Parse(_validAsciiHexString);
            var _tag = tlvs.FindAll("5F20");

            Assert.Empty(_tag);
        }

        [Fact]
        [Trait("Build", "Run")]
        public void FindAll_ShouldReturnEmptyList_OnPartialMatchWithMultiByteTag()
        {
            var tlvs = EmvTagList.Parse(_validAsciiHexString);
            var _tag = tlvs.FindAll("5F");

            Assert.Empty(_tag);
        }
    }
}
