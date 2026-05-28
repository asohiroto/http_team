using UnityEngine;

public class PlayerWallet : MonoBehaviour
{
    [SerializeField] private GameObject coinTextObject;
    [SerializeField] private int useCoins = 10;

    void Start()
    {
       //
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            if (CoinManager.instance != null)
            {
                // マネージャーにお金を加算してもらう
                CoinManager.instance.ReduceMoney(useCoins);
            }
        }
    }

}