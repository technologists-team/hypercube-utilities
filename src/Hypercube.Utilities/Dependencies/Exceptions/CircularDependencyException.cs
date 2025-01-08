namespace Hypercube.Utilities.Dependencies.Exceptions;

/// <summary>
/// Represents errors related to circular dependencies in the container.
/// </summary>
public class CircularDependencyException : Exception
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CircularDependencyException"/> class.
    /// </summary>
    /// <param name="type">The type causing the circular dependency.</param>
    public CircularDependencyException(Type type)
        : base($"A circular dependency was detected while resolving type: {type.FullName}.")
    {
        Type = type;
    }

    /// <summary>
    /// Gets the type causing the circular dependency.
    /// </summary>
    public Type Type { get; }
}
