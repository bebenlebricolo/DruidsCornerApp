using DruidsCornerApp.Models.Exceptions;
using DruidsCornerApp.Utils;
using Firebase.Auth;
using Firebase.Auth.Providers;
using FirebaseAdmin.Auth;
using Microsoft.Extensions.Logging;
using AuthenticationException = DruidsCornerApp.Models.Exceptions.AuthenticationException;

namespace DruidsCornerApp.Services.Authentication;

// Firebase server-side and admin toolkit, not meant to perform signIn operations
// Note : Firebase _fbAuthClient sdk do exist, but not on .NET ecosystem...
// Unofficial firebase _fbAuthClient side support for .net based apps
// https://github.com/step-up-labs/firebase-authentication-dotnet

public class FirebaseAuthenticationService : IAuthenticationService
{
    // private static FirebaseApp? _fbApp = null;
    private readonly FirebaseAuthClient _fbAuthClient;
    private readonly ILogger<FirebaseAuthenticationService> _logger;

    public FirebaseAuthenticationService(ILogger<FirebaseAuthenticationService> logger, 
                                 IAuthConfigProvider authConfigProvider)
    {
        _logger = logger;
        var authConfig = authConfigProvider.GetAuthConfig();
        var sha1Sig = PackageUtils.GetPackageDefaultSignature();
        if (sha1Sig == null)
        {
            throw new Exception("Empty SHA-1 signature for this app!");
        }
        var packageName = PackageUtils.GetPackageName();
        var sha1Signature = PackageUtils.SigToGoogleFormat(sha1Sig);
        
        var config = new FirebaseAuthConfig
        {
            ApiKey = authConfig.ApiKey,
            AuthDomain = $"{authConfig.AuthDomain}.firebaseapp.com",
            Providers = new []
            {
                // Add and configure individual providers
                new GoogleProvider().AddScopes(authConfig.JwtScopes.ToArray()),
                new EmailProvider()
            },
            HttpClient = new PlatformHttpClient(sha1Signature , packageName)
        };
        _fbAuthClient = new FirebaseAuthClient(config);
    }
    
    /// <summary>
    /// Creates a new user in the system.
    /// May throw if the user already exist, or lower level exception will be thrown.
    /// </summary>
    /// <param name="userArgs">User record arguments, coming from UI</param>
    /// <param name="cancellationToken">Used to monitor and abort operations that are too long.</param>
    /// <returns>New user record</returns>
    /// <exception cref="System.Security.Authentication.AuthenticationException"></exception>
    public async Task<UserCredential> CreateNewUserAsync(UserRecordArgs userArgs, CancellationToken cancellationToken)
    {
        // var auth = FirebaseAuth.GetAuth(_fbApp);
        UserCredential creds;
        try
        {
            // userRecord = await auth.CreateUserAsync(userArgs, cancellationToken);
            creds = await _fbAuthClient.CreateUserWithEmailAndPasswordAsync(userArgs.Email, userArgs.Password, userArgs.DisplayName);
        }
        catch (Firebase.Auth.FirebaseAuthException fbEx)
        {
            _logger.LogError($"Caught exception from firebase : {fbEx.Message}, reason = {fbEx.Reason.ToString()}");
            if (fbEx.Reason == AuthErrorReason.EmailExists)
            {
                throw new AuthenticationException("Email already exist", AuthenticationError.UserAlreadyExist);
            }

            // Propagate exception, we don't know enough to handle it at this stage
            throw;
        }

        return creds;
    }

    public async Task<string?> SignInBasicAsync(string email, string password, CancellationToken cancellationToken)
    {
        string? token;
        try
        {
            var authenticatedUser = await _fbAuthClient.SignInWithEmailAndPasswordAsync(email, password);
            token = await authenticatedUser.User.GetIdTokenAsync();
        }
        catch (FirebaseAdmin.Auth.FirebaseAuthException fbEx)
        {
            _logger.LogError($"Caught exception from firebase : {fbEx.Message}, reason = {fbEx.ErrorCode.ToString()}");
            throw;
        }

        return token;
    }

    public async Task<string?> SignInAsGuest(CancellationToken cancellationToken)
    {
        // ...and create your FirebaseAuthClient
        var anonymousUser = await _fbAuthClient.SignInAnonymouslyAsync();
        return await anonymousUser.User.GetIdTokenAsync();
    }

    public async Task<string> RefreshTokenAsync()
    {
        return await _fbAuthClient.User.GetIdTokenAsync(true);
    }

    public async Task SendPasswordResetEmailAsync(string email)
    {
        await _fbAuthClient.ResetEmailPasswordAsync(email);
    }
}