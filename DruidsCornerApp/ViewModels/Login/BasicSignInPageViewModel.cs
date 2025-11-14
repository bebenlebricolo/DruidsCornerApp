using System.Net.Quic;
using System.Web;
using DruidsCornerApp.Models.Login;
using DruidsCornerApp.Services.Authentication;
using Microsoft.Maui.Platform;

namespace DruidsCornerApp.ViewModels.Login;

using DruidsCornerApp.Models.Exceptions;
using DruidsCornerApp.Views.Login;
using DruidsCornerApp.Services;
using DruidsCornerApp.Utils;
using Firebase.Auth;
using Microsoft.Extensions.Logging;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

/// <summary>
/// Login page view model, controls the UI state and provides commands
/// </summary>
public partial class BasicSignInPageViewModel : BaseViewModel, IQueryAttributable
{
    private static readonly string LoginErrorStr = "Login error";
    private readonly IAuthenticationService _firebaseAuthService;
    private readonly ISecureStorageService _secureStorageService;
    private readonly IGoogleAccountManager _googleAccountManager;
    private readonly IAuthConfigProvider _authConfigProvider;
    private readonly IGuestAuthService _guestAuthService;
    private uint _loginErrorCounter = 0;

    private readonly ILogger<BasicSignInPageViewModel> _logger;

    [ObservableProperty]
    private string _email = string.Empty;

    [ObservableProperty]
    private string _password = string.Empty;

    [ObservableProperty]
    private bool _passwordObfuscated = true;

    public BasicSignInPageViewModel(ILogger<BasicSignInPageViewModel> logger,
                                    IAuthenticationService firebaseAuthService,
                                    ISecureStorageService secureStorageService,
                                    IGoogleAccountManager googleAccountManager,
                                    IAuthConfigProvider authConfigProvider,
                                    IGuestAuthService guestAuthService
    ) : base("Login", false)
    {
        _logger = logger;
        _firebaseAuthService = firebaseAuthService;
        _secureStorageService = secureStorageService;
        _googleAccountManager = googleAccountManager;
        _authConfigProvider = authConfigProvider;
        _guestAuthService = guestAuthService;
    }

    [RelayCommand]
    public async Task ForgotPasswordClicked(CancellationToken cancellationToken)
    {
        await Shell.Current.GoToAsync(Navigator.GetAccountPasswordResetPageRoute());
    }

    [RelayCommand]
    public async Task CreateAccountClicked(CancellationToken cancellationToken)
    {
        await Shell.Current.GoToAsync(Navigator.GetAccountCreationPageRoute(), true);
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

        var loginPage = (Shell.Current.CurrentPage as BasicSignInPage)!;

        if (string.IsNullOrEmpty(Password) || string.IsNullOrEmpty(Email))
        {
            Password = string.Empty;
            loginPage.SetPasswordEntryOutlineColor(Colors.Red);

            await PopupUtils.CreateAndShowErrorPopup("SignIn Error", "Invalid credentials");
        }
        else
        {
            // Check if credentials are valid
            try
            {
                var loginPopup = PopupUtils.CreateLoggingPopup();
                var popupTask = loginPopup.Show();
                var token = await _firebaseAuthService.SignInBasicAsync(Email, Password, cancellationToken);
                if (token == null)
                {
                    throw new AuthenticationException("Token cannot be retrieved", AuthenticationError.EmptyToken);
                }

                // Success !
                PopupUtils.SetLoginPopupCompletedTask(loginPopup, "Successfully SignedIn !");

                // Store credentials for later use
                await _secureStorageService.StoreAsync(AccountKeys.EmailKey, Email);
                await _secureStorageService.StoreAsync(AccountKeys.PasswordKey, Password);
                await _secureStorageService.StoreAsync(AccountKeys.TokenKey, token);
                await _secureStorageService.StoreAsync(AccountKeys.AccountStateKey, AccountStates.BasicCredsConnection.ToString());

                await Task.Delay(200);
                await loginPopup.Close();
                await Shell.Current.GoToAsync("..", true);
                _loginErrorCounter = 0;
            }
            catch (Firebase.Auth.FirebaseAuthException fbEx)
            {
                await PopupUtils.PopAllPopupsAsync(false);
                switch (fbEx.Reason)
                {
                    case AuthErrorReason.UserNotFound:
                    case AuthErrorReason.UnknownEmailAddress:
                        loginPage.AddPasswordHint("Create a new account ?");
                        await PopupUtils.CreateAndShowErrorPopup(LoginErrorStr,
                                                                 "User does not exist, maybe try to create a new account, or continue as Guest."
                                                                );
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
            }
            catch (AuthenticationException ex)
            {
                _logger.LogError($"Caught exception while authenticating : {ex.Message}, {ex.Error.ToString()}");
                // Pop the login popup and show the error one
                await PopupUtils.PopAllPopupsAsync(false);
                await PopupUtils.CreateAndShowErrorPopup(LoginErrorStr, "Login encountered some issue");
                Password = "";
            }
            catch (System.Exception ex)
            {
                _logger.LogError($"Caught exception while authenticating : {ex.Message}");
                await PopupUtils.PopAllPopupsAsync(false);
                await PopupUtils.CreateAndShowErrorPopup(LoginErrorStr, "Login encountered some issue");
                Password = "";
            }
        }
    }

    /// <summary>
    /// Go to application's normal shell, we need to switch between the "BootShell" which only handles login stuff
    /// and the regular normal shell where all actual app content is described.
    /// </summary>
    private void GoToApplicationShell()
    {
        // Go to the new AppShell now, where our application core is.
        Application.Current!.MainPage = new AppShell(_secureStorageService);
    }
    
    [RelayCommand]
    public async Task GoogleSignInButtonClicked()
    {
        var authConfig = await _authConfigProvider.GetAuthConfigAsync();
        var authService = new GoogleAuthService(authConfig.RefOauthClientid);
        var account = await authService.AuthenticateAsync(CancellationToken.None);
        if (account == null)
        {
            _logger.LogError("Could not authenticate user via Google SignIn Method");
            await PopupUtils.CreateAndShowErrorPopup("SignIn Error", "Failed to authenticate with Google SignIn");
        }
        else
        {
            GoToApplicationShell();
        }
    }

    [RelayCommand]
    public async Task ContinueWithoutAccountClicked(CancellationToken cancellationToken)
    {
        // At this point we know that the user does not want to Login anyway
        _secureStorageService.RemoveAllData();
        await _secureStorageService.StoreAsync(AccountKeys.AccountStateKey, AccountStates.GuestMode.ToString());
        var token = await _guestAuthService.GetPublicAccessTokenAsync(cancellationToken);

        // Store token !
        if (!string.IsNullOrEmpty(token))
        {
            await _secureStorageService.StoreAsync(AccountKeys.TokenKey, token);
        }

        GoToApplicationShell();
    }

    /// <summary>
    /// Apply query attributes when navigated back to this page
    /// </summary>
    /// <param name="query"></param>
    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (!query.Keys.Contains("email"))
        {
            return;
        }

        // Used when navigated back after a password reset for instance, 
        // When email was already filled once in another text entry, it's duplicated here so that user doesn't need to enter it again.
        string? email = HttpUtility.UrlDecode(query["email"].ToString());
        if (email != null)
        {
            Email = email;
        }
    }
}