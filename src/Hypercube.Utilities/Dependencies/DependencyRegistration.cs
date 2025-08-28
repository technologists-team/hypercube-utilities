namespace Hypercube.Utilities.Dependencies;

public readonly struct DependencyRegistration
{
    public readonly DependencyFactory Factory;
    public readonly DependencyLifetime Lifetime;

    public DependencyRegistration(DependencyFactory factory, DependencyLifetime lifetime)
    {
        Factory = factory;
        Lifetime = lifetime;
    }
}
