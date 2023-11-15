namespace DruidsCornerApiClient.Models.Exceptions;

public enum FailureModes
{
    /// <summary>
    /// AuthenticationFailure encompasses scenarios where
    /// * Token has expired (essentially)
    /// * Forbidden cases
    /// </summary>
    AuthenticationFailure,
    
    /// <summary>
    /// Client-side issue : something in the configuration of the client is wrong
    /// </summary>
    Misconfigured,
    
    /// <summary>
    /// Any other kind of errors that is opaque and needs further investigations
    /// </summary>
    Unknown
}