using JetBrains.Annotations;

namespace Hypercube.Utilities.Dependencies;

/// <summary>
/// An attribute used to mark fields for dependency injection.
/// </summary>
/// <remarks>
/// When applied to a field, this attribute marks it as a place where the dependency injection container
/// should inject a value when resolving an object. The field's type will be resolved through the container.
/// </remarks>
[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property | AttributeTargets.Method)]
[MeansImplicitUse]
public class DependencyAttribute : Attribute;
