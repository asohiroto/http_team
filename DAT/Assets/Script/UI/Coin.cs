using UnityEngine;

public class Coin : MonoBehaviour
{
    [SerializeField] private int amount = 100; // コイン1枚の価値
    [SerializeField] private AudioClip coinSE;

    CoinManager coin;

    private void OnTriggerEnter2D(Collider2D other)
    {
        coin = GameObject.Find("CoinManager").GetComponent<CoinManager>();

        if (other.CompareTag("Player"))
        {
            // マネージャーにお金を加算してもらう
            coin.AddMoney(amount);

            // コインSE
            if (coinSE != null)
            {
                AudioSource.PlayClipAtPoint(coinSE, transform.position);
            }

            Debug.Log($"{amount}円！");
            Destroy(gameObject);    //コインを取ったらお金オブジェクトを削除
        }
    }
}