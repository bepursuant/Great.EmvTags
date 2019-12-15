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
            //EmvTlvList a = EmvTags.ParseDol("86025F200F");

            var a = new EmvTlv() { Value = new byte[]{ 0x5F, 0x20 } };
            var b = new EmvTlv() { Value = "05AB" };

            var c = new Foo("05AB", new byte[] { 0x01 }, 0x2D);

        }
    }
}
