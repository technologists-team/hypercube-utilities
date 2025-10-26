using System.Runtime.CompilerServices;
using Hypercube.Utilities.Serialization.Hml.Core.Nodes;
using Hypercube.Utilities.Serialization.Hml.Core.Nodes.Value;

namespace Hypercube.Utilities.Serialization.Hml.Core;

public static class HmlParser
{
    public static IReadOnlyList<DefinitionNode> Parse(IReadOnlyList<Token> tokens)
    {
        var result = new List<DefinitionNode>();
        var pos = 0;

        while (!Match(tokens, ref pos, TokenType.EndOfFile))
            result.Add(ParseDefinition(tokens, ref pos));

        return result;
    }

    private static Token Consume(IReadOnlyList<Token> tokens, ref int pos, TokenType expected)
    {
        var token = Peek(tokens, ref pos);
        if (token.Type != expected)
            throw new Exception($"Expected {expected}, got {token.Type}");
        pos++;
        return token;
    }

    private static DefinitionNode ParseDefinition(IReadOnlyList<Token> tokens, ref int pos)
    {
        var name = Consume(tokens, ref pos, TokenType.Identifier).Value;
        Consume(tokens, ref pos, TokenType.LAngle);
        var typeName = Consume(tokens, ref pos, TokenType.Identifier).Value;
        Consume(tokens, ref pos, TokenType.RAngle);
        Consume(tokens, ref pos, TokenType.Colon);

        var value = ParseValue(tokens, ref pos);

        return new DefinitionNode
        {
            Name = name,
            TypeName = typeName,
            Value = value
        };
    }

    private static ValueNode ParseValue(IReadOnlyList<Token> tokens, ref int pos)
    {
        if (Match(tokens, ref pos, TokenType.LBrace)) return ParseObject(tokens, ref pos);
        if (Match(tokens, ref pos, TokenType.LBracket)) return ParseArray(tokens, ref pos);
        return ParseLiteral(tokens, ref pos);
    }

    private static ObjectNode ParseObject(IReadOnlyList<Token> tokens, ref int pos)
    {
        Consume(tokens, ref pos, TokenType.LBrace);
        var obj = new ObjectNode();

        while (!Match(tokens, ref pos, TokenType.RBrace))
        {
            var key = Consume(tokens, ref pos, TokenType.Identifier).Value;
            Consume(tokens, ref pos, TokenType.Colon);
            var val = ParseValue(tokens, ref pos);
            obj.Properties1[key] = val;
        }

        Consume(tokens, ref pos, TokenType.RBrace);
        return obj;
    }

    private static ArrayNode ParseArray(IReadOnlyList<Token> tokens, ref int pos)
    {
        Consume(tokens, ref pos, TokenType.LBracket);
        var arr = new ArrayNode();

        while (!Match(tokens, ref pos, TokenType.RBracket))
        {
            var key = Consume(tokens, ref pos, TokenType.Identifier).Value;
            Consume(tokens, ref pos, TokenType.Colon);
            var val = ParseValue(tokens, ref pos);
            arr.Elements.Add(new KeyValuePair<string, ValueNode>(key, val));
        }

        Consume(tokens, ref pos, TokenType.RBracket);
        return arr;
    }

    private static LiteralNode ParseLiteral(IReadOnlyList<Token> tokens, ref int pos)
    {
        var tok = Peek(tokens, ref pos);
        if (tok.Type is TokenType.String or TokenType.Number or TokenType.Boolean)
        {
            Next(tokens, ref pos);
            return new LiteralNode { Value = tok.Value };
        }

        throw new Exception($"Expected literal, got {tok.Type}");
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static Token Peek(IReadOnlyList<Token> tokens, ref int pos)
    {
        return tokens[pos];
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static Token Next(IReadOnlyList<Token> tokens, ref int pos)
    {
        return tokens[pos++];
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static bool Match(IReadOnlyList<Token> tokens, ref int pos, TokenType type)
    {
        return tokens[pos].Type == type;
    }
}