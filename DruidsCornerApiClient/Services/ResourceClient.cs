using System.Net.Http.Headers;
using System.Net.Mime;
using System.Text.Json;
using Microsoft.Extensions.Logging;

using DruidsCornerApiClient.Models;
using DruidsCornerApiClient.Models.Wrappers;
using DruidsCornerApiClient.Utils;
using DruidsCornerApiClient.Services.Interfaces;

namespace DruidsCornerApiClient.Services;

public class ResourceClient : IResourcesClient
{
    private ILogger<IBaseClient> _logger;
    private readonly HttpClient _httpClient;
    private readonly ClientConfiguration _configuration;
    
    public ResourceClient(ILogger<IBaseClient> logger,
                          HttpClient httpClient,
                          ClientConfiguration configuration  )
    {
        _logger = logger;
        _httpClient = httpClient;
        _configuration = configuration;
    }
    
    private string GetEndpointUrl(string endpointName)
    {
        var url = $"{WebConstants.Scheme}://{_configuration.Domain}/resources/{endpointName}";
        return url;
    }

    public async Task<ImageStream?> GetImageAsync(uint number, string token)
    {
        var url = GetEndpointUrl("image");
        url += $"?number={number}";
        
        var requestMessage = new HttpRequestMessage(HttpMethod.Get, url);
        requestMessage.Headers.Authorization = new AuthenticationHeaderValue(WebConstants.BearerStr, token);
        var response = await _httpClient.SendAsync(requestMessage);

        if ((int)response.StatusCode < 200 || (int)response.StatusCode >= 400)
        {
            _logger.LogError($"Could not retrieve image from recipe, caught issue within http response");
            _logger.LogError($"StatusCode : {response.StatusCode} ; Reason : {response.ReasonPhrase} ; Headers : {response.Headers}");
            return null;
        }

        try
        {
            var imageStream = await ImageStream.FromHttpResponseAsync(response);
            return imageStream;
        }
        catch (Exception ex)
        {
            _logger.LogError("Caught issue while reading Image as a FileStream");
            _logger.LogError($"Exception was : {ex.Message}");
            return null;
        }
    }

    public async Task<PdfStream?> GetPdfPageAsync(uint number, string token)
    {
        var url = GetEndpointUrl("pdf");
        url += $"?number={number}";
        
        var requestMessage = new HttpRequestMessage(HttpMethod.Get, url);
        requestMessage.Headers.Authorization = new AuthenticationHeaderValue(WebConstants.BearerStr, token);
        var response = await _httpClient.SendAsync(requestMessage);

        if ((int)response.StatusCode < 200 || (int)response.StatusCode >= 400)
        {
            _logger.LogError($"Could not retrieve pdf page from recipe, caught issue within http response");
            _logger.LogError($"StatusCode : {response.StatusCode} ; Reason : {response.ReasonPhrase} ; Headers : {response.Headers}");
            return null;
        }

        try
        {
            var pdfStream = await PdfStream.FromHttpResponseAsync(response);
            return pdfStream;
        }
        catch (Exception ex)
        {
            _logger.LogError("Caught issue while reading Pdf page as a FileStream");
            _logger.LogError($"Exception was : {ex.Message}");
            return null;
        }
    }
}