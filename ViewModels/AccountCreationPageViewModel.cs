using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using DruidsCornerApp.Models.Exceptions;
using DruidsCornerApp.Services;
using DruidsCornerApp.Views;
using Firebase.Auth;
using Microsoft.Maui.Platform;

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
    /// Used to toggle the password field from obfuscated mode to regular
    /// </summary>
    [ObservableProperty]
    private bool _passwordObfuscated = true;
    
    /// <summary>
    /// Password validation text entry field
    /// </summary>
    [ObservableProperty]
    private string _passwordValidation = string.Empty;

    /// <summary>
    /// Used to toggle the password validation field from obfuscated mode to regular
    /// </summary>
    [ObservableProperty]
    private bool _passwordValidationObfuscated = true;
    
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


    [ObservableProperty]
    private bool _isLoggingNow = false;
    
    [ObservableProperty]
    private ImageSource _passwordEyeIcon;

    [ObservableProperty]
    private ImageSource _passwordValidationEyeIcon;
    
    private readonly ImageSource _eyeOpenIcon;
    private readonly ImageSource _eyeClosedIcon;
    
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
        _eyeOpenIcon = ImageSource.FromFile("eye_open.svg");
        _eyeClosedIcon = ImageSource.FromFile("eye_closed.svg");

        _passwordEyeIcon = _eyeClosedIcon;
        _passwordValidationEyeIcon = _eyeClosedIcon;
    }

    protected AccountCreationPage GetPage()
    {
        return (Shell.Current.CurrentPage as AccountCreationPage)!;
    }

    [RelayCommand]
    public async Task CreateAccount(CancellationToken cancellationToken)
    {
#if __ANDROID__
        Platform.CurrentActivity.HideKeyboard(Platform.CurrentActivity.CurrentFocus);
#endif
        var page = GetPage();
        IsLoggingNow = true;

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
            await Shell.Current.DisplayAlert("Account creation status", "Successfully created new account!", "Ok");
            IsLoggingNow = false;
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
            IsLoggingNow = false;
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
    
    [RelayCommand]
    public void PasswordEyeFlicker()
    {
        PasswordObfuscated = !PasswordObfuscated;
        if (PasswordObfuscated)
        {
            PasswordEyeIcon = _eyeClosedIcon;
        }
        else
        {
            PasswordEyeIcon = _eyeOpenIcon;
        }
    }
    
    
    [RelayCommand]
    public void PasswordValidationEyeFlicker()
    {
        PasswordValidationObfuscated = !PasswordValidationObfuscated;
        if (PasswordValidationObfuscated)
        {
            PasswordValidationEyeIcon = _eyeClosedIcon;
        }
        else
        {
            PasswordValidationEyeIcon = _eyeOpenIcon;
        }
    }
}