using Hypercube.Utilities.Serialization.Hml;
using Hypercube.Utilities.Serialization.Hml.Core;

namespace Main;

public static class Program
{
    public static void Main()
    {
        var data = new
        {
            Name = "ТесмиДев",
            Age = 20,
            Obj = new {},
            Roles = new[]
            {
                "Programmer",
                "Driver"
            }
        };
        
        var options = new HmlSerializerOptions
        {
            Eol = false,
            Indented = true,
            IndentSize = 2
        };

        var serialized = HmlSerializer.Serialize(data, options);
        var tokens = HmlLexer.Tokenize(serialized);
        
        Console.WriteLine(HmlSerializer.Serialize(data, options));
        Console.WriteLine(string.Join(Environment.NewLine, tokens));
    }
}
