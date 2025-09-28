using Hypercube.Utilities.Unions;

namespace Hypercube.Utilities.UnitTests.Unions;

[TestFixture]
public sealed class Union8Tests
{
    private static readonly object[] NumberTestCases =
    {
        new object[] { (sbyte) 10 },
        new object[] { (byte) 10 },
        new object[] { (short) 30 },
        new object[] { (ushort) 40 },
        new object[] { 50 },
        new object[] { 60u },
        new object[] { 70L },
        new object[] { 80ul },
        new object[] { 80d },
    };

    [Test, TestCaseSource(nameof(NumberTestCases))]
    public void NumbersGetSetTest<T>(T expectedValue) where T : unmanaged
    {
        var union = new Union8('a');
        Assert.Throws<InvalidCastException>(() => { _ = union.Get<T>(); });
        
        union.Set(expectedValue);
        
        Assert.That(union.Get<T>(), Is.EqualTo(expectedValue));
        Assert.Throws<InvalidCastException>(() => { _ = union.Char; });
    }

    [Test]
    public void TypeAssignTest()
    {
        var union = new Union8(UnionTypeCode.Byte);
        
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
        var union = new Union8((byte) 200);
        
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
        var union = new Union8((sbyte) -120);
        
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
        var union = new Union8((short) 200);
        
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
        var union = new Union8((ushort) 200);
        
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
        var union = new Union8('a');
        
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
        var union = new Union8(false);
        
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
        var union = new Union8(1000);
        
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
        var union = new Union8(5000u);
        
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
        var union = new Union8(3.14f);
        
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
    
    [Test]
    public void LongTest()
    {
        var union = new Union8(1000l);
        
        Assert.Multiple(() =>
        {
            Assert.That(union.Type, Is.EqualTo(UnionTypeCode.Int64));
            Assert.That(union.Long, Is.EqualTo(1000l));
        });

        union.Long = -1000l;
        
        Assert.Multiple(() =>
        {
            Assert.That(union.Type, Is.EqualTo(UnionTypeCode.Int64));
            Assert.That(union.Long, Is.EqualTo(-1000l));
        });
    }
    
    [Test]
    public void ULongTest()
    {
        var union = new Union8(5000ul);
        
        Assert.Multiple(() =>
        {
            Assert.That(union.Type, Is.EqualTo(UnionTypeCode.UInt64));
            Assert.That(union.ULong, Is.EqualTo(5000ul));
        });

        union.ULong = 1000ul;
        
        Assert.Multiple(() =>
        {
            Assert.That(union.Type, Is.EqualTo(UnionTypeCode.UInt64));
            Assert.That(union.ULong, Is.EqualTo(1000ul));
        });
    }
    
    [Test]
    public void DoubleTest()
    {
        var union = new Union8(3.14);
        
        Assert.Multiple(() =>
        {
            Assert.That(union.Type, Is.EqualTo(UnionTypeCode.Double));
            Assert.That(union.Double, Is.EqualTo(3.14));
        });

        union.Double = 6.28;
        
        Assert.Multiple(() =>
        {
            Assert.That(union.Type, Is.EqualTo(UnionTypeCode.Double));
            Assert.That(union.Double, Is.EqualTo(6.28));
        });
    }
        
    [Test]
    public void DateTimeTest()
    {
        var union = new Union8(new DateTime(2025));
        
        Assert.Multiple(() =>
        {
            Assert.That(union.Type, Is.EqualTo(UnionTypeCode.DateTime));
            Assert.That(union.DateTime, Is.EqualTo(new DateTime(2025)));
        });

        union.DateTime = new DateTime(1984);
        
        Assert.Multiple(() =>
        {
            Assert.That(union.Type, Is.EqualTo(UnionTypeCode.DateTime));
            Assert.That(union.DateTime, Is.EqualTo( new DateTime(1984)));
        });
    }
}