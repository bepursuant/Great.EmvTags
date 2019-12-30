using System;
using Xunit;

namespace Great.EmvTags.Tests
{
    public class ExceptionTests
    {
        [Fact]
        [Trait("Build", "Run")]
        public void Parse_ShouldReturnEmptyList_OnEmptyString()
        {
            Assert.Empty(EmvTlvList.Parse(""));
        }

        [Fact]
        [Trait("Build", "Run")]
        public void Parse_ShouldReturnEmptyList_OnEmptyHexArray()
        {
            Assert.Empty(EmvTlvList.Parse(new byte[] { }));
        }
    }
}
