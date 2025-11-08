using Hypercube.Utilities.Serialization.Hml;

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
            Indented = true,
            IndentSize = 4
        };
        
        Console.WriteLine(HmlSerializer.Serialize(data, options));
    }
}
