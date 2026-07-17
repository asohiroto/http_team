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

    Vector2 buffPos;

    // 攻撃力-------------
    [SerializeField] GameObject atkObj;

    GameObject atkPrefab;

    // 防御力-------------
    [SerializeField] GameObject defObj;

    GameObject defPrefab;

    // 移動速度-----------
    [SerializeField] GameObject spdObj;

    GameObject spdPrefab;

    // 攻撃速度-----------
    [SerializeField] GameObject atkspdObj;

    GameObject atkspdPrefab;

    // 回復---------------
    [SerializeField] GameObject healObj;
    [SerializeField] GameObject overhealObj;

    GameObject healPrefab;
    GameObject overHealPrefab;

    // 反回復-------------
    [SerializeField] GameObject dishealobj;

    GameObject dishealprefab;

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

    // モルテンスピア関連---------------------
    [SerializeField] GameObject moltenSpear;

    public GameObject moltenSpearPrefab;

    Vector2 msPos = Vector2.zero;

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
                () => MoltenSpear(),
            };

        healDefaultAmount = healAmount;
        curseDefaultAmount = curseAmount;

        player = GameObject.Find("Player").GetComponent<PlayerController>();
        reach = GameObject.Find("Player").GetComponentInChildren<CardAttackReach>();
        skill = GameObject.Find("SkillManager").GetComponent<SkillManager>();
    }

    // Update is called once per frame
    void FixedUpdate()
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
        buffPos.x = player.currentPos.x;
        buffPos.y = player.currentPos.y + 1.0f;

        // エフェクトをプレイヤーに追従
        if (atkPrefab != null) atkPrefab.transform.position = buffPos;
        if (atkspdPrefab != null) atkspdPrefab.transform.position = buffPos;
        if (defPrefab != null) defPrefab.transform.position = buffPos;
        if (spdPrefab != null) spdPrefab.transform.position = buffPos;
        if (healPrefab != null) healPrefab.transform.position = buffPos;
        if (dishealprefab != null) dishealprefab.transform.position = buffPos;
        if (overHealPrefab != null) overHealPrefab.transform.position = player.currentPos;
    }

    void FireEnhance()
    {
        player.attackDamage *= 1.01f;
        player.defaultAttackDamage = player.attackDamage;

        atkPrefab = Effect(atkObj, buffPos);

        Debug.Log("攻撃強化！");
    }

    void WaterEnhance()
    {
        player.attackCd *= 0.99f;
        player.attackTime *= 0.99f;

        player.defaultAttackCd = player.attackCd;
        player.defaultAttackTime = player.attackTime;

        atkspdPrefab = Effect(atkspdObj, buffPos);

        Debug.Log("攻撃速度強化！");
    }

    void GroundEnhance()
    {
        defPrefab = Effect(defObj, buffPos);

        player.playerDefence *= 1.01f;
        player.defaultDefence *= 1.01f;

        Debug.Log("地属性のカードの効果"); // ここに地属性のカードの効果を実装
    }

    void ThunderEnhance()
    {
        player.speed *= 1.01f;
        player.defaultSpeed *= 1.01f;

        spdPrefab = Effect(spdObj, buffPos);

        Debug.Log("移動速度強化！");
    }

    void Heal()
    {
        player.playerHP += healAmount;
        if (player.playerHP > player.maxPlayerHP)
        {
            player.playerHP = player.maxPlayerHP;
        }

        healPrefab = Effect(healObj, buffPos);

        Debug.Log("体力回復！");
    }

    void Curse()
    {
        player.playerHP -= curseAmount;
        if (player.playerHP < curseAmount + 1)
        {
            player.playerHP = 1;
        }

        dishealprefab = Effect(dishealobj, buffPos);

        Debug.Log("体力減少！");
    }

    void FireBoost()
    {
        Debug.Log("ブースト中");

        player.attackDamage *= 1.3f;

        atkPrefab = Effect(atkObj, buffPos);

        boostFlag = true;
        boostCount = 0;
    }

    void WaterBoost()
    {
        Debug.Log("ブースト中");
        player.attackCd *= 0.7f;

        atkspdPrefab = Effect(atkspdObj, buffPos);

        boostFlag = true;
        boostCount = 0;
    }

    void GroundBoost()
    {
        Debug.Log("ブースト中");
        Debug.Log("一時的に防御力上昇！");

        player.playerDefence *= 0.7f;

        defPrefab = Effect(defObj, buffPos);
    }

    void ThunderBoost()
    {
        Debug.Log("ブースト中");
        player.speed *= 1.3f;

        spdPrefab = Effect(spdObj, buffPos);

        boostFlag = true;
        boostCount = 0;
    }

    void SaintBoost()
    {
        Debug.Log("ブースト中");
        healAmount *= 1.3f;

        healPrefab = Effect(healObj, buffPos);

        boostFlag = true;
        boostCount = 0;
    }

    void CursedBoost()
    {
        Debug.Log("ブースト中");
        curseAmount *= 1.3f;

        dishealprefab = Effect(dishealobj, buffPos);

        boostFlag = true;
        boostCount = 0;
    }

    void FireBall()
    {
        string name = ("FireBall");

        BallSkill(ref fbFlag, ref destPos, ref fbPos, fireBall, name, ref fireBallPrefab, player.currentPos);
    }

    void CursedFlame()
    {
        string name = ("CursedFlame");

        BallSkill(ref cfFlag, ref destPos, ref cfPos, cursedFlame, name, ref cursedFlamePrefab, player.currentPos);

        player.playerHP -= (curseAmount * 2);
    }

    void OverHeal()
    {
        Debug.Log("つかうの？");

        overHealPrefab = Effect(overhealObj, player.currentPos);

        player.playerHP += (healAmount * 2);
    }

    void WaterShot()
    {
        string name = ("WaterShot");

        BallSkill(ref wsFlag, ref destPos, ref wsPos, watershot, name, ref waterShotPrefab, player.currentPos);
    }

    void GroundShot()
    {
        string name = ("GroundShot");

        BallSkill(ref gsFlag, ref destPos, ref gsPos, groundshot, name, ref groundShotPrefab, player.currentPos);
    }

    void ThunderBall()
    {
        string name = ("ThunderBall");

        BallSkill(ref tbFlag, ref destPos, ref tbPos, thunderball, name, ref thunderballPrefab, player.currentPos);
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
        string name = ("HeavensFury");

        SpikeSkill(hfPos, heavensFury, name, ref heavensFuryPrefab);
    }


    void MoltenSpear()
    {
        string name = ("MoltenSpear");

        player.playerHP -= (curseAmount * 2);

        SpikeSkill(msPos, moltenSpear, name, ref moltenSpearPrefab);
    }

    void BallSkill(ref bool flag, ref Vector2 destPos, ref Vector2 objPos, GameObject skill, string name, ref GameObject skillPrefab, Vector2 skillPos)
    {
        if (!flag)
        {
            destPos = reach.playerToEnemyNol;

            objPos = destPos;

            float angle = Mathf.Atan2(destPos.y, destPos.x) * Mathf.Rad2Deg;

            // オブジェクトを作成する
            GameObject obj = Instantiate(skill);
            obj.transform.position = skillPos;
            obj.transform.name = name;
            obj.transform.rotation = Quaternion.Euler(0, 0, angle);

            skillPrefab = obj;

            flag = true; // フラグを立てる

            AudioSource.PlayClipAtPoint(skill.GetComponent<AudioSource>().clip, obj.transform.position);
        }
    }

    void SpikeSkill(Vector2 objPos, GameObject skill, string name, ref GameObject skillPrefab)
    {
        objPos = reach.mousePosWorld;

        GameObject obj = GameObject.Instantiate(skill);
        obj.transform.position = objPos;
        obj.transform.name = name;

        AudioSource.PlayClipAtPoint(obj.GetComponent<AudioSource>().clip, obj.transform.position);

        skillPrefab = obj;
    }

    public GameObject Effect(GameObject skill, Vector2 pos)
    {
        GameObject obj = GameObject.Instantiate(skill);
        obj.transform.position = pos;

        return obj;
    }

    void EndBoost()
    {
        player.attackDamage = player.defaultAttackDamage;
        player.attackCd = player.defaultAttackCd;
        player.attackTime = player.defaultAttackTime;
        player.speed = player.defaultSpeed;
        player.playerDefence = player.defaultDefence;
        Debug.Log("防御力が元に戻った");
        healAmount = healDefaultAmount;
        curseAmount = curseDefaultAmount;

        boostCount = 0;
    }
}
