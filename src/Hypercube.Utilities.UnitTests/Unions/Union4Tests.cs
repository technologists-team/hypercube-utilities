using Hypercube.Utilities.Unions;

namespace Hypercube.Utilities.UnitTests.Unions;

[TestFixture]
public sealed class Union4Tests
{
    private static readonly object[] NumberTestCases =
    {
        new object[] { (sbyte) 10 },
        new object[] { (byte) 10 },
        new object[] { (short) 30 },
        new object[] { (ushort) 40 },
        new object[] { 50 },
        new object[] { 60u },
    };

    [Test, TestCaseSource(nameof(NumberTestCases))]
    public void NumbersGetSetTest<T>(T expectedValue) where T : unmanaged
    {
        var union = new Union4('a');
        Assert.Throws<InvalidCastException>(() => { _ = union.Get<T>(); });
        
        union.Set(expectedValue);
        
        Assert.That(union.Get<T>(), Is.EqualTo(expectedValue));
        Assert.Throws<InvalidCastException>(() => { _ = union.Char; });
    }

    [Test]
    public void TypeAssignTest()
    {
        var union = new Union4(UnionTypeCode.Byte);
        
        Assert.Multiple(() =>
        {
            Assert.That(union.Type, Is.EqualTo(UnionTypeCode.Byte));
            Assert.That(union.Byte, Is.EqualTo((byte) 0));
        });

        union.Bool = true;
        Assert.That(union.Type, Is.EqualTo(UnionTypeCode.Boolean));
    }
    
    [Test]
    public void ByteTest()
    {
        var union = new Union4((byte) 200);
        
        Assert.Multiple(() =>
        {
            Assert.That(union.Type, Is.EqualTo(UnionTypeCode.Byte));
            Assert.That(union.Byte, Is.EqualTo((byte) 200));
        });
        
        union.Byte = 250;
        
        Assert.Multiple(() =>
        {
            Assert.That(union.Type, Is.EqualTo(UnionTypeCode.Byte));
            Assert.That(union.Byte, Is.EqualTo((byte) 250));
        });
    }
    
    [Test]
    public void SByteTest()
    {
        var union = new Union4((sbyte) -120);
        
        Assert.Multiple(() =>
        {
            Assert.That(union.Type, Is.EqualTo(UnionTypeCode.SByte));
            Assert.That(union.SByte, Is.EqualTo((sbyte) -120));
        });

        union.SByte = -100;
        
        Assert.Multiple(() =>
        {
            Assert.That(union.Type, Is.EqualTo(UnionTypeCode.SByte));
            Assert.That(union.SByte, Is.EqualTo((sbyte) -100));
        });
    }
    
    [Test]
    public void ShortTest()
    {
        var union = new Union4((short) 200);
        
        Assert.Multiple(() =>
        {
            Assert.That(union.Type, Is.EqualTo(UnionTypeCode.Int16));
            Assert.That(union.Short, Is.EqualTo((short) 200));
        });
        
        union.Short = 250;
        
        Assert.Multiple(() =>
        {
            Assert.That(union.Type, Is.EqualTo(UnionTypeCode.Int16));
            Assert.That(union.Short, Is.EqualTo((short) 250));
        });
    }
    
    [Test]
    public void UShortTest()
    {
        var union = new Union4((ushort) 200);
        
        Assert.Multiple(() =>
        {
            Assert.That(union.Type, Is.EqualTo(UnionTypeCode.UInt16));
            Assert.That(union.UShort, Is.EqualTo((ushort)200));
        });

        union.UShort = 250;
        
        Assert.Multiple(() =>
        {
            Assert.That(union.Type, Is.EqualTo(UnionTypeCode.UInt16));
            Assert.That(union.UShort, Is.EqualTo((ushort) 250));
        });
    }
    
    [Test]
    public void CharTest()
    {
        var union = new Union4('a');
        
        Assert.Multiple(() =>
        {
            Assert.That(union.Type, Is.EqualTo(UnionTypeCode.Char));
            Assert.That(union.Char, Is.EqualTo('a'));
        });

        union.Char = 'b';
        
        Assert.Multiple(() =>
        {
            Assert.That(union.Type, Is.EqualTo(UnionTypeCode.Char));
            Assert.That(union.Char, Is.EqualTo('b'));
        });
    }
    
    [Test]
    public void BoolTest()
    {
        var union = new Union4(false);
        
        Assert.Multiple(() =>
        {
            Assert.That(union.Type, Is.EqualTo(UnionTypeCode.Boolean));
            Assert.That(union.Bool, Is.EqualTo(false));
        });

        union.Bool = true;
        
        Assert.Multiple(() =>
        {
            Assert.That(union.Type, Is.EqualTo(UnionTypeCode.Boolean));
            Assert.That(union.Bool, Is.EqualTo(true));
        });
    }
    
    [Test]
    public void IntTest()
    {
        var union = new Union4(1000);
        
        Assert.Multiple(() =>
        {
            Assert.That(union.Type, Is.EqualTo(UnionTypeCode.Int32));
            Assert.That(union.Int, Is.EqualTo(1000));
        });

        union.Int = -1000;
        
        Assert.Multiple(() =>
        {
            Assert.That(union.Type, Is.EqualTo(UnionTypeCode.Int32));
            Assert.That(union.Int, Is.EqualTo(-1000));
        });
    }
    
    [Test]
    public void UIntTest()
    {
        var union = new Union4(5000u);
        
        Assert.Multiple(() =>
        {
            Assert.That(union.Type, Is.EqualTo(UnionTypeCode.UInt32));
            Assert.That(union.UInt, Is.EqualTo(5000u));
        });

        union.UInt = 1000u;
        
        Assert.Multiple(() =>
        {
            Assert.That(union.Type, Is.EqualTo(UnionTypeCode.UInt32));
            Assert.That(union.UInt, Is.EqualTo(1000u));
        });
    }
    
        
    [Test]
    public void FloatTest()
    {
        var union = new Union4(3.14f);
        
        Assert.Multiple(() =>
        {
            Assert.That(union.Type, Is.EqualTo(UnionTypeCode.Single));
            Assert.That(union.Float, Is.EqualTo(3.14f));
        });

        union.Float = 6.28f;
        
        Assert.Multiple(() =>
        {
            Assert.That(union.Type, Is.EqualTo(UnionTypeCode.Single));
            Assert.That(union.Float, Is.EqualTo(6.28f));
        });
    }
}