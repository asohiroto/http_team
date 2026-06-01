using UnityEngine;
using TMPro;

public class CoinManager : MonoBehaviour
{
    [Header("UI設定")]
    [SerializeField] private TextMeshProUGUI moneyText; 
    [SerializeField] private string unit = "G";      


    public int currentMoney = 0; // 所持金

    
    private void Start()
    {
        UpdateMoneyUI();
    }



    // 所持金増加
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

    // 所持金表示更新
    public void UpdateMoneyUI()
    {
        if (moneyText != null)
        {
            // 「所持金：100円」の形式で表示
            moneyText.text = $"{currentMoney} ";
        }
    }
}