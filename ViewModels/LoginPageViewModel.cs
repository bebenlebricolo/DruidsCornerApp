using DruidsCornerApp.Models.Exceptions;
using DruidsCornerApp.Views;
using DruidsCornerApp.Services;
using Firebase.Auth;

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
    private uint _loginErrorCounter = 0;
    
    
    [ObservableProperty]
    private string _email = string.Empty;

    [ObservableProperty]
    private string _password = string.Empty;

    public LoginPageViewModel(IAuthenticationService authenticationService)
    {
        Title = "Login";
        _authenticationService = authenticationService;
    }
    
    [RelayCommand]
    public async Task BackClicked(CancellationToken cancellationToken)
    {
        await Shell.Current.GoToAsync("..", animate:true);
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
        var loginPage = (Shell.Current.CurrentPage as LoginPage)!;
       
        if (   string.IsNullOrEmpty(Password) || string.IsNullOrEmpty(Email))
        {
            Password = string.Empty;
            loginPage.SetPasswordEntryOutlineColor(Colors.Red);
            loginPage.ClearPassword();
            await Shell.Current.DisplayAlert("Login failed", "Invalid credentials", "Ok");
        }
        else
        {
            // Check if credentials are valid
            try
            {
                var token = await _authenticationService.SignInBasicAsync(Email, Password, cancellationToken);
                if (token == null)
                {
                    await Shell.Current.DisplayAlert(LoginErrorStr, "Login encountered some issue", "Ok");
                    Password = "";
                }
                await SecureStorage.SetAsync("TOKEN", token!);
                _loginErrorCounter = 0;
            }
            catch (Firebase.Auth.FirebaseAuthException fbEx)
            {
                switch (fbEx.Reason)
                {
                    case AuthErrorReason.UserNotFound:
                    case AuthErrorReason.UnknownEmailAddress:
                        loginPage.AddPasswordHint("Create a new account ?");
                        await Shell.Current.DisplayAlert(LoginErrorStr,
                            "User does not exist, maybe try to create a new account, or continue as Guest.", "Ok");
                        break;
                    
                    case AuthErrorReason.InvalidEmailAddress:
                        loginPage.AddPasswordHint("Formatting : <someone>@<somewhere>.<xyz>", Colors.Red);
                        await Shell.Current.DisplayAlert(LoginErrorStr,
                            "Invalid email address, please check formatting", "Ok");
                        break;
                    
                    case AuthErrorReason.UserDisabled:
                        loginPage.AddPasswordHint("Create new account or login as Guest ?", Colors.Red);
                        await Shell.Current.DisplayAlert(LoginErrorStr,
                            "User account is disabled.", "Ok");
                        break;
                    
                    case AuthErrorReason.TooManyAttemptsTryLater:
                        loginPage.AddPasswordHint("Maybe consider resetting your password ?", Colors.Grey);
                        await Shell.Current.DisplayAlert(LoginErrorStr,
                            "Invalid email address, please check formatting", "Ok");
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

                        await Shell.Current.DisplayAlert(LoginErrorStr,
                            "Invalid password.", "Ok");
                        break;
                    
                    default :
                        loginPage.AddPasswordHint("Whoops ! Try again ?", Colors.Red);
                        await Shell.Current.DisplayAlert("Login error", "Login encountered some issue", "Ok");
                        break;
                }
                loginPage.ClearPassword();
            }
        }
        
    }
    
    // [RelayCommand]
    // public async Task Login()
    // {
    //     var loginPage = Shell.Current.CurrentPage as LoginPage;
    //     
    //     else
    //     {
    //         // Call API to attempt a login$
    //         // var loginModel = new LoginModel(Username, Password, 1);
    //         // var response = await _carApiService.Login(loginModel);
    //
    //         // if (response is not null && !string.IsNullOrEmpty(response.AccessToken))
    //         // {
    //         //     // Display a welcome message
    //         //     await SecureStorage.SetAsync("AccessToken", response.AccessToken);
    //         //     //
    //         //     // var handler = new JwtSecurityTokenHandler();
    //         //     // var jwt = handler.ReadJwtToken(response.AccessToken);
    //         //     //
    //         //     // await Shell.Current.DisplayAlert("Login status", _carApiService.StatusMessage, "Ok");
    //         //     //
    //         //     // // Build a menu on the fly ... based on the user role
    //         //     // var userInfo = new UserInfo(Username,
    //         //     //     jwt.Claims.FirstOrDefault(q => q.Type.Equals(ClaimTypes.Role))?.Value, Password);
    //         //     // App.UserInfo = userInfo;
    //         //     //
    //         //     // // Navigate to app's main page
    //         //     // MenuBuilder.BuildMenu();
    //         //     // var route = nameof(MainPage);
    //         //     // await Shell.Current.GoToAsync(route);
    //         //
    //         // }
    //         // else
    //         // {
    //         //     Password = "";
    //         // }
    //     }
    // }
}