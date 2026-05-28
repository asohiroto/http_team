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

    [SerializeField] float cardDrawFeeMagnif; // カード購入代金倍率

    int cardId = 0; // 生成するカードのID
    int cardDrawCount = 0; // これまでにカードを引いた回数
    int[] cardIdStart = { 0, 1, 2, 3, 6 };

    SkillManager skill;
    GameObject newCard;
    // CraftManager craft;
    CoinManager coinManager;


    public bool[] isCardPressed;

    void Start()
    {
        skill = GameObject.Find("SkillManager").GetComponent<SkillManager>();
        // craft = GetComponent<CraftManager>();
        coinManager = GameObject.Find("CoinManager").GetComponent<CoinManager>();

        isCardPressed = new bool[deckCardTrans.Length];

        for (int i = 0; i < 4; i++) // それぞれの手札の位置にランダムなカードを生成
        {
            cardId = Random.Range(0, 5);

            newCard = Instantiate(cardPrefab[cardIdStart[cardId]], deckCardTrans[i]); // 初期手札の生成

            ButtonListener(cardIdStart[cardId], newCard, i);
            DraggableCard dc = newCard.GetComponent<DraggableCard>();

            if(dc != null )
            {
                dc.cardIndex = i;
                dc.cardId = cardId;
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

    public GameObject CardGenerate(int ran, int chan) // 新たにランダムなカードを生成する
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
        int cardRandomId = Random.Range(0, 5);

        for (int i = 0; i < 4; i++)
        {
            if (deckCardTrans[i].childCount == 0)
            {
                if (coinManager.currentMoney - cardDrawFee > 0)
                {
                    cardDrawCount++;
                    cardDrawFee = (int)(cardDrawFee + cardDrawFeeBase * (1.0f + cardDrawFeeMagnif * cardDrawCount)); // カードの購入代金を倍率に回数をかけたものとする

                    GameObject obj = CardGenerate(cardIdStart[cardRandomId], i);
                    ButtonListener(cardIdStart[cardRandomId], obj, i);

                    coinManager.currentMoney -= cardDrawFee;
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

    public void ButtonListener(int id, GameObject targetCard, int index) // ボタンの入力を検知する
    {

        Button btn = targetCard.GetComponentInChildren<Button>();

        if (btn == null) return; // ボタンコンポーネントがついていなかった場合の安全装置


        switch (id)
        {
            case 0:

                btn.onClick.AddListener(async () => await skill.Enhance(index)); // ボタンの入力を検知するリスナーを付与

                break;

            case 1:

                btn.onClick.AddListener(async () => await skill.Heal(index));
                break;

            case 2:

                btn.onClick.AddListener(async () => await skill.Slash(index));
                break;

            case 3:

                btn.onClick.AddListener(async () => await skill.FireBall(index));
                break;

            case 4:

                btn.onClick.AddListener(async () => await skill.FireSlash(index));
                break;

            case 5:

                btn.onClick.AddListener(async () => await skill.HyperMode(index));
                break;

            case 6:
                btn.onClick.AddListener(async () => await skill.Curse(index));
                break;
        }

    }
}
