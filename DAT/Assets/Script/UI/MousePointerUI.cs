using UnityEngine;

public class MouseFollowerUI : MonoBehaviour
{
    // 作る必要はなく、ここで「使うよ」と名前だけ宣言しています
    private RectTransform rectTransform;

    void Start()
    {
        // ゲームが始まった瞬間に、Unityが自動的に自分のパーツ（RectTransform）を探してくれます
        rectTransform = GetComponent<RectTransform>();

        // ゲーム開始時は矢印を消しておく
        Cursor.visible = false;
    }

    void Update()
    {
               
    }
}