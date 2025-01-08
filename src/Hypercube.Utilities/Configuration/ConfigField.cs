namespace Hypercube.Utilities.Configuration;

public class ConfigField<T>
{
    public event Action<T>? OnValueChanged;

    public readonly string Name;
    private T _value;

    public T Value
    {
        get => _value;
        set
        {
            _value = value;
            OnValueChanged?.Invoke(value);
        }
    }

    public ConfigField(string name, T @default)
    {
        Name = name;
        _value = @default;
    }

    public void SetValue(object obj)
    {
        _value = (T) obj;
        OnValueChanged?.Invoke(_value);
    }

    public static implicit operator T(ConfigField<T> field)
    {
        return field.Value;
    }
}