using UnityEngine;

public class SettingManager : MonoBehaviour
{
    // インスペクターから薄い背景（DarkBackground）を登録するための変数
    [SerializeField] private GameObject darkBackground;

    // ボタンが押された時に実行する関数
    public void ShowBackground()
    {
        Debug.Log("表示");

        if (darkBackground != null)
        {
            darkBackground.SetActive(true); // 背景を表示する
        }
        else
        {
            // もし背景オブジェクトが登録されていない場合の警告ログ
        }
    }

    // 背景を閉じたい時に実行する関数
    public void HideBackground()
    {
        Debug.Log("非表示");

        if (darkBackground != null)
        {
            darkBackground.SetActive(false); // 背景を非表示にする
        }
    }
}