using System.Diagnostics.CodeAnalysis;
using Hypercube.Utilities.Dependencies;
using Hypercube.Utilities.Dependencies.Exceptions;
using JetBrains.Annotations;

namespace Hypercube.UnitTests.Dependencies;

[TestFixture]
public sealed class DependenciesContainerTests
{
    [Test]
    public void RegisterTest()
    {
        var container = new DependenciesContainer();
        
        Assert.DoesNotThrow(() =>
        {
            container.Register<IService, Service>();
            container.Instantiate<IService>();
        });
        
        container.Clear();
        
        Assert.DoesNotThrow(() =>
        {
            container.Register<IService>(_ => new Service());
            container.InstantiateAll();
        });
    }

    [Test]
    public void ClearTest()
    {
        var container = new DependenciesContainer();
        
        Assert.DoesNotThrow(() => container.Register<IService, Service>());
        
        container.Clear();
        
        Assert.Throws<TypeNotRegisteredException>(() => container.Resolve<IService>());
    }
    
    [Test]
    public void ResolveTest()
    {
        var container = new DependenciesContainer();
        
        Assert.Throws<TypeNotRegisteredException>(() => container.Resolve<IService>());
        
        container.Register<IService, Service>();
        
        Assert.DoesNotThrow(() => container.Resolve<IService>());
        Assert.That(container.Resolve<IService>(), Is.TypeOf<Service>());
        
        container.Register<A>();
        container.Register<B>();
        
        Assert.Throws<CircularDependencyException>(() => container.Resolve<A>());
    }

    [Test]
    public void InstanceTest()
    {
        var container = new DependenciesContainer();
        var instance = new Service();
        
        container.RegisterSingleton<IService>(instance);
        
        Assert.That(instance, Is.SameAs(container.Resolve<IService>()));
    }

    [Test]
    public void InjectTest()
    {
        var container = new DependenciesContainer();
        var instance = new DependentClass();
        
        container.Register<IService, Service>();
        container.Inject(instance);
        
        Assert.That(instance.Service, Is.Not.Null);
        Assert.That(instance.Service, Is.TypeOf<Service>());
    }
    
    [Test]
    public void TransientRegistrationTest()
    {
        var container = new DependenciesContainer();
        container.Register<IService, Service>(DependencyLifetime.Transient);

        var first = container.Resolve<IService>();
        var second = container.Resolve<IService>();
        
        using (Assert.EnterMultipleScope())
        {
            Assert.That(first, Is.TypeOf<Service>());
            Assert.That(second, Is.TypeOf<Service>());
        }
        
        Assert.That(first, Is.Not.SameAs(second));
    }

    [Test]
    public void TransientFactoryTest()
    {
        var container = new DependenciesContainer();
        var counter = 0;

        container.Register<IService>(_ =>
        {
            counter++;
            return new Service();
        }, DependencyLifetime.Transient);

        var first = container.Resolve<IService>();
        var second = container.Resolve<IService>();
        
        using (Assert.EnterMultipleScope())
        {
            Assert.That(first, Is.TypeOf<Service>());
            Assert.That(second, Is.TypeOf<Service>());
        }

        using (Assert.EnterMultipleScope())
        {
            Assert.That(first, Is.Not.SameAs(second));
            Assert.That(counter, Is.EqualTo(2));
        }
    }

    [Test]
    public void TransientDoesNotCacheInstances()
    {
        var container = new DependenciesContainer();

        container.Register<Service>(DependencyLifetime.Transient);

        var a = container.Instantiate<Service>();
        var b = container.Instantiate<Service>();

        Assert.That(a, Is.Not.SameAs(b));
    }

    private interface IService;
    private class Service : IService;

    private sealed class DependentClass
    {
        [SuppressMessage("ReSharper", "MemberHidesStaticFromOuterClass")]
        [SuppressMessage("Compiler", "CS0649")]
        [UsedImplicitly, Dependency]
        public readonly IService? Service;
    }

    [UsedImplicitly]
    private sealed class A
    {
        [SuppressMessage("Compiler", "CS0414")]
        [UsedImplicitly, Dependency]
        private readonly B _dependency = default!;
    }

    [UsedImplicitly]
    private sealed class B
    {
        [SuppressMessage("Compiler", "CS0414")]
        [UsedImplicitly, Dependency]
        private readonly A _dependency = default!;
    }
}