using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameSceneManager : MonoBehaviour
{
    public static GameSceneManager Instance;

    MyDeck deck;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        deck = GameObject.Find("MyDeck").GetComponent<MyDeck>();
    }

    // Update is called once per frame
    void Update()
    {
        // 【デバッグ用】各シーンへの切り替え
        if (Keyboard.current.lKey.wasPressedThisFrame)
        {
            SceneManager.LoadScene("ClearScene");
        }
        else if (Keyboard.current.kKey.wasPressedThisFrame)
        {
            SceneManager.LoadScene("GameOverScene");
        }
    }

}
