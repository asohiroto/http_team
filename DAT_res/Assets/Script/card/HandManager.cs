using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandManager : MonoBehaviour
{

    [SerializeField] GameObject[] CardPrefab; // 生成するカード


    [SerializeField] Transform[] deckCardTrans; // カードの生成場所
    void Start()
    {

        int cardId = 0;


        for (int i = 0; i < 4; i++) // それぞれの手札の位置にランダムなカードを生成
        {
            cardId = Random.Range(0, 3);

            Instantiate(CardPrefab[cardId], deckCardTrans[i]);


        }
    }
    // Update is called once per frame
    void Update()
    {

    }
}
