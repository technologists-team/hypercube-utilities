using System.Globalization;

namespace Hypercube.Utilities.Serialization.Hml;

// TODO: TrailingComma
public record HmlSerializerOptions()
{
    public CultureInfo CultureInfo { get; init; } = CultureInfo.InvariantCulture;
    public bool ListEol { get; init; }
    public bool ObjectEol { get; init; }
    public bool RootAsIdentifier { get; init; }
    public bool Indented { get; init; }
    public int IndentSize { get; init; } = 2;
}
