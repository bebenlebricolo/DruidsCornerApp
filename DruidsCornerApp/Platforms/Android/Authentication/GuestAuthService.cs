using System.Runtime.Serialization;
using System.Text.Json;
using Microsoft.Extensions.Logging;

namespace DruidsCornerApp.Services.Authentication;
using DruidsCornerApp.Models.Authentication;
public partial class GuestAuthService : IGuestAuthService
{
    public async partial Task<string?> GetPublicAccessTokenAsync(CancellationToken cancellationToken)
    {
        var authConfig = await _authConfigProvider.GetAuthConfigAsync();
        var fullUrl = new Uri($"{authConfig.AuthGatewayEndpoint}/{_publicTokenRoute}?apikey={authConfig.PublicAccessApiKey}");
        var request = new HttpRequestMessage(HttpMethod.Get, fullUrl);
        var response = await _httpClient.SendAsync(request, cancellationToken);

        if ((int) response.StatusCode != 200)
        {
            _logger.LogError($"Caught http error when retrieving public access token : {response.StatusCode}");
            return null;
        }

        try
        {
            var jsonOptions = new JsonSerializerOptions()
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
            var responseModel = await JsonSerializer.DeserializeAsync<AuthGatewayResponse>(await response.Content.ReadAsStreamAsync(), jsonOptions);
            return responseModel?.IdToken;
        }
        catch (SerializationException ex)
        {
            _logger.LogError($"Could not read data from http response : {ex.Message}");
        }

        return null;
    }
}