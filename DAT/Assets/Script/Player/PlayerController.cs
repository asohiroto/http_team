using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.VirtualTexturing;

public class PlayerController : MonoBehaviour
{
    // 移動に使う変数-----------------------------------------------------------------
    [SerializeField] public float speed = 0.1f; // プレイヤーのスピード
    [SerializeField] public float defaultSpeed = 0.1f; // プレイヤーのデフォルトスピード
    [SerializeField] private float dashSpeed = 0.5f; // プレイヤーのダッシュスピード
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

    // アタックに使う変数---------------------------------------------------------------
    PlayerAttack playerAttack;
    [SerializeField] GameObject attackObj; // アタックのエフェクトと当たり判定を持つオブジェクトを代入
    [SerializeField] public GameObject strongAttackObj;
    [SerializeField] public float attackTime = 1.0f; // アタック中の時間
    [SerializeField] float attackCd = 1.0f; // アタックのクールダウン
    float attackCdTimer; // クールダウンを実際にカウントする変数
    public int defaultAttackDamage = 5; // デフォルトのアタックダメージ
    public int attackDamage = 5; // アタックダメージ
    public Vector3 attackDir; // アタックする方向
    public float defaultFXRot = -90; // 攻撃エフェクトの方向補正
    public bool onAttack = false; // アタックしているかどうか
    public bool canAttack = false; // アタックする範囲内に敵がいるかどうかを判定する変数 

    // プレイヤーのステータス-------------------------------------------------------------
    public int playerHP = 100; // プレイヤーのHP
    public int maxPlayerHP = 100; // プレイヤーの最大HP
    [SerializeField] bool canDie = true;


    // アニメーションに使う変数------------------------------------------------------------
    SpriteRenderer spriteRenderer;
    [SerializeField] Sprite[] upSprite; // 上向きのアニメーションを代入
    [SerializeField] Sprite[] downSprite; // 下向きのアニメーションを代入
    [SerializeField] Sprite[] leftSprite; // 左向きのアニメーションを代入
    private int animIdx = 0; // アニメーションのコマ数を数える変数

    void Start()
    {
        currentPos = transform.position;
        onDash = false;
        attackCdTimer = 0f;
        spriteRenderer = GetComponent<SpriteRenderer>();
        playerHP = maxPlayerHP;
        attackDamage = defaultAttackDamage;
        Application.targetFrameRate = 60;
    }

    void FixedUpdate()
    {
        Move();
        InputManager();
        CheckDie();

        if (playerHP <= 0)
        {
            Destroy(gameObject);
        }
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
            //StartCoroutine(Attack());
            attackCdTimer = attackCd + attackTime;
        }

        // 強いアタック（デバック用）
        /*if(Input.GetKey(KeyCode.F))
        {
            StartCoroutine(StrongAttack());
        }*/

    }

    /// <summary>
    /// プレイヤーの向きと移動を処理する関数
    /// </summary>
    void Move()
    {
        moveDir = new Vector3(dirX, dirY, 0);

        if ((moveDir.x != 0 || moveDir.y != 0) && !onAttack)
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

        // プレイヤーの向き
        // アタックしている間はその向きを向くようになる
        // x方向の向きを変更
        if (dirX > 0 && !onAttack)
        {
            spriteRenderer.sprite = leftSprite[animIdx];
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        else if (dirX < 0 && !onAttack)
        {
            spriteRenderer.sprite = leftSprite[animIdx];
            transform.rotation = Quaternion.Euler(0, 180, 0);
        }

        // y方向の向きを変更
        if (dirX == 0 && !onAttack)
        {
            if (dirY > 0)
            {
                spriteRenderer.sprite = upSprite[animIdx];
            }
            else if (dirY < 0)
            {
                spriteRenderer.sprite = downSprite[animIdx];
            }
        }

        // 画面内制限
        currentPos.x = Mathf.Clamp(currentPos.x, -xLimit, xLimit);
        currentPos.y = Mathf.Clamp(currentPos.y, yMinLimit, yMaxLimit);

        // 正規化
        if (moveDir.magnitude >= 1)
        {
            moveDir.Normalize();
        }
    }


    /// <summary>
    /// プレイヤーのダッシュ処理の関数
    /// </summary>
    IEnumerator Dash()
    {
        onDash = true;
        dashDir = lastDir;
        yield return new WaitForSeconds(dashTime);
        onDash = false;
    }

    public void SlashTypeAndDir(GameObject slashType)
    {
        float flipX = 0;
        float rotZ = 0;
        //onAttack = true;
        attackDir = lastDir;

        GameObject obj = Instantiate(slashType, transform);
        obj.transform.position = transform.position + attackDir * 0.50f;

        // 斬撃の方向を決定
        if (attackDir.x > 0)
        {
            flipX = 0;
        }
        else if (attackDir.x < 0)
        {
            flipX = 180;
        }

        if (attackDir.x == 0)
        {
            // 真上・真下（左右の入力がないとき）
            if (attackDir.y > 0) rotZ = 90 + defaultFXRot;
            else if (attackDir.y < 0) rotZ = -90 + defaultFXRot;
            else rotZ = 0 + defaultFXRot; // 入力なし（真横など）
        }
        else
        {
            // 斜め入力（左右の入力があるとき）
            if (attackDir.y > 0) rotZ = 45 + defaultFXRot;
            else if (attackDir.y < 0) rotZ = -45 + defaultFXRot;
            else rotZ = 0 + defaultFXRot; // 真横
        }

        obj.transform.rotation = Quaternion.Euler(0, flipX, rotZ);
    }

    /// <summary>
    /// プレイヤーのアタック処理の関数
    /// </summary>
    IEnumerator Attack()
    {
        SlashTypeAndDir(attackObj);

        yield return new WaitForSeconds(0.1f); // アタック方向の固定を少し遅らせる

        onAttack = true;

        yield return new WaitForSeconds(attackTime);

        //Destroy(obj);
        onAttack = false;
    }



    public IEnumerator StrongAttack(GameObject slashType, int slashDamage) // 強斬り用の関数
    {
        SlashTypeAndDir(slashType);

        

        yield return new WaitForSeconds(0.1f); // アタック方向の固定を少し遅らせる

        onAttack = true;

        yield return new WaitForSeconds(attackTime);

        onAttack = false;
    }

    /// <summary>
    /// プレイヤーがダメージを受けたときの関数
    /// </summary>
    /// <param name="enemyAttack"></param>
    public void Damaged(int enemyAttack)
    {
        playerHP -= enemyAttack;
        Debug.Log("Player残りHP：" + enemyAttack);
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
