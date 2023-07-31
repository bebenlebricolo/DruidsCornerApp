namespace DruidsCornerApp.Views.Login;
using DruidsCornerApp.ViewModels.Login;

public partial class BasicSignInPage : ContentPage
{
    private Label? _passwordHintLabel = null;
    
    public BasicSignInPage(BasicSignInPageViewModel viewModel)
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
        PasswordEntry.BorderColor = color;
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
            var parentLayout = PasswordEntry.Parent as VerticalStackLayout;
            var passwordIndexInChildren = parentLayout!.IndexOf(PasswordEntry);
            parentLayout.Insert(passwordIndexInChildren + 1, _passwordHintLabel);
        }
    }
    
    public Color GetPasswordEntryOutlineColor()
    {
        return PasswordEntry.BorderColor;
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