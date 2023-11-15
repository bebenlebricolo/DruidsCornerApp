namespace DruidsCornerApiClient.Models.Exceptions;

/// <summary>
/// Thrown when interfacing with WebApis,  
/// </summary>
public class ClientException : Exception
{
    public FailureModes FailureMode;
    
    public ClientException(string message, FailureModes mode = FailureModes.Unknown) : base(message)
    {
        FailureMode = mode;
    }
}