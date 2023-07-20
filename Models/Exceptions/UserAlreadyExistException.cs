namespace DruidsCornerApp.Models.Exceptions;

using FirebaseAdmin.Auth;

public class UserAlreadyExistException : Exception
{
    public UserAlreadyExistException(FirebaseAuthException inner) : base("User already exist", inner)
    {
    }
}