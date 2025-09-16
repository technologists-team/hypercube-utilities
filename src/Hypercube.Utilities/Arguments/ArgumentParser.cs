using System.Diagnostics.CodeAnalysis;
using System.Globalization;

namespace Hypercube.Utilities.Arguments;

public class ArgumentParser
{
    private readonly Dictionary<string, ArgumentSpecification> _specifications = new(StringComparer.OrdinalIgnoreCase);
    
    private readonly Dictionary<string, object> _parsed = new(StringComparer.OrdinalIgnoreCase);

    public ArgumentParser AddListOption<T>(string name, string description = "")
    {
        return AddOption<T>(name, description, list: true);
    }
    
    public ArgumentParser AddOption<T>(string name, string description = "", T @default = default!, bool list = false)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Name required");
        
        if (_specifications.ContainsKey(name))
            throw new ArgumentException($"option '{name}' already defined");
        
        _specifications[name] = new ArgumentSpecification(name, description, typeof(T), @default!, list);
        return this;
    }
    
    public ArgumentParser AddFlag(string name, string description = "", bool @default = false)
    {
        return AddOption(name, description, @default);
    }
    
    public bool Has(string name)
    {
        return _parsed.ContainsKey(name);
    }
    
    public T Get<T>(string name)
    {
        if (!_specifications.TryGetValue(name, out var specification))
            throw new ArgumentException($"Option {name} not defined");

        if (!_parsed.TryGetValue(name, out var data))
            return (T) specification.Default;

        if (!specification.List)
            return (T) data;

        throw new AggregateException();
    }

    public List<T> GetList<T>(string name)
    {
        if (!_specifications.TryGetValue(name, out var specification))
            throw new ArgumentException($"Option {name} not defined");

        if (!specification.List)
            throw new AggregateException();
        
        return _parsed.TryGetValue(name, out var data)
            ? ((List<object>)data).ConvertAll(e => (T) e)
            : [];
    }

    public bool TryGet<T>(string name, [MaybeNullWhen(false)] out T value)
    {
        value = default;
        
        if (!_specifications.TryGetValue(name, out _))
            throw new ArgumentException($"Option {name} not defined");
        
        if (!_parsed.TryGetValue(name, out var data))
            return false;

        value = (T) data;
        return true;
    }
    
    public void Parse(string[] args)
    {
        _parsed.Clear();

        for (var i = 0; i < args.Length; i++)
        {
            var arg = args[i];
            
            // --name or --name=value
            if (!arg.StartsWith("--"))
                continue;
            
            var without = arg[2..];
            
            var namePart = without;
            string? valuePart = null;

            var eq = without.IndexOf('=');
            if (eq >= 0)
            {
                namePart = without[..eq];
                valuePart = without[(eq + 1)..];
            }

            if (!_specifications.TryGetValue(namePart, out var specification))
            {
                // Unknown option
                continue;
            }

            if (valuePart is not null)
            {
                Store(specification, valuePart);
                continue;
            }
            
            if (specification.IsFlag)
            {
                // Flag presence sets true, unless value is provided explicitly like --flag=false
                Store(specification, "true");
                continue;
            }

            // --name value
            if (i + 1 >= args.Length || args[i + 1].StartsWith($"-"))
                throw new ArgumentException($"Option {namePart} expects a value");
            
            i++;
            Store(specification, args[i]);
        }
    }
     
    private void Store(ArgumentSpecification specification, string value)
    {
        if (_parsed.TryGetValue(specification.Name, out var data))
        {
            if (!specification.List)
                throw new ArgumentException($"{specification.Name} specification cannot support multiplies values");
            
            ((List<object>) data).Add(value);
            return;
        }
        
        var converted = ConvertTo(value, specification.Type);
        if (specification.List)
            converted = new List<object> { converted };
        
        _parsed.Add(specification.Name, converted);
    }
    
    
    private static object ConvertTo(string raw, Type target)
    {
        var culture = CultureInfo.InvariantCulture;
        switch (Type.GetTypeCode(target))
        {
            case TypeCode.String:
                return raw;
                        
            case TypeCode.Char:
                return raw[0];
            
            case TypeCode.SByte:
                return sbyte.Parse(raw);
            
            case TypeCode.Byte:
                return byte.Parse(raw);
            
            case TypeCode.Int16:
                return short.Parse(raw);
            
            case TypeCode.UInt16:
                return ushort.Parse(raw);
            
            case TypeCode.Int32:
                return int.Parse(raw);
            
            case TypeCode.UInt32:
                return uint.Parse(raw);
            
            case TypeCode.Int64:
                return long.Parse(raw);

            case TypeCode.UInt64:
                return ulong.Parse(raw);

            case TypeCode.Single:
                return float.Parse(raw, culture);
            
            case TypeCode.Double:
                return double.Parse(raw, culture);

            case TypeCode.Decimal:
                return decimal.Parse(raw, culture);
            
            case TypeCode.Boolean:
                if (bool.TryParse(raw, out var value))
                    return value;

                var lower = raw.ToLowerInvariant();
                return lower switch
                {
                    "1" or "yes" or "y" or "on"  or "enable"  or "e" or "true"  or "t" => true,
                    "0" or "no"  or "n" or "off" or "disable" or "d" or "false" or "f" => false,
                    _ => throw new FormatException($"Cannot convert '{raw}' to bool")
                };

            case TypeCode.Empty:
            case TypeCode.Object:
            case TypeCode.DBNull:
                throw new Exception($"Unsupported type {target.FullName} ({Type.GetTypeCode(target)})");
            
            case TypeCode.DateTime:
                return DateTime.Parse(raw);
            
            default:
                return target.IsEnum
                    ? Enum.Parse(target, raw, ignoreCase: true)
                    : Convert.ChangeType(raw, target, culture);
        }
    }
}