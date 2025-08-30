using JetBrains.Annotations;

namespace Hypercube.Utilities.Debugging.Logger;

[PublicAPI]
public class ConsoleLogger : Logger
{
    public override void Echo(string message)
    {
        Console.WriteLine(message);
    }
}
