using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.VirtualTexturing;

public class PlayerController : MonoBehaviour
{
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

    float dashCd = 0.3f;
    float dashCdTimer = 0f;
    [SerializeField]float dashTime = 0.1f;
    Vector3 dashDir;

    GameObject attackObj;
    [SerializeField] float attackTime = 0.5f;
    float attackCd = 0.5f;
    float attackCdTimer;

    public int playerHP = 100;

    void Start()
    {
        currentPos = transform.position;
        onDash = false;
        attackCdTimer = 0f;
        rb = GetComponent<Rigidbody2D>();
        attackObj = transform.GetChild(0).gameObject; // プレイヤーの一番上にある子オブジェクトを取得
        attackObj.SetActive(false);
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
        if (Mouse.current.leftButton.isPressed)
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
        attackObj.SetActive(true);
        yield return new WaitForSeconds(attackTime);
        attackObj.SetActive(false);
    }
}
