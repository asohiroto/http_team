using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.XR;

public class DraggableCard : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler, IDropHandler
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

    // ドラッグ開始時に実行
    public void OnBeginDrag(PointerEventData eventData)
    {
        if (craft.craftFrag == 0) return; // クラフト状態じゃなければドラッグできない

        canvasGroup.blocksRaycasts = false; // レイキャストで自分自身を感知しない

        if (ghostImage != null) Destroy(ghostImage); // 存在しうるゴーストイメージは常に一つ

        Image originalImage = GetComponentInChildren<Image>();

        ghostImage = new GameObject("GhostImage"); // ドラッグ中にマウスに追従するゴーストイメージを作成
        ghostImage.transform.SetParent(canvas.transform);
        ghostImage.transform.SetAsLastSibling();

        RectTransform ghostRect = ghostImage.AddComponent<RectTransform>();
        ghostRect.sizeDelta = GetComponent<RectTransform>().sizeDelta;

        Image ghostImg = ghostImage.AddComponent<Image>();
        ghostImg.sprite = originalImage.sprite;
        ghostImg.color = new Color(1, 1, 1, 0.7f); // ゴーストイメージを半透明に

        CanvasGroup cg = ghostImage.AddComponent<CanvasGroup>();
        cg.blocksRaycasts = false; // レイキャストでゴーストイメージを感知しない
    }

    // ドラッグ中に実行
    public void OnDrag(PointerEventData eventData)
    {
        if (craft.craftFrag == 0 || ghostImage == null) return;

        RectTransform ghostRect = ghostImage.GetComponent<RectTransform>();
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvas.transform as RectTransform, // キャンバスを基準とする
            eventData.position, // マウスのスクリーン座標
            canvas.worldCamera, // 使用するカメラ
            out Vector2 localPoint // 変換結果を受け取る変数
            );
        ghostRect.localPosition = localPoint;
    }

    // ドロップ時に実行
    public void OnDrop(PointerEventData eventData)
    {
        if (craft.craftFrag == 0) return;

        DraggableCard dragged = eventData.pointerDrag?.GetComponent<DraggableCard>(); // pointerDragがnullじゃなければGetComponentを実行
        DraggableCard target = transform.GetComponentInChildren<DraggableCard>();

        if (dragged == null || dragged == this) return;

        int craftResult = craft.CraftItems(dragged.cardId, target.cardId); // IDをそれぞれ取得し、合成を実行

        if (craftResult < 0) return;

        int fromIndex = dragged.cardIndex; // それぞれの住所を取得
        int toIndex = this.cardIndex;

        Destroy(dragged.ghostImage);

        hand.DisCard(fromIndex);
        hand.DisCard(toIndex);

        int spawnIndex = Mathf.Min(fromIndex, toIndex); // より小さいほう（左側にあるカード）を生成する住所とする
        GameObject obj = hand.CardGenerate(craftResult, spawnIndex);
        hand.ButtonListener(craftResult, obj, spawnIndex);

        craft.craftFrag = 0;
    }

    // ドラッグ終了時に実行
    public void OnEndDrag(PointerEventData eventData)
    {
        if (ghostImage == null) return;

        canvasGroup.blocksRaycasts = true;
        Destroy(ghostImage);
        ghostImage = null;
    }
}
