#if UNITY_EDITOR
using UnityEditor;
using System.IO;
using UnityEngine;
using System.Collections.Generic;

class Build
{
	private static string BUILD_LOCATION = "-build_location";
	private static string BUILD_NAME = "-build_name";
    
	private static string BUILD_KEYSTORE_NAME = "-build_keystore_name";
	private static string BUILD_KEYSTORE_PASSWORD = "-build_keystore_password";
	private static string BUILD_KEYALIAS_NAME = "-build_keyalias_name";
	private static string BUILD_KEYALIAS_PASSWORD = "-build_keyalias_password";
	private static string BUILD_FORMAT = "-build_format";
    private static string CUSTOM_VERSION = "-custom_version";
    
    private static string APK = "apk";
    private static string AAB = "aab";
    
    private static string INCREMENT_MAJOR = "increment_major";
    private static string INCREMENT_MINOR = "increment_minor";
    private static string INCREMENT_PATCH = "increment_patch";

	static string GetArg(string argName)
	{
		string[] args = System.Environment.GetCommandLineArgs();
		int indexOfArg = System.Array.IndexOf(args, argName);
		
		if (indexOfArg >= 0)
		{	
			indexOfArg++;
			return args[indexOfArg];
		}
		else 
		{
			return null;
		}
	}
	
	static string GetBuildLocation(BuildTarget buildTarget)
	{
		string path = GetArg(BUILD_LOCATION);
		
		if (path == null)
		{
			path = EditorUserBuildSettings.GetBuildLocation(buildTarget);
		}

		return path;
	}
    
    static string GetBuildName()
    {
        return GetArg(BUILD_NAME);
    }

    
	static string GetKeystoreName()
	{
		return GetArg(BUILD_KEYSTORE_NAME);
	}
    
	static string GetKeystorePassword()
	{
		return GetArg(BUILD_KEYSTORE_PASSWORD);
	}
    
	static string GetKeyaliasName()
	{
		return GetArg(BUILD_KEYALIAS_NAME);
	}
    
	static string GetKeyaliasPassword()
	{
		return GetArg(BUILD_KEYALIAS_PASSWORD);
	}
    
    static string GetBuildFormat()
    {
        string result = GetArg(BUILD_FORMAT);
		return result != null ? result : APK;
    }
    
    static string GetCustomVersion()
    {
        return GetArg(CUSTOM_VERSION);
    }

	static string[] GetBuildScenes()
	{
		List<string> names = new List<string>();
		
		foreach(EditorBuildSettingsScene e in EditorBuildSettings.scenes)
		{
			if(e==null)
				continue;
			
			if(e.enabled)
				names.Add(e.path);
		}
		return names.ToArray();
	}
	
	static void iOS()
	{
		Debug.Log("Command line build\n------------------\n------------------");
		
		string[] scenes = GetBuildScenes();
		string path = GetBuildLocation(BuildTarget.iOS);
		if(scenes == null || scenes.Length==0 || path == null)
			return;
		
		Debug.Log(string.Format("Path: \"{0}\"", path));
		for(int i=0; i<scenes.Length; ++i)
		{
			Debug.Log(string.Format("Scene[{0}]: \"{1}\"", i, scenes[i]));
		}

        string finalPath = path + GetBuildName() + "/";
		Debug.Log(string.Format("Creating Directory \"{0}\" if it does not exist", finalPath));
		(new FileInfo(finalPath)).Directory.Create();

		Debug.Log(string.Format("Switching Build Target to {0}", "iOS"));
		EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTarget.iOS);

		Debug.Log("Starting Build!");
		BuildPipeline.BuildPlayer(scenes, finalPath, BuildTarget.iOS, BuildOptions.None);
	}
	
	static void Android()
	{
		Debug.Log("Command line build android version\n------------------\n------------------");
		
		string[] scenes = GetBuildScenes();
		string path = GetBuildLocation(BuildTarget.Android);
		if(scenes == null || scenes.Length==0 || path == null)
			return;
		
		Debug.Log(string.Format("Path: \"{0}\"", path));
		for(int i=0; i<scenes.Length; ++i)
		{
			Debug.Log(string.Format("Scene[{0}]: \"{1}\"", i, scenes[i]));
		}

		PlayerSettings.Android.keystoreName = GetKeystoreName();
		PlayerSettings.Android.keystorePass = GetKeystorePassword();
        
		PlayerSettings.Android.keyaliasName = GetKeyaliasName();
		PlayerSettings.Android.keyaliasPass = GetKeyaliasPassword();

		Debug.Log(string.Format("Creating Directory \"{0}\" if it does not exist", path));
		(new FileInfo(path)).Directory.Create();

		Debug.Log(string.Format("Switching Build Target to {0}", "Android"));
		EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTarget.Android);
        
        EditorUserBuildSettings.buildAppBundle = (GetBuildFormat() == AAB);
        
		Debug.Log("Starting Android Build!");
		BuildPipeline.BuildPlayer(scenes, path + GetBuildName() + "." + GetBuildFormat(), BuildTarget.Android, BuildOptions.None);
	}
    
    static void SetVersion()
    {
        string newVersion;
        int newVersionCode;
        
        string customVersion = GetCustomVersion();
        
        bool incrementMajor = (customVersion == INCREMENT_MAJOR);
        bool incrementMinor = (customVersion == INCREMENT_MINOR);
        bool incrementPatch = (customVersion == INCREMENT_PATCH);
        
        int versionMajor;
        int versionMinor;
        int versionPatch;
        
        if (incrementMajor || incrementMinor || incrementPatch)
        {
            string currentVersion = PlayerSettings.bundleVersion;
            string[] currentVersionParts = currentVersion.Split('.');
            
            if (currentVersionParts.Length < 3)
            {
                Debug.Log("Unsupported version increment format!");
                return;
            }
            
            versionMajor = int.Parse(currentVersionParts[0]);
            versionMinor = int.Parse(currentVersionParts[1]);
            versionPatch = int.Parse(currentVersionParts[2]);

            if (incrementMajor)
            {
                versionMajor++;
                versionMinor = 0;
                versionPatch = 0;
            }
            else if (incrementMinor)
            {
                versionMinor++;
                versionPatch = 0;
            }
            else if (incrementPatch)
            {
                versionPatch++;
            }
        }
        else
        {
            string[] customVersionParts = customVersion.Split('.');
            
            if (customVersionParts.Length < 3)
            {
                Debug.Log("Unsupported version format!");
                return;
            }
            
            versionMajor = int.Parse(customVersionParts[0]);
            versionMinor = int.Parse(customVersionParts[1]);
            versionPatch = int.Parse(customVersionParts[2]);
        }
        
        newVersion = versionMajor.ToString() + "." + versionMinor.ToString() + "." + versionPatch.ToString();
        newVersionCode = versionMajor * 10000 + versionMinor * 100 + versionPatch;
        
        PlayerSettings.bundleVersion             = newVersion;
        PlayerSettings.Android.bundleVersionCode = newVersionCode;
        PlayerSettings.iOS.buildNumber           = newVersionCode.ToString();

        AssetDatabase.SaveAssets();
    }
}
#endif