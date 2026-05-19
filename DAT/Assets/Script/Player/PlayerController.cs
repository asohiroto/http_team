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

    // ダッシュに使う変数
    float dashCd = 0.3f;
    float dashCdTimer = 0f;
    [SerializeField]float dashTime = 0.1f;
    Vector3 dashDir;

    // アタックに使う変数
    [SerializeField] GameObject attackObj;
    [SerializeField] float attackTime = 0.5f;
    [SerializeField]float attackCd = 0.5f;
    float attackCdTimer;
    Vector3 attackDir;

    GameObject[] attackTarget;
    GameObject closeTarget;
    float closeDist = 1000;
    float attackDist = 2.0f;

    public int playerHP = 100;

    void Start()
    {
        currentPos = transform.position;
        onDash = false;
        attackCdTimer = 0f;
        rb = GetComponent<Rigidbody2D>();
        attackTarget = GameObject.FindGameObjectsWithTag("Enemy");
    }

    void FixedUpdate()
    {
        Move();
        InputManager();
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
        if (closeDist <= attackDist)
        {
            if (attackCdTimer <= 0)
            {
                StartCoroutine(Attack());
                attackCdTimer = attackCd + attackTime;
            }
        }
    }

    // 移動処理(WASDのみ)
    void Move()
    {
        moveDir = new Vector3(dirX, dirY, 0);

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
        // アタックしている間はその向きを向くようにする（これから実装する）
        if(dirX > 0)
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        else if (dirX < 0)
        {
            transform.rotation = Quaternion.Euler(0, 180, 0);
        } // プレハブに変えます！

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
        dashDir = moveDir;
        yield return new WaitForSeconds(dashTime);
        onDash = false;
    }

    // アタック処理
    IEnumerator Attack()
    {
        GameObject obj = Instantiate(attackObj, this.transform);
        obj.transform.position = transform.position;

        yield return new WaitForSeconds(attackTime);
        Destroy(obj);
    }

    public void Damaged(int enemyAttack)
    {
        playerHP -= enemyAttack;
        Debug.Log("Player残りHP：" + enemyAttack);
    }
}
