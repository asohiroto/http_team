using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class HandManager : MonoBehaviour
{

    [SerializeField] GameObject[] CardPrefab; // 生成するカード
    [SerializeField] Transform[] deckCardTrans; // カードの生成場所
    [SerializeField] int discardInd; // 破棄するカードの番地
    [SerializeField] GameObject cursorPrefab; // カーソルのプレファブ

    Vector2 cursorPos;

    int cardId = 0; // 生成するカードのID
    public int[] cardUseId = new int[4]; // 配列に保存されているカードIDを保存する配列
    public int cardUse = 0; // 現在カーソルが指している手札の番号を保存

    GameObject cursorInstance; // 生成したカーソルを保持する変数

    void Start()
    {
        /*cursorPos.x = deckCardTrans[0].position.x + 0.8f;
        cursorPos.y = deckCardTrans[0].position.y - 1.0f;*/

        for (int i = 0; i < 4; i++) // それぞれの手札の位置にランダムなカードを生成
        {
            cardId = Random.Range(0, 4);

            Instantiate(CardPrefab[cardId], deckCardTrans[i]); // 初期手札の生成

            cardUseId[i] = cardId;
        }

        //cursorInstance = Instantiate(cursorPrefab, cursorPos, Quaternion.identity); // 初期カーソルの生成(位置のみ参照)
    }

    // Update is called once per frame
    void Update()
    {
        int nextCardId = Random.Range(0, 4);

        // 上部の数字キーによって破棄するカードを決定、カーソルを移動
        if (Keyboard.current.digit1Key.wasPressedThisFrame)
        {
            MoveCursor(0);
            cardUse = 0;
        }

        if (Keyboard.current.digit2Key.wasPressedThisFrame) 
        { 
            MoveCursor(1);
            cardUse = 1;
        }
        if (Keyboard.current.digit3Key.wasPressedThisFrame)
        {
            MoveCursor(2);
            cardUse = 2;
        }
        if(Keyboard.current.digit4Key.wasPressedThisFrame) 
        { 
            MoveCursor(3);
            cardUse = 3;
        }


        if (Keyboard.current.wKey.wasPressedThisFrame)
        {
            Card_Change(nextCardId, discardInd);
            cardUseId[discardInd] = nextCardId;

        }
    }

    void MoveCursor(int index)
    {
        discardInd = index;
        cursorInstance.transform.position = (Vector2)deckCardTrans[index].position + new Vector2(0.8f, -1.0f); // カーソルの位置を選択した場所へ移動させる
    }

    void Card_Change(int ran, int chan) // 特定のカードを捨て、新たにランダムなカードを生成する
    {
        Destroy(deckCardTrans[chan].GetChild(0).gameObject); // カードを捨てる処理
        Instantiate(CardPrefab[ran], deckCardTrans[chan]); // カードを作る処理
    }
}
