namespace Hypercube.Utilities;

public enum OS : byte
{
    None,
    
    Unknown,
    
    /// <summary>
    /// Microsoft Windows
    /// </summary>
    Windows,
    
    /// <summary>
    /// Linux (включая дистрибутивы для десктопов и серверов)
    /// </summary>
    Linux,
    
    /// <summary>
    /// Apple macOS
    /// </summary>
    MacOS,
    
    /// <summary>
    /// Google Android
    /// </summary>
    Android,
    
    /// <summary>
    /// Apple iOS
    /// </summary>
    IOS,
    
    /// <summary>
    /// Apple tvOS
    /// </summary>
    TvOS,
    
    /// <summary>
    /// Apple watchOS
    /// </summary>
    WatchOS,
    
    /// <summary>
    /// Apple Mac Catalyst (приложения iPadOS, запущенные на macOS)
    /// </summary>
    MacCatalyst,
    
    /// <summary>
    /// FreeBSD
    /// </summary>
    FreeBSD,
    
    /// <summary>
    /// WebAssembly (выполнение в браузере через Mono/Blazor)
    /// </summary>
    Browser
}