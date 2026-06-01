using JetBrains.Annotations;

namespace Hypercube.Utilities;

[PublicAPI]
public static class HyperOS
{
    public static OS Current
    {
        get
        {
            if (field == OS.None) 
                field = DetectOS();
                
            return field;
        }
        private set;
    }

    public static OS DetectOS()
    {
        if (OperatingSystem.IsMacCatalyst())
            return OS.MacCatalyst;
        
        if (OperatingSystem.IsWindows())
            return OS.Windows;
        
        if (OperatingSystem.IsLinux())
            return OS.Linux;
        
        if (OperatingSystem.IsMacOS())
            return OS.MacOS;
        
        if (OperatingSystem.IsAndroid())
            return OS.Android;
        
        if (OperatingSystem.IsIOS())
            return OS.IOS;
        
        if (OperatingSystem.IsTvOS())
            return OS.TvOS;
        
        if (OperatingSystem.IsWatchOS())
            return OS.WatchOS;
        
        if (OperatingSystem.IsFreeBSD())
            return OS.FreeBSD;
        
        if (OperatingSystem.IsBrowser())
            return OS.Browser;

        return OS.Unknown;
    }
}