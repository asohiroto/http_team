using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DraggableCard : MonoBehaviour, IBeginDragHandler, IEndDragHandler,IDragHandler
{
    private Canvas canvas;
    private CanvasGroup canvasGroup;
    private GameObject ghostImage;

    CraftManager craft;

    void Awake()
    {
        canvas = GetComponentInParent<Canvas>();
        canvasGroup = gameObject.AddComponent<CanvasGroup>();
        craft = GameObject.Find("CraftManager").GetComponentInParent<CraftManager>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (craft.craftFrag == 0) return;

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

    public void OnEndDrag(PointerEventData eventData)
    {
        if(ghostImage == null) return;

        canvasGroup.blocksRaycasts = true;
        Destroy(ghostImage);
        ghostImage = null;
    }
}
