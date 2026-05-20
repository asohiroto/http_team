using UnityEngine;
using TMPro;

public class CoinManager : MonoBehaviour
{
    public static CoinManager instance;

    [Header("UI設定")]
    [SerializeField] private TextMeshProUGUI moneyText; 
    [SerializeField] private string unit = "G";      


    private int currentMoney = 0; // 所持金

    private void Awake()
    {
        if (instance == null) { instance = this; }
    }

    private void Start()
    {
        UpdateMoneyUI();
    }



    // お金を追加する関数（コインから呼ばれる）
    public void AddMoney(int amount)
    {

        currentMoney += amount;
        UpdateMoneyUI();

    }

    public void ReduceMoney(int amount)　//　所持金マイナス防止
    {
        if(currentMoney < amount)
        {
            Debug.Log("購入できませんでした");
            return;
        }
        currentMoney -= amount;
        UpdateMoneyUI();
    }

    // 表示を更新する
    private void UpdateMoneyUI()
    {
        if (moneyText != null)
        {
            // 「所持金：100円」の形式で表示
            moneyText.text = $"Money: {currentMoney} ";
        }
    }
}