namespace DruidsCornerApp.Models.Exceptions;
using Firebase.Auth;

public enum AuthenticationError
{
    UserAlreadyExist,
    EmptyToken,
    Unknown
};

/// <summary>
/// Thrown when creating or authenticating user,  
/// </summary>
public class AuthenticationException : Exception
{
    public AuthenticationError Error;
    public AuthenticationException(string message, AuthenticationError error) : base(message)
    {
    }
}