using Hypercube.Utilities.Dependencies.Exceptions;

namespace Hypercube.Utilities.Dependencies;

/// <summary>
/// Implements an IoC (Inversion of Control) container for dependency injection.
/// </summary>
/// <remarks>
/// <para>
/// If a dependency is registered in this container with a type already declared in the parent container,
/// this will override it. When resolving the dependency, the value from this container will be returned.
/// </para>
/// </remarks>
/// <seealso cref="DependencyAttribute"/>
/// <seealso cref="IPostInject"/>
public interface IDependenciesContainer
{
    #region Register
    
  /// <summary>
    /// Registers a type for dependency injection using itself as the implementation.
    /// </summary>
    /// <typeparam name="T">The type to register.</typeparam>
    /// <param name="lifetime">The lifetime of the registered dependency.</param>
    void Register<T>(DependencyLifetime lifetime = DependencyLifetime.Singleton);

    /// <summary>
    /// Registers a type for dependency injection using itself as the implementation.
    /// </summary>
    /// <param name="type">The type to register.</param>
    /// <param name="lifetime">The lifetime of the registered dependency.</param>
    void Register(Type type, DependencyLifetime lifetime = DependencyLifetime.Singleton);

    /// <summary>
    /// Registers a type and its implementation for dependency injection.
    /// </summary>
    /// <typeparam name="TType">The service type to register.</typeparam>
    /// <typeparam name="TImpl">The implementation type of the service.</typeparam>
    /// <param name="lifetime">The lifetime of the registered dependency.</param>
    void Register<TType, TImpl>(DependencyLifetime lifetime = DependencyLifetime.Singleton);

    /// <summary>
    /// Registers a type with a specific implementation for dependency injection.
    /// </summary>
    /// <param name="type">The service type to register.</param>
    /// <param name="impl">The implementation type of the service.</param>
    /// <param name="lifetime">The lifetime of the registered dependency.</param>
    void Register(Type type, Type impl, DependencyLifetime lifetime = DependencyLifetime.Singleton);

    /// <summary>
    /// Registers a factory method for creating an instance of a type.
    /// </summary>
    /// <typeparam name="T">The service type to register.</typeparam>
    /// <param name="factory">The factory method used to create an instance of the service.</param>
    /// <param name="lifetime">The lifetime of the registered dependency.</param>
    void Register<T>(Func<IDependenciesContainer, T> factory, DependencyLifetime lifetime = DependencyLifetime.Singleton);

    /// <summary>
    /// Registers a factory method for creating an instance of a type.
    /// </summary>
    /// <typeparam name="T">The service type to register.</typeparam>
    /// <param name="factory">The factory method used to create an instance of the service.</param>
    /// <param name="lifetime">The lifetime of the registered dependency.</param>
    void Register<T>(Func<IDependenciesContainer, object?, T> factory, DependencyLifetime lifetime = DependencyLifetime.Singleton);
    
    /// <summary>
    /// Registers a factory method for creating an instance of a type.
    /// </summary>
    /// <param name="type">The service type to register.</param>
    /// <param name="factory">The factory delegate used to create an instance of the service.</param>
    /// <param name="lifetime">The lifetime of the registered dependency.</param>
    void Register(Type type, DependencyFactory factory, DependencyLifetime lifetime = DependencyLifetime.Singleton);

    /// <summary>
    /// Registers a specific instance of a type for dependency injection.
    /// </summary>
    /// <param name="type">The service type to register.</param>
    /// <param name="instance">The instance to bind to the service type.</param>
    void RegisterSingleton(Type type, object instance);

    /// <summary>
    /// Registers a specific instance of a type for dependency injection.
    /// </summary>
    /// <typeparam name="T">The service type to register.</typeparam>
    /// <param name="instance">The instance to bind to the service type.</param>
    void RegisterSingleton<T>(object instance);

    #endregion

    #region Resolve

    /// <summary>
    /// Resolves a type to its registered instance or creates a new instance.
    /// </summary>
    /// <typeparam name="T">The type to resolve.</typeparam>
    /// <returns>The resolved instance.</returns>
    T Resolve<T>();

    /// <summary>
    /// Resolves a type to its registered instance or creates a new instance.
    /// </summary>
    /// <param name="type">The type to resolve.</param>
    /// <returns>The resolved instance.</returns>
    /// <exception cref="CircularDependencyException">Thrown if circular dependencies are detected.</exception>
    /// <exception cref="TypeNotRegisteredException">Thrown if the type is not registered.</exception>
    object Resolve(Type type);

    #endregion

    #region Inject

    /// <summary>
    /// Injects dependencies into the fields of an existing object.
    /// </summary>
    /// <param name="instance">The object to inject dependencies into.</param>
    void Inject(object instance);

    #endregion

    #region Instantiate

    /// <summary>
    /// Instantiates all registered types that have not yet been instantiated.
    /// </summary>
    void InstantiateAll();

    /// <summary>
    /// Instantiates a specific type.
    /// </summary>
    /// <typeparam name="T">The type to instantiate.</typeparam>
    /// <returns>The instantiated object.</returns>
    T Instantiate<T>();

    /// <summary>
    /// Instantiates a specific type.
    /// </summary>
    /// <param name="type">The type to instantiate.</param>
    /// <returns>The instantiated object.</returns>
    object Instantiate(Type type);

    #endregion

    /// <summary>
    /// Clears all registered factories and instances in the container.
    /// </summary>
    public void Clear();
}