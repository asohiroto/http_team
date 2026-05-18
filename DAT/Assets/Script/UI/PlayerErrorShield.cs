using UnityEngine;

public class PlayerErrorShield : MonoBehaviour
{
    void Awake()
    {
        // ヒエラルキー上から「Player」という名前のオブジェクトを探す
        GameObject player = GameObject.Find("Player");

        if (player != null)
        {
            // 土台のStart()（40行目など）が実行される前に、
            // 子オブジェクトを強制的に10個作ってエラーを物理的に防ぐ
            while (player.transform.childCount < 10)
            {
                GameObject dummy = new GameObject("SafeDummy");
                dummy.transform.SetParent(player.transform);
            }
        }
    }
}