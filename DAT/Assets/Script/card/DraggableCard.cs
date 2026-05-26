using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.XR;

public class DraggableCard : MonoBehaviour, IBeginDragHandler, IEndDragHandler,IDragHandler, IDropHandler
{
    private Canvas canvas;
    private CanvasGroup canvasGroup;
    private GameObject ghostImage;

    public int cardIndex;
    public int cardId;

    CraftManager craft;
    HandManager hand;

    void Awake()
    {
        canvas = GetComponentInParent<Canvas>();
        canvasGroup = gameObject.AddComponent<CanvasGroup>();

        craft = GameObject.Find("CraftManager").GetComponent<CraftManager>();
        hand = GameObject.Find("HandManager").GetComponent<HandManager>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (craft.craftFrag == 0) return;

        canvasGroup.blocksRaycasts = false;

        if (ghostImage != null) Destroy(ghostImage);

        Image originalImage = GetComponentInChildren<Image>();

        ghostImage = new GameObject("GhostImage");
        ghostImage.transform.SetParent(canvas.transform);
        ghostImage.transform.SetAsLastSibling();

        RectTransform ghostRect = ghostImage.AddComponent<RectTransform>();
        ghostRect.sizeDelta = GetComponent<RectTransform>().sizeDelta;

        Image ghostImg = ghostImage.AddComponent<Image>();
        ghostImg.sprite = originalImage.sprite;
        ghostImg.color = new Color(1, 1, 1, 0.7f);

        CanvasGroup cg = ghostImage.AddComponent<CanvasGroup>();
        cg.blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (craft.craftFrag == 0 || ghostImage == null) return;

        RectTransform ghostRect = ghostImage.GetComponent<RectTransform>();
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvas.transform as RectTransform,
            eventData.position,
            canvas.worldCamera,
            out Vector2 localPoint
            );
        ghostRect.localPosition = localPoint;
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (craft.craftFrag == 0) return;

        DraggableCard dragged = eventData.pointerDrag?.GetComponent<DraggableCard>();
        DraggableCard target = transform.GetComponentInChildren<DraggableCard>();

        if (dragged == null || dragged == this) return;

        int craftResult = craft.CraftItems(dragged.cardId, target.cardId);
        if (craftResult < 0) return;

        int fromIndex = dragged.cardIndex;
        int toIndex = this.cardIndex;

        Destroy(dragged.ghostImage);

        hand.DisCard(fromIndex);
        hand.DisCard(toIndex);

        int spawnIndex = Mathf.Min(fromIndex, toIndex);
        GameObject obj = hand.CardGenerate(craftResult, spawnIndex);
        hand.ButtonListener(craftResult, obj, spawnIndex);

        craft.craftFrag = 0;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if(ghostImage == null) return;

        canvasGroup.blocksRaycasts = true;
        Destroy(ghostImage);
        ghostImage = null;
    }
}
