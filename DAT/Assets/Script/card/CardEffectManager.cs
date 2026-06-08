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

    bool boostFlag = false;

    // ファイアボール関連----------------------
    [SerializeField] float fbSpeed;

    [SerializeField] GameObject fireBall;

    public GameObject fireBallPrefab;

    Vector2 destPos = Vector2.zero;
    Vector2 fbPos = Vector2.zero;

    bool fbFlag = false;

    // カースドフレイム関連--------------------
    [SerializeField] float cfSpeed;

    [SerializeField] GameObject cursedFlame;

    public GameObject cursedFlamePrefab;

    Vector2 cfPos = Vector2.zero;

    bool cfFlag = false;

    // 共通------------------------------------
    PlayerController player;
    CardAttackReach reach;
    SkillManager skill;

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
                () => FireBall(),
                () => CursedFlame(),
                () => OverHeal(),
            };

        healDefaultAmount = healAmount;
        curseDefaultAmount = curseAmount;

        player = GameObject.Find("Player").GetComponent<PlayerController>();
        reach = GameObject.Find("Player").GetComponent<CardAttackReach>();
        skill = GameObject.Find("SkillManager").GetComponent<SkillManager>();
    }

    // Update is called once per frame
    void Update()
    {
        boostCount++;

        if (boostCount > 60 * boostTime && boostFlag == true)
        {
            Debug.Log("kaijo!");
            EndBoost();
        }

        // ファイアボール使用後の挙動
        if (fbFlag) skill.BallMove(fireBallPrefab, destPos, fbSpeed, fbPos, ref fbFlag);

        // カースドフレイム使用後の挙動
        if (cfFlag) skill.BallMove(cursedFlamePrefab, destPos, cfSpeed, cfPos, ref cfFlag);
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
        Debug.Log("ブースト中");

        player.attackDamage *= 1.3f;
        player.defaultAttackDamage = player.attackDamage;

        boostFlag = true;
        boostCount = 0;
    }

    void WaterBoost()
    {
        Debug.Log("ブースト中");
        player.attackCd *= 0.7f;

        boostFlag = true;
        boostCount = 0;
    }

    void GroundBoost()
    {
        Debug.Log("ブースト中");
        Debug.Log("一時的に防御力上昇！");
    }

    void ThunderBoost()
    {
        Debug.Log("ブースト中");
        player.speed *= 1.3f;

        boostFlag = true;
        boostCount = 0;
    }

    void SaintBoost()
    {
        Debug.Log("ブースト中");
        healAmount *= 1.3f;

        boostFlag = true;
        boostCount = 0;
    }

    void CursedBoost()
    {
        Debug.Log("ブースト中");
        curseAmount *= 1.3f;

        boostFlag = true;
        boostCount = 0;
    }

    void FireBall()
    {
        Debug.Log("使ったよ");

        if (!fbFlag)
        {
            destPos = reach.playerToEnemyNol;

            fbPos = destPos;

            float angle = Mathf.Atan2(destPos.y, destPos.x) * Mathf.Rad2Deg;

            // オブジェクトを作成する
            GameObject obj = Instantiate(fireBall);
            obj.transform.position = player.currentPos;
            obj.transform.name = ("FireBall");
            obj.transform.rotation = Quaternion.Euler(0, 0, angle);

            fireBallPrefab = obj;

            fbFlag = true; // フラグを立てる
        }
    }

    void CursedFlame()
    {
        Debug.Log("使ってるよ");

        if (!cfFlag)
        {
            player.playerHP -= curseAmount * 2;

            destPos = reach.playerToEnemyNol;

            cfPos = destPos;

            // オブジェクトを作成する
            GameObject obj = Instantiate(cursedFlame);
            obj.transform.position = player.currentPos;
            obj.transform.name = ("CursedFlame");

            cursedFlamePrefab = obj;

            cfFlag = true; // フラグを立てる

        }
    }

    void OverHeal()
    {
        Debug.Log("つかうの？");

        player.playerHP += (healAmount * 2);
    }

    void EndBoost()
    {
        player.attackDamage = player.defaultAttackDamage;
        player.attackCd = player.defaultAttackCd;
        player.attackTime = player.defaultAttackTime;
        Debug.Log("防御力が元に戻った");
        healAmount = healDefaultAmount;
        curseAmount = curseDefaultAmount;

        boostCount = 0;
    }

}
