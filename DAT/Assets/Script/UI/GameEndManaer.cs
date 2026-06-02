using UnityEngine;

public class GameEndManager : MonoBehaviour
{
    // ボタンが押されたときに呼び出すメソッド
    public void QuitGame()
    {
#if UNITY_EDITOR
        // Unityエディタ上では再生モードをオフにする
        UnityEditor.EditorApplication.isPlaying = false;
#else
            // ビルドした実際のゲームではアプリを終了する
            Application.Quit();
#endif
    }
}