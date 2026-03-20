namespace Hypercube.Utilities.Serialization.Hml.Core;

public enum TokenType : short
{
    // Primitives (must be before Identifier - more specific patterns)
    Number,
    Boolean,
    String,

    // Fields
    Identifier,

    // Symbols
    Colon,
    Equal,
    Semicolon,
    Comma,
    Dollar,

    // Groups
    LBracket,
    RBracket,
    LParen,
    RParen,
    LBrace,
    RBrace,
    LAngle,
    RAngle,

    // Comments
    Comment,
    LComment,
    RComment,

    // Ends
    EndOfFile,
    EndOfLine
}
