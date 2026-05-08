using UnityEngine;

public class Coin : MonoBehaviour
{
    public int value = 100; // このコインの金額

    // トリガー（接触）判定
    private void OnTriggerEnter(Collider other)
    {
        // ぶつかった相手が「Player」タグを持っているか確認
        if (other.CompareTag("Player"))
        {
            // 相手（プレイヤー）のWalletスクリプトを取得
            Wallet wallet = other.GetComponent<Wallet>();

            if (wallet != null)
            {
                // お金を増やす処理を呼ぶ
                wallet.AddMoney(value);

                // このお金オブジェクトを消す
                Destroy(gameObject);
            }
        }
    }
}