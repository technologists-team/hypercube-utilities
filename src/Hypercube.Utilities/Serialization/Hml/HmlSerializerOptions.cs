using System.Globalization;

namespace Hypercube.Utilities.Serialization.Hml;

public record HmlSerializerOptions
{
    public CultureInfo CultureInfo  => CultureInfo.InvariantCulture;
    public bool TrailingComma { get; init; } = true;

    public bool WriteIndented { get; init; }
    public int IndentSize { get; init; } = 2;

    public HmlSerializerOptions()
    {
    }
}