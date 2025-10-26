using System.Globalization;

namespace Hypercube.Utilities.Serialization.Hml;

public readonly struct HmlSerializerOptions
{
    public CultureInfo CultureInfo { get; init; } = CultureInfo.InvariantCulture;
    public bool TrailingComma { get; init; } = true;
    public int IndentSize { get; init; } = 2;

    public HmlSerializerOptions()
    {
    }
}