using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class ButtonPointerUpListener : MonoBehaviour, IPointerUpHandler, IPointerDownHandler, IPointerClickHandler
{
    public UnityEvent onPointerUp = new UnityEvent();
    public UnityEvent onPointerDown = new UnityEvent();
    public UnityEvent onPointerClick = new UnityEvent();

    public void OnPointerUp(PointerEventData eventData)
    {
        onPointerUp.Invoke();
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        onPointerDown.Invoke();
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        onPointerClick.Invoke();
    }
}
