using System;
using System.Collections.Generic;
using UnityEngine;

public class CardChanger : MonoBehaviour
{
    public int cardType; // カードの種類
    public string cardName; // カード名
    public string cardEffect; // カードの効果
    public Sprite cardImage; // カードのイメージ

    [SerializeField] private List<CardData> cardData;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    // カードの情報を変更するメソッド
    public int CardChange(int id)
    {
        CardData data = cardData[id];

        cardType = data.cardType;
        cardName = data.cardName;
        cardEffect = data.cardEffect;
        cardImage = data.cardImage;

        return cardType;

    }
}

[Serializable]
public class CardData
{
    public int cardType; // カードの種類
    public string cardName; // カード名
    public string cardEffect; // カードの効果
    public Sprite cardImage; // カードのイメージ
}
