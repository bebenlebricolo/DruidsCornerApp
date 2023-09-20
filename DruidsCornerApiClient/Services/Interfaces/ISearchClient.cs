using DruidsCornerApiClient.Models.RecipeDb;
using DruidsCornerApiClient.Models.References.Properties;
using DruidsCornerApiClient.Models.Search;

namespace DruidsCornerApiClient.Services.Interfaces;

public interface ISearchClient : IBaseClient
{
    /// <summary>
    /// Queries the database and finds all candidate recipe that match input queries.
    /// The queries object contains multiple individual queries, that can be used to further reduce the candidates.
    /// Amongst the possibilities, one can search yeasts, hops, styles and limit numerical values within ranges (like ABV, IBU, etc).
    /// </summary>
    /// <param name="queries"></param>
    /// <returns></returns>
    public Task<List<Recipe>?> SearchAllCandidatesAsync(Queries queries, string token);

    /// <summary>
    /// Queries the database and finds candidate hops that best match the list of queried names.
    /// This endpoint aims to dig into the resources database (known-good Hops list in this case)
    /// and find well known Hops that are currently being used in the database.
    /// Having well known hops is a first step to having top quality data and opens the way to the conversion algorithms.  
    /// </summary>
    /// <param name="names"></param>
    /// <returns></returns>
    public Task<List<HopProperty>?> SearchHopsByNameAsync(List<string> names, string token);

    /// <summary>
    /// Queries the database and finds Well-Known Malt candidates that best match the list of queried names.
    /// This endpoint aims to dig into the resources database (known-good Malts list in this case)
    /// and find well known Malts that are currently being used in the database.
    /// Having well known malts is a first step to having top quality data and opens the way to the conversion algorithms.
    /// </summary>
    /// <param name="names"></param>
    /// <returns></returns>
    public Task<List<MaltProperty>?> SearchMaltsByNameAsync(List<string> names, string token);

    /// <summary>
    /// Queries the database and finds Well-Known Yeasts candidates that best match the list of queried names.
    /// This endpoint aims to dig into the resources database (known-good Yeasts list in this case)
    /// and find well known Yeasts that are currently being used in the database.
    /// Having well known yeasts is a first step to having top quality data and opens the way to the conversion algorithms.
    /// </summary>
    /// <param name="names"></param>
    /// <returns></returns>
    public Task<List<YeastProperty>?> SearchYeastsByNameAsync(List<string> names, string token);

    /// <summary>
    /// Queries the database and finds Well-Known Styles candidates that best match the list of queried names.
    /// This endpoint aims to dig into the resources database (known-good Styles list in this case)
    /// and find well known Styles that are currently being used in the database.
    /// </summary>
    /// <param name="names"></param>
    /// <param name="minimumMatchingScore">Tweaks the minimum threshold used to select data.
    ///                                    The higher the threshold the harder it gets to find data</param>
    /// <returns></returns>
    public Task<List<StyleProperty>?> SearchStylesByNameAsync(List<string> names, string token, uint minimumMatchingScore = 50);
}