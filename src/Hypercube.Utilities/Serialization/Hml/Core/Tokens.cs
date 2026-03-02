namespace Hypercube.Utilities.Serialization.Hml.Core;

public static class Tokens
{
    public static readonly Dictionary<TokenType, string> TokenRegexs = new()
    {
        // Ends
        { TokenType.EndOfLine, @"\r?\n" },
        { TokenType.EndOfFile, @"\z" },

        // Fields
        { TokenType.Identifier, @"\b[a-zA-Z_!][a-zA-Z0-9_!]*\b|!" },
        
        // Primitives
        { TokenType.Number, @"\d+(\.\d+)?" },
        { TokenType.Boolean, @"\b(true|false)\b" },
        { TokenType.String,  """(?:"([^"\\\r\n]|\\.)*"|'([^'\\\r\n]|\\.)*')""" },
        
        // Symbols
        { TokenType.Colon, ":" },
        { TokenType.Equal, "=" },
        { TokenType.Semicolon, ";" },
        { TokenType.Comma, "," },
        { TokenType.Dollar, @"\$" },
        
        // Groups
        { TokenType.LAngle, @"\<" },
        { TokenType.RAngle, @"\>" },
        { TokenType.LBracket, @"\[" },
        { TokenType.RBracket, @"\]" },
        { TokenType.LParen, @"\(" },
        { TokenType.RParen, @"\)" },
        { TokenType.LBrace, @"\{" },
        { TokenType.RBrace, @"\}" },
        
        // Comments
        { TokenType.Comment, @"//[^\r\n]*" },
        { TokenType.LComment, "//*" },
        { TokenType.RComment, @"\*/" },
    };
}
