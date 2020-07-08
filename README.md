# Great.EmvTags

EMV Tag Parsing and Management Library

## Examples

```csharp
using Great.EmvTags;

// parse input from a string
EmvTagList tags = EmvTagList.Parse("6F1A840E315041592E5359532E4444463031A5088801025F2D02656E");

// search for a single tag
EmvTag _tag = tags.FindFirst("5F2D");

// search for multiple tags
EmvTagList _tags = tags.Findall("5F2D");

```

## Changelog

### 2.1.0
- Adds support for XML serialization of EmvTlv and EmvTlvList

### 2.0.0
- Revised byte/byte[]/string into an ExtendedByteArray object to simplify all calls - pass any of these types now!
- Added .Tlv property to EmvTlv and EmvTlvList to generate TLV data for either type

### 1.1.2
- Added EmvTag.Parse() method to parse value to a single tag
- Added FindFirst and FindAll methods that accept string tag inputs

### 1.1.0

- Fixes #1 - Incorrect Parsing of Multi-Byte Length Tags
- Closes #2 - Enhancements to Find Functionality
- Closes #3 - Add Integer Length Property

### 1.0.0

Initial release, based on BerTlv.NET library by Kyle Spearrin
