namespace Hypercube.Utilities.Arguments;

public readonly struct ArgumentSpecification
{
    public readonly string Name;
    public readonly string Description;
    public readonly Type Type;
    public readonly object Default;
    public readonly bool List;
    
    public bool IsFlag => Type == typeof(bool);

    public ArgumentSpecification(string name, string description, Type type, object @default, bool list)
    {
        Name = name;
        Type = type;
        Description = description;
        Default = @default;
        List = list;
    }   
}