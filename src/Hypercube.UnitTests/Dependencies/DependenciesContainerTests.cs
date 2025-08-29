using System.Diagnostics.CodeAnalysis;
using Hypercube.Utilities.Dependencies;
using Hypercube.Utilities.Dependencies.Exceptions;
using JetBrains.Annotations;

namespace Hypercube.UnitTests.Dependencies;

[TestFixture]
public sealed class DependenciesContainerTests
{
    [Test]
    public void Register()
    {
        var container = new DependenciesContainer();
        
        Assert.DoesNotThrow(() =>
        {
            container.Register<IService, Service>();
            container.Resolve<IService>();
        });
        
        container.Clear();
        
        Assert.DoesNotThrow(() =>
        {
            container.Register<IService>(_ => new Service());
            container.ResolveAll();
        });
    }

    [Test]
    public void Clear()
    {
        var container = new DependenciesContainer();
        
        Assert.DoesNotThrow(() => container.Register<IService, Service>());
        
        container.Clear();
        
        Assert.Throws<TypeNotRegisteredException>(() => container.Resolve<IService>());
    }
    
    [Test]
    public void Resolve()
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
    public void Instance()
    {
        var container = new DependenciesContainer();
        var instance = new Service();
        
        container.RegisterSingleton<IService>(instance);
        
        Assert.That(instance, Is.SameAs(container.Resolve<IService>()));
    }

    [Test]
    public void InjectField()
    {
        var container = new DependenciesContainer();
        var instance = new DependentFieldClass();
        
        container.Register<IService, Service>();
        container.Inject(instance);
        
        Assert.That(instance.Service, Is.Not.Null);
        Assert.That(instance.Service, Is.TypeOf<Service>());
    }
    
    [Test]
    public void InjectProperty()
    {
        var container = new DependenciesContainer();
        var instance = new DependentPropertyClass();
        
        container.Register<IService, Service>();
        container.Inject(instance);
        
        Assert.That(instance.Service, Is.Not.Null);
        Assert.That(instance.Service, Is.TypeOf<Service>());
    }
    
    [Test]
    public void InjectMethod()
    {
        var container = new DependenciesContainer();
        var instance = new DependentMethodClass();
        
        container.Register<IService, Service>();
        container.Inject(instance);
        
        Assert.That(instance.Service, Is.Not.Null);
        Assert.That(instance.Service, Is.TypeOf<Service>());
    }
    
    [Test]
    public void InjectConstructor()
    {
        var container = new DependenciesContainer();
        
        container.Register<IService, Service>();
        
        var instance = container.Instantiate<DependentConstructorClass>();
        
        Assert.That(instance.Service, Is.Not.Null);
        Assert.That(instance.Service, Is.TypeOf<Service>());
    }
    
    [Test]
    public void TransientRegistration()
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
    public void TransientFactory()
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

        var a = container.Resolve<Service>();
        var b = container.Resolve<Service>();

        Assert.That(a, Is.Not.SameAs(b));
    }

    [Test]
    public void Injected()
    {
        var container = new DependenciesContainer();
        var instance = new DependentFieldClass();
        
        container.Register<IService>((_, injected) =>
        {
            Assert.That(injected, Is.SameAs(instance));
            return new Service();
        });

        container.Inject(instance);

        Assert.That(instance.Service, Is.TypeOf<Service>());
    }

    [Test]
    public void InjectedNull()
    {
        var container = new DependenciesContainer();
        
        container.Register<IService>((_, injected) =>
        {
            Assert.That(injected, Is.Null);
            return new Service();
        });

        var service = container.Resolve<IService>();
        
        Assert.That(service, Is.TypeOf<Service>());
        Assert.That(service, Is.SameAs(container.Resolve<IService>()));
    }

    private interface IService;
    private class Service : IService;

    private sealed class DependentFieldClass
    {
        [SuppressMessage("ReSharper", "MemberHidesStaticFromOuterClass")]
        [UsedImplicitly, Dependency]
        public readonly IService? Service;
    }

    private sealed class DependentPropertyClass
    {
        [SuppressMessage("ReSharper", "MemberHidesStaticFromOuterClass")]
        [UsedImplicitly, Dependency]
        public IService? Service { get; private set; }
    }
    
    private sealed class DependentMethodClass
    {
        [SuppressMessage("ReSharper", "MemberHidesStaticFromOuterClass")]
        public IService? Service { get; private set; }

        [UsedImplicitly, Dependency]
        private void Method(IService service)
        {
            Service = service;
        }
    }
    
    [UsedImplicitly]
    private sealed class DependentConstructorClass
    {
        [SuppressMessage("ReSharper", "MemberHidesStaticFromOuterClass")]
        public readonly IService? Service;

        public DependentConstructorClass(IService service)
        {
            Service = service;
        }
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