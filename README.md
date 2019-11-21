# Great.EmvTags

EMV Tag Parsing and Management Library



## Get It


## Use It

First add the using statement:

```csharp
using Great.EmvTags;
```

then:

```csharp
ICollection<Tlv> tlvs = Tlv.ParseTlv("6F1A840E315041592E5359532E4444463031A5088801025F2D02656E");

// Use the tlvs collection now.
```

## Changelog

### 1.0.0

Initial release, based on BerTlv.NET library by Kyle Spearrin
