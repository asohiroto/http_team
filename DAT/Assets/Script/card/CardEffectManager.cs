using System;
using UnityEngine;

public class CardEffectManager : MonoBehaviour
{
    public Action[] cardEffect;

    PlayerController player;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // 1つ星のカードの効果を格納する配列を作成
        cardEffect = new Action[]
            {
                () => FireEnhance(),
                () => WaterEnhance(),
                () => GroundEnhance(),
                () => ThunderEnhance(),
                () => Heal(),
                () => Curse(),
            };

        player = GameObject.Find("Player").GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void FireEnhance()
    {
        player.attackDamage *= 1.01f;

        Debug.Log("攻撃強化！");
    }

    void WaterEnhance()
    {
        player.attackCd *= 0.99f;

        Debug.Log("攻撃速度強化！");
    }

    void GroundEnhance()
    {
        Debug.Log("地属性のカードの効果"); // ここに地属性のカードの効果を実装
    }

    void ThunderEnhance()
    {
        player.speed *= 1.01f;

        Debug.Log("移動速度強化！");
    }

    void Heal()
    {
        player.playerHP += 10;
        if (player.playerHP > player.maxPlayerHP)
        {
            player.playerHP = player.maxPlayerHP;
        }

        Debug.Log("体力回復！");
    }

    void Curse()
    {
        player.playerHP -= 10;
        if (player.playerHP < 11)
        {
            player.playerHP = 1;
        }

        Debug.Log("体力減少！");
    }
}
