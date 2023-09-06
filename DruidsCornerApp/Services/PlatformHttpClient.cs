namespace DruidsCornerApp.Services;

public class PlatformHttpClient : HttpClient
{
    public const string ANDROID_PKG_CERT_HEADER = "x-android-cert";
    public const string ANDROID_PKG_NAME_HEADER = "x-android-package";

    private string Sha1Cert = "";
    private string PkgName = "";

    public PlatformHttpClient(string sha1Cert, string pkgName)
    {
        Sha1Cert = sha1Cert;
        PkgName = pkgName;
        DefaultRequestHeaders.Add(ANDROID_PKG_CERT_HEADER, Sha1Cert);
        DefaultRequestHeaders.Add(ANDROID_PKG_NAME_HEADER, PkgName);
    }
}