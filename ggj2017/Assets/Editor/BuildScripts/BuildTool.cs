// C# example.
using System;
using System.IO;
using UnityEditor;
using UnityEngine;

public class BuildTool {
    private static string UNITY_BUILDS_FOLDER = "Builds";
    private static string BUILD_TYPE_WIN64 = "Win64";
    private static string BUILD_TYPE_WIN32 = "Win32";

    private static string EXECUTABLE_NAME = "GGJ2017";

    private static string BUILD_SCENE_NAME = "ggj2017/Assets/Editor/BuildScripts/Scenes/ggj2017.unity";

    #region Utilities
    private static void CreateFolder(string buildType) {
        string currentPath = Path.GetDirectoryName(Application.dataPath);
        string unityBuildsFolder = Path.Combine(currentPath, UNITY_BUILDS_FOLDER);
        //Create Unity Build folder
        if (!Directory.Exists(unityBuildsFolder))
            Directory.CreateDirectory(unityBuildsFolder);

        string buildTypeFolder = Path.Combine(unityBuildsFolder, buildType);
        //Create build type folder
        if (!Directory.Exists(buildTypeFolder))
            Directory.CreateDirectory(buildTypeFolder);

    }

    private static void SetPlayerSettingsRelease() {
        PlayerSettings.apiCompatibilityLevel = ApiCompatibilityLevel.NET_2_0;
    }

    #endregion
    #region Windows Builds

    [MenuItem("Builds/Win64")]
    public static void ProductionBuildForWin64() {
        SetPlayerSettingsRelease();
        BuildForWin64();
    }

    public static void BuildForWin64(string version = "") {
        Debug.Log("Building...");

        CreateFolder(BUILD_TYPE_WIN64);
        Debug.Log("Building...");
        //Get current path.
        string currentPath = Path.GetDirectoryName(Application.dataPath);

        // Get filename.
        string path = Path.Combine(Path.Combine(currentPath, UNITY_BUILDS_FOLDER), BUILD_TYPE_WIN64);
        string[] levels = new string[] { BUILD_SCENE_NAME };

        // Build player.
        BuildPipeline.BuildPlayer(levels, Path.Combine(path, EXECUTABLE_NAME) + version + ".exe", BuildTarget.StandaloneWindows, BuildOptions.None);
    }


    [MenuItem("Builds/Win32")]
    public static void ProductionBuildForWin32() {
        SetPlayerSettingsRelease();
        BuildForWin32();
    }

    public static void BuildForWin32(string version = "") {
        CreateFolder(BUILD_TYPE_WIN32);
        //Get current path.
        string currentPath = Path.GetDirectoryName(Application.dataPath);

        // Get filename.
        string path = Path.Combine(Path.Combine(currentPath, UNITY_BUILDS_FOLDER), BUILD_TYPE_WIN32);
        string[] levels = new string[] { BUILD_SCENE_NAME };

        // Build player.
        BuildPipeline.BuildPlayer(levels, Path.Combine(path, EXECUTABLE_NAME) + version + ".exe", BuildTarget.StandaloneWindows, BuildOptions.None);
    }

    #endregion

}