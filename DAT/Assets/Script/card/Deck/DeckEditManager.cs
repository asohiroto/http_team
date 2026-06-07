using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.XR;

public class DeckEditManager : MonoBehaviour, IDropHandler
{
    int cardId;

    [SerializeField] int cardIndex;

    GameObject handManager;

    HandManager hand;
    DeckManager deck;
    CardChanger change;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        handManager = GameObject.Find("HandManager");

        hand = handManager.GetComponent<HandManager>();
        change = handManager.GetComponent<CardChanger>();
        deck = GameObject.Find("DeckManager").GetComponent<DeckManager>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnDrop(PointerEventData eventData)
    {
        DraggableCard dragged = eventData.pointerDrag?.GetComponent<DraggableCard>();

        DeckEdit(dragged.cardId, cardIndex);
    }

    public void DeckEdit(int id, int index)
    {
        GameObject genCard = Instantiate(hand.cardPrefab[id], deck.deckTrans[index]); // カードを作る処理
        DraggableCard dc = genCard.GetComponentInChildren<DraggableCard>();

        CardEdit edit = genCard.GetComponentInChildren<CardEdit>();

        edit.ChangeCardName(change.cardName);
        edit.ChangeCardEffect(change.cardEffect);

        if (dc != null)
        {
            dc.cardIndex = index;
            dc.cardId = id;
        }

    }
}

