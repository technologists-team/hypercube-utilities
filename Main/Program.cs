using Hypercube.Utilities.Serialization.Hml;

namespace Main;

public static class Program
{
    public static void Main()
    {
        var data = new { Name = "ТесмиДев", Age = 20, Da = new {}, Roles = new[] {"Programmer", "Driver"} };
        //var data = new[] { new[] { "Programmer" }, new[] { "Programmer" }};
        Console.WriteLine(HmlSerializer.Serialize(data, new HmlSerializerOptions() { WriteIndented = true }));
    }
}