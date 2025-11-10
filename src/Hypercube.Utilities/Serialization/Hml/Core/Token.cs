using System.Diagnostics;

namespace Hypercube.Utilities.Serialization.Hml.Core;

[DebuggerDisplay("{Value} ({Type})")]
public readonly struct Token
{
    public readonly TokenType Type;
    public readonly string Value;

    public Token(TokenType type, string value)
    {
        Type = type;
        Value = value;
    }

    public override string ToString()
    {
        return $"{Value} ({Type})";
    }
}
