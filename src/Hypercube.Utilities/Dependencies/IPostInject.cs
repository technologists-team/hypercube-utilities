using JetBrains.Annotations;

namespace Hypercube.Utilities.Dependencies;

[PublicAPI]
public interface IPostInject
{
    void PostInject();
}