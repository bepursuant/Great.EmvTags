# Great.EmvTags

EMV Tag Parsing and Management Library

## Examples

```csharp
using Great.EmvTags;

// parse input from a string
var tags = EmvTagList.Parse("6F1A840E315041592E5359532E4444463031A5088801025F2D02656E");

// search for a tag
var tag = tags.FindFirst("5F2D");

```

## Changelog

### 1.1.0

- Fixes #1 - Incorrect Parsing of Multi-Byte Length Tags

### 1.0.0

Initial release, based on BerTlv.NET library by Kyle Spearrin
