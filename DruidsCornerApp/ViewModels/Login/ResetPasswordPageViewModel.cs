using DruidsCornerApp.Utils;
using Firebase.Auth;

namespace DruidsCornerApp.ViewModels.Login;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using DruidsCornerApp.Services;
using Microsoft.Extensions.Logging;


public partial class ResetPasswordPageViewModel : BaseViewModel
{
    private readonly IAuthenticationService _authenticationService;
    private readonly ISecureStorageService _secureStorageService;
    private readonly ILogger<ResetPasswordPageViewModel> _logger;

    /// <summary>
    /// User email
    /// </summary>
    [ObservableProperty]
    private string _email = string.Empty;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="authenticationService"></param>
    /// <param name="secureStorageService"></param>
    public ResetPasswordPageViewModel(ILogger<ResetPasswordPageViewModel> logger,
                                        IAuthenticationService authenticationService,
                                        ISecureStorageService secureStorageService) : base("Reset password", false)
    {
        _logger = logger;
        _authenticationService = authenticationService;
        _secureStorageService = secureStorageService;
    }

    [RelayCommand]
    public async Task SendPasswordValidationViaEmail()
    {
        // Verify Email formatting

        var validEmailFormat = CredentialsChecker.CheckEmailFormatting(Email);
        if (!validEmailFormat)
        {
            await PopupUtils.CreateAndShowErrorPopup("Email error", "Email formatting is wrong");
            return;
        }
        
        // Some code here
        var loginPopup = PopupUtils.CreateLoggingPopup("Sending email link");
        var runningTask = loginPopup.Show();

        try
        {
            await _authenticationService.SendPasswordResetEmailAsync(Email);
            await Task.Delay(2000);
        
            // Success !
            PopupUtils.SetLoginPopupCompletedTask(loginPopup, "Successfully Sent password reset email !");
            await Task.Delay(500);
            await loginPopup.Close();
            await Shell.Current.GoToAsync($"..?email={Email}", true);
        }
        catch (FirebaseAuthException fbEx)
        {
            _logger.LogError($"Caught error while sending password reset email. Reason = {fbEx.Reason.ToString()} ; Message ={fbEx.Message}");
            switch (fbEx.Reason)
            {
                case AuthErrorReason.InvalidEmailAddress:
                case AuthErrorReason.UnknownEmailAddress:
                    await PopupUtils.CreateAndShowErrorPopup("Password reset error", "Email address does not exist");
                    break;                    
                    
                case AuthErrorReason.UserDisabled:
                    await PopupUtils.CreateAndShowErrorPopup("Password reset error", "User account is marked as disabled");
                    break;  
                
                default:
                    await PopupUtils.CreateAndShowErrorPopup("Password reset error", "Caught some internal errors...");
                    break;
            }
        }
    }
}