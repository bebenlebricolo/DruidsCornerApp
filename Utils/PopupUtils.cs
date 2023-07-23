using Mopups.Services;

namespace DruidsCornerApp.Utils;
using DruidsCornerApp.Controls;

public static class PopupUtils
{

    public static async Task PopAllPopupsAsync(bool animate = true)
    {
        await MopupService.Instance.PopAllAsync(animate);
    }
    
    public static ErrorPopup CreateErrorPopup(string title, string message)
    {
        var popup = new ErrorPopup(title, message);
        return popup;
    }

    public static Task CreateAndShowErrorPopup(string title, string message)
    {
        var popup = CreateErrorPopup(title, message);
        return popup.Show();
    }

    public static LoginPopup CreateLoggingPopup(string title = "SignIn")
    {
        var popup = new LoginPopup(true);
        popup.GetActivityIndicator()!.IsRunning = true;
        popup.SetTitle(title);
        return popup;
    }
    
    public static Task CreateAndShowLoggingPopupAsync(string title = "SignIn")
    {
        var popup = CreateLoggingPopup(title);
        return popup.Show();
    }

    public static void SetLoginPopupCompletedTask(LoginPopup popup, string message)
    {
        popup.SetCentralElement(new Label()
        {
            Text = "✔",
            FontSize = 40,
            TextColor = Colors.Green,
            VerticalOptions = LayoutOptions.Center,
            HorizontalOptions = LayoutOptions.Center
        });
        popup.SetMessage(message);
    }

}