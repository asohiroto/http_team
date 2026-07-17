using UnityEngine;
using UnityEngine.InputSystem;

public class HandManager : MonoBehaviour
{
    // カードのプレハブと、カードを生成する位置の配列
    [SerializeField] public GameObject[] cardPrefab;
    [SerializeField] public Transform[] deckCardTrans;

    // 合成不可のマーク
    [SerializeField] GameObject discraftableMark;
    GameObject[] mark = new GameObject[5];
    public int[] markedIndexArray;

    // カードを引くための代金
    [SerializeField] public int cardDrawFee;

    // カードのIDと、引いた回数
    int cardId = 0;
    int cardDrawCount = 0;

    bool autoCraft = true;

    // 現在の手札のIDを配列に保存
    public int[] cardIdArray = new int[5];

    CoinManager coinManager;
    SkillManager skill;
    CardChanger change;
    MyDeck deck;
    CraftManager craft;

    void Start()
    {

        GameObject[] cardObjs = GameObject.FindGameObjectsWithTag("Card");

        foreach (GameObject obj in cardObjs) // スクリプトの取得
        {
            skill = obj.GetComponent<SkillManager>();
            if (skill != null) break;
        }

        craft = GameObject.Find("CraftManager").GetComponent<CraftManager>();
        coinManager = GameObject.Find("CoinManager").GetComponent<CoinManager>();

        change = GetComponent<CardChanger>();
        deck = GameObject.Find("MyDeck").GetComponent<MyDeck>();

        for (int i = 0; i < deckCardTrans.Length; i++) // それぞれの手札の位置にランダムなカードを生成
        {
            int randCard = Random.Range(0, 6);

            cardId = deck.myDeckId[randCard];

            CardGenerate(cardId, i);
        }
    }

    // Update is called once per frame
    void Update()
    {

        // 右クリックでカードを引く
        if (Mouse.current.rightButton.wasPressedThisFrame)
        {
            CardDraw();
        }
    }

    // カードを生成する関数
    public GameObject CardGenerate(int id, int ind) // 新たにカードを生成する
    {
        int cardType = change.CardChange(id);

        GameObject genCard = Instantiate(cardPrefab[cardType], deckCardTrans[ind]); // カードを作る処理
        DraggableCard dc = genCard.GetComponentInChildren<DraggableCard>();

        CardEdit edit = genCard.GetComponentInChildren<CardEdit>();

        edit.ChangeCardName(change.cardName);
        edit.ChangeCardEffect(change.cardEffect);
        edit.ChangeCardImage(change.cardImage);

        cardIdArray[ind] = id;
        //AutoCraft(id);

        if (dc != null)
        {
            dc.cardIndex = ind;
            dc.cardId = id;
        }

        return genCard;
    }

    // カードを引く処理
    public void CardDraw()
    {
        int randCard = Random.Range(0, 6);

        cardId = deck.myDeckId[randCard];

        for (int i = 0; i < deckCardTrans.Length; i++)
        {
            if (deckCardTrans[i].childCount == 0)
            {
                if (coinManager.currentMoney - cardDrawFee > 0)
                {
                    cardDrawCount++;

                    // カードを引くたびに代金が上がる
                    cardDrawFee = (int)(cardDrawFee + cardDrawCount);

                    CardGenerate(cardId, i);

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
