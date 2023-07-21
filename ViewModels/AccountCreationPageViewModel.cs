using Android.Accounts;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using DruidsCornerApp.Models.Exceptions;
using DruidsCornerApp.Services;
using DruidsCornerApp.Views;
using Firebase.Auth;
using Firebase.Auth.Providers;
using Xamarin.Google.Crypto.Tink.Util;

namespace DruidsCornerApp.ViewModels;

public partial class AccountCreationPageViewModel : BaseViewModel
{
    private static readonly string CredentialsErrorStr = "Credentials error";
    private readonly IAuthenticationService _authenticationService;
    private readonly ISecureStorageService _secureStorageService;
    
    /// <summary>
    /// User email
    /// </summary>
    [ObservableProperty]
    private string _email = string.Empty;

    /// <summary>
    /// User password field
    /// </summary>
    [ObservableProperty]
    private string _password = string.Empty;
    
    /// <summary>
    /// Password validation text entry field
    /// </summary>
    [ObservableProperty]
    private string _passwordValidation = string.Empty;

    /// <summary>
    /// User "display name" property
    /// </summary>
    [ObservableProperty]
    private string _displayName = string.Empty;

    /// <summary>
    /// Password status text icon is used to give a real-time hint about password and password validation entries comparison status
    /// </summary>
    [ObservableProperty]
    private string _passwordStatusTxtIcon = "?";

    /// <summary>
    /// Password status text icon is used to give a real-time hint about password and password validation entries comparison status
    /// </summary>
    [ObservableProperty]
    private Color _passwordStatusTxtIconColor = Colors.Grey;
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="authenticationService"></param>
    /// <param name="secureStorageService"></param>
    public AccountCreationPageViewModel(IAuthenticationService authenticationService,
                                        ISecureStorageService secureStorageService)
    {
        _authenticationService = authenticationService;
        _secureStorageService = secureStorageService;
    }

    protected AccountCreationPage GetPage()
    {
        return (Shell.Current.CurrentPage as AccountCreationPage)!;
    }

    [RelayCommand]
    public async Task CreateAccount(CancellationToken cancellationToken)
    {
        var page = GetPage();

        if (string.IsNullOrEmpty(Email) || string.IsNullOrEmpty(Password) || string.IsNullOrEmpty(PasswordValidation))
        {
            await Shell.Current.DisplayAlert(CredentialsErrorStr, "Invalid credentials", "Ok");
        }

        // Preprocess data
        Email = Email.Trim();
        if (!Password.Equals(PasswordValidation))
        {
            page.AddPasswordHint("⚠ Password and its validation counterpart must exactly match !");
            await Shell.Current.DisplayAlert(CredentialsErrorStr, "Password validation differ from original Password", "Ok");
        }

        // Todo : Start the "Loading" animation
        //var button = page.CreateAccountButton;

        // Create account
        var userArgs = new FirebaseAdmin.Auth.UserRecordArgs()
        {
            Password = Password,
            Email = Email,
            DisplayName = !string.IsNullOrEmpty(DisplayName) ? DisplayName : Email
        };

        try
        {
            var createdUser = await _authenticationService.CreateNewUserAsync(userArgs, cancellationToken);
        }
        catch (UserAlreadyExistException)
        {
        }
        catch (FirebaseAuthException fbEx)
        {
            switch (fbEx.Reason)
            {
                case AuthErrorReason.EmailExists:
                case AuthErrorReason.AlreadyLinked:
                case AuthErrorReason.AccountExistsWithDifferentCredential:
                    page.RemovePasswordHint();
                    page.AddEmailHint("Consider tweaking this email address a little bit");
                    await Shell.Current.DisplayAlert(CredentialsErrorStr, "Email address already exist.", "Ok");
                    break;

                case AuthErrorReason.WeakPassword:
                    page.RemoveEmailHint();
                    page.AddPasswordHint("Consider using an alphanumeric passphrase with complex characters.");
                    await Shell.Current.DisplayAlert(CredentialsErrorStr, "Password is considered too weak.", "Ok");
                    break;

                default:
                    await Shell.Current.DisplayAlert(CredentialsErrorStr, "Email address already exist.", "Ok");
                    break;
            }
        }
    }

    [RelayCommand]
    public void PasswordValidationTextChanged()
    {
        if (!Password.Equals(PasswordValidation))
        {
            // PasswordStatusTxtIcon = "⚠";
            PasswordStatusTxtIcon = "⛔";
            PasswordStatusTxtIconColor = Colors.LightCoral;
        }
        else
        {
            PasswordStatusTxtIcon = "✔";
            PasswordStatusTxtIconColor = Colors.Green;
        }
    }

    [RelayCommand]
    public async Task BackButtonClicked(CancellationToken cancellationToken)
    {
        // yup ... 
        await Shell.Current.GoToAsync("..");
    }
}