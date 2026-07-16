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

    }
}
