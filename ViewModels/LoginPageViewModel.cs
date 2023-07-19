using System.Security.Claims;
using AndroidX.AppCompat.View.Menu;
using DruidsCornerApp.Views;

namespace DruidsCornerApp.ViewModels;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

public partial class LoginPageViewModel : BaseViewModel
{
    [ObservableProperty]
    private string username = string.Empty;

    [ObservableProperty]
    private string password = string.Empty;

    public LoginPageViewModel()
    {
        Title = "Login";
    }

    [RelayCommand]
    public async Task Login()
    {
        var loginPage = Shell.Current.CurrentPage as LoginPage;
        if (string.IsNullOrEmpty(Username) || string.IsNullOrEmpty(Password))
        {
            Password = string.Empty;
        }
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