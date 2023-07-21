using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AndroidX.RecyclerView.Widget;
using DruidsCornerApp.ViewModels;

namespace DruidsCornerApp.Views;

public partial class AccountCreationPage : ContentPage
{
    private Label? _passwordHintLabel = null;
    private Label? _emailHintLabel = null;
    
    public AccountCreationPage(AccountCreationPageViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
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
            var parentLayout = (PasswordValidationFrame.Parent as VerticalStackLayout)!;
            var passwordIndexInChildren = parentLayout.IndexOf(PasswordValidationFrame);
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
        
        var parentLayout = (PasswordValidationFrame.Parent as VerticalStackLayout)!;
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
            var parentLayout = (EmailEntryFrame.Parent as VerticalStackLayout)!;
            var emailFrameIndex = parentLayout.IndexOf(EmailEntryFrame);
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
        
        var parentLayout = (EmailEntryFrame.Parent as VerticalStackLayout)!;
        var removed = parentLayout.Remove(_emailHintLabel);

        if (removed)
        {
            // Forget it now ...
            _emailHintLabel = null;
        }
        return removed;
    }
    
}