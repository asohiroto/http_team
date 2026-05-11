using UnityEngine;

public class Coin : MonoBehaviour
{
    // 接触したときに呼ばれる関数
    private void OnTriggerEnter(Collider other)
    {
        // 接触した相手のタグが "Player" かどうかを確認
        if (other.CompareTag("Player"))
        {
            // コイン獲得時の処理（ここにスコア加算などを書く）
            Debug.Log("コインを獲得しました！");

            // このオブジェクト（コイン）を削除
            Destroy(gameObject);
        }
    }
}