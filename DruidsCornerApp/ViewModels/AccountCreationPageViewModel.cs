using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using DruidsCornerApp.Models.Exceptions;
using DruidsCornerApp.Services;
using DruidsCornerApp.Utils;
using DruidsCornerApp.Views;
using Firebase.Auth;
using MetroLog;
using Microsoft.Extensions.Logging;
using Microsoft.Maui.Platform;
using Exception = System.Exception;

namespace DruidsCornerApp.ViewModels;

public partial class AccountCreationPageViewModel : BaseViewModel
{
    private static readonly string CredentialsErrorStr = "Credentials error";
    private readonly IAuthenticationService _authenticationService;
    private readonly ISecureStorageService _secureStorageService;
    private readonly ILogger<AccountCreationPageViewModel> _logger;

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
    public AccountCreationPageViewModel(ILogger<AccountCreationPageViewModel> logger,
                                        IAuthenticationService authenticationService,
                                        ISecureStorageService secureStorageService) : base("Account creation", false)
    {
        _logger = logger;
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
        if (Platform.CurrentActivity != null && Platform.CurrentActivity.CurrentFocus != null)
        {
            Platform.CurrentActivity.HideKeyboard(Platform.CurrentActivity.CurrentFocus);
        }
#endif
        var page = GetPage();
        IsLoggingNow = true;

        // Preprocess ...
        if (string.IsNullOrEmpty(Email) || string.IsNullOrEmpty(Password) || string.IsNullOrEmpty(PasswordValidation))
        {
            await Shell.Current.DisplayAlert(CredentialsErrorStr, "Password validation differ from original Password", "Ok");
        }

        // Preprocess data
        Email = Email.Trim();
        if (!Password.Equals(PasswordValidation))
        {
            page.AddPasswordHint("⚠ Password and its validation counterpart must exactly match !");
            await PopupUtils.CreateAndShowErrorPopup(CredentialsErrorStr, "Password validation differ from original Password");
        }

        // Create account
        var userArgs = new FirebaseAdmin.Auth.UserRecordArgs()
        {
            Password = Password,
            Email = Email,
            DisplayName = !string.IsNullOrEmpty(DisplayName) ? DisplayName : Email
        };

        try
        {
            var loginPopup = PopupUtils.CreateLoggingPopup("Account creation");
            var popupShowTask = loginPopup.Show();

            var createdUser = await _authenticationService.CreateNewUserAsync(userArgs, cancellationToken);
            PopupUtils.SetLoginPopupCompletedTask(loginPopup, "Successfully created new account !");
            await Task.Delay(500);
            await loginPopup.Close();

            var signinPopup = PopupUtils.CreateLoggingPopup("SignIn");
            signinPopup.SetMessage("Signing you in ...");
            var signinPopupTask = signinPopup.Show();
            var token = await _authenticationService.SignInBasicAsync(Email, Password, cancellationToken);
            if (token == null)
            {
                throw new AuthenticationException("Token cannot be retrieved", AuthenticationError.EmptyToken);
            }

            // Store user creds and token to let the app know user is
            // Authenticated
            await _secureStorageService.StoreEmailAsync(Email);
            await _secureStorageService.StoreTokenAsync(token);
            await _secureStorageService.StorePasswordAsync(Password);

            PopupUtils.SetLoginPopupCompletedTask(signinPopup, "Successfully authenticated !");
            await Task.Delay(1000);
            await signinPopup.Close();
            await Shell.Current.GoToAsync($"//{nameof(MainPage)}", true);
        }
        catch (AuthenticationException ex)
        {
            _logger.LogError($"Caught exception while authenticating : {ex.Message}, {ex.Error.ToString()}");
            await PopupUtils.PopAllPopupsAsync(true);
            page.AddEmailHint("Consider tweaking this email address a little bit");
            page.RemovePasswordHint();
            await PopupUtils.CreateAndShowErrorPopup(CredentialsErrorStr, "An account already exist with this email");
        }
        catch (FirebaseAuthException fbEx)
        {
            _logger.LogError($"Caught exception while authenticating : {fbEx.Message}, {fbEx.Reason.ToString()}");
            await PopupUtils.PopAllPopupsAsync(true);
            switch (fbEx.Reason)
            {
                case AuthErrorReason.EmailExists:
                case AuthErrorReason.AlreadyLinked:
                case AuthErrorReason.AccountExistsWithDifferentCredential:
                    page.RemovePasswordHint();
                    page.AddEmailHint("Consider tweaking this email address a little bit");
                    await PopupUtils.CreateAndShowErrorPopup(CredentialsErrorStr, "Email address already exist.");
                    break;

                case AuthErrorReason.WeakPassword:
                    page.RemoveEmailHint();
                    page.AddPasswordHint("Consider using an alphanumeric passphrase with complex characters.");
                    await PopupUtils.CreateAndShowErrorPopup(CredentialsErrorStr, "Password is considered too weak.");
                    break;

                default:
                    await PopupUtils.CreateAndShowErrorPopup(CredentialsErrorStr, "Email address already exist.");
                    break;
            }
        }
        catch (Exception ex)
        {
            // Log exception
            _logger.LogError($"Caught exception while authenticating : {ex.Message}");
            await PopupUtils.PopAllPopupsAsync(true);
        }

        IsLoggingNow = false;
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