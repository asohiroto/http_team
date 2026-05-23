using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.XR;

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
        skill = GameObject.Find("SkillManager").GetComponent<skill_manager>();

        for (int i = 0; i < 4; i++) // それぞれの手札の位置にランダムなカードを生成
        {
            cardId = Random.Range(0, 4);

            newCard = Instantiate(cardPrefab[cardId], deckCardTrans[i]); // 初期手札の生成

            ButtonListener(cardId, newCard, i);

            cardUseId[i] = cardId;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    GameObject CardGenerate(int ran, int chan) // 新たにランダムなカードを生成する
    {
        GameObject genCard = Instantiate(cardPrefab[ran], deckCardTrans[chan]); // カードを作る処理
        return genCard;
    }

    public void CardDraw()
    {
        int cardRandomId = Random.Range(0, 4);

        for(int i = 0; i < 4; i++)
        {
            if(deckCardTrans[i].childCount == 0)
            {
                GameObject obj = CardGenerate(cardRandomId, i);
                ButtonListener(cardRandomId, obj, i);

                break;
            }
        }

    }


    public void DisCard(int chan) // 手札を捨てる
    {
        Destroy(deckCardTrans[chan].GetChild(0).gameObject);
    }

    void ButtonListener(int id, GameObject targetCard, int index) // ボタンの入力を検知する
    {

        Button btn = targetCard.GetComponentInChildren<Button>();

        // ボタンコンポーネントがついていなかった場合の安全装置
        if (btn == null) return;

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

                btn.onClick.AddListener(async() => await skill.FireBall(index));

                break;
        }

    }
}
