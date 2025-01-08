using System.Reflection;
using System.Text.Json;
using Hypercube.Utilities.Debugging.Logger;
using Hypercube.Utilities.Dependencies;
using Hypercube.Utilities.Helpers;
using JetBrains.Annotations;

namespace Hypercube.Utilities.Configuration;

public class ConfigManager : IConfigManager
{
    [Dependency] private readonly ILogger _logger = default!;
    
    private static readonly Dictionary<string, Dictionary<string, FieldInfo>> Fields = new();

    public void Init()
    {
        foreach (var (type, attr) in ReflectionHelper.GetAllTypesWithAttribute<ConfigAttribute>())
        {
            var buffer = new Dictionary<string, FieldInfo>();
            var fields = type
                .GetFields(BindingFlags.Static | BindingFlags.Public)
                .Where(f => f.FieldType.IsGenericType &&
                            f.FieldType.GetGenericTypeDefinition() == typeof(ConfigField<>));
            foreach (var field in fields)
            {
                dynamic fieldObj = field.GetValue(null) ?? throw new InvalidOperationException();
                var jsonName = (string)fieldObj.Name;
                buffer.Add(jsonName, field);
            }

            Fields.Add(attr.ConfigFileName, buffer);
        }

        Load();
        Save();
    }
    
    [PublicAPI]
    public void Load()
    {
        foreach (var (configName, fieldsDict) in Fields)
        {
            var path = $"{configName}";
            if (!File.Exists(path))
            {
                _logger.Warning($"Unable to find config {path}");
                continue;
            }

            var configDict = JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(File.ReadAllText(path));
            if (configDict == null) continue;
            foreach (var (configEntryName, field) in fieldsDict)
            {
                if (!configDict.TryGetValue(configEntryName, out var value))
                    continue;
                
                try
                {
                    dynamic dField = field.GetValue(null) ?? throw new InvalidOperationException();
                    var generic = field.FieldType.GetGenericArguments()[0];
                    dField.SetValue(
                        JsonSerializer.Deserialize(value.GetRawText(), generic) ??
                        throw new InvalidOperationException());
                }
                catch (Exception e)
                {
                    _logger.Error(e, $"Exception while setting field {configEntryName}");
                }
            }
        }
    }

    [PublicAPI]
    public void Save()
    {
        foreach (var (configName, fieldsDict) in Fields)
        {
            var path = $"{configName}";
            var configJson = JsonSerializer.Serialize(ReadFromFieldsToDict(fieldsDict), new JsonSerializerOptions
            {
                WriteIndented = true
            });
            
            File.WriteAllText(path, configJson);
        }
    }
    
    private Dictionary<string, object> ReadFromFieldsToDict(Dictionary<string, FieldInfo> fields)
    {
        var output = new Dictionary<string, object>();
        foreach (var (jsonName, fieldInfo) in fields)
        {
            dynamic fieldObj = fieldInfo.GetValue(null)  ?? throw new InvalidOperationException();
            output.Add(jsonName, (object)fieldObj.Value);
        }

        return output;
    }
}