using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Xunit;

namespace Great.EmvTags.Tests
{
    public class TagGenerationTests
    {
        [Fact]
        [Trait("Build", "Run")]
        public void Test1()
        {
            EmvTlv a = EmvTlvParser.ParseTlv("6F1A840E315041592E5359532E4444463031A5088801025F2D02656E");
        }
    }
}
