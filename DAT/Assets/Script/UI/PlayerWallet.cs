using UnityEngine;

public class PlayerWallet : MonoBehaviour
{
    // 常に最新のUIから数字を読み取るため、startCoins や currentCoins の変数は削除しました
    [SerializeField] private GameObject coinTextObject;
    [SerializeField] private int useCoins = 10;

    void Start()
    {
        // 他のシステムがUIを更新するのを邪魔しないため、Start時は何もしません
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