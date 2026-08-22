using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// Drag anywhere in this area to turn the camera. Produces a per-frame delta
/// scaled to match what Input.GetAxis("Mouse X"/"Mouse Y") reports, so the
/// existing sensitivity setting means roughly the same thing on both platforms.
/// Action buttons sit above this area in the hierarchy and swallow their own
/// touches, so tapping a button never drags the camera.
/// </summary>
public class TouchLookArea : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    /// <summary>Mouse-delta equivalent per pixel dragged.</summary>
    private const float PixelsToAxis = 0.1f;

    private int activePointer = -1;
    private Vector2 accumulated;

    /// <summary>Look delta for this frame, cleared at the end of every frame.</summary>
    public Vector2 Delta { get; private set; }

    /// <inheritdoc/>
    public void OnPointerDown(PointerEventData eventData)
    {
        if (activePointer == -1)
        {
            activePointer = eventData.pointerId;
        }
    }

    /// <inheritdoc/>
    public void OnDrag(PointerEventData eventData)
    {
        if (eventData.pointerId != activePointer)
        {
            return;
        }

        accumulated += eventData.delta * PixelsToAxis;
    }

    /// <inheritdoc/>
    public void OnPointerUp(PointerEventData eventData)
    {
        if (eventData.pointerId == activePointer)
        {
            activePointer = -1;
        }
    }

    private void Update()
    {
        // Published one frame at a time: the event system and gameplay scripts
        // both run in Update with no guaranteed order, so the value has to
        // survive the whole frame and be cleared afterwards.
        Delta = accumulated;
        accumulated = Vector2.zero;
    }

    private void OnDisable()
    {
        activePointer = -1;
        accumulated = Vector2.zero;
        Delta = Vector2.zero;
    }
}
