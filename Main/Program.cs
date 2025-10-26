using Hypercube.Utilities.Serialization.Hml;
using Hypercube.Utilities.Serialization.Hml.Core;

namespace Main;

public static class Program
{
    public static void Main()
    {
        var data = new { Name = "ТесмиДев", Age = 20, Roles = new[] {"Programmer", "Driver"} };
        HmlCompiler.SerializeNew(data, new HmlSerializerOptions());
    }
}