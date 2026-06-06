using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.XR;

public class HandManager : MonoBehaviour
{
    // カードのプレハブと、カードを生成する位置の配列
    [SerializeField] public GameObject[] cardPrefab;
    [SerializeField] public Transform[] deckCardTrans;

    // カードを引くための代金
    [SerializeField] int cardDrawFee;

    // カードのIDと、引いた回数
    int cardId = 0;
    int cardDrawCount = 0;

    // 現在の手札のIDを配列に保存
    public int[] cardIdArray = new int[5];

    CoinManager coinManager;
    SkillManager skill;
    CardChanger change;

    void Start()
    {

        GameObject[] cardObjs = GameObject.FindGameObjectsWithTag("Card");

        foreach (GameObject obj in cardObjs) // スクリプトの取得
        {
            skill = obj.GetComponent<SkillManager>();
            if (skill != null) break;
        }

        // craft = GetComponent<CraftManager>();
        coinManager = GameObject.Find("CoinManager").GetComponent<CoinManager>();

        change = GetComponent<CardChanger>();




        for (int i = 0; i < deckCardTrans.Length; i++) // それぞれの手札の位置にランダムなカードを生成
        {
            cardId = Random.Range(0, 6);

            int cardIdStart = change.CardChange(cardId);

            CardGenerate(cardIdStart, i);
        }
    }

    // Update is called once per frame
    void Update()
    {
        // デバッグ用　Mキーでお金を増やす
        if (Keyboard.current.mKey.wasPressedThisFrame)
        {
            coinManager.currentMoney += 100;
        }

        // 右クリックでカードを引く
        if (Mouse.current.rightButton.wasPressedThisFrame)
        {
            CardDraw();
        }
    }

    // カードを生成する関数
    public void CardGenerate(int id, int ind) // 新たにカードを生成する
    {
        GameObject genCard = Instantiate(cardPrefab[id], deckCardTrans[ind]); // カードを作る処理
        DraggableCard dc = genCard.GetComponentInChildren<DraggableCard>();

        CardEdit edit = genCard.GetComponentInChildren<CardEdit>();

        edit.ChangeCardName(change.cardName);
        edit.ChangeCardEffect(change.cardEffect);

        cardIdArray[ind] = id;

        if (dc != null)
        {
            dc.cardIndex = ind;
            dc.cardId = id;
        }
    }

    // カードを引く処理
    public void CardDraw()
    {
        int cardRandomId = Random.Range(0, 6);

        for (int i = 0; i < 5; i++)
        {
            if (deckCardTrans[i].childCount == 0)
            {
                if (coinManager.currentMoney - cardDrawFee > 0)
                {
                    cardDrawCount++;

                    // カードを引くたびに代金が上がる
                    cardDrawFee = (int)(cardDrawFee + cardDrawCount * 10);

                    int cardIdDraw = change.CardChange(cardRandomId);

                    CardGenerate(cardIdDraw, i);

                    coinManager.ReduceMoney(cardDrawFee);
                }
                else
                {
                    Debug.Log("財布が少しばかり軽過ぎるようだ……");
                }
                break;
            }
        }

    }

    public void DisCard(int ind) // 手札を捨てる
    {
        if (deckCardTrans[ind].childCount <= 0)
        {
            Debug.Log("破壊対象が存在しません");
        }
        else
        {
            cardIdArray[ind] = -1;
            Destroy(deckCardTrans[ind].GetChild(0).gameObject);
        }
    }

}
