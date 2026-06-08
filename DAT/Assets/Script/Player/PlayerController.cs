using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;


public class PlayerController : MonoBehaviour
{
    // 移動に使う変数-----------------------------------------------------------------
    [SerializeField] public float speed = 0.05f; // プレイヤーのスピード
    [SerializeField] public float defaultSpeed = 0.05f; // プレイヤーのデフォルトスピード
    [SerializeField] private float dashSpeed = 0.3f; // プレイヤーのダッシュスピード
    private bool onDash; // ダッシュ中かどうか

    float dirX; // プレイヤーのX軸方向 (-1, 0, 1)のどれか
    float dirY; // プレイヤーのy軸方向(-1, 0, 1)のどれか
    [SerializeField] float xLimit = 8.5f; // x軸の移動制限
    [SerializeField] float yMaxLimit = 4.7f; // y軸の移動制限
    [SerializeField] float yMinLimit = -1.6f; // y軸の移動制限
    public Vector3 currentPos; // プレイヤーの現在のポジション
    Vector3 moveDir; // 入力の向きを代入

    public Vector3 lastDir = new Vector3(1, 0, 0); // プレイヤーが前のフレームで入力した方向

    // ダッシュに使う変数---------------------------------------------------------------
    float dashCd = 0.3f; // ダッシュのクールダウン
    float dashCdTimer = 0f; // クールダウンを実際にカウントする変数
    [SerializeField] float dashTime = 0.1f; // ダッシュの継続時間
    Vector3 dashDir; // ダッシュする方向
    [SerializeField] GameObject dashLeftObj;
    [SerializeField] GameObject dashUpObj;

    // アタックに使う変数---------------------------------------------------------------
    PlayerAttack playerAttack;
    PlayerAttackReach playerAttackReach;
    GameObject reachObj;
    [SerializeField] GameObject attackObj; // アタックのエフェクトと当たり判定を持つオブジェクトを代入
    [SerializeField] public GameObject strongAttackObj;
    [SerializeField] public float defaultAttackTime = 0.5f;
    [SerializeField] public float defaultAttackCd = 0.5f;
    [SerializeField] public float attackTime; // アタック中の時間
    [SerializeField] public float attackCd; // アタックのクールダウン
    float attackCdTimer; // クールダウンを実際にカウントする変数
    public float defaultAttackDamage = 5.0f; // デフォルトのアタックダメージ
    public float attackDamage = 5.0f; // アタックダメージ
    public Vector3 attackDir; // アタックする方向
    public float defaultFXRot = -90; // 攻撃エフェクトの方向補正
    public bool onAttack = false; // アタックしているかどうか
    public bool canAttack = false; // アタックする範囲内に敵がいるかどうかを判定する変数 
    public float distanceAttackFX = 0.50f; // 攻撃エフェクトとプレイヤーとの距離を設定する（攻撃する方向の単位ベクトルにこの値をかける） 

    // プレイヤーのステータス-------------------------------------------------------------
    public float playerHP = 100.0f; // プレイヤーのHP
    public float maxPlayerHP = 100.0f; // プレイヤーの最大HP
    public float defaultdeffence;
    public float deffence = 1.0f;

    [SerializeField] bool canDie = true;
    private float damageFXTime = 0.2f; // ダメージエフェクトをする時間

    // アニメーションに使う変数------------------------------------------------------------
    enum State{idle, walk, dash, attack};
    private State state;
    private SpriteRenderer spriteRenderer;
    private Animator anim;

    GameObject destroyObje; // 麻生が追加
    GameObject normalAttack;

    void Start()
    {
        currentPos = transform.position;
        onDash = false;
        attackCdTimer = 0f;
        attackCd = defaultAttackCd;
        attackTime = defaultAttackTime;
        spriteRenderer = GetComponent<SpriteRenderer>();
        playerHP = maxPlayerHP;
        attackDamage = defaultAttackDamage;
        state = State.idle;
        anim = GetComponent<Animator>();
        reachObj = transform.Find("AttackReach").gameObject;
        playerAttackReach = reachObj.GetComponent<PlayerAttackReach>();
        Application.targetFrameRate = 60;
        
    }

    void FixedUpdate()
    {
        Move();
        InputManager();
        CheckDie();
        //if(animFinish) StartCoroutine(Animation());
    }
    /// <summary>
    /// 入力の受け付け、ダッシュやアタックのクールダウンを数える関数
    /// </summary>
    void InputManager()
    {
        // 移動
        dirX = Input.GetAxisRaw("Horizontal");
        dirY = Input.GetAxisRaw("Vertical");

        // ダッシュ
        dashCdTimer -= Time.deltaTime;
        if (Keyboard.current.spaceKey.isPressed)
        {
            if (dashCdTimer <= 0)
            {
                StartCoroutine(Dash());
                dashCdTimer = dashCd + dashTime;
            }
        }

        // アタック
        // プレイヤーの攻撃可能範囲に入っていてかつクールダウン中でなければ攻撃する
        attackCdTimer -= Time.deltaTime;
        if (attackCdTimer <= 0 && canAttack)
        {
            StartCoroutine(Attack());
            attackCdTimer = attackCd + attackTime;
        }
    }

    /// <summary>
    /// プレイヤーの向きと移動を処理する関数
    /// </summary>
    void Move()
    {
        moveDir = new Vector3(dirX, dirY, 0);
        // 正規化
        moveDir.Normalize();

        if (moveDir.x != 0 || moveDir.y != 0)
        {
            lastDir = moveDir;
        }

        if (onDash)
        {
            currentPos += dashDir * dashSpeed;
        }
        else
        {
            currentPos += moveDir * speed;
        }
        transform.position = currentPos;

        //プレイヤーが歩いているときのアニメーション
        anim.SetBool("walk", moveDir != Vector3.zero);

        // プレイヤーの向き
        // アタックしている間はその向きを向くようになる
        // x方向の向きを変更
        if (lastDir.x > 0 && !onAttack)
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        else if (lastDir.x < 0 && !onAttack)
        {
            transform.rotation = Quaternion.Euler(0, 180, 0);
        }

        // 画面内制限
        currentPos.x = Mathf.Clamp(currentPos.x, -xLimit, xLimit);
        currentPos.y = Mathf.Clamp(currentPos.y, yMinLimit, yMaxLimit);
    }


    /// <summary>
    /// プレイヤーのダッシュ処理の関数
    /// </summary>
    IEnumerator Dash()
    {
        onDash = true;
        dashDir = lastDir;
        GameObject obj;
        // ダッシュで土煙がでるようにする
        if(dashDir.x == 0)
        {
            obj = Instantiate(dashUpObj);
            if(dashDir.y >= 0) obj.transform.rotation = Quaternion.Euler(180, 0, 0);
            else obj.transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        else
        {
            obj = Instantiate(dashLeftObj);
            obj.transform.rotation = transform.rotation;
        }
        obj.transform.position = transform.position;
        yield return new WaitForSeconds(dashTime);
        onDash = false;
    }

    public void SlashTypeAndDir(GameObject slashType,ref GameObject obje)
    {
        float flipX = 0;
        float rotZ = 0;
        attackDir = playerAttackReach.playerToEnemyNol;

        GameObject obj = Instantiate(slashType);
        obj.transform.position = transform.position + attackDir * distanceAttackFX;
        obje = obj;

        // プレイヤーから敵までの方向を取得し、8方向（縦横斜め）に変換する
        if (attackDir.x > 0)
        {
            flipX = 0;
        }
        else if (attackDir.x < 0)
        {
            flipX = 180;
        }
        // 斬撃の方向を決定
        if(Mathf.Abs(attackDir.x) <= 1 && Mathf.Abs(attackDir.x) > Mathf.Cos(Mathf.PI / 8))
        {
            // 横方向の処理
            rotZ = 0 + defaultFXRot;
        }
        else if(Mathf.Abs(attackDir.x) > Mathf.Cos(Mathf.PI * 3 / 8))
        {
            // 斜め方向の処理
            if (attackDir.y > 0) rotZ = 45 + defaultFXRot;
            else if (attackDir.y < 0) rotZ = -45 + defaultFXRot;
        }
        else
        {
            // 縦方向の処理
            if (attackDir.y > 0) rotZ = 90 + defaultFXRot;
            else if (attackDir.y < 0) rotZ = -90 + defaultFXRot;
        }

        obj.transform.rotation = Quaternion.Euler(0, flipX, rotZ);
    }

    /// <summary>
    /// プレイヤーのアタック処理の関数
    /// </summary>
    IEnumerator Attack()
    {
        SlashTypeAndDir(attackObj,ref normalAttack);

        onAttack = true;

        yield return new WaitForSeconds(attackTime);

        onAttack = false;
    }

    /// <summary>
    /// プレイヤーがダメージを受けたときの関数
    /// </summary>
    /// <param name="enemyAttack"></param>
    public void Damaged(float enemyAttack)
    {
        playerHP -= enemyAttack;
        Debug.Log("Player残りHP：" + enemyAttack);
        StartCoroutine(BackDamageColor());
    }

    /// <summary>
    /// ダメージを受けた時プレイヤーの色を変える関数
    /// </summary>
    /// <returns></returns>
    private IEnumerator BackDamageColor()
    {
        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(damageFXTime);
        spriteRenderer.color = Color.white;
    }

    /// <summary>
    /// プレイヤーのHPが０以下かどうかをチェック
    /// </summary>
    void CheckDie()
    {
        if (playerHP <= 0 && canDie)
        {
            Destroy(gameObject);
        }
    }
}
