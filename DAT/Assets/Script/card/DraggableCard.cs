using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;

public class DraggableCard : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler, IDropHandler
{
    private Canvas canvas;
    private CanvasGroup canvasGroup;
    private GameObject ghostImage;

    [SerializeField] GameObject cardUse;
    [SerializeField] GameObject cardCreate;

    GameObject cardUsePrefab;

    // カードの位置と、ID
    public int cardIndex;
    public int cardId;

    // 合成の結果
    int craftResult;

    public bool wasDroppedOnCard = false;

    string currentScene;

    Vector2 pos;

    CraftManager craft;
    HandManager hand;
    SkillManager skill;
    CardEffectManager effect;
    CardChanger change;
    PlayerController player;

    void Start()
    {
        currentScene = SceneManager.GetActiveScene().name;

        // ドラッグするためのキャンバスと、ドラッグ中にレイキャストでこのカードを感知しないようにするためのCanvasGroupを取得
        canvas = GetComponentInParent<Canvas>();
        canvasGroup = gameObject.AddComponent<CanvasGroup>();

        if (currentScene != "DeckScene")
        {
            player = GameObject.Find("Player").GetComponent<PlayerController>();
            effect = GameObject.Find("CardEffectManager").GetComponent<CardEffectManager>();
            change = GameObject.Find("HandManager").GetComponent<CardChanger>();

            GameObject[] objs = GameObject.FindGameObjectsWithTag("Card");
            foreach (GameObject obj in objs) // それぞれ探す
            {
                if (hand == null) hand = obj.GetComponent<HandManager>();

                if (craft == null) craft = obj.GetComponent<CraftManager>();

                if (skill == null) skill = obj.GetComponent<SkillManager>();

                if (hand != null && craft != null && skill != null) break;
            }
        }
    }

    void Update()
    {
        if (currentScene != "DeckScene")
        {
            pos = player.currentPos;

            if (cardUsePrefab != null)
            {
                cardUsePrefab.transform.position = pos;
                Debug.Log(player.currentPos);
                Debug.Log(cardUsePrefab.transform.position);
            }
        }
    }

    // ドラッグ開始時に実行
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

        // レイキャストでゴーストイメージを感知しない
        cg.blocksRaycasts = false;

        if (currentScene != "DeckScene")
        {
            for (int i = 0; i < hand.deckCardTrans.Length; i++)
            {
                if (craft.CraftCards(dragged.cardId, hand.cardIdArray[i]) < 0 && hand.cardIdArray[i] >= 0)
                {
                    hand.DiscraftableMark(i);
                }
            }
        }

        if (currentScene == "DeckScene")
        {
            DataPanelManager dataPanel = GameObject.Find("DeckManager").GetComponent<DataPanelManager>();

            dataPanel.CardDataPanel(dragged.cardId);
        }
    }

    // ドラッグ中に実行
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

    // ドロップされた時に実行
    public void OnDrop(PointerEventData eventData)
    {
        if (currentScene != "DeckScene")
        {
            // ドロップされたカードと、ドロップ先のカードを取得
            DraggableCard dragged = eventData.pointerDrag?.GetComponent<DraggableCard>();
            DraggableCard target = transform.GetComponentInChildren<DraggableCard>();

            // ドロップされたカードが存在しない、またはドロップ先のカードと同じ場合は何もしない
            if (dragged == null || dragged == this) return;

            // DeckSceneではカードの合成は行わないため、合成の処理を行わない
            if (SceneManager.GetActiveScene().name != "DeckScene")
            {
                dragged.wasDroppedOnCard = true;

                // ドロップされたカードとドロップ先のカードのIDをCraftManagerに渡して、合成の結果のカードのIDを取得
                craftResult = craft.CraftCards(dragged.cardId, target.cardId);

                if (craftResult < 0)
                {
                    Debug.Log("なにかが違うようだ……？");
                    return;
                }

                // ドロップされたカードとドロップ先のカードの位置を取得
                int fromIndex = dragged.cardIndex;
                int toIndex = this.cardIndex;

                Destroy(dragged.ghostImage);

                // ドロップされたカードとドロップ先のカードを手札から消す
                hand.DisCard(fromIndex);
                hand.DisCard(toIndex);

                GameObject obj = hand.CardGenerate(craftResult, toIndex);
                Instantiate(cardCreate, obj.transform);

                for (int i = 0; i < hand.deckCardTrans.Length; i++)
                {
                    if (hand.markedIndexArray[i] == 1)
                    {
                        hand.DestroyMark(i);
                    }
                }
            }
        }
    }
    // ドラッグ終了時に実行
    public void OnEndDrag(PointerEventData eventData)
    {
        if (currentScene != "DeckScene")
        {
            // ドラッグ中に作成したゴーストイメージが存在しない場合は何もしない
            if (ghostImage == null) return;

            if (wasDroppedOnCard)
            {
                wasDroppedOnCard = false;
            }
            else
            {
            // カードの効果を発動させる
                effect.cardEffect[cardId]();

                cardUsePrefab = effect.Effect(cardUse, pos);

                hand.DisCard(cardIndex);
            }

            for (int i = 0; i < hand.deckCardTrans.Length; i++)
            {
                if (hand.markedIndexArray[i] == 1)
                {
                    hand.DestroyMark(i);
                }
            }
        }

        // ドラッグ中はレイキャストでこのカードを感知しないようにしていたのを元に戻す
        canvasGroup.blocksRaycasts = true;
        Destroy(ghostImage);
        ghostImage = null;
    }
}
