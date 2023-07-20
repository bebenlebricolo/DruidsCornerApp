using System.Diagnostics;
using DruidsCornerApp.Models.Exceptions;
using FirebaseAdmin;

namespace DruidsCornerApp.Services;
using FirebaseAdmin.Auth;

public static class AuthenticationService
{
    private static FirebaseApp? _fbApp = null;

    private static void Init()
    {
        if (_fbApp == null)
        {
            _fbApp = FirebaseApp.Create();
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
    public static async Task<UserRecord> CreateNewUser(UserRecordArgs userArgs, CancellationToken cancellationToken)
    {
        Init();
        var auth = FirebaseAuth.GetAuth(_fbApp);
        UserRecord userRecord;

        try
        {
            userRecord = await auth.CreateUserAsync(userArgs, cancellationToken);
        }
        catch (FirebaseAuthException fbEx)
        {
            // Todo : Log something here
            if (fbEx.AuthErrorCode == AuthErrorCode.EmailAlreadyExists)
            {
                throw new UserAlreadyExistException(fbEx);
            }
            
            // Propagate exception, we don't know enough to handle it at this stage
            throw;
        }

        return userRecord;
    }
    
    public static async Task<string?> SignInBasic(string email, string password, CancellationToken cancellationToken)
    {
        Init();
        var auth = FirebaseAuth.GetAuth(_fbApp);
        string? token;
        try
        {
            UserRecord userRecord = await auth.GetUserByEmailAsync(email, cancellationToken);
            token = await auth.CreateCustomTokenAsync(userRecord.Uid);
            
            
        }
        catch (FirebaseAuthException fbEx)
        {
            // Todo log error here
            throw;
        }
        
        return token;
    }
}