using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class DataPanelManager : MonoBehaviour
{
    [SerializeField] TMP_Text cardName;
    [SerializeField] Transform cardImageTrans;
    [SerializeField] TMP_Text cardEffect;
    [SerializeField] TMP_Text cardEvolution;

    CardChanger changer;
    DeckManager deck;
    CraftManager craft;

    string currentScene;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        changer = GameObject.Find("DeckManager").GetComponent<CardChanger>();
        deck = GameObject.Find("DeckManager").GetComponent<DeckManager>();
        craft = GameObject.Find("CraftManager").GetComponent<CraftManager>();

        currentScene = SceneManager.GetActiveScene().name;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void CardDataPanel(int id)
    {
        if (currentScene == "DeckScene")
        {
            CardData data = changer.cardData[id];

            cardName.text = data.cardName;

            cardEffect.text = data.cardEffect;

            int type = changer.CardChange(id);

            GameObject obj = Instantiate(deck.possessionCards[type], cardImageTrans);
            DraggableCard dc = obj.GetComponentInChildren<DraggableCard>();
            CardEdit edit = obj.GetComponentInChildren<CardEdit>();

            edit.ChangeCardName(changer.cardName);
            edit.ChangeCardEffect(changer.cardEffect);

            if (dc != null)
            {
                dc.cardId = id;
            }

            int evoId = craft.CraftCards(id, id);
            if (evoId > 0)
            {
                CardData evoData = changer.cardData[evoId];
                cardEvolution.text = evoData.cardName;
            }
            else
            {
                cardEvolution.text = "ないよ";
            }
        }
    }
}
