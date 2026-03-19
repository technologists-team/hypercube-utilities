using Hypercube.Utilities.Serialization.Hml;
using Hypercube.Utilities.Serialization.Hml.Core;

namespace Main;

public static class Program
{
    public static void Main()
    {
        var data = new TestData
        {
            Name = "ТесмиДев",
            Age = 20,
            Object = new TestObject(),
            Roles =
            [
                "Programmer",
                "Driver"
            ],
            Recursion = new TestData
            {
                Name = "TornadoTech",
                Age = 0x1F1A,
                Object = null!,
                Roles = [],
                Recursion = null,
            }
        };
        
        var options = new HmlSerializerOptions
        {
            ListEol = false,
            Indented = true,
            IndentSize = 2
        };
        
        var serialized = HmlSerializer.Serialize(data, options);
        var deserialized = HmlSerializer.Deserialize<TestData>(serialized, options);
        
        Console.WriteLine(serialized);
    }

    private class TestData
    {
        public string Name;
        public int Age;
        public TestObject Object;
        public List<string> Roles;
        public TestData? Recursion;
    }

    private class TestObject;
}
