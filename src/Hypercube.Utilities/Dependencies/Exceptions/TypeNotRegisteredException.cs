using JetBrains.Annotations;

namespace Hypercube.Utilities.Dependencies.Exceptions;

/// <summary>
/// Represents errors related to resolving unregistered types in the container.
/// </summary>
public class TypeNotRegisteredException : Exception
{
    /// <summary>
    /// Gets the unregistered type being resolved.
    /// </summary>
    [PublicAPI]
    public readonly Type Type;

    /// <summary>
    /// Initializes a new instance of the <see cref="TypeNotRegisteredException"/> class.
    /// </summary>
    /// <param name="type">The unregistered type being resolved.</param>
    public TypeNotRegisteredException(Type type)
        : base($"The type {type.FullName} is not registered in the dependency container.")
    {
        Type = type;
    }
}
