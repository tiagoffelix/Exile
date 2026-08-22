using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Spawns the on-screen controls on mobile, once per gameplay scene, without
/// any scene needing a reference to them. Desktop builds never reach the spawn,
/// so nothing about the Windows version changes.
/// </summary>
public static class TouchControlsBootstrap
{
    private const string Day3DScene = "3D";
    private const string Night2DScene = "2D";

    /// <summary>
    /// True on a real handheld build. In the editor it is false unless the
    /// "Build/Preview touch controls in Play Mode" toggle is on, which lets the
    /// overlay be checked in the Device Simulator. The editor branch compiles
    /// out of player builds entirely, so shipped behaviour is unchanged.
    /// </summary>
    private static bool UseTouchControls
    {
        get
        {
#if UNITY_EDITOR
            return Application.isMobilePlatform
                || UnityEditor.EditorPrefs.GetBool(PreviewPrefKey, false);
#else
            return Application.isMobilePlatform;
#endif
        }
    }

#if UNITY_EDITOR
    public const string PreviewPrefKey = "Exile.PreviewTouchControls";
#endif

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    private static void Install()
    {
        if (!UseTouchControls)
        {
            return;
        }

        // Landscape only: both halves of the game are built around a wide view.
        Screen.orientation = ScreenOrientation.AutoRotation;
        Screen.autorotateToLandscapeLeft = true;
        Screen.autorotateToLandscapeRight = true;
        Screen.autorotateToPortrait = false;
        Screen.autorotateToPortraitUpsideDown = false;

        SceneManager.sceneLoaded += OnSceneLoaded;
        Spawn(SceneManager.GetActiveScene());
    }

    private static void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (mode == LoadSceneMode.Single)
        {
            Spawn(scene);
        }
    }

    private static void Spawn(Scene scene)
    {
        if (scene.name == Day3DScene)
        {
            TouchControls.Create(TouchControls.Layout.Day3D);
        }
        else if (scene.name == Night2DScene)
        {
            TouchControls.Create(TouchControls.Layout.Night2D);
        }
    }
}
