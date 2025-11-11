using System.Diagnostics;
using System.Runtime.CompilerServices;

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
        return $"{Type}: {ValueFormat()}";
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public string ValueFormat()
    {
        return Type switch
        {
            TokenType.EndOfLine => @"\r\n",
            TokenType.EndOfFile => @"\z",
            _ => Value
        };
    }
}
