using UnityEngine;

/// <summary>
/// One place every gameplay script asks for input, so the same code drives
/// keyboard and mouse on Windows and on-screen controls on Android.
///
/// When no touch layer is registered (every desktop build) each property
/// forwards straight to the original UnityEngine.Input call the script used
/// before, so desktop behaviour is unchanged.
///
/// When a <see cref="TouchControls"/> instance is registered the desktop reads
/// are skipped entirely. That matters on Android: Unity's legacy Input reports
/// a screen tap as mouse button 0, so leaving the mouse reads live would make
/// every joystick drag and every button tap also swing the sword.
/// </summary>
public static class GameInput
{
    private static TouchControls touch;

    /// <summary>True while on-screen controls are driving input.</summary>
    public static bool UsingTouch { get { return touch != null; } }

    internal static void Register(TouchControls controls)
    {
        touch = controls;
    }

    internal static void Unregister(TouchControls controls)
    {
        if (touch == controls)
        {
            touch = null;
        }
    }

    // ----- movement -------------------------------------------------------

    /// <summary>Smoothed strafe axis, as Input.GetAxis("Horizontal").</summary>
    public static float Horizontal
    {
        get { return touch == null ? Input.GetAxis("Horizontal") : touch.Move.x; }
    }

    /// <summary>Smoothed forward axis, as Input.GetAxis("Vertical").</summary>
    public static float Vertical
    {
        get { return touch == null ? Input.GetAxis("Vertical") : touch.Move.y; }
    }

    /// <summary>Unsmoothed strafe axis, as Input.GetAxisRaw("Horizontal").</summary>
    public static float HorizontalRaw
    {
        get { return touch == null ? Input.GetAxisRaw("Horizontal") : touch.Move.x; }
    }

    /// <summary>Unsmoothed forward axis, as Input.GetAxisRaw("Vertical").</summary>
    public static float VerticalRaw
    {
        get { return touch == null ? Input.GetAxisRaw("Vertical") : touch.Move.y; }
    }

    // ----- looking --------------------------------------------------------

    /// <summary>Horizontal look delta, as Input.GetAxis("Mouse X").</summary>
    public static float LookX
    {
        get { return touch == null ? Input.GetAxis("Mouse X") : touch.Look.x; }
    }

    /// <summary>Vertical look delta, as Input.GetAxis("Mouse Y").</summary>
    public static float LookY
    {
        get { return touch == null ? Input.GetAxis("Mouse Y") : touch.Look.y; }
    }

    /// <summary>
    /// The "hold to stop the camera" input, right mouse button on desktop.
    /// Touch has no equivalent and does not need one: the camera only turns
    /// while a finger is dragging the look area, so it is already still.
    /// </summary>
    public static bool FreeLookHoldPressed
    {
        get { return touch == null && Input.GetMouseButtonDown(1); }
    }

    /// <inheritdoc cref="FreeLookHoldPressed"/>
    public static bool FreeLookHoldReleased
    {
        get { return touch == null && Input.GetMouseButtonUp(1); }
    }

    /// <inheritdoc cref="FreeLookHoldPressed"/>
    public static bool FreeLookHoldHeld
    {
        get { return touch == null && Input.GetMouseButton(1); }
    }

    // ----- actions --------------------------------------------------------

    /// <summary>Attack, left mouse button on desktop.</summary>
    public static bool AttackPressed
    {
        get { return touch == null ? Input.GetMouseButtonDown(0) : touch.ConsumeAttack(); }
    }

    /// <summary>Block, held right mouse button on desktop (2D night combat).</summary>
    public static bool BlockHeld
    {
        get { return touch == null ? Input.GetMouseButton(1) : touch.BlockHeld; }
    }

    /// <summary>Interact, E on desktop.</summary>
    public static bool InteractPressed
    {
        get { return touch == null ? Input.GetKeyDown(KeyCode.E) : touch.ConsumeInteract(); }
    }

    /// <summary>Open or close the shop, R on desktop.</summary>
    public static bool ShopPressed
    {
        get { return touch == null ? Input.GetKeyDown(KeyCode.R) : touch.ConsumeShop(); }
    }

    /// <summary>The dance easter egg, T on desktop.</summary>
    public static bool EmotePressed
    {
        get { return touch == null ? Input.GetKeyDown(KeyCode.T) : touch.ConsumeEmote(); }
    }

    /// <summary>Place the building currently being positioned.</summary>
    public static bool PlacePressed
    {
        get { return touch == null ? Input.GetMouseButtonDown(0) : touch.ConsumePlace(); }
    }

    /// <summary>
    /// Pause. Escape on desktop; on Android Unity also reports the hardware
    /// back button as Escape, so the back button pauses without extra work.
    /// </summary>
    public static bool PausePressed
    {
        get { return Input.GetKeyDown(KeyCode.Escape) || (touch != null && touch.ConsumePause()); }
    }

    // ----- pointer --------------------------------------------------------

    /// <summary>
    /// Where building placement aims. The mouse on desktop; the centre of the
    /// screen on touch, so the player aims by turning the camera and commits
    /// with the Place button instead of needing a hovering cursor.
    /// </summary>
    public static Vector3 PointerPosition
    {
        get
        {
            return touch == null
                ? Input.mousePosition
                : new Vector3(Screen.width * 0.5f, Screen.height * 0.5f, 0f);
        }
    }
}
