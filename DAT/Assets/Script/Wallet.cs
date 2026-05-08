using UnityEngine;
using TMPro; // TextMeshProを使うために必要

public class Wallet : MonoBehaviour
{
    public int money = 0;           // 所持金
    public TextMeshProUGUI scoreText; // UIテキストの参照

    void Start()
    {
        UpdateUI();
    }

    // お金を加算するメソッド
    public void AddMoney(int amount)
    {
        money += amount;
        UpdateUI();
    }

    // UI表示を更新する
    void UpdateUI()
    {
        scoreText.text = "Money: " + money.ToString();
    }
}