using System.Diagnostics;
using System.Security.Authentication;
using DruidsCornerApp.Models.Exceptions;
using Firebase.Auth.Providers;
using Firebase.Auth.Repository;
using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using AuthenticationException = DruidsCornerApp.Models.Exceptions.AuthenticationException;

namespace DruidsCornerApp.Services;

// Firebase server-side and admin toolkit, not meant to perform signIn operations
// Note : Firebase _fbAuthClient sdk do exist, but not on .NET ecosystem...
using FirebaseAdmin.Auth;

// Unofficial firebase _fbAuthClient side support for .net based apps
// https://github.com/step-up-labs/firebase-authentication-dotnet
using Firebase.Auth;

public class AuthenticationService : IAuthenticationService
{
    // private static FirebaseApp? _fbApp = null;
    private static FirebaseAuthClient _fbAuthClient;
    private readonly IAuthConfigProvider _authConfigProvider;

    public AuthenticationService(IAuthConfigProvider authConfigProvider)
    {
        _authConfigProvider = authConfigProvider;
        var authConfig = _authConfigProvider.GetAuthConfig();
        var config = new FirebaseAuthConfig
        {
            ApiKey = authConfig.ApiKey,
            AuthDomain = authConfig.AuthDomain,
            Providers = new []
            {
                // Add and configure individual providers
                new GoogleProvider().AddScopes(authConfig.JwtScopes.ToArray()),
                new EmailProvider()
            }
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
    /// <exception cref="UserAlreadyExistException"></exception>
    public async Task<UserCredential> CreateNewUserAsync(UserRecordArgs userArgs, CancellationToken cancellationToken)
    {
        // var auth = FirebaseAuth.GetAuth(_fbApp);
        UserCredential creds;
        try
        {
            // userRecord = await auth.CreateUserAsync(userArgs, cancellationToken);
            creds = await _fbAuthClient!.CreateUserWithEmailAndPasswordAsync(userArgs.Email, userArgs.Password, userArgs.DisplayName);
        }
        catch (Firebase.Auth.FirebaseAuthException fbEx)
        {
            // Todo : Log something here
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
            var authenticatedUser = await _fbAuthClient!.SignInWithEmailAndPasswordAsync(email, password);
            token = await authenticatedUser.User.GetIdTokenAsync();
        }
        catch (FirebaseAdmin.Auth.FirebaseAuthException)
        {
            // Todo log error here
            throw;
        }

        return token;
    }

    public async Task<string?> SignInAsGuest(CancellationToken cancellationToken)
    {
        // ...and create your FirebaseAuthClient
        var anonymousUser = await _fbAuthClient!.SignInAnonymouslyAsync();
        return await anonymousUser.User.GetIdTokenAsync();
    }

    public async Task<string> RefreshTokenAsync()
    {
        return await _fbAuthClient!.User.GetIdTokenAsync(true);
    }
    
}