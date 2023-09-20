using DruidsCornerApiClient.Models.Wrappers;

namespace DruidsCornerApiClient.Services.Interfaces;

public interface IResourcesClient : IBaseClient
{
   /// <summary>
   /// Fetches the pdf page of the recipe matching the queried number.
   /// Returns a null reference in case no resource was found
   /// Throws DruidsCornerApiClientException in case of authentication issues
   /// </summary>
   /// <param name="number"></param>
   /// <param name="token"></param>
   /// <returns></returns>
   public Task<ImageStream?> GetImageAsync(uint number, string token);

   /// <summary>
   /// Fetches the pdf page for the recipe matching the queried number.
   /// Returns a null reference in case no resource was found
   /// Throws DruidsCornerApiClientException in case of authentication issues
   /// </summary>
   /// <param name="number"></param>
   /// <param name="token"></param>
   /// <returns></returns>
   public Task<PdfStream?> GetPdfPageAsync(uint number, string token);
}