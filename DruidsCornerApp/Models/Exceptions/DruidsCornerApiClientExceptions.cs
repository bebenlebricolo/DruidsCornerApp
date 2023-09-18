namespace DruidsCornerApp.Models.Exceptions;

/// <summary>
/// Thrown when interfacing with WebApis,  
/// </summary>
public class DruidsCornerApiClientException : Exception
{
    public DruidsCornerApiClientException(string message) : base(message)
    {
    }
}