namespace Hypercube.Utilities.Dependencies;

/// <summary>
/// Delegate for creating dependency instances.
/// </summary>
/// <remarks>
/// This delegate is used to define a factory that will create instances of dependencies
/// within the container. It allows you to control the creation and injection of dependencies
/// by passing the current container and optionally injected objects.
/// </remarks>
/// <param name="container">The dependency container that provides access to resolved types.</param>
/// <param name="injected">The injected dependencies that can be used to create a new instance.</param>
/// <returns>A dependency instance that is to be returned by the factory.</returns>
public delegate object DependencyFactory(IDependenciesContainer container, object? injected);
