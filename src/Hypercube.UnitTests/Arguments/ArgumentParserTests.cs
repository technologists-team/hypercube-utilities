using Hypercube.Utilities.Arguments;

namespace Hypercube.UnitTests.Arguments;

[TestFixture]
public sealed class ArgumentParserTests
{
    [Test]
    public void FlagTest()
    {
        var parser = new ArgumentParser()
            .AddFlag("verbose");
        
        parser.Parse(new[] { "--verbose" });
        
        Assert.True(parser.Get<bool>("verbose"));
    }
}