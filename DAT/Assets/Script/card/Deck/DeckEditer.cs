using UnityEngine;
using UnityEngine.EventSystems;

public class DeckEditer : MonoBehaviour, IDropHandler
{
    MyDeck deck;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        deck = GameObject.Find("MyDeck").GetComponent<MyDeck>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnDrop(PointerEventData eventData)
    {
        DraggableCard dragged = eventData.pointerDrag?.GetComponent<DraggableCard>();

        if (dragged == null) return;

        for(int i = 0;i < 8; i++)
        {
            if (deck.myDeckId[i] == -1)
            {
                deck.myDeckId[i] = dragged.cardId;
                break;
            }
        }
    }
}
