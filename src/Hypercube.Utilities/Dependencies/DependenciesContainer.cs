using System.Reflection;
using Hypercube.Utilities.Dependencies.Exceptions;
using Hypercube.Utilities.Extensions;
using JetBrains.Annotations;

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
[PublicAPI]
public class DependenciesContainer
{
    /// <summary>
    /// Binding flags to identify constructors for dependency injection.
    /// </summary>
    private const BindingFlags ConstructorFlags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;

    /// <summary>
    /// The parent container for cascading dependency resolution.
    /// </summary>
    private readonly DependenciesContainer? _parent;

    /// <summary>
    /// A dictionary that maps types to factory methods for creating instances.
    /// </summary>
    private readonly Dictionary<Type, Func<DependenciesContainer, object>> _factories = new();

    /// <summary>
    /// A dictionary of already created instances of dependencies.
    /// </summary>
    private readonly Dictionary<Type, object> _instances = new();

    /// <summary>
    /// Tracks types currently being resolved to detect circular dependencies.
    /// </summary>
    private readonly HashSet<Type> _resolutions = new();

    /// <summary>
    /// Lock object to ensure thread safety.
    /// </summary>
    private readonly object _lock = new();

    /// <summary>
    /// Initializes a new instance of the <see cref="DependenciesContainer"/> class.
    /// </summary>
    /// <param name="parent">An optional parent container for cascading resolution.</param>
    public DependenciesContainer(DependenciesContainer? parent = null)
    {
        _parent = parent;
    }

    /// <summary>
    /// Registers a type for dependency injection using itself as the implementation.
    /// </summary>
    /// <typeparam name="T">The type to register.</typeparam>
    public void Register<T>()
    {
        Register(typeof(T));
    }

    /// <summary>
    /// Registers a type for dependency injection using itself as the implementation.
    /// </summary>
    /// <param name="type">The type to register.</param>
    public void Register(Type type)
    {
        Register(type, type);
    }

    /// <summary>
    /// Registers a type and its implementation for dependency injection.
    /// </summary>
    /// <typeparam name="TType">The type to register.</typeparam>
    /// <typeparam name="TImpl">The implementation of the registered type.</typeparam>
    public void Register<TType, TImpl>()
    {
        Register(typeof(TType), typeof(TImpl));
    }

    /// <summary>
    /// Registers a type with a specific implementation for dependency injection.
    /// </summary>
    /// <param name="type">The type to register.</param>
    /// <param name="impl">The implementation type of the registered type.</param>
    public void Register(Type type, Type impl)
    {
        // Retrieve all constructors of the implementation type
        var constructors = impl.GetConstructors(ConstructorFlags);
        if (constructors.Length != 1)
            throw new InvalidRegistrationException($"The type {impl.FullName} must have exactly one constructor.");

        // Ensure there is exactly one constructor
        var constructor = constructors[0];
        var constructorParams = constructor.GetParameters();

        // Check for any constructor parameters
        if (constructorParams.Length != 0)
            throw new InvalidRegistrationException($"The constructor of {impl.FullName} must not have parameters.");

        // Create an instance using the constructor
        Register(type, _ => constructor.Invoke([]));
    }

    /// <summary>
    /// Registers a specific instance of a type for dependency injection.
    /// </summary>
    /// <param name="type">The type to register.</param>
    /// <param name="instance">The instance to register.</param>
    public void Register(Type type, object instance)
    {
        Register(type, _ => instance);
    }
    
    /// <summary>
    /// Registers a specific instance of a type for dependency injection.
    /// </summary>
    /// <typeparam name="T">The type to register.</typeparam>
    /// <param name="instance">The instance to register.</param>
    public void Register<T>(object instance)
    {
        Register(typeof(T), _ => instance);
    }

    /// <summary>
    /// Registers a factory method for creating an instance of a type.
    /// </summary>
    /// <typeparam name="T">The type to register.</typeparam>
    /// <param name="factory">The factory method to create an instance of the type.</param>
    public void Register<T>(Func<DependenciesContainer, T> factory)
    {
        Register(typeof(T), container => factory.Invoke(container) ?? throw new InvalidOperationException("Factory method returned null."));
    }

    /// <summary>
    /// Registers a factory method for creating an instance of a type.
    /// </summary>
    /// <param name="type">The type to register.</param>
    /// <param name="factory">The factory method to create an instance of the type.</param>
    public void Register(Type type, Func<DependenciesContainer, object> factory)
    {
        lock (_lock)
        {
            _factories[type] = factory;
        }
    }

    /// <summary>
    /// Resolves a type to its registered instance or creates a new instance.
    /// </summary>
    /// <typeparam name="T">The type to resolve.</typeparam>
    /// <returns>The resolved instance.</returns>
    public T Resolve<T>()
    {
        return (T)Resolve(typeof(T));
    }

    /// <summary>
    /// Injects dependencies into the fields of an existing object.
    /// </summary>
    /// <param name="instance">The object to inject dependencies into.</param>
    public void Inject(object instance)
    {
        Inject(instance, autoInject: false);
    }

    /// <summary>
    /// Instantiates all registered types that have not yet been instantiated.
    /// </summary>
    public void InstantiateAll()
    {
        lock (_lock)
        {
            foreach (var (type, _) in _factories)
            {
                if (_instances.ContainsKey(type))
                    continue;

                Instantiate(type);
            }
        }
    }

    /// <summary>
    /// Instantiates a specific type.
    /// </summary>
    /// <typeparam name="T">The type to instantiate.</typeparam>
    /// <returns>The instantiated object.</returns>
    public object Instantiate<T>()
    {
        return Instantiate(typeof(T));
    }

    /// <summary>
    /// Instantiates a specific type.
    /// </summary>
    /// <param name="type">The type to instantiate.</param>
    /// <returns>The instantiated object.</returns>
    public object Instantiate(Type type)
    {
        lock (_lock)
        {
            var instance = _factories[type].Invoke(this);
            _instances[type] = instance;
            Inject(instance, autoInject: true);
            return instance;
        }
    }

    /// <summary>
    /// Injects dependencies into an instance and performs post-injection processing.
    /// </summary>
    /// <param name="instance">The object to inject dependencies into.</param>
    /// <param name="autoInject">Whether to perform automatic injection.</param>
    /// <exception cref="InvalidOperationException">Thrown if the instance is null.</exception>
    private void Inject(object instance, bool autoInject)
    {
        if (instance is null)
            throw new InvalidOperationException("Instance cannot be null.");

        var type = instance.GetType();

        // Inject dependencies into fields marked with DependencyAttribute
        foreach (var field in type.GetAllFields())
        {
            if (!Attribute.IsDefined(field, typeof(DependencyAttribute)))
                continue;

            field.SetValue(instance, Resolve(field.FieldType));
        }

        // Call PostInject if the instance implements IPostInject
        if (instance is IPostInject postInject)
            postInject.PostInject();
    }

    /// <summary>
    /// Resolves a type to its registered instance or creates a new instance.
    /// </summary>
    /// <param name="type">The type to resolve.</param>
    /// <returns>The resolved instance.</returns>
    /// <exception cref="CircularDependencyException">Thrown if circular dependencies are detected.</exception>
    /// <exception cref="TypeNotRegisteredException">Thrown if the type is not registered.</exception>
    public object Resolve(Type type)
    {
        lock (_lock)
        {
            if (!_resolutions.Add(type))
                throw new CircularDependencyException(type);

            try
            {
                if (type == typeof(DependenciesContainer))
                    return this;
                
                if (_instances.TryGetValue(type, out var instance))
                    return instance;

                if (_factories.ContainsKey(type))
                    return Instantiate(type);

                if (_parent is not null)
                    return _parent.Resolve(type);

                throw new TypeNotRegisteredException(type);
            }
            finally
            {
                _resolutions.Remove(type);
            }
        }
    }
    
    /// <summary>
    /// Clears all registered factories and instances in the container.
    /// </summary>
    public void Clear()
    {
        lock (_lock)
        {
            _instances.Clear();
            _factories.Clear();
            _resolutions.Clear();
        }
    }
}
