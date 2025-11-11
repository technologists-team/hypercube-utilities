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
        throw new Exception();
    }
    
    public static T Deserialize<T>(string content, HmlSerializerOptions? options = null)
    {
        /*options ??= new HmlSerializerOptions();
        
        var tokens = HmlLexer.Tokenize(content);
        var ast = HmlParser.Parse(tokens);*/

        return default!; //compiler.Compile(ast, options);
    }
}