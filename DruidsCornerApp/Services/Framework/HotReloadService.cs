
#if DEBUG
[assembly: System.Reflection.Metadata.MetadataUpdateHandlerAttribute(typeof(DruidsCornerApp.Services.Framework.HotReloadService))]
namespace DruidsCornerApp.Services.Framework;
public static class HotReloadService
{
    public static event Action<Type[]?>? UpdateApplicationEvent;

    internal static void ClearCache(Type[]? types) 
    {

    }

    internal static void UpdateApplication(Type[]? types) 
    {
        UpdateApplicationEvent?.Invoke(types);
    }
}
#endif
