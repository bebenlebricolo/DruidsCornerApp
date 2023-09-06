using Microsoft.Extensions.Logging;

namespace DruidsCornerApp.Services.Authentication;

public partial class GuestAuthService : IGuestAuthService
{
    private ILogger<GuestAuthService> _logger;
    private IAuthConfigProvider _authConfigProvider;
    private HttpClient _httpClient;
    private const string _publicTokenRoute = "auth/public-access-token";

    public GuestAuthService(ILogger<GuestAuthService> logger,
                            IAuthConfigProvider authConfigProvider,
                            HttpClient httpClient
    )
    {
        _logger = logger;
        _authConfigProvider = authConfigProvider;
        _httpClient = httpClient;
    }


#if __ANDROID__
    public partial Task<string?> GetPublicAccessTokenAsync(CancellationToken cancellationToken);
#else
    public Task<string?> GetPublicAccessTokenAsync(CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
#endif
}