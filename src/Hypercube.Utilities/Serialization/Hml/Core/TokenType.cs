namespace Hypercube.Utilities.Serialization.Hml.Core;

public enum TokenType : short
{
    // Fields
    Identifier,
    
    // Primitives
    Number,
    Boolean,
    String,

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
