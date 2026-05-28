using UnityEngine;

public class Coin : MonoBehaviour
{
    [SerializeField] private int amount = 100; // コイン1枚の価値

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (CoinManager.instance != null)
            {
                // マネージャーにお金を加算してもらう
                CoinManager.instance.AddMoney(amount);
            }

            Debug.Log($"{amount}円！");
            Destroy(gameObject);    //コインを取ったらお金オブジェクトを削除
        }
    }
}