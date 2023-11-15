using System.Net.Mime;

namespace DruidsCornerApiClient.Models.Wrappers;

/// <summary>
/// Image stream class, used to return the result of the GetImage() service that downloads an image from
/// DruidsCornerApi database
/// </summary>
public class ImageStream
{
    /// <summary>
    /// Contains the memory stream retrieved from an HttpResponse
    /// </summary>
    public MemoryStream? Stream { get; private set; } = null;

    /// <summary>
    /// Image name
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Image encoding
    /// </summary>
    public string Format { get; set; } = string.Empty;

    public static async Task<ImageStream> FromHttpResponseAsync(HttpResponseMessage response)
    {
        var contentType = response.Content.Headers.ContentType!;
        var contentDisposition = response.Content.Headers.ContentDisposition!;

        // Should return "PNG" or "JPG" or any other kind of image format.
        var format = contentType.MediaType?.Replace("image/", "")!;
        var name = contentDisposition.FileName!;
        var imageStream = new ImageStream()
        {
            Stream = (await response.Content.ReadAsStreamAsync() as MemoryStream)!,
            Format = format,
            Name = name
        };

        return imageStream;
    }
}