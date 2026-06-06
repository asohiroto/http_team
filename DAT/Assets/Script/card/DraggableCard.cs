using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.XR;

public class DraggableCard : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler, IDropHandler
{
    private Canvas canvas;
    private CanvasGroup canvasGroup;

    private GameObject ghostImage;
    private GameObject cardEffectManager;

    // カードの位置と、ID
    public int cardIndex;
    public int cardId;

    // 合成の結果
    int craftResult;

    // 合成の成功フラグ
    bool craftSucces = false;

    CraftManager craft;
    HandManager hand;
    SkillManager skill;

    // スクリプトをタプルで格納
    private (Enhance enhance, Heal heal, Slash slash, FireBall fireBall, FireSlash fireSlash, HyperMode hyperMode, Curse curse, CursedFlame cursedFlame, OverHeal overHeal, Absorb absorb) cardEffect;

    void Start()
    {
        canvas = GetComponentInParent<Canvas>();
        canvasGroup = gameObject.AddComponent<CanvasGroup>();
        cardEffectManager = GameObject.Find("CardEffectManager");

        GameObject[] objs = GameObject.FindGameObjectsWithTag("Card");
        foreach (GameObject obj in objs) // それぞれ探す
        {
            if (hand == null) hand = obj.GetComponent<HandManager>();

            if (craft == null) craft = obj.GetComponent<CraftManager>();

            if (skill == null) skill = obj.GetComponent<SkillManager>();

            if (hand != null && craft != null && skill != null) break;
        }

        // 各スクリプトを取得
        if (cardEffectManager != null)
        {
            cardEffectManager.TryGetComponent(out cardEffect.enhance);
            cardEffectManager.TryGetComponent(out cardEffect.heal);
            cardEffectManager.TryGetComponent(out cardEffect.slash);
            cardEffectManager.TryGetComponent(out cardEffect.fireBall);
            cardEffectManager.TryGetComponent(out cardEffect.fireSlash);
            cardEffectManager.TryGetComponent(out cardEffect.hyperMode);
            cardEffectManager.TryGetComponent(out cardEffect.curse);
            cardEffectManager.TryGetComponent(out cardEffect.cursedFlame);
            cardEffectManager.TryGetComponent(out cardEffect.overHeal);
            cardEffectManager.TryGetComponent(out cardEffect.absorb);
        }

    }


    // ドラッグ開始時に実行
    public void OnBeginDrag(PointerEventData eventData)
    {
        // レイキャストで自分自身を感知しない
        canvasGroup.blocksRaycasts = false;

        // 存在しうるゴーストイメージは常に一つ
        if (ghostImage != null) Destroy(ghostImage);

        Image originalImage = GetComponentInChildren<Image>();

        // ドラッグ中にマウスに追従するゴーストイメージを作成
        ghostImage = new GameObject("GhostImage");
        ghostImage.transform.SetParent(canvas.transform);
        ghostImage.transform.SetAsLastSibling();

        RectTransform ghostRect = ghostImage.AddComponent<RectTransform>();
        ghostRect.sizeDelta = GetComponent<RectTransform>().sizeDelta * 0.5f;

        Image ghostImg = ghostImage.AddComponent<Image>();
        ghostImg.sprite = originalImage.sprite;

        // ゴーストイメージを半透明に
        ghostImg.color = new Color(1, 1, 1, 0.7f);

        CanvasGroup cg = ghostImage.AddComponent<CanvasGroup>();

        // レイキャストでゴーストイメージを感知しない
        cg.blocksRaycasts = false;

        //// 手札の中の合成できるカード、出来ないカードを判別し、マーキング
        //for(int i = 0; i < 4; i++)
        //{
        //    int craftResultPre = craft.CraftCards(cardId, hand.cardIdArray[i]);

        //    if (i != cardIndex)
        //    {
        //        if (craftResultPre < 0)
        //        {
        //            hand.DiscraftableMark(i);
        //        }
        //    }
        //}

    }

    // ドラッグ中に実行
    public void OnDrag(PointerEventData eventData)
    {
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
        DraggableCard dragged = eventData.pointerDrag?.GetComponent<DraggableCard>(); // pointerDragがnullじゃなければGetComponentを実行
        DraggableCard target = transform.GetComponentInChildren<DraggableCard>();

        if (dragged == null || dragged == this) return;

        dragged.craftSucces = true;

        craftResult = craft.CraftCards(dragged.cardId, target.cardId); // IDをそれぞれ取得し、合成を実行

        if (craftResult < 0)
        {
            dragged.craftSucces = true;
            Debug.Log("なにかが違うようだ……？");
            return;
        }

        int fromIndex = dragged.cardIndex; // それぞれの住所を取得
        int toIndex = this.cardIndex;

        Destroy(dragged.ghostImage);

        hand.DisCard(fromIndex);
        hand.DisCard(toIndex);

        int spawnIndex = Mathf.Min(fromIndex, toIndex); // より小さいほう（左側にあるカード）を生成する住所とする

        hand.CardGenerate(craftResult, spawnIndex);


    }

    // ドラッグ終了時に実行
    public void OnEndDrag(PointerEventData eventData)
    {
        if (ghostImage == null) return;

        // 合成を行っていなければ、使用する
        if (skill.useFlag)
        {
            Debug.Log(skill.useFlag);
            switch (cardId)
            {
                case 0:
                    cardEffect.enhance.Effect(cardIndex);

                    break;
                case 1:
                    cardEffect.heal.Effect(cardIndex);

                    break;
                case 2:
                    cardEffect.slash.Effect(cardIndex);

                    break;
                case 3:
                    cardEffect.fireBall.Effect(cardIndex, skill.mousePosWorld);

                    break;
                case 4:
                    cardEffect.fireSlash.Effect(cardIndex);

                    break;
                case 5:
                    cardEffect.hyperMode.Effect(cardIndex);

                    break;
                case 6:
                    cardEffect.curse.Effect(cardIndex);

                    break;
                case 7:
                    cardEffect.cursedFlame.Effect(cardIndex, skill.mousePosWorld);

                    break;
                case 8:
                    cardEffect.overHeal.Effect(cardIndex);

                    break;
                case 9:
                    cardEffect.absorb.Effect(cardIndex);

                    break;
            }
        }
        else
        {
            Debug.Log(skill.useFlag);
            skill.useFlag = true;
        }

        //for (int i = 0; i < 4; i++)
        //{
        //    if (hand.markedIndexArray[i] == 1)
        //    {
        //        hand.DestroyMark(i);
        //    }
        //}

        craftSucces = false;
        canvasGroup.blocksRaycasts = true;
        Destroy(ghostImage);
        ghostImage = null;
    }
}
