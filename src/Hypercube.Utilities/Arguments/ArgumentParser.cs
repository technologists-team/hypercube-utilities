using System.Globalization;
using Hypercube.Utilities.Extensions;

namespace Hypercube.Utilities.Arguments;

public class ArgumentParser
{
    private readonly Dictionary<string, ArgumentSpecification> _specifications = new(StringComparer.OrdinalIgnoreCase);
    
    private readonly Dictionary<string, List<string>> _parsedValues = new(StringComparer.OrdinalIgnoreCase);
    private readonly List<string> _positionals = [];
    
    public ArgumentParser AddOption<T>(string name, string description = "", T @default = default!, bool list = false)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Name required");
        
        if (_specifications.ContainsKey(name))
            throw new ArgumentException($"option '{name}' already defined");
        
        _specifications[name] = new ArgumentSpecification(name, description,typeof(T), @default, list);
        return this;
    }
    
    public ArgumentParser AddFlag(string name, string description = "", bool @default = false)
    {
        return AddOption(name, description, @default);
    }
    
    public bool Has(string name)
    {
        return _parsedValues.ContainsKey(name);
    }
    
    public T Get<T>(string name)
    {
        if (!_specifications.TryGetValue(name, out var specification))
            throw new ArgumentException($"option '{name}' is not defined");

        if (!_parsedValues.TryGetValue(name, out var list) || list.Count == 0)
        {
            // return default if present
            if (specification.Default != null && !specification.Default.Equals(GetDefaultForType(specification.Type)))
                return (T)specification.Default;
            // for flags default false
            if (specification.IsFlag) return (T)(object)false;
            throw new KeyNotFoundException($"option '{name}' not provided and has no default");
        }

        var value = list.Last(); // last occurrence
        return (T)ConvertTo(value, typeof(T));
    }
    
    public void Parse(string[] args)
    {
        _parsedValues.Clear();
        _positionals.Clear();

        for (var i = 0; i < args.Length; i++)
        {
            var a = args[i];
            
            if (a == "--")
            {
                // Everything after is positional
                for (var j = i + 1; j < args.Length; j++)
                    _positionals.Add(args[j]);
                
                break;
            }
            
            if (a.StartsWith("--"))
            {
                // --name or --name=value
                var without = a[2..];

                string? namePart = null; 
                string? valuePart = null;
                
                var eq = without.IndexOf('=');
                if (eq >= 0)
                {
                    namePart = without[..eq];
                    valuePart = without[(eq + 1)..];
                }
                else
                {
                    namePart = without;
                }
                
                if (!_specifications.TryGetValue(namePart, out var specification))
                {
                    // Unknown option -> store anyway as raw
                    Store(namePart, valuePart ?? "true");
                    continue;
                }

                if (specification.IsFlag)
                {
                    // Flag: presence sets true, unless value is provided explicitly like --flag=false
                    Store(specification, valuePart ?? "true");
                }
                else
                {
                    if (valuePart != null)
                        Store(specification, valuePart);
                    else
                    {
                        // take next arg as value if available
                        if (i + 1 < args.Length && !args[i + 1].StartsWith("-"))
                        {
                            i++;
                            Store(specification, args[i]);
                        }
                        else
                        {
                            throw new ArgumentException($"option --{namePart} expects a value");
                        }
                    }
                }
            }
            else
            {
                // positional
                _positionals.Add(a);
            }
        }

        // fill missing defaults
        foreach (var (name, specification) in _specifications)
        {
            if (_parsedValues.ContainsKey(name))
                continue;
            
            if (specification.List)
            {
                _parsedValues[name] = [];
                continue;
            }
            
            if (specification.Default is not null && !specification.Default.Equals(specification.Type.GetDefault()))
            {
                _parsedValues[name] = [specification.Default.ToString()];
            }
        }
    }
     
    private void Store(ArgumentSpecification specification, string value)
    {
        Store(specification.Name, value);
    }

    private void Store(string name, string value) 
    {
        _parsedValues.GetOrInstantiate(name).Add(value);
    }
    
    private static object ConvertTo(string raw, Type target)
    {
        var culture = CultureInfo.InvariantCulture;

        switch (Type.GetTypeCode(target))
        {
            case TypeCode.String:
                return raw;

            case TypeCode.Int32:
                return int.Parse(raw, culture);

            case TypeCode.Int64:
                return long.Parse(raw, culture);

            case TypeCode.Double:
                return double.Parse(raw, culture);
            
            case TypeCode.Char:
                return raw[0];

            case TypeCode.Boolean:
                if (bool.TryParse(raw, out var b))
                    return b;

                var lr = raw.ToLowerInvariant();
                return lr switch
                {
                    "1" or "yes" or "y" or "on"  or "enable"  or "true"  or "t" => true,
                    "0" or "no"  or "n" or "off" or "disable" or "false" or "f" => false,
                    _ => throw new FormatException($"Cannot convert '{raw}' to bool")
                };

            default:
                return target.IsEnum
                    ? Enum.Parse(target, raw, ignoreCase: true)
                    : Convert.ChangeType(raw, target, culture);
        }
    }
}