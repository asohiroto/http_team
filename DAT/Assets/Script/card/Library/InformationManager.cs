using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class InformationManager : MonoBehaviour
{
    [SerializeField] TMP_Text cardName;
    [SerializeField] TMP_Text cardEffect;
    [SerializeField] Image cardImage;

    LibraryManager libManager;
    CardChanger changer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        libManager = FindFirstObjectByType<LibraryManager>();
        changer = GetComponent<CardChanger>();

        changer.CardChange(libManager.cardId);

        cardName.text = changer.cardName;
        cardEffect.text = changer.cardEffect;

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void CloseScene()
    {
        SceneManager.UnloadSceneAsync("InformationScene");
    }

}
