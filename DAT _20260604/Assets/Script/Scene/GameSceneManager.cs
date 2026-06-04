using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameSceneManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Keyboard.current.lKey.wasPressedThisFrame)
        {
            SceneManager.LoadScene("ClearScene");
        }
        else if(Keyboard.current.kKey.wasPressedThisFrame)
        {
            SceneManager.LoadScene("GameOverScene");
        }
    }
}
