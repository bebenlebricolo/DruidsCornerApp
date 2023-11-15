using System.Net;
using DruidsCornerApiClient.Models.Exceptions;

namespace DruidsCornerApiClient.Services.Interfaces;

public abstract class BaseClient
{
    // Essentially empty, serves as a root for ILogger<IBaseClient>
    public void HandleResponseStatus(HttpResponseMessage response)
    {
        if (response.StatusCode == HttpStatusCode.Unauthorized || response.StatusCode == HttpStatusCode.Forbidden)
        {
            throw new ClientException("Caught authorization issues", FailureModes.AuthenticationFailure);
        }
    }
}