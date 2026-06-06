using UnityEngine;
using UnityEngine.EventSystems;

public class HandPanel : MonoBehaviour, IDropHandler
{
    
    public void OnDrop(PointerEventData eventData)
    {
        DraggableCard dragged = eventData.pointerDrag?.GetComponent<DraggableCard>();

        if (dragged == null) return;

        dragged.wasDroppedOnCard = true;
    }
}
