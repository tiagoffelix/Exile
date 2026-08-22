using UnityEditor;

/// <summary>
/// Editor-only switch that spawns the Android on-screen controls during Play
/// Mode so they can be checked in the Device Simulator without deploying an
/// APK. It has no effect on player builds.
/// </summary>
public static class TouchControlsPreview
{
    private const string MenuPath = "Build/Preview touch controls in Play Mode";

    [MenuItem(MenuPath)]
    private static void Toggle()
    {
        var enabled = !EditorPrefs.GetBool(TouchControlsBootstrap.PreviewPrefKey, false);
        EditorPrefs.SetBool(TouchControlsBootstrap.PreviewPrefKey, enabled);
    }

    [MenuItem(MenuPath, true)]
    private static bool ToggleValidate()
    {
        Menu.SetChecked(MenuPath, EditorPrefs.GetBool(TouchControlsBootstrap.PreviewPrefKey, false));
        return true;
    }
}
