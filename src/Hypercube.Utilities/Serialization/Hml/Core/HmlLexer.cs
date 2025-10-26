using System.Text;
using System.Text.RegularExpressions;

namespace Hypercube.Utilities.Serialization.Hml.Core;

public static class HmlLexer
{
    private static readonly Regex Regex;

    static HmlLexer()
    {
        var finalPatternBuilder = new StringBuilder();
        var i = 0;
        foreach (var kv in Tokens.TokenRegexs)
        {
            finalPatternBuilder.Append($"(?<{kv.Key}>{kv.Value})");
            if (i < Tokens.TokenRegexs.Count - 1)
                finalPatternBuilder.Append('|');
            i++;
        }
        
        Regex = new Regex(finalPatternBuilder.ToString(), RegexOptions.Compiled | RegexOptions.Multiline);
    }
    
    public static IReadOnlyList<Token> Tokenize(string input)
    {
        var tokens = new List<Token>();

        foreach (Match match in Regex.Matches(input))
        {
            var tokenType = Enum.GetValues<TokenType>()
                .FirstOrDefault(type => match.Groups[type.ToString()].Success);

            var value = match.Value;
            
            if (string.IsNullOrWhiteSpace(value))
                continue;

            tokens.Add(new Token(tokenType, value));
        }
        
        tokens.Add(new Token(TokenType.EndOfFile, string.Empty));

        return tokens;
    }
}