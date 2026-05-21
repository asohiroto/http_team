using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.VirtualTexturing;

public class PlayerController : MonoBehaviour
{
    // 移動に使う変数
    [SerializeField]private float speed = 0.1f;
    [SerializeField]private float defaultSpeed = 0.1f;
    [SerializeField] private float dashSpeed = 0.5f;
    private bool onDash;

    float dirX;
    float dirY;
    [SerializeField]float xLimit = 8.5f;
    [SerializeField]float yLimit = 4.7f;
    Vector3 currentPos;
    Vector3 moveDir;
    Rigidbody2D rb;

    Vector3 lastDir = new Vector3(1, 0, 0);

    // ダッシュに使う変数
    float dashCd = 0.3f;
    float dashCdTimer = 0f;
    [SerializeField]float dashTime = 0.1f;
    Vector3 dashDir;

    // アタックに使う変数
    [SerializeField] GameObject attackObj;
    [SerializeField] float attackTime = 2.0f;
    [SerializeField]float attackCd = 0.5f;
    float attackCdTimer;
    public int defaultAttackDamage = 5;
    public int attackDamage = 5;
    Vector3 attackDir;
    float defaultFXRot = -90; // 攻撃エフェクトの方向補正
    bool onAttack = false;

    GameObject[] attackTarget;
    GameObject closeTarget;
    float closeDist = 1000;
    float attackDist = 2.0f;

    public int playerHP = 100;
    public int maxPlayerHP = 100;
    public bool canAttack;

    // アニメーションに使う変数
    SpriteRenderer spriteRenderer;
    [SerializeField] Sprite[] upSprite; // 上向き
    [SerializeField] Sprite[] downSprite; // 下向き
    [SerializeField] Sprite[] leftSprite; // 左向き
    private int animIdx = 0;

    void Start()
    {
        currentPos = transform.position;
        onDash = false;
        attackCdTimer = 0f;
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        playerHP = maxPlayerHP;
        attackDamage = defaultAttackDamage;
        Application.targetFrameRate = 60;
    }

    void FixedUpdate()
    {
        Move();
        InputManager();

        if(playerHP <= 0)
        {
            Destroy(gameObject);
        }
    }

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

        attackCdTimer -= Time.deltaTime;
        if (attackCdTimer <= 0)
        {
            StartCoroutine(Attack());
            attackCdTimer = attackCd + attackTime;
        }
        
    }

    // 移動処理(WASDのみ)
    void Move()
    {
        moveDir = new Vector3(dirX, dirY, 0);

        if((moveDir.x != 0 || moveDir.y != 0) && !onAttack)
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
        if(dirX > 0 && !onAttack)
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
        if(dirX == 0 && !onAttack)
        {
            if(dirY > 0)
            {
                spriteRenderer.sprite = upSprite[animIdx];
            }
            else if(dirY < 0)
            {
                spriteRenderer.sprite = downSprite[animIdx];
            }
        }

        // 画面内制限
        currentPos.x = Mathf.Clamp(currentPos.x, -xLimit, xLimit);
        currentPos.y = Mathf.Clamp(currentPos.y, -yLimit, yLimit);
        
        // 正規化
        if (moveDir.magnitude >= 1)
        {
            moveDir.Normalize();
        }
    }


    // ダッシュ処理の関数
    IEnumerator Dash()
    {
        onDash = true;
        dashDir = lastDir;
        yield return new WaitForSeconds(dashTime);
        onDash = false;
    }

    // アタック処理
    IEnumerator Attack()
    {
        float flipX = 0;
        float rotZ = 0;
        onAttack = true;
        attackDir = lastDir;

        GameObject obj = Instantiate(attackObj, this.transform);
        obj.transform.position = transform.position + attackDir * 0.5f;
        
        // 斬撃の方向を決定
        if(attackDir.x > 0)
        {
            flipX = 0;
        }
        else if(attackDir.x < 0)
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

        yield return new WaitForSeconds(attackTime);

        Destroy(obj);
        onAttack = false;
    }

    public void Damaged(int enemyAttack)
    {
        playerHP -= enemyAttack;
        Debug.Log("Player残りHP：" + enemyAttack);
    }
}
