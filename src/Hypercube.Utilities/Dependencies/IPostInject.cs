namespace Hypercube.Utilities.Dependencies;

/// <summary>
/// Defines an interface for types that require post-injection processing.
/// </summary>
/// <remarks>
/// The <see cref="PostInject"/> method is called automatically by the dependency injection container
/// after all dependencies have been injected into the object.
/// </remarks>
public interface IPostInject
{
    /// <summary>
    /// Called after all dependencies have been injected into the object.
    /// </summary>
    /// <remarks>
    /// This method allows for additional initialization or setup logic that might depend on the injected
    /// dependencies. It is invoked immediately after the injection process, making it a useful point to
    /// complete the configuration or behavior of the object.
    /// </remarks>
    void PostInject();
}
