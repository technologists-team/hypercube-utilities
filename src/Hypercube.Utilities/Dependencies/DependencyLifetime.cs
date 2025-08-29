namespace Hypercube.Utilities.Dependencies;

/// <summary>
/// Defines the lifetime of a dependency in the dependency injection container.
/// </summary>
/// <remarks>
/// This enum specifies how long a registered dependency should live in the container.
/// It controls the lifespan of the instances that are created and resolved by the container.
/// </remarks>
public enum DependencyLifetime
{
    /// <summary>
    /// The dependency is created each time it is resolved.
    /// This ensures that a new instance is provided whenever it is requested.
    /// </summary>
    Transient,

    /// <summary>
    /// The dependency is created once and shared throughout the application's lifetime.
    /// This ensures that the same instance is returned each time it is requested.
    /// </summary>
    Singleton
}
