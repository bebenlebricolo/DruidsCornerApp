namespace DruidsCornerApp.Views.Login;
using DruidsCornerApp.ViewModels.Login;

public partial class AccountCreationPage : AutoReloadPage
{
    private Label? _passwordHintLabel = null;
    private Label? _emailHintLabel = null;
    
    public AccountCreationPage(AccountCreationPageViewModel viewModel)
    {
        BindingContext = viewModel;
    }

    public override void  Build()
    {
        InitializeComponent();
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
            var parentLayout = (PasswordValidationEntry.Parent as VerticalStackLayout)!;
            var passwordIndexInChildren = parentLayout.IndexOf(PasswordValidationEntry);
            parentLayout.Insert(passwordIndexInChildren + 1, _passwordHintLabel);
        }
    }

    /// <summary>
    /// Removes the password hint from parent layout
    /// </summary>
    /// <returns>Removal status.</returns>
    public bool RemovePasswordHint()
    {
        if (_passwordHintLabel == null)
        {
            return true;
        }
        
        var parentLayout = (PasswordValidationEntry.Parent as VerticalStackLayout)!;
        var removed = parentLayout.Remove(_passwordHintLabel);

        if (removed)
        {
            // Forget it now ...
            _passwordHintLabel = null;
        }
        return removed;
    }
    
    /// <summary>
    /// Adds a small label right below the User email field
    /// </summary>
    /// <param name="text"></param>
    /// <param name="color"></param>
    public void AddEmailHint(string text, Color? color = null)
    {
        bool isNew = false;
        if (_emailHintLabel == null)
        {
            _emailHintLabel = new Label();
            isNew = true;
        }
        
        _emailHintLabel.Text = text;
        _emailHintLabel.TextColor = color ?? Colors.Grey;
        _emailHintLabel.FontAttributes = FontAttributes.Italic;
        _emailHintLabel.Padding = new Thickness()
        {
            Left = 20.0
        };
        
        // Only care about inserting the object if we've created a new object, 
        // Otherwise the _emailHintLabel should still be pointing at the right object in memory (referenced)
        if (isNew)
        {
            var parentLayout = (EmailVerticalStackLayout as VerticalStackLayout)!;
            var emailFrameIndex = parentLayout.IndexOf(EmailEntry);
            parentLayout.Insert(emailFrameIndex + 1, _emailHintLabel);
        }
    }

    /// <summary>
    /// Removes the password hint from parent layout
    /// </summary>
    /// <returns>Removal status.</returns>
    public bool RemoveEmailHint()
    {
        if (_emailHintLabel == null)
        {
            return true;
        }
        
        var parentLayout = (EmailVerticalStackLayout.Parent as VerticalStackLayout)!;
        var removed = parentLayout.Remove(_emailHintLabel);

        if (removed)
        {
            // Forget it now ...
            _emailHintLabel = null;
        }
        return removed;
    }
    
}