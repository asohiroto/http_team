using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Android;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DraggablePanel : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    private Canvas canvas;
    private CanvasGroup canvasGroup;
    private GameObject ghostImage;

    public int cardId = -1;
    public int cardInd = -1;

    MyDeck deck;
    DeckManager manager;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // ドラッグするためのキャンバスと、ドラッグ中にレイキャストでこのカードを感知しないようにするためのCanvasGroupを取得
        canvas = GetComponentInParent<Canvas>();
        canvasGroup = gameObject.AddComponent<CanvasGroup>();

        deck = GameObject.Find("MyDeck").GetComponent<MyDeck>();
        manager = GameObject.Find("DeckManager").GetComponent<DeckManager>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        // ドラッグ中のカードを取得
        DraggableCard dragged = eventData.pointerDrag?.GetComponent<DraggableCard>();

        // ドラッグ中はレイキャストでこのカードを感知しないようにする
        canvasGroup.blocksRaycasts = false;

        // 存在しうるゴーストイメージは常に一つ
        if (ghostImage != null) Destroy(ghostImage);

        Image originalImage = GetComponentInChildren<Image>();

        // ドラッグ中にマウスに追従するゴーストイメージを作成
        ghostImage = new GameObject("GhostImage");
        ghostImage.transform.SetParent(canvas.transform);
        ghostImage.transform.SetAsLastSibling();

        // ゴーストイメージのサイズを元のカードの半分に設定
        RectTransform ghostRect = ghostImage.AddComponent<RectTransform>();
        ghostRect.sizeDelta = GetComponent<RectTransform>().sizeDelta * 0.5f;

        Image ghostImg = ghostImage.AddComponent<Image>();
        ghostImg.sprite = originalImage.sprite;

        // ゴーストイメージを半透明に
        ghostImg.color = new Color(1, 1, 1, 0.7f);

        CanvasGroup cg = ghostImage.AddComponent<CanvasGroup>();
    }

    public void OnDrag(PointerEventData eventData)
    {
        // マウスのスクリーン座標をキャンバスのローカル座標に変換して、ゴーストイメージを追従させる
        RectTransform ghostRect = ghostImage.GetComponent<RectTransform>();

        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvas.transform as RectTransform,  // キャンバスを基準とする
            eventData.position,                 // マウスのスクリーン座標
            canvas.worldCamera,                 // 使用するカメラ
            out Vector2 localPoint              // 変換結果を受け取る変数
            );

        ghostRect.localPosition = localPoint;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        deck.myDeckId[cardInd] = -1;

        manager.deckCount--;
        manager.DeckCounter(manager.deckCount);

        Destroy(gameObject);

        // ドラッグ中はレイキャストでこのカードを感知しないようにしていたのを元に戻す
        canvasGroup.blocksRaycasts = true;
        Destroy(ghostImage);
        ghostImage = null;
    }
}
