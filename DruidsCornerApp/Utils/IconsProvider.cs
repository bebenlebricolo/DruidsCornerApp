using System.Reflection;

namespace DruidsCornerApp.Utils;

/// <summary>
/// This class is a local icon provider which caches
/// ImageSources so that we don't have them crippling around everywhere and duplicated in memory N million times.
/// </summary>
public class IconsProvider
{
    private static IconsProvider? _instance = null;
    private Dictionary<string, ImageSource> _iconStore = new();
    private List<string> _availableIcons = new();

    private IconsProvider()
    {
    }
    
    public static IconsProvider Instance()
    {
        if (_instance == null)
        {
            _instance = new IconsProvider();
            _instance._availableIcons = ListAvailableIcons();
        }
        return _instance;
    }

    private static List<string> ListAvailableIcons()
    {
        return new List<string>()
        {
            "about.svg",
            "account.svg",
            "back.svg",
            "barleyu.svg",
            "compass_flat_simple.svg",
            "erlenmeyer.svg",
            "erlenmeyer_merged.svg",
            "explorer_icon.svg",
            "explorer_simple_flat_darkmode.svg",
            "eye_closed.png",
            "eye_open.svg",
            "gear.svg",
            "hamburger.svg",
            "heart.svg",
            "heart_full.svg",
            "hop.svg",
            "labelled_stock_box.svg",
            "magic_wand.svg",
            "magic_wand_merged.svg",
            "mail.svg",
            "mailsent.svg",
            "minus_sign.svg",
            "option_dots_h.svg",
            "option_dots_h_circled.svg",
            "option_dots_v.svg",
            "option_dots_v_circled.svg",
            "padlock.svg",
            "plus_sign.svg",
            "qrcode.svg",
            "recipe_book_2.svg",
            "recipebook.svg",
            "star.svg",
            "stock_box.svg",
            "up_chevron.svg",
            "yeast.svg",
            "yeast_full.svg"
        };
    }
    
    // private List<string> ListXamlFiles()
    // {
    //     var resourceNames = Assembly.GetExecutingAssembly().GetManifestResourceNames();
    //     var iconsList = new List<string>();
    //     foreach (var name in resourceNames)
    //     {
    //         if (name.Contains("Icons"))
    //         {
    //             var iconName = Path.GetFileName(name);
    //             iconsList.Add(iconName);
    //         }
    //     }
    //
    //     return iconsList;
    // }

    /// <summary>
    /// Lazy get image from icons file sources.
    /// If icon is not cached yet, it'll be loaded and cached on the fly, ready for next requests.
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public ImageSource? GetFile(string name)
    {
        if (_availableIcons.Contains(name))
        {
            _iconStore.TryGetValue(name, out var output);
            if (output != null)
            {
                return output;
            }
            
            // Load on the fly
            var image = ImageSource.FromFile(name);
            _iconStore.Add(name, image);
            return image;
        }

        return null;
    }
    
    
    
    
}