using System.Reflection;
using JetBrains.Annotations;

namespace Hypercube.Utilities.Extensions;

/// <summary>
/// This static class provides extension methods for the <see cref="Type"/> class, 
/// enabling easier manipulation and inspection of types in a reflective context.
/// It includes methods for checking type properties, retrieving fields, and exploring the class hierarchy.
/// </summary>
[PublicAPI]
public static class TypeExtension
{
    /// <summary>
    /// A constant for retrieving fields that are accessible (including non-public), instance-level, declared in the type itself, and public.
    /// </summary>
    public const BindingFlags AccessibleInstanceFields = BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly | BindingFlags.Public;
    
    /// <summary>
    /// Checks if the type is a static class.
    /// </summary>
    /// <param name="type">The type to check.</param>
    /// <returns>True if the type is static, false otherwise.</returns>
    public static bool IsStatic(this Type type)
    {
        return type.IsAbstract && type.IsSealed;
    }
    
    /// <summary>
    /// Checks if the type is executable, meaning it is neither abstract nor an interface.
    /// </summary>
    /// <param name="type">The type to check.</param>
    /// <returns>True if the type is executable (not abstract or interface), false otherwise.</returns>
    public static bool IsExecutableType(this Type type)
    {
        return (!type.IsAbstract && !type.IsInterface) || type.IsStatic();
    }
    
    /// <summary>
    /// Checks if the type is abstract or an interface.
    /// </summary>
    /// <param name="type">The type to check.</param>
    /// <returns>True if the type is abstract or an interface, false otherwise.</returns>
    public static bool IsAbstractType(this Type type)
    {
        return (type.IsAbstract || type.IsInterface) && !type.IsStatic();
    }

    /// <summary>
    /// Retrieves all fields of the type, including private, readonly, and inherited fields, 
    /// based on the specified <see cref="BindingFlags"/>.
    /// </summary>
    /// <param name="type">The type from which to retrieve the fields.</param>
    /// <param name="bindingFlags">The flags that control which fields are retrieved, default is <see cref="AccessibleInstanceFields"/>.</param>
    /// <returns>An IEnumerable of <see cref="FieldInfo"/> representing the fields.</returns>
    public static IEnumerable<FieldInfo> GetAllFields(this Type type, BindingFlags bindingFlags = AccessibleInstanceFields)
    {
        return GetClassHierarchy(type)
            .SelectMany(p => p.GetFields(bindingFlags));
    }
    
    /// <summary>
    /// Gets the class hierarchy starting from the given type.
    /// </summary>
    /// <param name="type">The type whose hierarchy to retrieve.</param>
    /// <returns>An IEnumerable of <see cref="Type"/> representing the type and its base classes.</returns>
    public static IEnumerable<Type> GetClassHierarchy(this Type type)
    {
        yield return type;

        while (type.BaseType != null)
        {
            type = type.BaseType;
            yield return type;
        }
    }
}