using System;
using UnityEngine;

public class CardEffectManager : MonoBehaviour
{

    [SerializeField] float healAmount;
    float healDefaultAmount;
    
    [SerializeField] float curseAmount;
    float curseDefaultAmount;
    
    [SerializeField] int boostTime;

    int boostCount = 0;
    
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
                () => FireBoost(),
                () => WaterBoost(),
                () => GroundBoost(),
                () => ThunderBoost(),
                () => SaintBoost(),
                () => CursedBoost(),
            };
        
        healDefaultAmount = healAmount;
        curseDefaultAmount = curseAmount;

        player = GameObject.Find("Player").GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        boostCount++;

        if(boostCount > 60 * boostTime)
        {
            EndBoost();
        }
    }

    void FireEnhance()
    {
        player.attackDamage *= 1.01f;

        Debug.Log("攻撃強化！");
    }

    void WaterEnhance()
    {
        player.attackCd *= 0.99f;
        player.attackTime *= 0.99f;

        player.defaultAttackCd = player.attackCd;
        player.defaultAttackTime = player.attackTime;

        Debug.Log("攻撃速度強化！");
    }

    void GroundEnhance()
    {
        Debug.Log("地属性のカードの効果"); // ここに地属性のカードの効果を実装
    }

    void ThunderEnhance()
    {
        player.speed *= 1.01f;
        player.defaultSpeed = player.speed;

        Debug.Log("移動速度強化！");
    }

    void Heal()
    {
        player.playerHP += healAmount;
        if (player.playerHP > player.maxPlayerHP)
        {
            player.playerHP = player.maxPlayerHP;
        }

        Debug.Log("体力回復！");
    }

    void Curse()
    {
        player.playerHP -= curseAmount;
        if (player.playerHP < curseAmount + 1)
        {
            player.playerHP = 1;
        }

        Debug.Log("体力減少！");
    }

    void FireBoost()
    {
        player.attackDamage *= 1.3f;
        player.defaultAttackDamage = player.attackDamage;

        boostCount = 0;
    }

    void WaterBoost()
    {
        player.attackCd *= 0.7f;
        

        boostCount = 0;
    }

    void GroundBoost()
    {
        Debug.Log("一時的に防御力上昇！");
    }

    void ThunderBoost()
    {
        player.speed *= 1.3f;

        boostCount = 0;
    }

    void SaintBoost()
    {
        healAmount *= 1.3f;

        boostCount = 0;
    }

    void CursedBoost()
    {
        curseAmount *= 1.3f;

        curseAmount = 0;
    }
    void EndBoost()
    {
        player.attackDamage = player.defaultAttackDamage;
        player.attackCd = player.defaultAttackCd;
        player.attackTime = player.defaultAttackTime;
        Debug.Log("防御力が元に戻った");
        healAmount = healDefaultAmount;
        curseAmount = curseDefaultAmount;

        curseAmount = 0;
    }
}
