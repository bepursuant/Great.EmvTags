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
            EmvTlvList a = EmvTags.ParseDol("86025F200F");
        }
    }
}
