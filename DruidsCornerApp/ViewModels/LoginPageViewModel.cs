using DruidsCornerApp.Controls;
using DruidsCornerApp.Models.Exceptions;
using DruidsCornerApp.Views;
using DruidsCornerApp.Services;
using DruidsCornerApp.Utils;
using Firebase.Auth;
using Microsoft.Extensions.Logging;
using Microsoft.Maui.Controls.PlatformConfiguration;
using Microsoft.Maui.Platform;
using Mopups.Services;

namespace DruidsCornerApp.ViewModels;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

/// <summary>
/// Login page view model, controls the UI state and provides commands
/// </summary>
public partial class LoginPageViewModel : BaseViewModel
{
    private static readonly string LoginErrorStr = "Login error";
    private readonly IAuthenticationService _authenticationService;
    private readonly ISecureStorageService _secureStorageService;
    private uint _loginErrorCounter = 0;

    private readonly ILogger<LoginPageViewModel> _logger;

    [ObservableProperty]
    private string _email = string.Empty;

    [ObservableProperty]
    private string _password = string.Empty;

    [ObservableProperty]
    private bool _passwordObfuscated = true;

    [ObservableProperty]
    private ImageSource _eyeIcon;

    private ImageSource _eyeOpenIcon;
    private ImageSource _eyeClosedIcon;


    public LoginPageViewModel(ILogger<LoginPageViewModel> logger,
                              IAuthenticationService authenticationService,
                              ISecureStorageService secureStorageService) : base("Login", false)
    {
        _logger = logger;
        _authenticationService = authenticationService;
        _secureStorageService = secureStorageService;

        _eyeOpenIcon = ImageSource.FromFile("eye_open.svg");
        _eyeClosedIcon = ImageSource.FromFile("eye_closed.svg");

        EyeIcon = _eyeClosedIcon;
    }

    [RelayCommand]
    public async Task BackClicked(CancellationToken cancellationToken)
    {
        await Shell.Current.GoToAsync("..", animate: true);
    }

    [RelayCommand]
    public async Task ForgotPasswordClicked(CancellationToken cancellationToken)
    {
        await Shell.Current.DisplayAlert("Forgotten password ?", "Too bad !", "Ok");
    }

    [RelayCommand]
    public async Task CreateAccountClicked(CancellationToken cancellationToken)
    {
        await Shell.Current.GoToAsync(nameof(AccountCreationPage));
    }

    [RelayCommand]
    public async Task Login(CancellationToken cancellationToken)
    {
#if __ANDROID__
        if (Platform.CurrentActivity != null && Platform.CurrentActivity.CurrentFocus != null)
        {
            Platform.CurrentActivity.HideKeyboard(Platform.CurrentActivity.CurrentFocus);
        }
#endif

        var loginPage = (Shell.Current.CurrentPage as LoginPage)!;

        if (string.IsNullOrEmpty(Password) || string.IsNullOrEmpty(Email))
        {
            Password = string.Empty;
            loginPage.SetPasswordEntryOutlineColor(Colors.Red);
            loginPage.ClearPassword();
            await PopupUtils.CreateAndShowErrorPopup("SignIn Error", "Invalid credentials");
        }
        else
        {
            // Check if credentials are valid
            try
            {
                var loginPopup = PopupUtils.CreateLoggingPopup();
                var popupTask = loginPopup.Show();
                var token = await _authenticationService.SignInBasicAsync(Email, Password, cancellationToken);
                if (token == null)
                {
                    throw new AuthenticationException("Token cannot be retrieved", AuthenticationError.EmptyToken);
                }

                // Success !
                PopupUtils.SetLoginPopupCompletedTask(loginPopup, "Successfully SignedIn !");

                // Store credentials for later use
                await _secureStorageService.StoreEmailAsync(Email);
                await _secureStorageService.StorePasswordAsync(Password);
                await _secureStorageService.StoreTokenAsync(token);

                await Task.Delay(200);
                await loginPopup.Close();
                await Shell.Current.GoToAsync("..", true);
                _loginErrorCounter = 0;
            }
            catch (Firebase.Auth.FirebaseAuthException fbEx)
            {
#if __ANDROID__

#endif
                
                await PopupUtils.PopAllPopupsAsync(false);
                switch (fbEx.Reason)
                {
                    case AuthErrorReason.UserNotFound:
                    case AuthErrorReason.UnknownEmailAddress:
                        loginPage.AddPasswordHint("Create a new account ?");
                        await PopupUtils.CreateAndShowErrorPopup(LoginErrorStr,
                            "User does not exist, maybe try to create a new account, or continue as Guest.");
                        break;

                    case AuthErrorReason.InvalidEmailAddress:
                        loginPage.AddPasswordHint("Formatting : <someone>@<somewhere>.<xyz>", Colors.Red);
                        await PopupUtils.CreateAndShowErrorPopup(LoginErrorStr, "Invalid email address, please check formatting");
                        break;

                    case AuthErrorReason.UserDisabled:
                        loginPage.AddPasswordHint("Create new account or login as Guest ?", Colors.Red);
                        await PopupUtils.CreateAndShowErrorPopup(LoginErrorStr, "User account is disabled.");
                        break;

                    case AuthErrorReason.TooManyAttemptsTryLater:
                        loginPage.AddPasswordHint("Maybe consider resetting your password ?", Colors.Grey);
                        await PopupUtils.CreateAndShowErrorPopup(LoginErrorStr, "Invalid email address, please check formatting");
                        break;

                    case AuthErrorReason.WrongPassword:
                        _loginErrorCounter++;
                        if (_loginErrorCounter <= 2)
                        {
                            loginPage.AddPasswordHint("Wrong password : try again ?", Colors.Red);
                        }
                        else
                        {
                            loginPage.AddPasswordHint("Wrong password : forgotten password ?", Colors.Red);
                        }

                        await PopupUtils.CreateAndShowErrorPopup(LoginErrorStr, "Invalid password.");
                        break;

                    default:
                        loginPage.AddPasswordHint("Whoops ! Try again ?", Colors.Red);
                        await PopupUtils.CreateAndShowErrorPopup(LoginErrorStr, "Login encountered some issue");
                        break;
                }

                Password = "";
                loginPage.ClearPassword();
            }
            catch (AuthenticationException ex)
            {
                _logger.LogError($"Caught exception while authenticating : {ex.Message}, {ex.Error.ToString()}");
                // Pop the login popup and show the error one
                await PopupUtils.PopAllPopupsAsync(false);
                await PopupUtils.CreateAndShowErrorPopup(LoginErrorStr, "Login encountered some issue");
                Password = "";
                loginPage.ClearPassword();
            }
            catch (System.Exception)
            {
                await PopupUtils.PopAllPopupsAsync(false);
                loginPage.ClearPassword();
                Password = "";
                loginPage.ClearPassword();
            }
        }
    }


    [RelayCommand]
    public void EyeFlicker()
    {
        PasswordObfuscated = !PasswordObfuscated;
        if (PasswordObfuscated)
        {
            EyeIcon = _eyeClosedIcon;
        }
        else
        {
            EyeIcon = _eyeOpenIcon;
        }
    }
}