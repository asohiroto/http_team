using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class HandManager : MonoBehaviour
{

    [SerializeField] GameObject[] cardPrefab; // 生成するカード
    [SerializeField] Transform[] deckCardTrans; // カードの生成場所

    int cardId = 0; // 生成するカードのID
    public int[] cardUseId = new int[4]; // 配列に保存されているカードIDを保存する配列

    skill_manager skill;

    GameObject newCard;

    void Start()
    {
        skill = GameObject.Find("CardManager").GetComponent<skill_manager>();

        for (int i = 0; i < 4; i++) // それぞれの手札の位置にランダムなカードを生成
        {
            cardId = Random.Range(0, 4);

            newCard =Instantiate(cardPrefab[cardId], deckCardTrans[i]); // 初期手札の生成

            ButtonListener(cardId, newCard);

            cardUseId[i] = cardId;
        }
    }

    // Update is called once per frame
    void Update()
    {
        int nextCardId = Random.Range(0, 4);

        // 上部の数字キーによって破棄するカードを決定、カーソルを移動
        if (Keyboard.current.digit1Key.wasPressedThisFrame)
        {
            DisCard(0);

            newCard = CardGenerate(nextCardId, 0);
            cardUseId[0] = nextCardId;

            ButtonListener(nextCardId, newCard);
        }

        if (Keyboard.current.digit2Key.wasPressedThisFrame)
        {
            DisCard(1);

            newCard = CardGenerate(nextCardId, 1);
            cardUseId[1] = nextCardId;

            ButtonListener(nextCardId, newCard);

        }
        if (Keyboard.current.digit3Key.wasPressedThisFrame)
        {
            DisCard(2);

            newCard = CardGenerate(nextCardId, 2);
            cardUseId[2] = nextCardId;

            ButtonListener(nextCardId, newCard);

        }
        if (Keyboard.current.digit4Key.wasPressedThisFrame)
        {
            DisCard(3);

            newCard = CardGenerate(nextCardId, 3);
            cardUseId[3] = nextCardId;

            ButtonListener(nextCardId, newCard);
        }

    }

    GameObject CardGenerate(int ran, int chan) // 特定のカードを捨て、新たにランダムなカードを生成する
    {
        GameObject genCard =Instantiate(cardPrefab[ran], deckCardTrans[chan]); // カードを作る処理

        return genCard;
    }

    void DisCard(int chan)
    {
        Destroy(deckCardTrans[chan].GetChild(0).gameObject);
    }

    void ButtonListener(int Ind, GameObject targetCard)
    {

        Button btn = targetCard.GetComponentInChildren<Button>();

        // ボタンコンポーネントがついていなかった場合の安全装置
        if (btn == null) return;

        switch (Ind) 
        {
            case 0:

                btn.onClick.AddListener(skill.Enhance);

                break;

            case 1:

                btn.onClick.AddListener(skill.Heal);

                break;

            case 2:

                btn.onClick.AddListener(skill.Slash);

                break;

            case 3:

                btn.onClick.AddListener(skill.FireBall);

                break;
        }
        
    }
}
