
#if __ANDROID__
using System.Text;
using Android.Content.PM;
    using Android.Util;
    using Java.Security;
    using Signature = Android.Content.PM.Signature;
#endif

namespace DruidsCornerApp.Utils;

public static class PackageUtils
{
#if __ANDROID__
    private static PackageInfo? GetAndroidPackageInfoCompat()
    {
        if (Platform.CurrentActivity == null) return null;
        var packageManager = Platform.CurrentActivity.PackageManager;
        if (packageManager == null) return null;

        // Note : Warning disabled because Dotnet deprecated this function whereas the Java sdk only deprecated the getPackageInfo(string, int) version
        // to the profit of its enum counterpart : getPackageInfo(string, Enum) which is safer.
#pragma warning disable CS0618
        var info = packageManager.GetPackageInfo(GetPackageName(), PackageInfoFlags.Signatures);
        return info;
    }
#endif

#if __ANDROID__
    private static IList<Signature>? GetAndroidSignaturesCompat()
    {
        var info = GetAndroidPackageInfoCompat();
        if (info == null) return null;
        
        var apiLevel = Android.OS.Build.VERSION.SdkInt;
        if ((int) apiLevel < 21)
        {
            // Don't know how to handle the situation for those devices yet !
            return null;
        }

        IList<Android.Content.PM.Signature>? signatures = null;
        if ((int)apiLevel >= 21 && (int)apiLevel < 28)
        {
#pragma warning disable CA1416
#pragma warning disable CA1422
            signatures = info.Signatures;
        }
        else
        {
            if (info.SigningInfo == null) return null; 

            var signingInfo = info.SigningInfo;
            if (signingInfo == null) return null;

            signatures = signingInfo.GetSigningCertificateHistory();
        }

        return signatures;
    }
#endif
    
#if __ANDROID__

    private static IList<Signature>? GetAndroidSignatures()
    {
        var info = GetAndroidPackageInfoCompat();
        if (info == null) return null;

        // Saw that even on some high Apis levels (30+), the Signatures array is still there
        // but there's nothing in the SigningInfo, so fallback to the Signatures (...)
        return info.Signatures;
    }

    /// <summary>
    /// Stolen from https://stackoverflow.com/a/54791043/8716917
    /// </summary>
    /// <param name="digest"></param>
    /// <returns></returns>
    private static string AndroidConvertDigestToSha1String(byte[] digest)
    {
        var toRet = new StringBuilder();
        for (int i = 0; i < digest.Length; i++) {
            if (i != 0) toRet.Append(":");
            int b = digest[i] & 0xff;
            String hex = b.ToString("X2");
            
            if (hex.Length == 1) toRet.Append("0");
            toRet.Append(hex);
        }

        return toRet.ToString();
    }
    
#endif
    
    /// <summary>
    /// Reads the current .apk application SHA-1 fingerprint and returns it raw
    /// </summary>
    /// <returns></returns>
    public static List<string>? GetPackageSha1Signatures()
    {
#if  __ANDROID__
        var signatures = GetAndroidSignaturesCompat();
        if (signatures == null)
        {
            // Try the fallback methods
            signatures = GetAndroidSignatures();
        }
       
        if (signatures == null) return null;
        
        // Among signatures, return the one that match the SHA footprint
        var sigList = new List<string>();
        foreach (var signature in signatures)
        {
            MessageDigest messageDigest = MessageDigest.GetInstance("SHA1");
            var byteArray = signature.ToByteArray();
            if (byteArray == null)
            {
                continue;
            }

            messageDigest.Update(byteArray);
            var digest = messageDigest.Digest();
            var hashStr = AndroidConvertDigestToSha1String(digest);
            sigList.Add(hashStr);
        }
#else
        List<string> sigList = null;
#endif

        return sigList;
    }

    /// <summary>
    /// Reads the current .apk application name 
    /// </summary>
    /// <returns></returns>
    public static string GetPackageName()
    {
        return AppInfo.PackageName;
    }
}