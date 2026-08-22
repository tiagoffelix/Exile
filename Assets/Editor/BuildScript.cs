using System;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEngine;

/// <summary>
/// Reproducible builds for Exile, driven from the Build menu or from the
/// command line:
///
/// "Unity.exe" -quit -batchmode -nographics -logFile - ^
///   -projectPath "&lt;repo&gt;\Exile" -buildTarget Android ^
///   -executeMethod BuildScript.BuildAndroid
///
/// The Android entry point sets the mobile-specific player settings here rather
/// than leaving them saved in ProjectSettings, so simply opening the project
/// does not quietly change how the Windows build is produced.
/// </summary>
public static class BuildScript
{
    private const string AndroidOutput = "Builds/Android/Exile.apk";
    private const string WindowsOutput = "Builds/Windows/Exile.exe";
    private const string BundleIdentifier = "com.tiagofelix.exile";

    private static string[] Scenes
    {
        get { return EditorBuildSettings.scenes.Where(scene => scene.enabled).Select(scene => scene.path).ToArray(); }
    }

    /// <summary>Builds the APK published as a direct download on itch.io.</summary>
    [MenuItem("Build/Exile Android APK")]
    public static void BuildAndroid()
    {
        // An APK, not an app bundle: itch.io serves a file people sideload.
        EditorUserBuildSettings.buildAppBundle = false;
        EditorUserBuildSettings.androidBuildSystem = AndroidBuildSystem.Gradle;
        EditorUserBuildSettings.androidCreateSymbols = AndroidCreateSymbols.Disabled;
        EditorUserBuildSettings.development = false;

        PlayerSettings.applicationIdentifier = BundleIdentifier;

        // Both halves of the game are designed around a wide view.
        PlayerSettings.defaultInterfaceOrientation = UIOrientation.LandscapeLeft;
        PlayerSettings.allowedAutorotateToPortrait = false;
        PlayerSettings.allowedAutorotateToPortraitUpsideDown = false;
        PlayerSettings.allowedAutorotateToLandscapeLeft = true;
        PlayerSettings.allowedAutorotateToLandscapeRight = true;
        PlayerSettings.useAnimatedAutorotation = true;

        // ARM64 is the current requirement for modern devices, and it needs
        // IL2CPP; Mono can only produce the 32-bit ARMv7 slice.
        PlayerSettings.SetScriptingBackend(BuildTargetGroup.Android, ScriptingImplementation.IL2CPP);
        PlayerSettings.Android.targetArchitectures = AndroidArchitecture.ARM64;
        PlayerSettings.Android.minSdkVersion = AndroidSdkVersions.AndroidApiLevel23;
        PlayerSettings.Android.targetSdkVersion = AndroidSdkVersions.AndroidApiLevelAuto;
        PlayerSettings.Android.startInFullscreen = true;
        PlayerSettings.Android.renderOutsideSafeArea = false;
        PlayerSettings.SetManagedStrippingLevel(BuildTargetGroup.Android, ManagedStrippingLevel.Low);

        // No keystore is configured or committed. Unity signs with the local
        // debug key, which installs fine from itch.io but is not a store build.
        PlayerSettings.Android.useCustomKeystore = false;

        Run(BuildTarget.Android, BuildTargetGroup.Android, AndroidOutput);
    }

    /// <summary>Builds the Windows version.</summary>
    [MenuItem("Build/Exile Windows")]
    public static void BuildWindows()
    {
        Run(BuildTarget.StandaloneWindows64, BuildTargetGroup.Standalone, WindowsOutput);
    }

    private static void Run(BuildTarget target, BuildTargetGroup group, string relativeOutput)
    {
        string[] scenes = Scenes;
        if (scenes.Length == 0)
        {
            throw new Exception("No scenes are enabled in Build Settings.");
        }

        string projectRoot = Directory.GetParent(Application.dataPath).FullName;
        string output = Path.Combine(projectRoot, relativeOutput.Replace('/', Path.DirectorySeparatorChar));

        string folder = Path.GetDirectoryName(output);
        if (!Directory.Exists(folder))
        {
            Directory.CreateDirectory(folder);
        }

        if (EditorUserBuildSettings.activeBuildTarget != target)
        {
            Debug.Log($"Switching active build target to {target}.");
            EditorUserBuildSettings.SwitchActiveBuildTarget(group, target);
        }

        BuildReport report = BuildPipeline.BuildPlayer(new BuildPlayerOptions
        {
            scenes = scenes,
            locationPathName = output,
            target = target,
            targetGroup = group,
            options = BuildOptions.None
        });

        BuildSummary summary = report.summary;
        Debug.Log($"{target} build {summary.result}: {summary.totalSize / (1024 * 1024)} MB in {summary.totalTime}, at {output}");

        if (summary.result != BuildResult.Succeeded)
        {
            throw new Exception($"{target} build failed with {summary.totalErrors} error(s).");
        }
    }
}
