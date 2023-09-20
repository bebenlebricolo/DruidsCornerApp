using System.Net.Mime;

namespace DruidsCornerApiClient.Models.Wrappers;

/// <summary>
/// Pdf page stream class, used to return the result of the GetPdfPage() service that downloads an image from
/// DruidsCornerApi database
/// </summary>
public class PdfStream
{
    /// <summary>
    /// Contains the memory stream retrieved from an HttpResponse
    /// </summary>
    public MemoryStream? Stream { get; private set; } = null;

    /// <summary>
    /// Image name
    /// </summary>
    public string Name { get; set; } = string.Empty;

    public static async Task<PdfStream> FromHttpResponseAsync(HttpResponseMessage response)
    {
        var contentDisposition = response.Content.Headers.ContentDisposition!;

        // Should return "PNG" or "JPG" or any other kind of image format.
        var name = contentDisposition.FileName!;
        var pdfStream = new PdfStream()
        {
            Stream = (await response.Content.ReadAsStreamAsync() as MemoryStream)!,
            Name = name
        };

        return pdfStream;
    }
}