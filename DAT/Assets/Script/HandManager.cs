using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandManager : MonoBehaviour
{

    [SerializeField] GameObject[] CardPrefab;
    //[SerializeField] Transform deckCardTrans1;

    [SerializeField] Transform[] deckCardTrans;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //Instantiate(CardPrefab, deckCardTrans1);

        for (int i = 0; i < 4; i++)
        {

            Instantiate(CardPrefab[i], deckCardTrans[i]);

        }
    }
    // Update is called once per frame
    void Update()
    {

    }
}
