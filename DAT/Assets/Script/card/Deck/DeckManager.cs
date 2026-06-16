using TMPro;
using UnityEngine;

public class DeckManager : MonoBehaviour
{
    [SerializeField] public Transform[] cardTrans;
    [SerializeField] public GameObject[] possessionCards;

    [SerializeField] public Transform[] deckTrans;
    [SerializeField] public GameObject deckCard;

    [SerializeField] public TMP_Text deckCounter;

    public int deckCount = 0;

    CardChanger change;

    DeckCardChanger changer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        change = GetComponent<CardChanger>();

        for (int i = 0; i < cardTrans.Length; i++)
        {
            CardGenerate(i);
        }

        DeckCounter(deckCount);
    }

    // Update is called once per frame
    void Update()
    {

    }

    // カードを生成する関数
    public void CardGenerate(int ind) // 新たにカードを生成する
    {
        int id = change.CardChange(ind);

        GameObject genCard = Instantiate(possessionCards[id], cardTrans[ind]); // カードを作る処理
        DraggableCard dc = genCard.GetComponentInChildren<DraggableCard>();

        CardEdit edit = genCard.GetComponentInChildren<CardEdit>();

        edit.ChangeCardName(change.cardName);
        edit.ChangeCardEffect(change.cardEffect);

        if (dc != null)
        {
            dc.cardIndex = ind;
            dc.cardId = ind;
        }
    }

    public void DeckRegistrate(int i, int cardId)
    {

        GameObject obj = Instantiate(deckCard, deckTrans[i]);

        changer = obj.GetComponent<DeckCardChanger>();
        changer.DeckCardChange(cardId);

        DraggablePanel panel = obj.GetComponentInChildren<DraggablePanel>();

        panel.cardId = cardId;
        panel.cardInd = i;

        deckCount++;
        DeckCounter(deckCount);
    }

    public void DeckCounter(int count)
    {
        deckCounter.text = $"{count} / 6枚";
    }
}
