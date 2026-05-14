using UnityEngine;

public class Coin2D : MonoBehaviour
{
    [SerializeField] private int amount = 100; // このコイン1枚の価値

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
            Destroy(gameObject);
        }
    }
}