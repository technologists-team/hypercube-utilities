using Hypercube.Utilities.Serialization.Hml.Core;

namespace Hypercube.Utilities.Serialization.Hml;

public static class HmlSerializer
{
    public static string Serialize(object obj, HmlSerializerOptions? options = null)
    {
        return HmlCompiler.Serialize(obj, options ?? new HmlSerializerOptions());
    }
    
    public static object Deserialize(string content, HmlSerializerOptions? options = null)
    {
        options ??= new HmlSerializerOptions();
        
        var tokens = HmlLexer.Tokenize(content);
        var parser = new HmlParser(tokens, options);
        var ast = parser.Parse();

        return HmlDeserializer.Compile<object>(ast, options);
    }
    
    public static T Deserialize<T>(string content, HmlSerializerOptions? options = null)
    {
        options ??= new HmlSerializerOptions();
        
        var tokens = HmlLexer.Tokenize(content);
        var parser = new HmlParser(tokens, options);
        var ast = parser.Parse();

        return HmlDeserializer.Compile<T>(ast, options);
    }
}