using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class DeckSceneManager : MonoBehaviour
{
    DeckRegistrate regis;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        regis = GameObject.Find("Panel").GetComponentInChildren<DeckRegistrate>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DeckSceneChange()
    {
        if (regis.CheckMyDeck(-1))
        {
            SceneManager.LoadScene("AllSceneProto");
        }
        else
        {
            Debug.Log("できぬ");
        }
    }


}
