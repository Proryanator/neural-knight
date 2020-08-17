using System.Collections.Generic;
using System.IO;
using UnityEditor;

public class BuildProject{

	// define the scenes to build for this project
	private const string SCENE_PREFIX = "Assets/Scenes/";
	private static string[] sceneNames = { "first-map" };

	static void PerformBuild (){
		var args = FindArgs();
		
		// let's create some build options!
		BuildPlayerOptions options = new BuildPlayerOptions();
		options.scenes = GetScenePaths(SCENE_PREFIX, sceneNames);
		
		string fullPathAndName = AdjustToSystemPath(args.targetDir + args.appName);	

		System.Console.WriteLine("Output directory will be: [" + fullPathAndName + "]");
		
		// note: can we make this dynamic or build it to multiple platforms?
		options.target = BuildTarget.StandaloneWindows64;
		options.targetGroup = BuildTargetGroup.Standalone;
		
		// will this create that folder if it doesn't exist?
		options.locationPathName = fullPathAndName;

		BuildPipeline.BuildPlayer(options);
	}

	/// <summary>
	/// Takes an array of scene names, and a prefix, and returns you a properly formatted
	/// array of scenes with prefixes added.
	/// </summary>
	/// <param name="sceneNames"></param>
	/// <returns></returns>
	private static string[] GetScenePaths(string scenePrefix, string[] sceneNames){
		List<string> list = new List<string>();
		
		foreach (string scene in sceneNames){
			list.Add(AdjustToSystemPath(scene));
		}

		return list.ToArray();
	}
	
	/// <summary>
	/// Replaces initial directory slash with system appropriate one.
	/// </summary>
	private static string AdjustToSystemPath(string filepath){
		return filepath.Replace(Path.DirectorySeparatorChar, '/');
	}
	
	private static Args FindArgs()
	{
		var returnValue = new Args();
 
		// find: -executeMethod
		//   +1: JenkinsBuild.BuildMacOS
		//   +2: FindTheGnome
		//   +3: D:\Jenkins\Builds\Find the Gnome\47\output
		string[] args = System.Environment.GetCommandLineArgs();
		var execMethodArgPos = -1;
		bool allArgsFound = false;
		for (int i = 0; i < args.Length; i++)
		{
			if (args[i] == "-executeMethod")
			{
				execMethodArgPos = i;
			}
			var realPos = execMethodArgPos == -1 ? -1 : i - execMethodArgPos - 2;
			if (realPos < 0)
				continue;
 
			if (realPos == 0)
				returnValue.appName = args[i];
			if (realPos == 1)
			{
				returnValue.targetDir = args[i];
				if (!returnValue.targetDir.EndsWith(System.IO.Path.DirectorySeparatorChar + ""))
					returnValue.targetDir += System.IO.Path.DirectorySeparatorChar;
 
				allArgsFound = true;
			}
		}
 
		if (!allArgsFound)
			System.Console.WriteLine("[JenkinsBuild] Incorrect Parameters for -executeMethod Format: -executeMethod JenkinsBuild.BuildWindows64 <app name> <output dir>");
 
		return returnValue;
	}
	
	private class Args
	{
		public string appName = "AppName";
		public string targetDir = "~/Desktop";
	}
}