using DruidsCornerApp.Models.Exceptions;
using Firebase.Auth;
using FirebaseAdmin.Auth;

namespace DruidsCornerApp.Services;

public interface IAuthenticationService
{
    /// <summary>
    /// Creates a new user in the system.
    /// May throw if the user already exist, or lower level exception will be thrown.
    /// </summary>
    /// <param name="userArgs">User record arguments, coming from UI</param>
    /// <param name="cancellationToken">Used to monitor and abort operations that are too long.</param>
    /// <returns>New user record</returns>
    /// <exception cref="UserAlreadyExistException"></exception>
    public Task<UserCredential> CreateNewUserAsync(UserRecordArgs userArgs, CancellationToken cancellationToken);

    /// <summary>
    /// SignIn with basic credentials (User email and Password)
    /// </summary>
    /// <param name="email"></param>
    /// <param name="password"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<string?> SignInBasicAsync(string email, string password, CancellationToken cancellationToken);

    /// <summary>
    /// SignIn as a new guest user
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<string?> SignInAsGuest(CancellationToken cancellationToken);

    /// <summary>
    /// Refreshes a token
    /// </summary>
    /// <returns></returns>
    public Task<string> RefreshTokenAsync();
}