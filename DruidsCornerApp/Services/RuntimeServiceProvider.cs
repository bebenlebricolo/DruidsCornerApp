using Firebase.Auth.Requests;

namespace DruidsCornerApp.Services;

/// <summary>
/// I need to have a f*@!?;m***!! ServiceCollectionProvider at hands so that I can manually
/// retrieve services at runtime, since some pages are built on-the-fly using parameterless constructors, thus preventing me
/// to use the MVVM model because parameterless means that the instantiated objects (which are views or pages) require their viewmodel
/// to properly function...
/// Note : this might evolve in the future as I discover how to do it properly in the ecosystem, but this method will do the job for now.
/// </summary>
public class RuntimeServiceProvider
{
    public static RuntimeServiceProvider?  Instance { get; private set; }
    
    private readonly IServiceProvider _provider;

    private RuntimeServiceProvider(IServiceProvider provider)
    {
        _provider = provider;
    }

    public static void Create(IServiceProvider provider)
    {
        if (Instance == null)
        {
            Instance = new RuntimeServiceProvider(provider);
        }
    }
    

    /// <summary>
    /// Service collection built in the MauiProgram.cs file, where the builder
    /// is fully configured.
    /// </summary>
    public T? GetService<T>()
    {
        return _provider.GetService<T>();
    }
}