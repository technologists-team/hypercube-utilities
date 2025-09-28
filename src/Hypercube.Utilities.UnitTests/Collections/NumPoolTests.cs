using Hypercube.Utilities.Collections;

namespace Hypercube.Utilities.UnitTests.Collections;

[TestFixture]
public sealed class NumPoolTests
{
    [Test]
    public void Next()
    {
        var bytePool = new NumPool<byte>();
        Assert.That(bytePool.Next, Is.EqualTo(0));
        Assert.That(bytePool.Next, Is.EqualTo(1));
        Assert.That(bytePool.Next, Is.EqualTo(2));
        Assert.That(bytePool.Next, Is.TypeOf<byte>());
        
        var intPool = new NumPool<int>();
        Assert.That(intPool.Next, Is.EqualTo(0));
        Assert.That(intPool.Next, Is.EqualTo(1));
        Assert.That(intPool.Next, Is.EqualTo(2));
        Assert.That(intPool.Next, Is.TypeOf<int>());
        
        var longPool = new NumPool<long>();
        Assert.That(longPool.Next, Is.EqualTo(0L));
        Assert.That(longPool.Next, Is.EqualTo(1L));
        Assert.That(longPool.Next, Is.EqualTo(2L));
        Assert.That(longPool.Next, Is.TypeOf<long>());
        
        var doublePool = new NumPool<double>();
        Assert.That(doublePool.Next, Is.EqualTo(0D));
        Assert.That(doublePool.Next, Is.EqualTo(1D));
        Assert.That(doublePool.Next, Is.EqualTo(2D));
        Assert.That(doublePool.Next, Is.TypeOf<double>());
    }

    [Test]
    public void Release()
    {
        var pool = new NumPool<int>();
        var first = pool.Next;
        
        _ = pool.Next;
        
        pool.Release(first);
        
        Assert.That(pool.Next, Is.EqualTo(first));
        Assert.That(pool.Next, Is.EqualTo(2));
    }

    [Test]
    public void ReleaseInvalidValuesThrow()
    {
        var pool = new NumPool<int>();
        _ = pool.Next;
        _ = pool.Next;
        
        Assert.That(() => pool.Release(-1), Throws.ArgumentException);
        Assert.That(() => pool.Release(100), Throws.ArgumentException);
        
        pool.Release(0);
        
        Assert.That(() => pool.Release(0), Throws.ArgumentException);
    }

    [Test]
    public void NextPrefersReleasedNumbersLifoOrder()
    {
        var pool = new NumPool<int>();
        
        var n0 = pool.Next;
        var n1 = pool.Next;
        
        _ = pool.Next;
        
        pool.Release(n1);
        pool.Release(n0);
        
        Assert.That(pool.Next, Is.EqualTo(0));
        Assert.That(pool.Next, Is.EqualTo(1));
        Assert.That(pool.Next, Is.EqualTo(3));
    }
}
