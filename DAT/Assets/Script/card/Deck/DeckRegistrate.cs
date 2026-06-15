using UnityEngine;
using UnityEngine.EventSystems;

public class DeckRegistrate : MonoBehaviour, IDropHandler
{
    MyDeck deck;
    DeckManager manager;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        deck = GameObject.Find("MyDeck").GetComponent<MyDeck>();
        manager = GameObject.Find("DeckManager").GetComponent<DeckManager>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnDrop(PointerEventData eventData)
    {
        DraggableCard dragged = eventData.pointerDrag?.GetComponent<DraggableCard>();

        for (int i = 0; i < 6; i++)
        {
            if (deck.myDeckId[i] == -1 && CheckMyDeck(dragged.cardId) == true)
            {
                deck.myDeckId[i] = dragged.cardId;

                manager.DeckRegistrate(i, dragged.cardId);

                break;

            }
        }
    }

    public bool CheckMyDeck(int id)
    {
        bool check = true;

        for (int i = 0; i < 6; i++)
        {
            if(id == deck.myDeckId[i])
            {
                check = false;
            }
        }

        return check;
    }
}
