using Hypercube.Utilities.Collections.Bit;

namespace Hypercube.Utilities.UnitTests.Collections;

[TestFixture]
public sealed class BitSetTests
{
    [Test]
    public void ConstructorTest()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => _ = new BitSet(0));
        Assert.Throws<ArgumentOutOfRangeException>(() => _ = new BitSet(-1));
        
        var bitSet = new BitSet(100);
        Assert.That(bitSet.Size, Is.EqualTo(100));
    }
    
    [Test]
    public void HasTest()
    {
        var bitSet = new BitSet(64);
        bitSet.Set(10);
        
        Assert.Throws<ArgumentOutOfRangeException>(() => bitSet.Has(-1));
        Assert.Throws<ArgumentOutOfRangeException>(() => bitSet.Has(64));
        
        using (Assert.EnterMultipleScope())
        {
            Assert.That(bitSet.Has(10), Is.True);
            Assert.That(bitSet.Has(11), Is.False);
        }
    }

    [Test]
    public void ClearTest()
    {
        var bitSet = new BitSet(64);
        bitSet.Set(10);
        bitSet.Reset(10);
        
        Assert.That(bitSet.Has(10), Is.False);
    }

    [Test]
    public void EqualsTest()
    {
        var bitSet1 = new BitSet(64);
        bitSet1.Set(10);
        
        var bitSet2 = new BitSet(64);
        bitSet2.Set(10);
        
        var bitSet3 = new BitSet(64);
        bitSet3.Set(11);

        Assert.That(bitSet1, Is.EqualTo(bitSet2));
        Assert.That(bitSet1, Is.Not.EqualTo(bitSet3));
    }

    [Test]
    public void GetHashCodeTest()
    {
        var bitSet1 = new BitSet(64);
        bitSet1.Set(10);
        
        var bitSet2 = new BitSet(64);
        bitSet2.Set(10);
        
        Assert.That(bitSet1.GetHashCode(), Is.EqualTo(bitSet2.GetHashCode()));
    }

    [Test]
    public void ToStringTest()
    {
        var bitSet = new BitSet(8);
        bitSet.Set(0);
        bitSet.Set(2);
        bitSet.Set(4);
        
        Assert.That(bitSet.ToString(), Is.EqualTo("00010101"));
    }

    [Test]
    public void ApplyOperatorTest()
    {
        var bitSet1 = new BitSet(64);
        var bitSet2 = new BitSet(32);
        
        Assert.Throws<ArgumentException>(() => BitSet.ApplyOperator(bitSet1, bitSet2, (a, b) => a | b));
    }

    [Test]
    public void BitwiseOrTest()
    {
        var bitSet1 = new BitSet(8);
        bitSet1.Set(0);
        bitSet1.Set(2);

        var bitSet2 = new BitSet(8);
        bitSet2.Set(1);
        bitSet2.Set(2);

        var result = bitSet1 | bitSet2;

        using (Assert.EnterMultipleScope())
        {
            Assert.That(result.Has(0), Is.True);
            Assert.That(result.Has(1), Is.True);
            Assert.That(result.Has(2), Is.True);
            Assert.That(result.Has(3), Is.False);
        }
    }

    [Test]
    public void BitwiseAndTest()
    {
        var bitSet1 = new BitSet(8);
        bitSet1.Set(0);
        bitSet1.Set(2);

        var bitSet2 = new BitSet(8);
        bitSet2.Set(1);
        bitSet2.Set(2);

        var result = bitSet1 & bitSet2;

        using (Assert.EnterMultipleScope())
        {
            Assert.That(result.Has(0), Is.False);
            Assert.That(result.Has(1), Is.False);
            Assert.That(result.Has(2), Is.True);
            Assert.That(result.Has(3), Is.False);
        }
    }

    [Test]
    public void BitwiseXorTest()
    {
        var bitSet1 = new BitSet(8);
        bitSet1.Set(0);
        bitSet1.Set(2);

        var bitSet2 = new BitSet(8);
        bitSet2.Set(1);
        bitSet2.Set(2);

        var result = bitSet1 ^ bitSet2;

        using (Assert.EnterMultipleScope())
        {
            Assert.That(result.Has(0), Is.True);
            Assert.That(result.Has(1), Is.True);
            Assert.That(result.Has(2), Is.False);
            Assert.That(result.Has(3), Is.False);
        }
    }

    [Test]
    public void BitwiseNotTest()
    {
        var bitSet = new BitSet(8);
        bitSet.Set(0);
        bitSet.Set(2);

        var result = ~bitSet;

        using (Assert.EnterMultipleScope())
        {
            Assert.That(result.Has(0), Is.False);
            Assert.That(result.Has(1), Is.True);
            Assert.That(result.Has(2), Is.False);
            Assert.That(result.Has(3), Is.True);
        }
    }
    
    [Test]
    public void AllTest()
    {
        var mask = new BitSet(8);
        mask.Set(1);
        mask.Set(3);

        var bitSet = new BitSet(8);
        bitSet.Set(1);
        bitSet.Set(3);
        bitSet.Set(5);

        var missing = new BitSet(8);
        missing.Set(1);

        using (Assert.EnterMultipleScope())
        {
            Assert.That(mask.All(bitSet), Is.True);
            Assert.That(mask.All(missing), Is.False);
        }
    }

    [Test]
    public void AnyTest()
    {
        var mask = new BitSet(8);
        mask.Set(1);
        mask.Set(3);

        var bitSet = new BitSet(8);
        bitSet.Set(3);

        var none = new BitSet(8);
        none.Set(5);

        using (Assert.EnterMultipleScope())
        {
            Assert.That(mask.Any(bitSet), Is.True);
            Assert.That(mask.Any(none), Is.False);
        }
    }

    [Test]
    public void NoneTest()
    {
        var mask = new BitSet(8);
        mask.Set(1);
        mask.Set(3);

        var bitSet = new BitSet(8);
        bitSet.Set(5);

        var conflict = new BitSet(8);
        conflict.Set(3);

        using (Assert.EnterMultipleScope())
        {
            Assert.That(mask.None(bitSet), Is.True); 
            Assert.That(mask.None(conflict), Is.False);
        }
    }
}