using System.Globalization;

namespace Hypercube.Utilities.Serialization.Hml;

public record HmlSerializerOptions
{
    public CultureInfo CultureInfo { get; init; } = CultureInfo.InvariantCulture;
    public bool Eol { get; init; } = false;
    public bool Indented { get; init; }
    public int IndentSize { get; init; } = 2;
}
