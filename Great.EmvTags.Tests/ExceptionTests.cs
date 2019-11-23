using System;
using Xunit;

namespace Great.EmvTags.Tests
{
    public class ExceptionTests
    {
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
    }
}
