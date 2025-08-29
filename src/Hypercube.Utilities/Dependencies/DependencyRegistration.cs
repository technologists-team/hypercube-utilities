namespace Hypercube.Utilities.Dependencies;

/// <summary>
/// Represents the registration details of a dependency in the dependency injection container.
/// </summary>
/// <remarks>
/// This structure holds the factory method for creating the dependency instance, as well as
/// the lifetime configuration of the dependency (whether it is transient or singleton).
/// </remarks>
/// <seealso cref="IDependenciesContainer"/>
public readonly struct DependencyRegistration
{
    /// <summary>
    /// Gets the factory method used to create an instance of the dependency.
    /// </summary>
    public readonly DependencyFactory Factory;

    /// <summary>
    /// Gets the lifetime configuration of the dependency.
    /// </summary>
    public readonly DependencyLifetime Lifetime;

    /// <summary>
    /// Initializes a new instance of the <see cref="DependencyRegistration"/> struct.
    /// </summary>
    /// <param name="factory">The factory method for creating the dependency instance.</param>
    /// <param name="lifetime">The lifetime configuration of the dependency.</param>
    public DependencyRegistration(DependencyFactory factory, DependencyLifetime lifetime)
    {
        Factory = factory;
        Lifetime = lifetime;
    }
}
