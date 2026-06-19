using UnityEngine;

public class PauseManager : MonoBehaviour
{
    public void PauseGame()
    {
        // ゲームを停止する
        Time.timeScale = 0f;
        Debug.Log("ゲーム停止");
    }

    public void ResumeGame()
    {
        // ゲーム再開
        Time.timeScale = 1f;
        Debug.Log("ゲーム再開");
    }
}