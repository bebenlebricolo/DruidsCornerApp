using DruidsCornerApp.Views;
using FirebaseAdmin;

namespace DruidsCornerApp.ViewModels;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using FirebaseAdmin.Auth;

public partial class LoginPageViewModel : BaseViewModel
{
    [ObservableProperty]
    private string _username = string.Empty;

    [ObservableProperty]
    private string _password = string.Empty;

    public LoginPageViewModel()
    {
        Title = "Login";
    }

    
    [RelayCommand]
    public async void BtnBack_OnClicked(object? sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("..", animate:true);
    }

    [RelayCommand]
    public async void BtnForgottenPassword_OnClicked(object? sender, EventArgs e)
    {
        await Shell.Current.DisplayAlert("Forgotten password ?", "Too bad !", "Ok");
    }

    [RelayCommand]
    public async Task Login()
    {
        var loginPage = (Shell.Current.CurrentPage as LoginPage)!;
        var passwordFrame = loginPage.PasswordFrame;
        var passwordEntry = loginPage.PasswordEntry;
        if (string.IsNullOrEmpty(Username) || string.IsNullOrEmpty(Password))
        {
            Password = string.Empty;
            passwordFrame.BorderColor = Colors.Red;
            passwordEntry.Text = "";
            await Shell.Current.DisplayAlert("Login failed", "Invalid credentials", "Ok");
        }
        else
        {
            // Check if credentials are valid
            // Todo : call firebase apis to check that
            var firebaseApp = new FirebaseApp(new AppOptions(), "DruidsCornerCloud");
            var authHandler = FirebaseAuth.GetAuth();
        }
        
    }
    
    [RelayCommand]
    public async Task Login()
    {
        var loginPage = Shell.Current.CurrentPage as LoginPage;
        
        else
        {
            // Call API to attempt a login$
            // var loginModel = new LoginModel(Username, Password, 1);
            // var response = await _carApiService.Login(loginModel);

            // if (response is not null && !string.IsNullOrEmpty(response.AccessToken))
            // {
            //     // Display a welcome message
            //     await SecureStorage.SetAsync("AccessToken", response.AccessToken);
            //     //
            //     // var handler = new JwtSecurityTokenHandler();
            //     // var jwt = handler.ReadJwtToken(response.AccessToken);
            //     //
            //     // await Shell.Current.DisplayAlert("Login status", _carApiService.StatusMessage, "Ok");
            //     //
            //     // // Build a menu on the fly ... based on the user role
            //     // var userInfo = new UserInfo(Username,
            //     //     jwt.Claims.FirstOrDefault(q => q.Type.Equals(ClaimTypes.Role))?.Value, Password);
            //     // App.UserInfo = userInfo;
            //     //
            //     // // Navigate to app's main page
            //     // MenuBuilder.BuildMenu();
            //     // var route = nameof(MainPage);
            //     // await Shell.Current.GoToAsync(route);
            //
            // }
            // else
            // {
            //     Password = "";
            // }
        }
    }
}