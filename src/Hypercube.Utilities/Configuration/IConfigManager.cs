using JetBrains.Annotations;

namespace Hypercube.Utilities.Configuration;

[PublicAPI]
public interface IConfigManager
{
    void Init();
    void Load();
    void Save();
}