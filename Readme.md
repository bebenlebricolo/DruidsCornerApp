# Druid's Corner App
The Druid's Corner Application is focused around homebrewing and was developed as a side-project.
With this app, you can browse and view recipes retrieved from the infamous DiyDog book.

## Search service
You can also use powerful filters on the recipe database in order to fetch them using a wide variety of criteria such as :
* Their main ingredients :
  * Hops
  * Yeasts
  * Malts
* Extra ingredients
  * Extra mash ingredients (such as dextrose, maltose, sugars)
  * Extra boiling or fermentation ingredients (such as chilli, coffee)
* Their characteristics
  * Style
  * ABV (Alcohol by value)
  * IBU (International Bitterness Units)
  * EBC (European Brewery Convention) -> Used for color ordering
  * SRM (Standard Reference Method) -> Another color scale

## Conversion service
Once you've found a recipe that matches your needs, you can use the conversion tools of the app in order to recompute recipe parameters and characteristics to suit your needs.
To name a few, you can constraint your recipe with its target volume, and more importantly modify ingredients amounts and characteristics to suit what you have at hands.
Also, there is a substitution service that allows you to swap yeats and hops for their best counterparts, that you already have in stock or that are available in your area.

## Recipe management
Of course you can set some recipes as your favorite, build recipe collections and derive your own recipes using one of brewDog's recipe as a template.


# Development
Development on .net maui is a bit crooked, especially on Linux, so there is a little check list I keep for myself, but if it can be useful to someone (...) here it is.

## Arch Configuration :
```bash
# Open JDK 11 is required by MAUI SDK
sudo pacman -S jdk11-openjdk

# Used to work for dotnet 7.0 :
sudo pacman -S dotnet-runtime dotnet-sdk aspnet-runtime

# For dotnet 8.0 :
yay -S dotnet-sdk-bin

# Then, install the maui-android workload
sudo dotnet workload install maui-android
```

# Visual Studio Code Net MAUI extension configuration
1: link to the required openjdk installation (using .NET maui extension)
- Found at:  `/usr/lib/jvm/java-11-openjdk`
- /!\ OpenJdk 11 is the only one suitable for this !

2 Check for extension requirements (under the tab "Output" > ".NET MAUI")
```
Android SDK recommended required components:
	platforms/android-33  installed
	build-tools/32.0.0  not found
	platform-tools  installed
	cmdline-tools/7.0  not found
Android SDK recommended optional components:
	emulator  installed
	system-images/android-33/google_apis/x86_64  not found
```

3 Go to Android studio and install the tools (check the "Show package details" checkbox to see all versions)
- Install Android SDK Build Tools 32.0.0
- Install Android SDK CommandLine Tools 7.0 (not latest)

# Rider configuration
> Note I had issues with the autocompletion (which never worked on dotnet MAUI projects so far for me on Linux), so I gave a try to Rider from JetBrains and it does the job quite nicely.
> Just remember to correctly set the Android SDK paths and openjdk11 paths in Rider's settings :
`Rider > File > Settings ... > Build, Execution, Deployment > Android`
There is also a required Xamarin plugin required by Rider when working on this app.
Usually it asks for it itself :smile:.

## Rider quirks
Sometimes, Rider will show errors in files whereas everything builds just fine.
This can be solved by invalidate caches (File > Invalidate caches > check all checkboxes and restart )
-> Also, don't forget to select the right targeted framework when developing (Net X.Y Android).

# Authentication configuration setup
This application needs to connect to Firebase in order to perform user authentication / authorization.
A Firebase public ApiKey is then required for the app to connect to Firebase servers.
This configuration needs to be written into a "AuthConfig.json" file, located under the [DruidsCornerApp/.config](DruidsCornerApp/.config) folder.
A template version exist and can be found here : [DruidsCornerApp/.config/TemplateAuthConfig.json](DruidsCornerApp/.config/TemplateAuthConfig.json).
```json
{
  "FIREBASE_API_KEY" : "Abcdefghijklmnopqrstuvwxyz",
  "FIREBASE_AUTH_DOMAIN" : "my-project-name-in-firebase-settings",
  "JWT_SCOPES" : [
    "scope 1",
    "scope 2",
    "scope 3"
  ]
}
```
Real ApiKey and AuthDomain can be found in [Firebase console project parameters](https://console.firebase.google.com/project/).
Furthermore, JWT Scopes also need to be provided, and they are used by the application to ask permissions on user's behalf when authenticating against Google OAuth2.

# Signed Android app and public *ApiKey limitations*
In order to secure calls to Firebase servers, the public ApiKey is protected and leverages Google Cloud apiKey limitations.
As such, only this app is whitelisted alongside its certificate (SHA1 sum).
SHA-1 footprints can be retrieved from a key like so :
```bash
keytool -list -v -alias <key alias> -keystore <keyfile location>
```
*Note : I experienced issues with the **default jvm runtime** (hence keytool) on my machine as it **gave a 32-wide SHA-1 fingerprint**, because the default jvm
happened to not be the one I needed.
So ensure the keytool used comes from the **open-jdk 11** !*

# Sign the .apk in both Debug and Release modes (for debug and deployment purposes)
In Rider :
select ProjectProperties > <Debug | Release> "Sign the .APK file using the next keystore information" > File in the blanks !
```
* Keystore : <path to the keystore file>
* Password : <Keystore password>
* Keystore alias : <Keystore alias>
* Keystore alias password : <Keystore alias password>
```