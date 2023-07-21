using System.Diagnostics;
using DruidsCornerApp.Models.Exceptions;
using Firebase.Auth.Providers;
using Firebase.Auth.Repository;
using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;

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
    private static FirebaseAuthClient? _fbAuthClient = null;

    public AuthenticationService()
    {
        // if (_fbApp == null)
        // {
        //     var appOptions = new AppOptions()
        //     {
        //         Credential = new GoogleCredential
        //         {
        //             
        //         }
        //     };
        //     _fbApp = FirebaseApp.Create();
        // }

        if (_fbAuthClient == null)
        {
            var config = new FirebaseAuthConfig
            {
                ApiKey = "someapikey",
                AuthDomain = "somedomain",
                Providers = new FirebaseAuthProvider[]
                {
                    // Add and configure individual providers
                    new GoogleProvider().AddScopes(
                        "https://www.googleapis.com/auth/cloud-platform",
                        "https://www.googleapis.com/auth/userinfo.email",
                        "https://www.googleapis.com/auth/userinfo.profile",
                        "openid"),
                    new EmailProvider()
                }
            };
            _fbAuthClient = new FirebaseAuthClient(config);
        }
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
                throw new UserAlreadyExistException(fbEx);
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