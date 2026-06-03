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

    GameObject newCard;

    CoinManager coinManager;
    SkillManager skill;

    public bool[] isCardPressed;

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

        isCardPressed = new bool[deckCardTrans.Length];

        for (int i = 0; i < 4; i++) // それぞれの手札の位置にランダムなカードを生成
        {
            cardId = Random.Range(0, cardIdStart.Length);

            newCard = Instantiate(cardPrefab[cardIdStart[cardId]], deckCardTrans[i]); // 初期手札の生成

            // ButtonListener(cardIdStart[cardId], newCard, i);
            DraggableCard dc = newCard.GetComponent<DraggableCard>();

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

    public GameObject CardGenerate(int ran, int chan) // 新たにカードを生成する
    {
        GameObject genCard = Instantiate(cardPrefab[ran], deckCardTrans[chan]); // カードを作る処理
        DraggableCard dc = genCard.GetComponentInChildren<DraggableCard>();

        if(dc != null)
        {
            dc.cardIndex = chan;
            dc.cardId = ran;
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
                    // ButtonListener(cardIdStart[cardRandomId], obj, i);

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

    public void DisCard(int chan) // 手札を捨てる
    {
        if (deckCardTrans[chan].childCount <= 0)
        {
            Debug.Log("破壊対象が存在しません");
        }
        else
        {
            Destroy(deckCardTrans[chan].GetChild(0).gameObject);
        }
    }
}
