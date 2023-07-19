# Arch Configuration :
sudo pacman -S dotnet-runtime dotnet-sdk aspnet-runtime
sudo pacman -S jdk11-openjdk
sudo dotnet workload intall maui-android

# Visual Studio Code Net MAUI extension configuration
1: link to the required openjdk installation (using .NET maui extension)
-> /usr/lib/jvm/java-11-openjdk

2 Check for extension requirements (under the tab "Output" > ".NET MAUI")
Android SDK recommended required components:
	platforms/android-33  installed
	build-tools/32.0.0  not found
	platform-tools  installed
	cmdline-tools/7.0  not found
Android SDK recommended optional components:
	emulator  installed
	system-images/android-33/google_apis/x86_64  not found

3 Go to Android studio and install the tools (check the "Show package details" checkbox to see all versions)
-> Install Android SDK Build Tools 32.0.0
-> Install Android SDK CommandLine Tools 7.0 (not latest)