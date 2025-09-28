using Hypercube.Utilities.Arguments;

namespace Hypercube.Utilities.UnitTests.Arguments;

[TestFixture]
public sealed class ArgumentParserTests
{
    private static readonly object[] NumberTestCases =
    {
        new object[] { "sbyte", (sbyte) 10 },
        new object[] { "byte", (byte) 10 },
        new object[] { "short", (short) 30 },
        new object[] { "ushort", (ushort) 40 },
        new object[] { "int", 50 },
        new object[] { "uint", 60u },
        new object[] { "long", 50L },
        new object[] { "ulong", 60ul }
    };
    
    [Test]
    public void FlagParsingTest()
    {
        var parser = new ArgumentParser()
            .AddFlag("verbose");
        
        parser.Parse(["--verbose"]);
        
        Assert.That(parser.Get<bool>("verbose"), Is.True);
    }
    
    [Test, TestCaseSource(nameof(NumberTestCases))]
    public void NumberParsingTest<T>(string key, T expectedValue)
    {
        var parser = new ArgumentParser()
            .AddOption<T>(key);
        
        parser.Parse([$"--{key}={expectedValue}"]);

        Assert.That(parser.Get<T>(key), Is.EqualTo(expectedValue));
    }

    [Test]
    public void ListParseTest()
    {
        var parser = new ArgumentParser()
            .AddListOption<string>("tag");
        
        parser.Parse(["--tag", "Just", "--tag", "a", "--tag", "Test"]);
        
        Assert.That(parser.GetList<string>("tag"), Is.EqualTo(["Just", "a", "Test"]));
    }
    
    [Test]
    public void DefaultsTest()
    {
        var parser = new ArgumentParser()
            .AddOption<string>("name", @default: "Tornado");
        
        parser.Parse([]);
        
        Assert.That(parser.Get<string>("name"), Is.EqualTo("Tornado"));
    }
}