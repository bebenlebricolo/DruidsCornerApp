using DruidsCornerApiClient.Models;
using DruidsCornerApiClient.Services.Interfaces;
using Microsoft.Extensions.Logging;

namespace DruidsCornerApiClient.Services;

/// <summary>
/// MainClient class is the main entry point of DruidsCornerApi C# Client.
/// It embeds several subclients, that can used like this :
/// var client = new MainClient(...);
/// var recipes = await client.Recipe.GetAllAsync(...);
/// var result = await client.Search.SingleQueryAsync(...);
/// </summary>
public class MainClient : IBaseClient
{
    private readonly ILogger<IBaseClient> _logger;
    private readonly HttpClient _httpClient;
    private readonly ClientConfiguration _clientConfiguration;

    /// <summary>
    /// Recipe web api client
    /// </summary>
    public IRecipeClient? Recipe { get; private set; }

    /// <summary>
    /// Resources web api client
    /// </summary>
    public IResourcesClient? Resources { get; private set; }

    /// <summary>
    /// Search web api client
    /// </summary>
    public ISearchClient? Search { get; private set; }

    private bool ClientsOk()
    {
        bool ready = Recipe != null;
        ready &= Resources != null;
        ready &= Search != null;

        return ready;
    }


    /// <summary>
    /// Instantiates a DruidsCornerApi MainClient class.
    /// Member clients can be injected here, or left null for automatic instantiation
    /// (constructor will take care about initialising the members itself).
    /// </summary>
    /// <param name="logger">Main logger, shared amongst all subclients</param>
    /// <param name="clientConfiguration">DruidsCornerApi client configuration</param>
    /// <param name="httpClient">HttpClient that'll be used by member api clients</param>
    /// <param name="recipe">Optional Recipe webapi client, autoinstantiated if left null</param>
    /// <param name="resources">Optional Resources webapi client, autoinstantiated if left null</param>
    /// <param name="search">Optional Search webapi client, autoinstantiated if left null</param>
    public MainClient(ILogger<IBaseClient> logger,
                      ClientConfiguration clientConfiguration,
                      HttpClient httpClient,
                      IRecipeClient? recipe = null,
                      IResourcesClient? resources = null,
                      ISearchClient? search = null
    )
    {
        _logger = logger;
        _clientConfiguration = clientConfiguration;
        _httpClient = httpClient;
        
        Recipe = recipe;
        Resources = resources;
        Search = search;

        Init();
    }

    /// <summary>
    /// Initialises the member api clients
    /// </summary>
    /// <param name="forceReinit"></param>
    public void Init(bool forceReinit = false)
    {
        if (!forceReinit && ClientsOk())
        {
            return;
        }

        // Instantiate a recipe client
        if (Recipe == null)
        {
            Recipe = new RecipeClient(_logger, _httpClient, _clientConfiguration);
        }

        // Instantiate a recipe client
        if (Resources == null)
        {
            Resources = new ResourceClient(_logger, _httpClient, _clientConfiguration);
        }

        // Instantiate a recipe client
        if (Search == null)
        {
            Search = new SearchClient(_logger, _httpClient, _clientConfiguration);
        }
    }
}