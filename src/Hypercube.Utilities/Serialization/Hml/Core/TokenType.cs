namespace Hypercube.Utilities.Serialization.Hml.Core;

public enum TokenType : short
{
    Identifier,
    Number,
    String,
    Colon,
    Equal,
    LBracket,
    RBracket,
    LParen,
    RParen,
    Comma,
    Comment,
    Indent,
    Dedent,
    EndOfFile,
    LBrace,
    RBrace,
    Boolean,
    Char,
    LAngle,
    RAngle
}
