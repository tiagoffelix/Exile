using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// A floating movement stick: the ring appears wherever the thumb lands inside
/// the zone rather than at a fixed spot, so the player does not have to look
/// down to find it. Reports a value shaped like Input.GetAxis, in the range
/// -1..1 on each axis.
/// </summary>
public class TouchJoystick : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    private RectTransform zone;
    private RectTransform ring;
    private RectTransform knob;
    private float radius = 140f;
    private int activePointer = -1;
    private Vector2 origin;

    /// <summary>Current stick value, -1..1 per axis.</summary>
    public Vector2 Value { get; private set; }

    /// <summary>Wires up the visuals created by <see cref="TouchControls"/>.</summary>
    public void Bind(RectTransform zoneRect, RectTransform ringRect, RectTransform knobRect, float ringRadius)
    {
        zone = zoneRect;
        ring = ringRect;
        knob = knobRect;
        radius = ringRadius;
        SetVisible(false);
    }

    /// <inheritdoc/>
    public void OnPointerDown(PointerEventData eventData)
    {
        if (activePointer != -1)
        {
            return;
        }

        activePointer = eventData.pointerId;

        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
                zone, eventData.position, eventData.pressEventCamera, out Vector2 local))
        {
            origin = local;
            ring.anchoredPosition = local;
            knob.anchoredPosition = Vector2.zero;
            SetVisible(true);
        }
    }

    /// <inheritdoc/>
    public void OnDrag(PointerEventData eventData)
    {
        if (eventData.pointerId != activePointer)
        {
            return;
        }

        if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(
                zone, eventData.position, eventData.pressEventCamera, out Vector2 local))
        {
            return;
        }

        Vector2 offset = local - origin;
        Vector2 clamped = Vector2.ClampMagnitude(offset, radius);

        knob.anchoredPosition = clamped;
        Value = clamped / radius;
    }

    /// <inheritdoc/>
    public void OnPointerUp(PointerEventData eventData)
    {
        if (eventData.pointerId != activePointer)
        {
            return;
        }

        activePointer = -1;
        Value = Vector2.zero;
        knob.anchoredPosition = Vector2.zero;
        SetVisible(false);
    }

    private void OnDisable()
    {
        activePointer = -1;
        Value = Vector2.zero;
    }

    private void SetVisible(bool visible)
    {
        if (ring != null)
        {
            ring.gameObject.SetActive(visible);
        }
    }
}
