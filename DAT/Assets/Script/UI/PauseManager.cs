using System.Runtime.CompilerServices;
using UnityEngine;


public class PauseManager : MonoBehaviour
{
    [SerializeField] private AudioSource bgmAudioSource;
    public void PauseGame()
    {
        // ゲームを停止する
        Time.timeScale = 0f;
        bgmAudioSource.Pause(); //BGM一時停止
        Debug.Log("ゲーム停止");
    }

    public void ResumeGame()
    {
        // ゲーム再開
        Time.timeScale = 1f;
        bgmAudioSource.UnPause();   // BGMを途中から再開
        Debug.Log("ゲーム再開");
    }
}