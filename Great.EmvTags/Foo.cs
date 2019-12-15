using System;
using System.Collections.Generic;
using System.Text;

namespace Great.EmvTags
{
    public class Foo
    {
        public ExtendedByteArray Tag { get; set; }
        public ExtendedByteArray Length { get; set; }
        public ExtendedByteArray Value { get; set; }

        public Foo(ExtendedByteArray tag, ExtendedByteArray length, ExtendedByteArray value)
        {
            Tag = tag;
            Length = length;
            Value = value;
        }
    }
}
