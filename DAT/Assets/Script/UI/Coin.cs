using UnityEngine;

public class Coin2D : MonoBehaviour
{
    // 2Dのトリガー判定
    private void OnTriggerEnter2D(Collider2D other)
    {
        // 1. まず何かが触れたら名前を出す（最優先デバッグ）
        Debug.Log("接触検知！ 相手の名前: " + other.name);

        // 2. タグが Player かどうか判定
        if (other.CompareTag("Player"))
        {
            Debug.Log("プレイヤーがコインをゲットしました！");

            // コインを消す
            Destroy(gameObject);
        }
    }
}