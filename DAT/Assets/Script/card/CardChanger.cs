using UnityEngine;

public class CardChanger : MonoBehaviour
{
    public int cardType; // カードの種類
    public string cardName; // カード名
    public string cardEffect; // カードの効果
    public Sprite cardImage; // カードのイメージ

    int[] types = { 0, 1, 2, 3, 4, 5 };

    string[] names = { 
        "FireEnhance",
        "WaterEnhance",
        "ThunderEnhance",
        "GroundEnhance",
        "Heal",
        "Curse" 
    };


    string[] effects = {
        "攻撃力が１％上がる　(永続)",
        "攻撃速度が１％上がる　(永続)",
        "移動速度が１％上がる　(永続)",
        "防御力が１％上がる　(永続)",
        "体力１０回復",
        "体力１０減少"
    };


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    // カードの種類を変更する関数
    public int CardChange(int id)
    {

        cardType = types[id];
        cardName = names[id];
        cardEffect = effects[id];

        return types[id];

    }
}
