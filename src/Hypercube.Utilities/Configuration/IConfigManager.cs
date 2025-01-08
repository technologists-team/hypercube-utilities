namespace Hypercube.Utilities.Configuration;

public interface IConfigManager
{
    void Init();
    void Load();
    void Save();
}