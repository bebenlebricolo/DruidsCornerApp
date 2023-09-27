using DruidsCornerApp.Views.MainContext;

namespace DruidsCornerApp.Utils;
using DruidsCornerApp.Views;
using DruidsCornerApp.Views.Login;

/// <summary>
/// Implements a very simple navigator service whose role is to serve Route names
/// for all pages of this app
/// </summary>
public static class Navigator
{
    public const string LoginRootPage = "Login";
    
    public static string GetRootPage()
    {
        return "//";
    }

    public static string GetRootLoginPage()
    {
        return LoginRootPage;
    }

    public static string GetWelcomePageRoute()
    {
        return $"{nameof(WelcomePage)}";
    }
    
    public static string GetBasicSignInPageRoute()
    {
        return $"{GetRootLoginPage()}/{nameof(BasicSignInPage)}";
    }
    
    public static string GetAccountCreationPageRoute()
    {
        return $"{GetRootLoginPage()}/{nameof(AccountCreationPage)}";
    }
    
    public static string GetAccountPasswordResetPageRoute()
    {
        return $"{GetRootLoginPage()}/{nameof(ResetPasswordPage)}";
    }

    public static string GetGoogleSignInPageRoute()
    {
        return $"{GetRootLoginPage()}/{nameof(GoogleSignInPage)}";
    }

    public static string GetRecipesBrowserPageRoute()
    {
        return $"{GetRootLoginPage()}/{nameof(RecipeExplorerPage)}";
    }
    
}