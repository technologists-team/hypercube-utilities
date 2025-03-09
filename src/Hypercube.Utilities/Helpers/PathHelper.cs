using JetBrains.Annotations;

namespace Hypercube.Utilities.Helpers;

[PublicAPI]
public static class PathHelper
{
    public static bool FileSystemCaseSensitive => !OperatingSystem.IsWindows() && !OperatingSystem.IsMacOS();
    public static string ExecutionDirectory => AppDomain.CurrentDomain.BaseDirectory;
    
    public static string GetExecutionRelativeFile(string file)
    {
        return Path.GetFullPath(Path.Combine(ExecutionDirectory, file));
    }
    
    public static IEnumerable<string> GetFiles(string path)
    {
        return Directory.EnumerateFiles(path, "*", SearchOption.AllDirectories);
    }
}