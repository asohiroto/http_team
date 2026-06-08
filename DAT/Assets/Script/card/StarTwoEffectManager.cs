using System;
using UnityEngine;
using UnityEngine.XR;

public class StarTwoEffectManager : MonoBehaviour
{
    // ファイアボール関連----------------------
    [SerializeField] float fbSpeed;

    [SerializeField] GameObject fireBall;

    public GameObject fireBallPrefab;

    Vector2 destPos = Vector2.zero;
    Vector2 fbPos = Vector2.zero;

    bool fbFlag = false;

    // カースドフレイム関連--------------------
    [SerializeField] float cfSpeed;
    [SerializeField] int curseAmount;

    [SerializeField] GameObject cursedFlame;

    public GameObject cursedFlamePrefab;

    Vector2 cfPos = Vector2.zero;

    bool cfFlag = false;

    // オーバーヒール関連---------------------
    [SerializeField] int overHealAmount;

    // 共通------------------------------------
    public Action[] starTwoEffect;

    PlayerController player;
    CardAttackReach reach;
    SkillManager skill;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.Find("Player").GetComponent<PlayerController>();
        reach = GameObject.Find("Player").GetComponent<CardAttackReach>();
        skill = GameObject.Find("SkillManager").GetComponent<SkillManager>();

        starTwoEffect = new Action[]
            {
                () => FireBall(),
                () => CursedFlame(),
                () => OverHeal(),
            };

    }

    // Update is called once per frame
    void Update()
    {
        // ファイアボール使用後の挙動
        if (fbFlag) skill.BallMove(fireBallPrefab, destPos, fbSpeed, fbPos, ref fbFlag);

        // カースドフレイム使用後の挙動
        if (cfFlag) skill.BallMove(cursedFlamePrefab, destPos, cfSpeed, cfPos, ref cfFlag);
    }

    void FireBall()
    {
        if (!fbFlag)
        {
            destPos = reach.playerToEnemyNol;

            fbPos = destPos;

            // マウスがさしたポイントへの単位ベクトルを作成
            destPos.Normalize();

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
        if (!cfFlag)
        {
            player.playerHP -= curseAmount * 2;

            destPos = reach.playerToEnemyNol;

            cfPos = destPos;

            destPos.Normalize();

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
        player.playerHP += overHealAmount;
    }
}
