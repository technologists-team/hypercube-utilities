using Hypercube.Utilities.Serialization.Hml;

namespace Hypercube.Utilities.UnitTests.Serialization;

[TestFixture]
public sealed class HmlTests
{
    [Test]
    public void Serialize()
    {
        const string expected = 
        """
        {
          Name: 'Excalibur'
          Level: 99
          IsLegendary: true
          Price: 1250.75
          Description: 'Ancient sword imbued with holy light.'
          Tags: [
            'weapon',
            'holy',
            'two-handed',
          ]
          WeaponStats: {
            Damage: 10
            Speed: 0.8
            Durability: 100
            CritChance: 0.25
          }
          Components: [
            {
              Type: 'blade'
              Material: 'mythril'
              Sharpness: 0.95
              Gem: null
            },
            {
              Type: 'hilt'
              Material: 'dragon bone'
              Sharpness: null
              Gem: 'sapphire'
            },
          ]
          Nested: {
            Inner: {
              DeepValue: 'Nested level 2'
            }
          }
          MixedList: [
            'string',
            42,
            false,
            {
              X: 1
              Y: 2
            },
          ]
        }
        
        """;
        
        var serialized = HmlSerializer.Serialize(new Weapon());
        Assert.That(serialized, Is.EqualTo(expected));
    }
    
    [Test]
    public void SerializeTrailingComma()
    {
        const string expected = 
        """
        {
          Name: 'Excalibur'
          Level: 99
          IsLegendary: true
          Price: 1250.75
          Description: 'Ancient sword imbued with holy light.'
          Tags: [
            'weapon',
            'holy',
            'two-handed'
          ]
          WeaponStats: {
            Damage: 10
            Speed: 0.8
            Durability: 100
            CritChance: 0.25
          }
          Components: [
            {
              Type: 'blade'
              Material: 'mythril'
              Sharpness: 0.95
              Gem: null
            },
            {
              Type: 'hilt'
              Material: 'dragon bone'
              Sharpness: null
              Gem: 'sapphire'
            }
          ]
          Nested: {
            Inner: {
              DeepValue: 'Nested level 2'
            }
          }
          MixedList: [
            'string',
            42,
            false,
            {
              X: 1
              Y: 2
            }
          ]
        }

        """;

        var options = new HmlSerializerOptions { TrailingComma = false };
        
        var serialized = HmlSerializer.Serialize(new Weapon(), options);
        Assert.That(serialized, Is.EqualTo(expected));
    }
    
    #region Serialization object
    
    private class Weapon
    {
        private string Name { get; set; } = "Excalibur";
        private int Level { get; set; } = 99;
        private bool IsLegendary { get; set; } = true;
        private double Price { get; set; } = 1250.75;
        private string Description { get; set; } = "Ancient sword imbued with holy light.";

        private List<string> Tags { get; set; } = new()
        {
            "weapon", "holy", "two-handed"
        };

        private Stats WeaponStats { get; set; } = new()
        {
            Damage = 10,
            Speed = 0.8,
            Durability = 100,
            CritChance = 0.25
        };

        private List<Component> Components { get; set; } = new()
        {
            new Component
            {
                Type = "blade",
                Material = "mythril",
                Sharpness = 0.95
            },
            new Component
            {
                Type = "hilt",
                Material = "dragon bone",
                Gem = "sapphire"
            }
        };

        private NestedObject Nested { get; set; } = new()
        {
            Inner = new InnerObject
            {
                DeepValue = "Nested level 2"
            }
        };

        private List<object> MixedList { get; set; } = new()
        {
            "string",
            42,
            false,
            new Position { X = 1, Y = 2 }
        };
    }

    private class Stats
    {
        public int Damage { get; set; }
        public double Speed { get; set; }
        public int Durability { get; set; }
        public double CritChance { get; set; }
    }

    private class Component
    {
        public string Type { get; set; } = string.Empty;
        public string Material { get; set; } = string.Empty;
        public double? Sharpness { get; set; }
        public string? Gem { get; set; }
    }

    private class NestedObject
    {
        public InnerObject Inner { get; set; } = new();
    }

    private class InnerObject
    {
        public string DeepValue { get; set; } = string.Empty;
    }

    private class Position
    {
        public int X { get; set; }
        public int Y { get; set; }
    }
    
    #endregion
}