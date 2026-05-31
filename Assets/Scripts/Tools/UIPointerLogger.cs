using UnityEngine;
using UnityEngine.EventSystems;

public class UIPointerLogger : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler, IDragHandler
{
    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log($"PointerDown on {gameObject.name} (pointerId={eventData.pointerId})");
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        Debug.Log($"PointerUp on {gameObject.name} (pointerId={eventData.pointerId})");
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log($"PointerEnter on {gameObject.name}");
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Debug.Log($"PointerExit on {gameObject.name}");
    }

    public void OnDrag(PointerEventData eventData)
    {
        Debug.Log($"OnDrag on {gameObject.name} delta={eventData.delta}");
    }
}
