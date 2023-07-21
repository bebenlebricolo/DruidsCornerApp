namespace DruidsCornerApp.Models.Exceptions;
using Firebase.Auth;

/// <summary>
/// Thrown when creating a new user, when email address is already defined in database. 
/// </summary>
public class UserAlreadyExistException : Exception
{
    public UserAlreadyExistException(FirebaseAuthException inner) : base("User already exist", inner)
    {
    }
}