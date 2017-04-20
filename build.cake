#addin "Cake.FileHelpers"

var TARGET = Argument ("target", Argument ("t", "Default"));
var version = EnvironmentVariable ("APPVEYOR_BUILD_VERSION") ?? Argument("version", "1.0.0");

var keystore = EnvironmentVariable ("keystore") ?? Argument("keystore", "ennerperez.keystore");
var keystore_alias = EnvironmentVariable ("keystore-alias") ?? Argument("keystore-alias", "ennerperez");
var keystore_password = EnvironmentVariable ("keystore-password") ?? Argument("keystore-password", "");

var solutions = new Dictionary<string, string> {
 	{ "./Teaching.Skills.sln", "Any" },
};

var packages = new Dictionary<string, string> {
	{"./teaching.skills.droid/Teaching.Skills.Droid.csproj", "Any"},
};

var BuildAction = new Action<Dictionary<string, string>> (collection =>
{

	foreach (var sln in collection) 
    {

		// If the platform is Any build regardless
		//  If the platform is Win and we are running on windows build
		//  If the platform is Mac and we are running on Mac, build
		if ((sln.Value == "Any")
				|| (sln.Value == "Win" && IsRunningOnWindows ())
				|| (sln.Value == "Mac" && IsRunningOnUnix ())) 
        {
			
			// Bit of a hack to use nuget3 to restore packages for project.json
			if (IsRunningOnWindows ()) 
            {
				
				Information ("RunningOn: {0}", "Windows");

				NuGetRestore (sln.Key, new NuGetRestoreSettings
                {
					ToolPath = "./tools/nuget3.exe"
				});

				// Windows Phone / Universal projects require not using the amd64 msbuild
				MSBuild (sln.Key, c => 
                { 
					c.Configuration = "Release";
					c.MSBuildPlatform = Cake.Common.Tools.MSBuild.MSBuildPlatform.x86;
				});
			} 
            else 
            {
                // Mac is easy ;)
				NuGetRestore (sln.Key);

				DotNetBuild (sln.Key, c => c.Configuration = "Release");
			}
		}
	}
});

var BuildAndroidAction = new Action<Dictionary<string, string>> (collection =>
{

	foreach (var sln in collection) 
    {

		// If the platform is Any build regardless
		//  If the platform is Win and we are running on windows build
		//  If the platform is Mac and we are running on Mac, build
		if ((sln.Value == "Any")
				|| (sln.Value == "Win" && IsRunningOnWindows ())
				|| (sln.Value == "Mac" && IsRunningOnUnix ())) 
        {
			
			// Bit of a hack to use nuget3 to restore packages for project.json
			if (IsRunningOnWindows ()) 
            {
				
				Information ("RunningOn: {0}", "Windows");

				// Windows Phone / Universal projects require not using the amd64 msbuild
				MSBuild (sln.Key, c => 
                { 
					c.Configuration = "Release";
					c.MSBuildPlatform = Cake.Common.Tools.MSBuild.MSBuildPlatform.x86;
					c.Properties.Add("AndroidKeyStore", new [] {"true"});
					c.Properties.Add("AndroidSigningKeyStore", new [] {keystore});
					c.Properties.Add("AndroidSigningKeyAlias", new [] {keystore_alias});
					c.Properties.Add("AndroidSigningKeyPass", new [] {keystore_password});
					c.Properties.Add("AndroidSigningStorePass", new [] {keystore_password});				
					c.Targets.Add("SignAndroidPackage");
				});
			} 
            else 
            {
                // Mac is easy ;)
				
				DotNetBuild (sln.Key, c => 
				{
					c.Configuration = "Release";
					c.Properties.Add("AndroidKeyStore", new [] {"true"});
					c.Properties.Add("AndroidSigningKeyStore", new [] {keystore});
					c.Properties.Add("AndroidSigningKeyAlias", new [] {keystore_alias});
					c.Properties.Add("AndroidSigningKeyPass", new [] {keystore_password});
					c.Properties.Add("AndroidSigningStorePass", new [] {keystore_password});				
					c.Targets.Add("SignAndroidPackage");
				});
			}
		}
	}
});

Task("Solutions").Does(()=>
{
    BuildAction(solutions);
});

Task ("Platforms")
	.IsDependentOn ("Solutions")
	.Does (() =>
{

	var android = new Dictionary<string, string>();

	foreach (var sln in packages) 
    {
		if (sln.Key.Contains("Droid"))
			android.Add(sln.Key, sln.Value);
	}

	BuildAndroidAction(android);
	
});

Task ("Default").IsDependentOn("Platforms");

Task ("Clean").Does (() => 
{
	CleanDirectory ("./component/tools/");

	CleanDirectories ("./build/");

	CleanDirectories ("./**/bin");
	CleanDirectories ("./**/obj");
});

RunTarget (TARGET);