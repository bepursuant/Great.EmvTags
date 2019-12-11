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
            EmvTag a = new EmvTag();

            a.TagBytes = new byte[] { 0x5A };
            a.LengthBytes = new byte[] { 0x81, 0xC1 };

            Debug.WriteLine($"TagHex: {a.TagHex} ");
            Debug.WriteLine($"LengthHex: {a.LengthHex} ");
            Debug.WriteLine($"ValueHex: {a.ValueHex} ");
        }
    }
}
