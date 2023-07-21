using DruidsCornerApp.ViewModels;
namespace DruidsCornerApp.Views;

public partial class LoginPage : ContentPage
{
    private Label? _passwordHintLabel = null;
    
    public LoginPage(LoginPageViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }

    public string GetPassword()
    {
        return PasswordEntry.Text;
    }

    public void SetPasswordEntryOutlineColor(Color color)
    {
        PasswordFrame.BorderColor = color;
    }

    /// <summary>
    /// Adds a small label right below the Password field with some textual hints
    /// </summary>
    /// <param name="text"></param>
    /// <param name="color"></param>
    public void AddPasswordHint(string text, Color? color = null)
    {
        bool isNew = false;
        if (_passwordHintLabel == null)
        {
            _passwordHintLabel = new Label();
            isNew = true;
        }
        
        _passwordHintLabel.Text = text;
        _passwordHintLabel.TextColor = color ?? Colors.Grey;
        _passwordHintLabel.FontAttributes = FontAttributes.Italic;
        _passwordHintLabel.Padding = new Thickness()
        {
            Left = 20.0
        };
        
        // Only care about inserting the object if we've created a new object, 
        // Otherwise the _passwordHintLabel should still be pointing at the right object in memory (referenced)
        if (isNew)
        {
            var parentLayout = PasswordFrame.Parent as VerticalStackLayout;
            var passwordIndexInChildren = parentLayout!.IndexOf(PasswordFrame);
            parentLayout.Insert(passwordIndexInChildren + 1, _passwordHintLabel);
        }
    }
    
    public Color GetPasswordEntryOutlineColor()
    {
        return PasswordFrame.BorderColor;
    }

    public void ClearPassword()
    {
        PasswordEntry.Text = "";
    }

    public string GetEmail()
    {
        return EmailEntry.Text;
    }
    
    
}