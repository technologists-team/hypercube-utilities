using System.Reflection;
using Hypercube.Utilities.Extensions;
using JetBrains.Annotations;

namespace Hypercube.Utilities.Helpers;

/// <summary>
/// A helper class for working with reflection.
/// Provides methods for finding types, methods, properties and fields,
/// and to manage their values using attributes and other metadata.
/// </summary>
[PublicAPI]
public static class ReflectionHelper
{
    /// <summary>
    /// Standard flags to look for type members, including instances, public and non-public members.
    /// </summary>
    private static readonly BindingFlags DefaultFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;
    
    /// <summary>
    /// Retrieves all types from all loaded assemblies that are decorated with a specific attribute.
    /// </summary>
    /// <typeparam name="T">The type of the attribute to search for.</typeparam>
    /// <returns>A dictionary where the key is the type and the value is the attribute instance.</returns>
    public static Dictionary<Type, T> GetAllTypesWithAttribute<T>() where T : Attribute
    {
        var assemblies = AppDomain.CurrentDomain.GetAssemblies();
        var result = new Dictionary<Type, T>();
        
        foreach (var assembly in assemblies)
        {
            var types = assembly.GetTypes();
            foreach (var type in types)
            {
                var attribute = type.GetCustomAttribute<T>();
                if (attribute is not null)
                    result.Add(type, attribute);
            }
        }

        return result;
    }

    /// <summary>
    /// Sets the value of a property in the given object using reflection, based on the provided property name.
    /// </summary>
    /// <param name="obj">The object that contains the property.</param>
    /// <param name="name">The name of the property to be modified.</param>
    /// <param name="value">The value to assign to the field.</param>
    /// <param name="flags">Optional binding flags to control the search for the property.</param>
    /// <exception cref="ArgumentException">Thrown if the property is not found in the object.</exception>
    public static void SetProperty(object obj, string name, object value, BindingFlags? flags = null)
    {
        var type = obj.GetType();
        var propertyInfo = type.GetProperty(name, flags ?? DefaultFlags);

        if (propertyInfo is null)
            throw new ArgumentException($"Property {name} not found in type {type.FullName}");
        
        propertyInfo.SetValue(obj, value);
    }
    
    /// <summary>
    /// Gets the value of a property in the given object using reflection, based on the provided property name.
    /// </summary>
    /// <param name="obj">The object that contains the property.</param>
    /// <param name="name">The name of the property to be retrieved.</param>
    /// <param name="flags">Optional binding flags to control the search for the property.</param>
    /// <returns>The value of the property.</returns>
    /// <exception cref="ArgumentException">Thrown if the property is not found in the object.</exception>
    public static object? GetProperty(object obj, string name, BindingFlags? flags = null)
    {
        var type = obj.GetType();
        var propertyInfo = type.GetProperty(name, flags ?? DefaultFlags);

        if (propertyInfo is null)
            throw new ArgumentException($"Property {name} not found in type {type.FullName}");

        return propertyInfo.GetValue(obj);
    }

    /// <summary>
    /// Sets the value of a field in the given object using reflection, based on the provided field name.
    /// </summary>
    /// <param name="obj">The object that contains the field.</param>
    /// <param name="name">The name of the field to be modified.</param>
    /// <param name="value">The value to assign to the field.</param>
    /// <param name="flags">Optional binding flags to control the search for the field.</param>
    /// <exception cref="ArgumentException">Thrown if the field is not found in the object.</exception>
    public static void SetField(object obj, string name, object value, BindingFlags? flags = null)
    {
        var type = obj.GetType();
        var fieldInfo = type.GetField(name, flags ?? DefaultFlags);
        
        if (fieldInfo is null)
            throw new ArgumentException($"Field {name} not found in type {type.FullName}");

        fieldInfo.SetValue(obj, value);
    }
    
    /// <summary>
    /// Gets the value of a field in the given object using reflection, based on the provided field name.
    /// </summary>
    /// <param name="obj">The object that contains the field.</param>
    /// <param name="name">The name of the field to be retrieved.</param>
    /// <param name="flags">Optional binding flags to control the search for the field.</param>
    /// <returns>The value of the field.</returns>
    /// <exception cref="ArgumentException">Thrown if the field is not found in the object.</exception>
    public static object? GetField(object obj, string name, BindingFlags? flags = null)
    {
        var type = obj.GetType();
        var fieldInfo = type.GetField(name, flags ?? DefaultFlags);

        if (fieldInfo is null)
            throw new ArgumentException($"Field {name} not found in type {type.FullName}");

        return fieldInfo.GetValue(obj);
    }
    
    /// <summary>
    /// Retrieves all executable methods with a specific attribute from all loaded assemblies.
    /// </summary>
    /// <typeparam name="T">The type of the attribute to search for.</typeparam>
    /// <param name="flags">Flags that control which methods are retrieved (e.g., public, non-public, instance).</param>
    /// <returns>A list of <see cref="MethodInfo"/> objects representing methods with the specified attribute.</returns>
    public static List<MethodInfo> GetExecutableMethodsWithAttributeFromAllAssemblies<T>(BindingFlags? flags = null) where T : Attribute
    {
        var assemblies = AppDomain.CurrentDomain.GetAssemblies();
        return assemblies.SelectMany(assembly => GetExecutableMethodsWithAttribute<T>(assembly, flags ?? DefaultFlags))
            .ToList();
    }
    
    /// <summary>
    /// Retrieves all executable methods with a specific attribute from the currently executing assembly.
    /// </summary>
    /// <typeparam name="T">The type of the attribute to search for.</typeparam>
    /// <param name="flags">Flags that control which methods are retrieved (e.g., public, non-public, instance).</param>
    /// <returns>A list of <see cref="MethodInfo"/> objects representing methods with the specified attribute.</returns>
    public static List<MethodInfo> GetExecutableMethodsWithAttribute<T>(BindingFlags? flags = null) where T : Attribute
    {
        return GetExecutableMethodsWithAttribute<T>(Assembly.GetExecutingAssembly(), flags ?? DefaultFlags);
    }
    
    /// <summary>
    /// Retrieves all executable methods with a specific attribute from the specified assembly.
    /// </summary>
    /// <typeparam name="T">The type of the attribute to search for.</typeparam>
    /// <param name="assembly">The assembly to inspect.</param>
    /// <param name="flags">Flags that control which methods are retrieved (e.g., public, non-public, instance).</param>
    /// <returns>A list of <see cref="MethodInfo"/> objects representing methods with the specified attribute.</returns>
    public static List<MethodInfo> GetExecutableMethodsWithAttribute<T>(Assembly assembly, BindingFlags? flags) where T : Attribute
    {
        return assembly.GetTypes() 
            .Where(t => t.IsExecutableType()) // Filter only executable types
            .SelectMany(t => t.GetMethods(flags ?? DefaultFlags)) // Get methods from each type
            .Where(m => m.HasCustomAttribute<T>()) // Filter methods with the specified attribute
            .ToList();
    }
    
    /// <summary>
    /// Retrieves all methods with a specific attribute from a given type.
    /// </summary>
    /// <typeparam name="T">The type of the attribute to search for.</typeparam>
    /// <param name="type">The type to inspect.</param>
    /// <param name="flags">Flags that control which methods are retrieved (e.g., public, non-public, instance).</param>
    /// <returns>A list of <see cref="MethodInfo"/> objects representing methods with the specified attribute.</returns>
    public static List<MethodInfo> GetMethodsWithAttribute<T>(Type type, BindingFlags? flags) where T : Attribute
    {
        return type.GetMethods(flags ?? DefaultFlags)
            .Where(m => m.HasCustomAttribute<T>()) // Filter methods with the specified attribute
            .ToList();
    }
    
    /// <summary>
    /// Retrieves the first attribute of a specific type from a method.
    /// </summary>
    /// <typeparam name="T">The type of the attribute to retrieve.</typeparam>
    /// <param name="method">The method to inspect.</param>
    /// <returns>The first <see cref="T"/> attribute found, or null if not found.</returns>
    public static T? GetAttribute<T>(MethodInfo method)
        where T : Attribute
    {
        return method.GetCustomAttributes()
            .OfType<T>()
            .FirstOrDefault(); // Return the first matching attribute
    }

    /// <summary>
    /// Retrieves all attributes of a specific type from a method.
    /// </summary>
    /// <typeparam name="T">The type of the attributes to retrieve.</typeparam>
    /// <param name="method">The method to inspect.</param>
    /// <returns>A list of <see cref="T"/> attributes found on the method.</returns>
    public static List<T> GetAttributes<T>(MethodInfo method)
        where T : Attribute
    {
        return method.GetCustomAttributes()
            .OfType<T>()
            .ToList(); // Return all matching attributes
    }

    /// <summary>
    /// Retrieves all instantiable subclasses of a specified parent type, from all loaded assemblies.
    /// </summary>
    /// <param name="parent">The parent type to search for subclasses of.</param>
    /// <returns>A list of <see cref="Type"/> objects representing the subclasses of the specified parent type.</returns>
    public static List<Type> GetInstantiableSubclasses(Type parent)
    {
        var assemblies = AppDomain.CurrentDomain.GetAssemblies();
        return (from assembly in assemblies
            from type in assembly.GetTypes()
            where type.IsAssignableTo(parent) && type.IsExecutableType() // Only executable types
            select type).ToList();
    }
    
    /// <summary>
    /// Retrieves all instantiable subclasses of a specified parent type, from a specific assembly.
    /// </summary>
    /// <param name="assembly">The assembly to search in.</param>
    /// <param name="parent">The parent type to search for subclasses of.</param>
    /// <returns>A list of <see cref="Type"/> objects representing the subclasses of the specified parent type.</returns>
    public static List<Type> GetInstantiableSubclasses(Assembly assembly, Type parent)
    {
        return (from type in assembly.GetTypes()
            where type.IsAssignableTo(parent) && type.IsExecutableType()
            select type).ToList();
    }
    
    /// <summary>
    ///  Retrieves all instantiable subclasses of <typeparamref name="T"/> from all loaded assemblies.
    /// </summary>
    public static List<Type> GetInstantiableSubclasses<T>()
    {
        return GetInstantiableSubclasses(typeof(T));
    }

    /// <summary>
    /// Retrieves all instantiable subclasses of <typeparamref name="T"/> from a specific assembly.
    /// </summary>
    public static List<Type> GetInstantiableSubclasses<T>(Assembly assembly)
    {
        return GetInstantiableSubclasses(assembly, typeof(T));
    }

    public static IReadOnlyList<ValueInfo> GetValueInfos<T>(BindingFlags? flags = null)
    {
        return GetValueInfos(typeof(T), flags);
    }

    public static IReadOnlyList<ValueInfo> GetValueInfos(object obj, BindingFlags? flags = null)
    {
        return GetValueInfos(obj.GetType(), flags);
    }
    
    public static IReadOnlyList<ValueInfo> GetValueInfos(Type type, BindingFlags? flags = null)
    {
        flags ??= DefaultFlags;

        var result = new List<ValueInfo>();
        
        foreach (var info in type.GetProperties((BindingFlags) flags))
        {
            result.Add(new ValueInfo(info));
        }
        
        foreach (var info in type.GetFields((BindingFlags) flags))
        {
            if (info.Name.Contains("k__BackingField") || info.Name.Contains("i__Field"))
                continue;
            
            if (Attribute.IsDefined(info, typeof(System.Runtime.CompilerServices.CompilerGeneratedAttribute)))
                continue;
            
            result.Add(new ValueInfo(info));
        }

        return result;
    }

    public readonly struct ValueInfo
    {
        private readonly FieldInfo? _fieldInfo;
        private readonly PropertyInfo? _propertyInfo;

        public string Name => _fieldInfo?.Name ?? _propertyInfo?.Name ?? string.Empty;
        
        public ValueInfo(FieldInfo fieldInfo)
        {
            _fieldInfo = fieldInfo;
        }

        public ValueInfo(PropertyInfo propertyInfo)
        {
            _propertyInfo = propertyInfo;
        }

        public object? GetValue(object? obj)
        {
            if (_fieldInfo is not null)
                return _fieldInfo.GetValue(obj);
            
            if (_propertyInfo is not null)
                return _propertyInfo.GetValue(obj);

            // How
            throw new InvalidOperationException();
        }
        
        public void SetValue(object? obj, object? value)
        {
            _fieldInfo?.SetValue(obj, value);
            _propertyInfo?.SetValue(obj, value);
        }
    }
}
