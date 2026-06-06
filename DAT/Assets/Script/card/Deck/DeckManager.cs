using System;
using UnityEngine;

public class DeckManager : MonoBehaviour
{
    [SerializeField] public GameObject[] deckPrefab;
    [SerializeField] public GameObject[] cardPrefab;

    [SerializeField] public Transform[] deckTrans;
    [SerializeField] public Transform[] cardTrans;

    CardChanger change;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        change = GameObject.Find("CardChanger").GetComponent<CardChanger>();

        for (int i = 0; i < cardPrefab.Length; i++)
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
        GameObject genCard = Instantiate(cardPrefab[id], cardTrans[ind]); // カードを作る処理
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
