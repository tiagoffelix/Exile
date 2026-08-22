using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// An on-screen action button. Reacts on finger down rather than on release,
/// which is what an attack or an interact needs to feel responsive, and hands
/// each press to exactly one reader so a single tap cannot fire twice.
/// </summary>
public class TouchButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    private bool pressQueued;
    private Image background;
    private Color idleColour;
    private Color heldColour;

    /// <summary>True for as long as the finger stays on the button.</summary>
    public bool Held { get; private set; }

    /// <summary>Colours used for the idle and pressed states.</summary>
    public void SetColours(Image image, Color idle, Color held)
    {
        background = image;
        idleColour = idle;
        heldColour = held;
        background.color = idleColour;
    }

    /// <summary>
    /// Returns true once per press. The caller takes the press with it, so the
    /// same tap is never delivered to a second reader or a second frame.
    /// </summary>
    public bool ConsumePress()
    {
        if (!pressQueued)
        {
            return false;
        }

        pressQueued = false;
        return true;
    }

    /// <inheritdoc/>
    public void OnPointerDown(PointerEventData eventData)
    {
        pressQueued = true;
        Held = true;

        if (background != null)
        {
            background.color = heldColour;
        }
    }

    /// <inheritdoc/>
    public void OnPointerUp(PointerEventData eventData)
    {
        Held = false;

        if (background != null)
        {
            background.color = idleColour;
        }
    }

    private void OnDisable()
    {
        pressQueued = false;
        Held = false;
    }
}
