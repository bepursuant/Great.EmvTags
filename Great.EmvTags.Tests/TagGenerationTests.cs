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
            EmvTlv t = new EmvTlv("9A", "191122");
            var a = t.Tlv;
        }
    }
}
