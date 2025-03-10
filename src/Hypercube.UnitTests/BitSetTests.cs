using Hypercube.Utilities;

namespace Hypercube.UnitTests;

[TestFixture]
public sealed class BitSetTests
{
    [Test]
    public void Constructor_ThrowsArgumentOutOfRangeException_WhenSizeIsLessThanOne()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => _ = new BitSet(0));
        Assert.Throws<ArgumentOutOfRangeException>(() => _ = new BitSet(-1));
    }

    [Test]
    public void Constructor_CreatesBitSetWithCorrectSize()
    {
        var bitSet = new BitSet(100);
        Assert.That(bitSet.Size, Is.EqualTo(100));
    }
    
    
    [Test]
    public void Set_And_Has_WorkCorrectly()
    {
        var bitSet = new BitSet(64);
        bitSet.Set(10);
        
        using (Assert.EnterMultipleScope())
        {
            Assert.That(bitSet.Has(10), Is.True);
            Assert.That(bitSet.Has(11), Is.False);
        }
    }

    [Test]
    public void Clear_WorksCorrectly()
    {
        var bitSet = new BitSet(64);
        bitSet.Set(10);
        bitSet.Clear(10);
        
        Assert.That(bitSet.Has(10), Is.False);
    }

    [Test]
    public void Has_ThrowsArgumentOutOfRangeException_WhenIndexIsOutOfRange()
    {
        var bitSet = new BitSet(64);
        
        Assert.Throws<ArgumentOutOfRangeException>(() => bitSet.Has(-1));
        Assert.Throws<ArgumentOutOfRangeException>(() => bitSet.Has(64));
    }

    [Test]
    public void Equals_ReturnsTrue_ForEqualBitSets()
    {
        var bitSet1 = new BitSet(64);
        bitSet1.Set(10);
        
        var bitSet2 = new BitSet(64);
        bitSet2.Set(10);

        Assert.That(bitSet1, Is.EqualTo(bitSet2));
    }

    [Test]
    public void Equals_ReturnsFalse_ForDifferentBitSets()
    {
        var bitSet1 = new BitSet(64);
        bitSet1.Set(10);
        
        var bitSet2 = new BitSet(64);
        bitSet2.Set(11);

        Assert.That(bitSet1, Is.Not.EqualTo(bitSet2));
    }

    [Test]
    public void GetHashCode_ReturnsSameValue_ForEqualBitSets()
    {
        var bitSet1 = new BitSet(64);
        bitSet1.Set(10);
        
        var bitSet2 = new BitSet(64);
        bitSet2.Set(10);
        
        Assert.That(bitSet1.GetHashCode(), Is.EqualTo(bitSet2.GetHashCode()));
    }

    [Test]
    public void ToString_ReturnsCorrectRepresentation()
    {
        var bitSet = new BitSet(8);
        bitSet.Set(0);
        bitSet.Set(2);
        bitSet.Set(4);
        
        Assert.That(bitSet.ToString(), Is.EqualTo("00010101"));
    }

    [Test]
    public void ApplyOperator_ThrowsArgumentException_WhenSizesDoNotMatch()
    {
        var bitSet1 = new BitSet(64);
        var bitSet2 = new BitSet(32);
        
        Assert.Throws<ArgumentException>(() => BitSet.ApplyOperator(bitSet1, bitSet2, (a, b) => a | b));
    }

    [Test]
    public void BitwiseOr_Operator_WorksCorrectly()
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
    public void BitwiseAnd_Operator_WorksCorrectly()
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
    public void BitwiseXor_Operator_WorksCorrectly()
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
    public void BitwiseNot_Operator_WorksCorrectly()
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
}