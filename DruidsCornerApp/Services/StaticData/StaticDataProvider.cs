using System.Reflection;

namespace DruidsCornerApp.Services.StaticData;

/// <summary>
/// Encodes which static file is to be retrieved by StaticDataProvider (acts as a catalogue)
/// </summary>
public enum Source
{
    /// <summary>
    /// hops.json resource file, collection of hops with the appropriate formatting.
    /// </summary>
    Hops,   
    
    /// <summary>
    /// yeasts.json resource file, collection of yeasts with the appropriate formatting.
    /// </summary>
    Yeasts
}

/// <summary>
/// StaticDataProvider is used to retrieve static data that can be used to populate the UI with real data
/// without having to make real requests to webservers.
/// This is useful while building the UI, and will probably be useful as well when booting the app and the http calls
/// are quite long to get back.
/// They will act as a default dataset before the app's cache is constructed and http calls are resolved.
/// </summary>
public static class StaticDataProvider
{
    
    /// <summary>
    /// Returns a stream to the targeted resource file.
    /// </summary>
    /// <param name="source"></param>
    /// <returns></returns>
    public static Stream? GetFileStream(Source source)
    {
        string filesource = "";
        switch (source)
        {
            case Source.Hops :
                filesource = "hops.json";
                break;
            case Source.Yeasts :
                filesource = "yeasts.json";
                break;
            default :
                return null;
        }
        var assembly = Assembly.GetExecutingAssembly();
        var resourcePath = assembly.GetManifestResourceNames().Single(str => str.EndsWith(Path.GetFileName(filesource)));
        
        Stream? stream = assembly.GetManifestResourceStream(resourcePath)!;
        return stream;
    }
}