using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.XR;

public class HandManager : MonoBehaviour
{
    [SerializeField] public GameObject[] cardPrefab; // 生成するカード
    [SerializeField] public Transform[] deckCardTrans; // カードの生成場所

    [SerializeField] int cardDrawFee; // カードを引く代金
    [SerializeField] int cardDrawFeeBase; // カードを引く代金

    int cardId = 0; // 生成するカードのID
    int cardDrawCount = 0; // これまでにカードを引いた回数
    int[] cardIdStart = { 0, 1, 2, 3, 6, 9}; // 初期手札にできるカードのID

    public int[] cardIdArray = new int[4]; // カードのIDを配列に保存
    public int[] markedIndexArray = new int [4]; // マークした位置を配列に保存

    [SerializeField] GameObject discraftableMark; // 合成不可マーク

    GameObject newCard;

    GameObject[] mark = new GameObject[4];

    CoinManager coinManager;
    SkillManager skill;


    void Start()
    {

        GameObject[] cardObjs = GameObject.FindGameObjectsWithTag("Card");

        foreach(GameObject obj in cardObjs) // スクリプトの取得
        {
            skill = obj.GetComponent<SkillManager>();
            if (skill != null) break;
        }

        // craft = GetComponent<CraftManager>();
        coinManager = GameObject.Find("CoinManager").GetComponent<CoinManager>();


        for (int i = 0; i < deckCardTrans.Length; i++) // それぞれの手札の位置にランダムなカードを生成
        {
            cardId = Random.Range(0, cardIdStart.Length);

            newCard = Instantiate(cardPrefab[cardIdStart[cardId]], deckCardTrans[i]); // 初期手札の生成

            DraggableCard dc = newCard.GetComponent<DraggableCard>();

            cardIdArray[i] = cardId;

            if(dc != null )
            {
                dc.cardIndex = i;
                dc.cardId = cardIdStart[cardId];
            }

        }
    }

    // Update is called once per frame
    void Update()
    {
        if(Keyboard.current.mKey.wasPressedThisFrame)
        {
            coinManager.currentMoney += 100;
        }
    }

    public GameObject CardGenerate(int id, int ind) // 新たにカードを生成する
    {
        GameObject genCard = Instantiate(cardPrefab[id], deckCardTrans[ind]); // カードを作る処理
        DraggableCard dc = genCard.GetComponentInChildren<DraggableCard>();

        cardIdArray[ind] = id; 

        if(dc != null)
        {
            dc.cardIndex = ind;
            dc.cardId = id;
        }
        return genCard;
    }

    public void CardDraw()
    {
        int cardRandomId = Random.Range(0, cardIdStart.Length);

        for (int i = 0; i < 4; i++)
        {
            if (deckCardTrans[i].childCount == 0)
            {
                if (coinManager.currentMoney - cardDrawFee > 0)
                {
                    cardDrawCount++;
                    cardDrawFee = (int)(cardDrawFee +  cardDrawCount); // カードの購入代金を倍率に回数をかけたものとする

                    GameObject obj = CardGenerate(cardIdStart[cardRandomId], i);

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

    // 合成不可の場合マーキング
    public void DiscraftableMark(int ind)
    {
        mark[ind] = Instantiate(discraftableMark, deckCardTrans[ind]);
        markedIndexArray[ind] = 1;

    }

    // マークを削除
    public void DestroyMark(int ind)
    {
        Destroy(mark[ind]);
        markedIndexArray[ind] = 0;
    }
}
