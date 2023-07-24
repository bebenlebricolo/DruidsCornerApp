namespace DruidsCornerApp.Controls;

public class ErrorPopup : BasePopup
{
    public ErrorPopup(string title, string message)
    {
        var errorIcon = "!";
        var errorLabel = new Label()
        {
            Text = errorIcon,
            TextColor = Colors.Red,
            FontSize = 40,
            HorizontalOptions = LayoutOptions.Center,
            VerticalOptions = LayoutOptions.Center
        };
        SetCentralElement(errorLabel);

        TitleLabel.Text = title;
        MessageLabel.Text = message;
        OkButton.IsVisible = true;
    }
}