using System.Collections.Generic;
using System.IO;
using UnityEditor;

public class BuildProject{

	// define the scenes to build for this project
	private const string SCENE_PREFIX = "Assets/Scenes/";
	private static string[] sceneNames = { "first-map" };

	private const string TARGET_DIR =
		"C:/Windows/System32/config/systemprofile/AppData/Local/Jenkins.jenkins/workspace/Neural Defender/artifacts/";
	
	private const string APP_NAME = "NeuralDefender";
	
	static void PerformBuild (){
		// let's create some build options!
		BuildPlayerOptions options = new BuildPlayerOptions();
		options.scenes = GetScenePaths(SCENE_PREFIX, sceneNames);
		
		string fullPathAndName = AdjustToSystemPath(TARGET_DIR + APP_NAME);	

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
}