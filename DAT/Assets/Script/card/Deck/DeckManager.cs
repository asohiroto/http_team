using System;
using UnityEngine;

public class DeckManager : MonoBehaviour
{
    [SerializeField] public Transform[] deckTrans;
    [SerializeField] public Transform[] cardTrans;
    [SerializeField] public GameObject[] possessionCards;

    MyDeck deckId;
    CardChanger change;
    HandManager hand;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        deckId = GameObject.Find("MyDeck").GetComponent<MyDeck>();
        change = GetComponent<CardChanger>();

        for (int i = 0; i < cardTrans.Length; i++)
        {
            int cardId = change.CardChange(i);

            CardGenerate(cardId, i);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // カードを生成する関数
    public void CardGenerate(int id, int ind) // 新たにカードを生成する
    {
        GameObject genCard = Instantiate(possessionCards[id], cardTrans[ind]); // カードを作る処理
        DraggableCard dc = genCard.GetComponentInChildren<DraggableCard>();

        CardEdit edit = genCard.GetComponentInChildren<CardEdit>();

        edit.ChangeCardName(change.cardName);
        edit.ChangeCardEffect(change.cardEffect);

        if (dc != null)
        {
            dc.cardIndex = ind;
            dc.cardId = id;
        }
    }
}
