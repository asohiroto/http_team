using System;
using UnityEngine;

public class CardEffectManager : MonoBehaviour
{
    // エンハンス関連-------------------------
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

    // ウォーターショット関連-----------------
    [SerializeField] float wsSpeed;

    [SerializeField] GameObject watershot;

    public GameObject waterShotPrefab;

    Vector2 wsPos = Vector2.zero;

    bool wsFlag = false;

    // グラウンドショット関連-----------------
    [SerializeField] float gsSpeed;

    [SerializeField] GameObject groundshot;

    public GameObject groundShotPrefab;

    Vector2 gsPos = Vector2.zero;

    bool gsFlag = false;

    // サンダーボール関連--------------------
    [SerializeField] float tbSpeed;

    [SerializeField] GameObject thunderball;

    public GameObject thunderballPrefab;

    Vector2 tbPos = Vector2.zero;

    bool tbFlag = false;

    // ラバスプラッシュ関連--------------------
    [SerializeField] GameObject lavaSprash;

    public GameObject lavaSprashPrefab;

    Vector2 lsPos = Vector2.zero;

    // ウォータースプラッシュ関連--------------
    [SerializeField] GameObject waterSprash;

    public GameObject waterSprashPrefab;

    Vector2 wspPos = Vector2.zero;

    // グラウンドスパイク関連-----------------
    [SerializeField] GameObject groundSpike;

    public GameObject groundSpikePrefab;

    Vector2 gspPos = Vector2.zero;

    // サンダーボルト関連---------------------
    [SerializeField] GameObject thunderBolt;

    public GameObject thunderBoltPrefab;

    Vector2 tboPos = Vector2.zero;

    // ヘブンズフューリー関連-----------------
    [SerializeField] GameObject heavensFury;

    public GameObject heavensFuryPrefab;

    Vector2 hfPos = Vector2.zero;

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
                () => WaterShot(),
                () => GroundShot(),
                () => ThunderBall(),
                () => LavaSprash(),
                () => WaterSprash(),
                () => GroundSpike(),
                () => ThunderBolt(),
                () => HeavensFury(),
            };

        healDefaultAmount = healAmount;
        curseDefaultAmount = curseAmount;

        player = GameObject.Find("Player").GetComponent<PlayerController>();
        reach = GameObject.Find("Player").GetComponentInChildren<CardAttackReach>();
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

        // ウォーターショット使用後の挙動
        if (wsFlag) skill.BallMove(waterShotPrefab, destPos, wsSpeed, wsPos, ref wsFlag);

        // グランドショット使用後の挙動
        if (gsFlag) skill.BallMove(groundShotPrefab, destPos, gsSpeed, gsPos, ref gsFlag);

        // サンダーボール使用後の挙動
        if (tbFlag) skill.BallMove(thunderballPrefab, destPos, tbSpeed, tbPos, ref tbFlag);
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
        string name = ("FireBall");

        BallSkill(ref fbFlag, ref destPos, ref fbPos, fireBall, name, ref fireBallPrefab);
    }

    void CursedFlame()
    {
        string name = ("CursedFlame");

        BallSkill(ref cfFlag, ref destPos, ref cfPos, cursedFlame, name, ref cursedFlamePrefab);

        player.playerHP -= (curseAmount * 2);
    }

    void OverHeal()
    {
        Debug.Log("つかうの？");

        player.playerHP += (healAmount * 2);
    }

    void WaterShot()
    {
        string name = ("WaterShot");

        BallSkill(ref wsFlag, ref destPos, ref wsPos, watershot, name, ref waterShotPrefab);
    }

    void GroundShot()
    {
        string name = ("GroundShot");

        BallSkill(ref gsFlag, ref destPos, ref gsPos, groundshot, name, ref groundShotPrefab);
    }

    void ThunderBall()
    {
        string name = ("ThunderBall");

        BallSkill(ref tbFlag, ref destPos, ref tbPos, thunderball, name, ref thunderballPrefab);
    }

    void LavaSprash()
    {
        string name = ("LavaSprash");

        SpikeSkill(lsPos, lavaSprash, name, ref lavaSprashPrefab);
    }

    void WaterSprash()
    {
        string name = ("WaterSprash");

        SpikeSkill(wspPos, waterSprash, name, ref waterSprashPrefab);
    }

    void GroundSpike()
    {
        string name = ("GroundSpike");

        SpikeSkill(gspPos, groundSpike, name, ref groundSpikePrefab);
    }

    void ThunderBolt()
    {
        string name = ("ThunderBolt");

        SpikeSkill(tboPos, thunderBolt, name, ref thunderBoltPrefab);
    }

    void HeavensFury()
    {
        string name = ("heavensFury");

        SpikeSkill(hfPos, heavensFury, name, ref heavensFuryPrefab);
    }

    void BallSkill(ref bool flag, ref Vector2 destPos, ref Vector2 objPos, GameObject skill, string name, ref GameObject skillPrefab)
    {
        if (!flag)
        {
            destPos = reach.playerToEnemyNol;

            objPos = destPos;

            float angle = Mathf.Atan2(destPos.y, destPos.x) * Mathf.Rad2Deg;

            // オブジェクトを作成する
            GameObject obj = Instantiate(skill);
            obj.transform.position = player.currentPos;
            obj.transform.name = name;
            obj.transform.rotation = Quaternion.Euler(0, 0, angle);

            skillPrefab = obj;

            flag = true; // フラグを立てる
        }
    }

    void SpikeSkill(Vector2 objPos, GameObject skill, string name, ref GameObject skillPrefab)
    {
        objPos = reach.mousePosWorld;

        GameObject obj = GameObject.Instantiate(skill);
        obj.transform.position = objPos;
        obj.transform.name = name;

        skillPrefab = obj;
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
