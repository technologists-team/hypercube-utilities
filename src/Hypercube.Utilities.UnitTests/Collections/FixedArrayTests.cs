using Hypercube.Utilities.Collections;

namespace Hypercube.Utilities.UnitTests.Collections;

[TestFixture]
public sealed class FixedArrayTests
{
    [Test]
    public void GenerationTest()
    {
        var array = new FixedArray2<int>();
        
        Assert.That(array[0], Is.EqualTo(0));
        Assert.That(array[1], Is.EqualTo(0));

        array[0] = 10;
        
        Assert.That(array[0], Is.EqualTo(10));
    }
}
