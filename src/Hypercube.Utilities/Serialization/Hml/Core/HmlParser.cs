using System.Runtime.CompilerServices;
using Hypercube.Utilities.Serialization.Hml.Core.Nodes;
using Hypercube.Utilities.Serialization.Hml.Core.Nodes.Value;

namespace Hypercube.Utilities.Serialization.Hml.Core;

public class HmlParser
{
    private IReadOnlyList<Token> _tokens;
    private int _position;
    private INode _context;

    private Token Current => _tokens[_position];
    
    public HmlParser(IReadOnlyList<Token> tokens)
    {
        _tokens = tokens;
    }

    public void Reset()
    {
        _context = new RootNode();
        _position = 0;
    }
    
    public RootNode Parse()
    {
        Reset();

        while (!Match(TokenType.EndOfFile))
        {
            switch (Current.Type)
            {
                case TokenType.LBrace:
                    ParseObject();
                    break;
                
                case TokenType.LBracket:
                    ParseArray();
                    break;
            }
        }

        return _context;
    }

    private Token Consume(TokenType expected)
    {
        var token = Current;
        if (token.Type != expected)
            throw new Exception($"Expected {expected}, got {token.Type}");
        _position++;
        return token;
    }

    /*private static DefinitionNode ParseDefinition(IReadOnlyList<Token> tokens, ref int pos)
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
    }*/

    private IValueNode ParseValue()
    {
        if (Match(TokenType.LBrace)) return ParseObject();
        if (Match(TokenType.LBracket)) return ParseArray();
        //return ParseLiteral(tokens, ref pos);
        return default!;
    }

    private void ParseObject()
    {
        Consume(TokenType.LBrace);
        var obj = new ObjectNode();
        obj.SetParent(_context);

        _context = obj;

        while (!Match(TokenType.RBrace))
        {
            if (!Match(TokenType.String) && !Match(TokenType.Identifier))
            {
                
            }
            var key = Consume(TokenType.Identifier).Value;
            Consume(TokenType.Colon);
            var val = ParseValue();
            
            obj.Add(new KeyValuePairNode(new IdentifierNode(key), val));
        }

        Consume(TokenType.RBrace);
    }

    private ListNode ParseArray()
    {
        Consume(TokenType.LBracket);
        var arr = new ListNode();

        while (!Match(TokenType.RBracket))
        {
            var key = Consume(TokenType.Identifier).Value;
            Consume(TokenType.Colon);
            var val = ParseValue();
            //arr.Elements.Add(new KeyValuePair<string, ValueNode>(key, val));
        }

        Consume(TokenType.RBracket);
        return arr;
    }

   /* private static LiteralNode ParseLiteral(IReadOnlyList<Token> tokens, ref int pos)
    {
        var tok = Peek(tokens, ref pos);
        if (tok.Type is TokenType.String or TokenType.Number or TokenType.Boolean)
        {
            Next(tokens, ref pos);
            return new LiteralNode { Value = tok.Value };
        }

        throw new Exception($"Expected literal, got {tok.Type}");
    }*/

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private Token Next()
    {
        return _tokens[_position++];
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private bool Match(TokenType type)
    {
        return _tokens[_position].Type == type;
    }
}