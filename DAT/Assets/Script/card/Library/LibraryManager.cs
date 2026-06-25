using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LibraryManager : MonoBehaviour
{
    [SerializeField] Transform[] LibTrans;
    [SerializeField] GameObject[] LibCards;

    CardChanger changer;

    public int cardId;
 
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        changer = GetComponent<CardChanger>();

        for (int i = 0; i < changer.cardData.Count; i++)
        {
            int id = changer.CardChange(i);

            int tempId = i;

            GameObject genCard = Instantiate(LibCards[id], LibTrans[i]);

            Button but = genCard.GetComponent<Button>();
            but.onClick.AddListener(() => InformationAwake(tempId));

            CardEdit edit = genCard.GetComponentInChildren<CardEdit>();

            edit.ChangeCardName(changer.cardName);
            edit.ChangeCardEffect(changer.cardEffect);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    void InformationAwake(int id)
    {
        cardId = id;

        SceneManager.LoadSceneAsync("InformationScene", LoadSceneMode.Additive);
    }
}
