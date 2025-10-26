namespace Hypercube.Utilities.Serialization.Hml.Core;

public static class Tokens
{
    public static readonly Dictionary<TokenType, string> TokenRegexs = new()
    {
        { TokenType.Number, @"\d+(\.\d+)?" },
        { TokenType.Boolean, @"\b(true|false)\b" },
        
        { TokenType.String, "\"(?<str>(?:[^\"\\\\]|\\\\.)*)\"" },
        { TokenType.Char, @"'(?<ch>(?:[^'\\]|\\.))'" },
        
        { TokenType.Identifier, @"\b[a-zA-Z_!][a-zA-Z0-9_!]*\b|!" },
        
        { TokenType.Colon, ":" },
        { TokenType.Equal, "=" },
        { TokenType.LAngle, @"\<" },
        { TokenType.RAngle, @"\>" },
        { TokenType.LBracket, @"\[" },
        { TokenType.RBracket, @"\]" },
        { TokenType.LParen, @"\(" },
        { TokenType.RParen, @"\)" },
        { TokenType.LBrace, @"\{" },
        { TokenType.RBrace, @"\}" },
        { TokenType.Comma, "," },
        
        { TokenType.Comment, "#.*" },
        
        { TokenType.Indent, @"(?<=\n)([ \t]+)(?=\S)" },
        { TokenType.Dedent, @"(?<=\n)(?=\S)" },
        
        { TokenType.EndOfFile, @"\z" }
    };
}
