using System.Runtime.CompilerServices;
using Hypercube.Utilities.Serialization.Hml.Core.Nodes;
using Hypercube.Utilities.Serialization.Hml.Core.Nodes.Value;
using Hypercube.Utilities.Serialization.Hml.Core.Nodes.Value.Primitives;
using Hypercube.Utilities.Serialization.Hml.Exceptions;
using JetBrains.Annotations;

namespace Hypercube.Utilities.Serialization.Hml.Core;

[PublicAPI]
public sealed class HmlParser
{
    private readonly IReadOnlyList<Token> _tokens;
    private readonly HmlSerializerOptions _options;

    private INode _context = null!;
    private int _position;

    private Token Current => _tokens[_position];
    
    public HmlParser(IReadOnlyList<Token> tokens, HmlSerializerOptions options)
    {
        _tokens = tokens;
        _options = options;
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

        return (RootNode) _context;
    }
    
    private Token Consume(params TokenType[] expected)
    {
        var token = Current;
        if (!expected.Contains(token.Type))
            throw new HmlException($"Expected {string.Join(" or ", expected)}, got {token.Type}");

        _position++;
        return token;
    }
    
    private Token Consume(TokenType expected)
    {
        var token = Current;
        if (token.Type != expected)
            throw new HmlException($"Expected {expected}, got {token.Type}");
        
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
        if (Match(TokenType.LBrace))
            return ParseObject();
        
        if (Match(TokenType.LBracket))
            return ParseArray();
        
        return ParseLiteral();
    }

    private ObjectNode ParseObject()
    {
        Consume(TokenType.LBrace);
        
        if (Match(TokenType.EndOfLine))
            Consume(TokenType.EndOfLine);
        
        var obj = new ObjectNode();
        obj.SetParent(_context);

        if (_context is RootNode rootNode)
            rootNode.Child = obj;
        
        var previousContext = _context;
        _context = obj;

        while (!Match(TokenType.RBrace))
        {
            var key = Consume(TokenType.Identifier, TokenType.String).Value;
            
            if (Match(TokenType.Colon))
                Consume(TokenType.Colon);
            
            var val = ParseValue();
            obj.Add(new KeyValuePairNode(new IdentifierNode(key), val));
            
            if (Match(TokenType.Semicolon))
                Consume(TokenType.Semicolon);
            
            if (Match(TokenType.EndOfLine))
                Consume(TokenType.EndOfLine);
        }
        
        _context = previousContext;

        Consume(TokenType.RBrace);
        return obj;
    }

    private ListNode ParseArray()
    {
        Consume(TokenType.LBracket);
        
        if (Match(TokenType.EndOfLine))
            Consume(TokenType.EndOfLine);

        var list = new ListNode();
        list.SetParent(_context);
        
        if (_context is RootNode rootNode)
            rootNode.Child = list;

        var previousContext = _context;
        _context = list;

        while (!Match(TokenType.RBracket))
        {
            // NOTE: Why dictionary parsing here?
            // var key = Consume(TokenType.Identifier).Value;
            // Consume(TokenType.Colon);
            
            var value = ParseValue();
            list.Elements.Add(value);
            
            // list.Elements.Add(new KeyValuePair<string, IValueNode>(key, value));
            
            if (Match(TokenType.Semicolon))
                Consume(TokenType.Semicolon);
            
            if (Match(TokenType.EndOfLine))
                Consume(TokenType.EndOfLine);
        }

        _context = previousContext;
        
        Consume(TokenType.RBracket);
        return list;
    }

    private IValueNode ParseLiteral()
    {
        var token = Current;
        _position++;
        
        return token.Type switch
        {
            TokenType.Number => new NumberValueNode(decimal.Parse(token.Value, _options.CultureInfo)),
            TokenType.Boolean => new BoolValue(bool.Parse(token.Value)),
            TokenType.String => new StringValueNode(token.Value.Trim('"').Trim('\'')),
            TokenType.Identifier when token.Value == "null" => new NullValueNode(),
            _ => throw new HmlException($"Cannot parse literal: {token.Type} ({token.Value})")
        };
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private Token Next() => _tokens[_position++];

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private bool Match(TokenType type) => _tokens[_position].Type == type;
}