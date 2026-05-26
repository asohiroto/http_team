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

    int cardId = 0; // 生成するカードのID
    public int[] cardUseId = new int[10]; // 配列に保存されているカードIDを保存する配列

    skill_manager skill;
    GameObject newCard;
    CraftManager craft;

    public bool[] isCardPressed;

    void Start()
    {
        skill = GameObject.Find("SkillManager").GetComponent<skill_manager>();
        craft = GetComponent<CraftManager>();

        isCardPressed = new bool[deckCardTrans.Length];

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

    public GameObject CardGenerate(int ran, int chan) // 新たにランダムなカードを生成する
    {
        GameObject genCard = Instantiate(cardPrefab[ran], deckCardTrans[chan]); // カードを作る処理
        return genCard;
    }

    public void CardDraw()
    {
        int cardRandomId = Random.Range(0, 4);

        for (int i = 0; i < 4; i++)
        {
            if (deckCardTrans[i].childCount == 0)
            {
                GameObject obj = CardGenerate(cardRandomId, i);
                ButtonListener(cardRandomId, obj, i);

                break;
            }
        }

    }

    public void DisCard(int chan) // 手札を捨てる
    {
        if (deckCardTrans[chan].childCount < 0)
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

        ButtonPointerUpListener pointerUp = btn.gameObject.AddComponent<ButtonPointerUpListener>();

        pointerUp.onPointerDown.AddListener(() => isCardPressed[index] = true);
        pointerUp.onPointerUp.AddListener(() => isCardPressed[index] = false);


        switch (id)
        {
            case 0:

                pointerUp.onPointerClick.AddListener(async () => await skill.Enhance(index)); // ボタンの入力を検知するリスナーを付与
                pointerUp.onPointerDown.AddListener(() => skill.CraftMethod(id, index));
                //pointerUp.onPointerUp.AddListener(() => skill.CraftMethod(id, index));
                break;

            case 1:

                pointerUp.onPointerClick.AddListener(async () => await skill.Heal(index));
                pointerUp.onPointerDown.AddListener(() => skill.CraftMethod(id, index));
                //pointerUp.onPointerUp.AddListener(() => skill.CraftMethod(id, index));
                break;

            case 2:

                pointerUp.onPointerClick.AddListener(async () => await skill.Slash(index));
                pointerUp.onPointerDown.AddListener(() => skill.CraftMethod(id, index));
                //pointerUp.onPointerUp.AddListener(() => skill.CraftMethod(id, index));
                break;

            case 3:

                pointerUp.onPointerClick.AddListener(async () => await skill.FireBall(index));
                pointerUp.onPointerDown.AddListener(() => skill.CraftMethod(id, index));
                //pointerUp.onPointerUp.AddListener(() => skill.CraftMethod(id, index));
                break;

            case 4:

                pointerUp.onPointerClick.AddListener(async () => await skill.FireSlash(index));
                pointerUp.onPointerDown.AddListener(() => skill.CraftMethod(id, index));
                //pointerUp.onPointerUp.AddListener(() => skill.CraftMethod(id, index));
                break;

            case 5:

                pointerUp.onPointerClick.AddListener(async () => await skill.HyperMode(index));
                pointerUp.onPointerDown.AddListener(() => skill.CraftMethod(id, index));
                //pointerUp.onPointerUp.AddListener(() => skill.CraftMethod(id, index));
                break;
        }

    }
}
