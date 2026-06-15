using UnityEngine;
using UnityEngine.EventSystems;

public class FloatingJoystick : MonoBehaviour, 
    IPointerDownHandler,
    IDragHandler, 
    IPointerUpHandler
{
    [SerializeField] private RectTransform stickBase;
    [SerializeField] private RectTransform stickKnob;
    [SerializeField] private GameObject visualRoot;
    [SerializeField] private float maxRadius = 40f;

    public Vector2 Direction { get; private set; }
    public bool IsPressed { get; private set; }

    private RectTransform touchZoneRect;

    private void Awake()
    {
        touchZoneRect = GetComponent<RectTransform>();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            touchZoneRect, 
            eventData.position, 
            null , 
            out Vector2 localPoint
            );

        stickBase.anchoredPosition = localPoint;

        stickKnob.anchoredPosition = Vector2.zero;

        IsPressed = true;
        
        visualRoot.SetActive( true );

        Direction = Vector2.zero;
    }

    public void OnDrag(PointerEventData eventData)
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            stickBase,
            eventData.position,
            null,
            out Vector2 localPoint
            );

        stickKnob.anchoredPosition = Vector2.ClampMagnitude(localPoint, maxRadius);

        Direction = stickKnob.anchoredPosition / maxRadius;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        IsPressed = false;

        visualRoot.SetActive(false);

        Direction = Vector2.zero;
    }
}
