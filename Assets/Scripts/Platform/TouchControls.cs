using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// Builds and owns the Android on-screen controls.
///
/// Everything here is created in code. Exile's scenes and prefabs are not
/// touched, so the Windows build is bit-for-bit the game it always was and
/// there is no mobile-only prefab to keep in sync when the scenes change.
///
/// Two layouts, matching the two halves of the game:
/// Day3D  - move, look, attack, interact, shop, place a building, emote, pause.
/// Night2D - move, attack, block.
/// </summary>
public class TouchControls : MonoBehaviour
{
    /// <summary>Which set of controls to show.</summary>
    public enum Layout
    {
        /// <summary>Daytime exploration, gathering and building.</summary>
        Day3D,

        /// <summary>Nighttime side-on combat.</summary>
        Night2D
    }

    private const float ReferenceWidth = 1920f;
    private const float ReferenceHeight = 1080f;
    private const float JoystickRadius = 150f;

    private static readonly Color PanelIdle = new Color(1f, 1f, 1f, 0.30f);
    private static readonly Color PanelHeld = new Color(1f, 0.85f, 0.45f, 0.65f);
    private static readonly Color StickRing = new Color(1f, 1f, 1f, 0.22f);
    private static readonly Color StickKnob = new Color(1f, 1f, 1f, 0.55f);

    private RectTransform safeArea;
    private GameObject gameplayGroup;
    private TouchJoystick joystick;
    private TouchLookArea lookArea;

    private TouchButton attackButton;
    private TouchButton blockButton;
    private TouchButton interactButton;
    private TouchButton shopButton;
    private TouchButton emoteButton;
    private TouchButton placeButton;
    private TouchButton pauseButton;

    private Rect appliedSafeArea;
    private float placeCheckTimer;
    private Font labelFont;
    private static Sprite discSprite;

    /// <summary>Movement stick value, -1..1 per axis.</summary>
    public Vector2 Move { get { return joystick == null ? Vector2.zero : joystick.Value; } }

    /// <summary>Look delta for this frame, in mouse-axis units.</summary>
    public Vector2 Look { get { return lookArea == null ? Vector2.zero : lookArea.Delta; } }

    /// <summary>True while the block button is held.</summary>
    public bool BlockHeld { get { return blockButton != null && blockButton.Held; } }

    /// <summary>Takes a queued attack press, if any.</summary>
    public bool ConsumeAttack() { return attackButton != null && attackButton.ConsumePress(); }

    /// <summary>Takes a queued interact press, if any.</summary>
    public bool ConsumeInteract() { return interactButton != null && interactButton.ConsumePress(); }

    /// <summary>Takes a queued shop press, if any.</summary>
    public bool ConsumeShop() { return shopButton != null && shopButton.ConsumePress(); }

    /// <summary>Takes a queued emote press, if any.</summary>
    public bool ConsumeEmote() { return emoteButton != null && emoteButton.ConsumePress(); }

    /// <summary>Takes a queued building-placement press, if any.</summary>
    public bool ConsumePlace() { return placeButton != null && placeButton.ConsumePress(); }

    /// <summary>Takes a queued pause press, if any.</summary>
    public bool ConsumePause() { return pauseButton != null && pauseButton.ConsumePress(); }

    /// <summary>Creates the control set for a scene.</summary>
    public static TouchControls Create(Layout layout)
    {
        GameObject root = new GameObject("TouchControls");
        TouchControls controls = root.AddComponent<TouchControls>();
        controls.Build(layout);
        return controls;
    }

    private void OnEnable()
    {
        GameInput.Register(this);
    }

    private void OnDisable()
    {
        GameInput.Unregister(this);
    }

    private void Update()
    {
        ApplySafeArea();

        // The shop and both pause menus stop the clock. Hiding the gameplay
        // controls then keeps them from covering those menus, while the pause
        // button stays up so the player can always get back out.
        bool paused = Time.timeScale <= 0f;
        if (gameplayGroup != null && gameplayGroup.activeSelf == paused)
        {
            gameplayGroup.SetActive(!paused);
        }

        UpdatePlaceButton();
    }

    /// <summary>
    /// The Place button only makes sense while a building blueprint is in the
    /// scene waiting to be positioned, so it follows that object in and out.
    /// </summary>
    private void UpdatePlaceButton()
    {
        if (placeButton == null)
        {
            return;
        }

        placeCheckTimer -= Time.unscaledDeltaTime;
        if (placeCheckTimer > 0f)
        {
            return;
        }

        placeCheckTimer = 0.2f;

        bool placing = FindObjectOfType<Blueprint>() != null;
        if (placeButton.gameObject.activeSelf != placing)
        {
            placeButton.gameObject.SetActive(placing);
        }
    }

    private void Build(Layout layout)
    {
        labelFont = ResolveFont();
        EnsureEventSystem();

        Canvas canvas = gameObject.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        // Above the HUD but below nothing else the game puts on screen; the
        // group is hidden outright whenever a menu takes over.
        canvas.sortingOrder = 100;

        CanvasScaler scaler = gameObject.AddComponent<CanvasScaler>();
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = new Vector2(ReferenceWidth, ReferenceHeight);
        scaler.screenMatchMode = CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;
        scaler.matchWidthOrHeight = 0.5f;

        gameObject.AddComponent<GraphicRaycaster>();

        safeArea = CreateRect("SafeArea", transform);
        Stretch(safeArea);
        ApplySafeArea();

        gameplayGroup = CreateRect("Gameplay", safeArea).gameObject;
        Stretch((RectTransform)gameplayGroup.transform);

        BuildLookArea();
        BuildJoystick();
        BuildButtons(layout);
    }

    private void BuildLookArea()
    {
        RectTransform look = CreateRect("LookArea", gameplayGroup.transform);
        look.anchorMin = new Vector2(0.42f, 0f);
        look.anchorMax = Vector2.one;
        look.offsetMin = Vector2.zero;
        look.offsetMax = Vector2.zero;

        AddInvisibleRaycastTarget(look);
        lookArea = look.gameObject.AddComponent<TouchLookArea>();
    }

    private void BuildJoystick()
    {
        RectTransform zone = CreateRect("MoveZone", gameplayGroup.transform);
        zone.anchorMin = Vector2.zero;
        zone.anchorMax = new Vector2(0.42f, 1f);
        zone.offsetMin = Vector2.zero;
        zone.offsetMax = Vector2.zero;

        AddInvisibleRaycastTarget(zone);

        RectTransform ring = CreateRect("Ring", zone);
        ring.sizeDelta = new Vector2(JoystickRadius * 2f, JoystickRadius * 2f);
        ring.anchorMin = new Vector2(0.5f, 0.5f);
        ring.anchorMax = new Vector2(0.5f, 0.5f);
        AddImage(ring, StickRing, false);

        RectTransform knob = CreateRect("Knob", ring);
        knob.sizeDelta = new Vector2(JoystickRadius, JoystickRadius);
        knob.anchorMin = new Vector2(0.5f, 0.5f);
        knob.anchorMax = new Vector2(0.5f, 0.5f);
        AddImage(knob, StickKnob, false);

        joystick = zone.gameObject.AddComponent<TouchJoystick>();
        joystick.Bind(zone, ring, knob, JoystickRadius);
    }

    private void BuildButtons(Layout layout)
    {
        // Pause sits outside the gameplay group so it survives a paused clock.
        pauseButton = CreateButton(safeArea, "Pause", "II", new Vector2(1f, 1f), new Vector2(-100f, -100f), 110f);

        if (layout == Layout.Day3D)
        {
            attackButton = CreateButton(gameplayGroup.transform, "Attack", "Hit", new Vector2(1f, 0f), new Vector2(-150f, 160f), 200f);
            interactButton = CreateButton(gameplayGroup.transform, "Interact", "Use", new Vector2(1f, 0f), new Vector2(-350f, 165f), 160f);
            shopButton = CreateButton(gameplayGroup.transform, "Shop", "Shop", new Vector2(1f, 0f), new Vector2(-160f, 370f), 160f);
            emoteButton = CreateButton(gameplayGroup.transform, "Emote", "Emote", new Vector2(1f, 0f), new Vector2(-350f, 370f), 130f);
            placeButton = CreateButton(gameplayGroup.transform, "Place", "Place", new Vector2(1f, 0f), new Vector2(-530f, 170f), 160f);
            placeButton.gameObject.SetActive(false);
        }
        else
        {
            attackButton = CreateButton(gameplayGroup.transform, "Attack", "Hit", new Vector2(1f, 0f), new Vector2(-150f, 160f), 200f);
            blockButton = CreateButton(gameplayGroup.transform, "Block", "Block", new Vector2(1f, 0f), new Vector2(-360f, 170f), 170f);
        }
    }

    private TouchButton CreateButton(Transform parent, string name, string label, Vector2 anchor, Vector2 position, float size)
    {
        RectTransform rect = CreateRect(name, parent);
        rect.anchorMin = anchor;
        rect.anchorMax = anchor;
        rect.pivot = new Vector2(0.5f, 0.5f);
        rect.anchoredPosition = position;
        rect.sizeDelta = new Vector2(size, size);

        Image image = AddImage(rect, PanelIdle, true);

        RectTransform textRect = CreateRect("Label", rect);
        Stretch(textRect);

        Text text = textRect.gameObject.AddComponent<Text>();
        text.font = labelFont;
        text.text = label;
        text.alignment = TextAnchor.MiddleCenter;
        text.color = new Color(0.06f, 0.05f, 0.04f, 0.9f);
        text.fontSize = Mathf.RoundToInt(size * 0.26f);
        text.raycastTarget = false;
        text.horizontalOverflow = HorizontalWrapMode.Overflow;
        text.verticalOverflow = VerticalWrapMode.Overflow;

        TouchButton button = rect.gameObject.AddComponent<TouchButton>();
        button.SetColours(image, PanelIdle, PanelHeld);
        return button;
    }

    private void ApplySafeArea()
    {
        if (safeArea == null || Screen.width <= 0 || Screen.height <= 0)
        {
            return;
        }

        Rect area = Screen.safeArea;
        if (area == appliedSafeArea)
        {
            return;
        }

        appliedSafeArea = area;

        Vector2 min = new Vector2(area.xMin / Screen.width, area.yMin / Screen.height);
        Vector2 max = new Vector2(area.xMax / Screen.width, area.yMax / Screen.height);

        safeArea.anchorMin = min;
        safeArea.anchorMax = max;
        safeArea.offsetMin = Vector2.zero;
        safeArea.offsetMax = Vector2.zero;
    }

    private static void EnsureEventSystem()
    {
        if (EventSystem.current != null)
        {
            return;
        }

        GameObject events = new GameObject("EventSystem");
        events.AddComponent<EventSystem>();
        events.AddComponent<StandaloneInputModule>();
    }

    private static RectTransform CreateRect(string name, Transform parent)
    {
        GameObject created = new GameObject(name, typeof(RectTransform));
        created.transform.SetParent(parent, false);
        return (RectTransform)created.transform;
    }

    private static void Stretch(RectTransform rect)
    {
        rect.anchorMin = Vector2.zero;
        rect.anchorMax = Vector2.one;
        rect.offsetMin = Vector2.zero;
        rect.offsetMax = Vector2.zero;
    }

    private static Image AddImage(RectTransform rect, Color colour, bool raycastTarget)
    {
        Image image = rect.gameObject.AddComponent<Image>();
        image.sprite = Disc;
        image.type = Image.Type.Simple;
        image.color = colour;
        image.raycastTarget = raycastTarget;
        return image;
    }

    /// <summary>
    /// A soft-edged white disc generated once at runtime. Unity's built-in
    /// UI skin sprites live in the editor's resources and are not dependable in
    /// a player build, so the controls draw their own.
    /// </summary>
    private static Sprite Disc
    {
        get
        {
            if (discSprite != null)
            {
                return discSprite;
            }

            const int size = 128;
            const float radius = size * 0.5f;

            Texture2D texture = new Texture2D(size, size, TextureFormat.RGBA32, false)
            {
                wrapMode = TextureWrapMode.Clamp,
                filterMode = FilterMode.Bilinear,
                hideFlags = HideFlags.HideAndDontSave
            };

            Color[] pixels = new Color[size * size];
            for (int y = 0; y < size; y++)
            {
                for (int x = 0; x < size; x++)
                {
                    float distance = Vector2.Distance(new Vector2(x + 0.5f, y + 0.5f), new Vector2(radius, radius));
                    // One pixel of feathering so the edge is not stair-stepped.
                    float alpha = Mathf.Clamp01(radius - distance);
                    pixels[(y * size) + x] = new Color(1f, 1f, 1f, alpha);
                }
            }

            texture.SetPixels(pixels);
            texture.Apply(false, true);

            discSprite = Sprite.Create(texture, new Rect(0f, 0f, size, size), new Vector2(0.5f, 0.5f), 100f, 0u, SpriteMeshType.FullRect);
            discSprite.hideFlags = HideFlags.HideAndDontSave;
            return discSprite;
        }
    }

    private static void AddInvisibleRaycastTarget(RectTransform rect)
    {
        // A fully transparent Image still receives touches, which is exactly
        // what an input zone needs: it catches drags without drawing anything.
        Image image = rect.gameObject.AddComponent<Image>();
        image.color = new Color(0f, 0f, 0f, 0f);
        image.raycastTarget = true;
    }

    private static Font ResolveFont()
    {
        // Arial is the runtime built-in in 2021.3; newer editors renamed it.
        Font font = Resources.GetBuiltinResource<Font>("Arial.ttf");

        if (font == null)
        {
            font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        }

        if (font == null)
        {
            // Android ships Roboto; naming it keeps the label legible there.
            font = Font.CreateDynamicFontFromOSFont("Roboto", 32);
        }

        return font;
    }
}
