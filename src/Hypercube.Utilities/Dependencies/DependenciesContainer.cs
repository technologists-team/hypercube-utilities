using System.Reflection;
using Hypercube.Utilities.Dependencies.Exceptions;
using Hypercube.Utilities.Extensions;
using JetBrains.Annotations;

namespace Hypercube.Utilities.Dependencies;

/// <inheritdoc/>
[PublicAPI]
public class DependenciesContainer : IDependenciesContainer
{
    /// <summary>
    /// Binding flags to identify constructors for dependency injection.
    /// </summary>
    private const BindingFlags ConstructorFlags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;

    /// <summary>
    /// The parent container for cascading dependency resolution.
    /// </summary>
    private readonly DependenciesContainer? _parent;

    private readonly Dictionary<Type, DependencyRegistration> _registrations = new();

    /// <summary>
    /// A dictionary of already created instances of dependencies.
    /// </summary>
    private readonly Dictionary<Type, object> _instances = new();

    /// <summary>
    /// Tracks types currently being resolved to detect circular dependencies.
    /// </summary>
    private readonly HashSet<Type> _resolutions = [];

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

    #region Register
    
    /// <inheritdoc/>
    public void Register<T>(DependencyLifetime lifetime = DependencyLifetime.Singleton)
    {
        Register(typeof(T), lifetime);
    }

    /// <inheritdoc/>
    public void Register(Type type, DependencyLifetime lifetime = DependencyLifetime.Singleton)
    {
        Register(type, type, lifetime);
    }

    /// <inheritdoc/>
    public void Register<TType, TImpl>(DependencyLifetime lifetime = DependencyLifetime.Singleton)
    {
        Register(typeof(TType), typeof(TImpl), lifetime);
    }

    /// <inheritdoc/>
    public void Register(Type type, Type impl, DependencyLifetime lifetime = DependencyLifetime.Singleton)
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
        Register(type, (_, _) => constructor.Invoke([]), lifetime);
    }

    /// <inheritdoc/>
    public void Register<T>(Func<IDependenciesContainer, T> factory, DependencyLifetime lifetime = DependencyLifetime.Singleton)
    {
        Register(typeof(T), (container, _) => factory.Invoke(container) ?? throw new InvalidOperationException("Factory method returned null."), lifetime);
    }
    
    /// <inheritdoc/>
    public void Register<T>(Func<IDependenciesContainer, object?, T> factory, DependencyLifetime lifetime = DependencyLifetime.Singleton)
    {
        Register(typeof(T), (container, injected) => factory.Invoke(container, injected) ?? throw new InvalidOperationException("Factory method returned null."), lifetime);
    }

    /// <inheritdoc/>
    public void Register(Type type, DependencyFactory factory, DependencyLifetime lifetime = DependencyLifetime.Singleton)
    {
        lock (_lock) _registrations[type] = new DependencyRegistration(factory, lifetime);
    }

    /// <inheritdoc/>
    public void RegisterSingleton(Type type, object instance)
    {
        Register(type, (_, _) => instance, lifetime: DependencyLifetime.Singleton);
    }
    
    /// <inheritdoc/>
    public void RegisterSingleton<T>(object instance)
    {
        Register(typeof(T), (_, _) => instance, lifetime: DependencyLifetime.Singleton);
    }

    #endregion

    #region Resolve
    
    /// <inheritdoc/>
    public T Resolve<T>()
    {
        return (T) Resolve(typeof(T));
    }
    
    /// <inheritdoc/>
    public object Resolve(Type type)
    {
        return Resolve(type, null);
    }
    
    #endregion

    #region Inject

    /// <inheritdoc/>
    public void Inject(object instance)
    {
        Inject(instance, autoInject: false);
    }

    #endregion

    #region Instantiate

    /// <inheritdoc/>
    public void InstantiateAll()
    {
        lock (_lock)
        {
            foreach (var (type, registration) in _registrations)
            {
                if (registration.Lifetime == DependencyLifetime.Transient)
                    continue;
                
                if (_instances.ContainsKey(type))
                    continue;

                Instantiate(type);
            }
        }
    }

    /// <inheritdoc/>
    public T Instantiate<T>()
    {
        return (T) Instantiate(typeof(T));
    }

    /// <inheritdoc/>
    public object Instantiate(Type type)
    {
        return Instantiate(type, null);
    }

    #endregion

    /// <inheritdoc/>
    public void Clear()
    {
        lock (_lock)
        {
            _instances.Clear();
            _registrations.Clear();
            _resolutions.Clear();
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

            field.SetValue(instance, Resolve(field.FieldType, instance));
        }

        // Call PostInject if the instance implements IPostInject
        if (instance is IPostInject postInject)
            postInject.PostInject();
    }

    private object Resolve(Type type, object? injected)
    {
        lock (_lock)
        {
            if (!_resolutions.Add(type))
                throw new CircularDependencyException(type);

            try
            {
                if (type == typeof(IDependenciesContainer) || type == typeof(DependenciesContainer))
                    return this;

                if (_instances.TryGetValue(type, out var instance))
                    return instance;

                if (_registrations.ContainsKey(type))
                    return Instantiate(type, injected);
                
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
    
    private object Instantiate(Type type, object? injected)
    {
        lock (_lock)
        {
            var registration = _registrations[type];
            var instance = registration.Factory.Invoke(this, injected);
            
            if (registration.Lifetime == DependencyLifetime.Singleton)
                _instances[type] = instance;
            
            Inject(instance, autoInject: true);
            return instance;
        }
    }
}
