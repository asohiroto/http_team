using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleBgm : MonoBehaviour
{
    private static TitleBgm instance;
    private AudioSource audioSource;

    void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);

        audioSource = GetComponent<AudioSource>();

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "StartScene")
        {
            // タイトルに戻ったら再生
            if (!audioSource.isPlaying)
            {
                audioSource.Play();
            }
        }
        else if (scene.name == "AllSceneProto" || scene.name == "ClearScene" || scene.name == "GameOverScene" || scene.name == "DeckScene" )
        {
            // クリアやゲームオーバーでは停止
            audioSource.Stop();
        }
    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}