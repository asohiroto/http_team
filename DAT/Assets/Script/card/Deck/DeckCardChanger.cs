using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DeckCardChanger : MonoBehaviour
{
    [SerializeField] public Image cardBase;
    [SerializeField] public TMP_Text cardName;

    CardChanger change;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void DeckCardChange(int cardId)
    {
        change = GameObject.Find("DeckManager").GetComponent<CardChanger>();

        cardName.text = change.CardNameChange(cardId);

        int colorCode = change.CardIdChange(cardId);

        switch (colorCode)
        {
            case 0: cardBase.color = Color.red; break;
            case 1: cardBase.color = Color.blue; break;
            case 2: cardBase.color = Color.brown; break;
            case 3: cardBase.color = Color.yellow; break;
            case 4: cardBase.color = Color.softYellow; break;
            case 5: cardBase.color = Color.black; break;
        }
    }
}
