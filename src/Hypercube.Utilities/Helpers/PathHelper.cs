﻿using JetBrains.Annotations;

namespace Hypercube.Utilities.Helpers;

[PublicAPI]
public static class PathHelper
{
    public static string GetExecDirectory()
    {
        return AppDomain.CurrentDomain.BaseDirectory;
    }

    public static string GetExecRelativeFile(string file)
    {
        return Path.GetFullPath(Path.Combine(GetExecDirectory(), file));
    }
    
    public static IEnumerable<string> GetFiles(string path)
    {
        return Directory.EnumerateFiles(path, "*", SearchOption.AllDirectories);
    }
    
    public static bool IsFileSystemCaseSensitive()
    {
        return !OperatingSystem.IsWindows() && !OperatingSystem.IsMacOS();
    }
}