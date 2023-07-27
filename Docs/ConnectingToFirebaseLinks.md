https://stackoverflow.com/questions/71877262/how-do-i-connect-my-maui-app-to-a-firestore-database-via-service-account-json

# Api key restrictions and how to deal with it (Android SHA-1 lock)
Restricting Firebase Api key is ... key in order to enhance security.
However very soon we discover that we need to somehow send our Android package SHA-1 cert and package name values to Firebase auth/google servers.
Otherwise we might take an error like this one : 
```json
{
  "error": {
    "code": 403,
    "message": "Requests from this Android client application <empty> are blocked.",
    "errors": [
      {
        "message": "Requests from this Android client application <empty> are blocked.",
        "domain": "global",
        "reason": "forbidden"
      }
    ],
    "status": "PERMISSION_DENIED",
    "details": [
      {
        "@type": "type.googleapis.com/google.rpc.ErrorInfo",
        "reason": "API_KEY_ANDROID_APP_BLOCKED",
        "domain": "googleapis.com",
        "metadata": {
          "service": "identitytoolkit.googleapis.com",
          "consumer": "projects/smqlsdùmqlsk"
        }
      }
    ]
  }
}
```

In order to solve this issue, we need to send those headers :
```json
{
  "x-android-package" : "[YOUR_PACKAGE_NAME]",
  "x-android-cert" : "[YOUR_PACKAGE_SHA-1_CERT]"
}
```

Something like this : 
```bash
curl  -X POST \
  'https://identitytoolkit.googleapis.com/v1/accounts:signInWithPassword?key=[ANDROID_RESTRICTED_API_KEY]' \
  --header 'Accept: application/json' \
  --header 'x-android-package: [YOUR_PACKAGE_NAME]' \
  --header 'x-android-cert: [YOUR_APP_SHA-1_FINGERPRINT] \
  --header 'Content-Type: application/json' \
  --data-raw '{
  "email" : "someone@somewhere.com",
  "password" : "mysuperpassword",
  "returnSecureToken" : true
}'
```
