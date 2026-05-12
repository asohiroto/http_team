using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField]private float speed = 0.1f;
    [SerializeField]private float defaultSpeed = 0.1f;
    [SerializeField] private float dashSpeed;
    private bool onDash;


    float dirX;
    float dirY;
    [SerializeField]float xLimit = 8.5f;
    [SerializeField]float yLimit = 4.7f;
    Vector3 currentPos;
    Vector3 moveDir;
    Rigidbody2D rb;

    float dashCd = 0.4f;
    float dashCdTimer = 0f;
    [SerializeField]float dashTime = 0.1f;
    Vector3 dashDir;

    


    void Start()
    {
        currentPos = transform.position;
        onDash = false;
        rb = GetComponent<Rigidbody2D>();
        
    }

    void FixedUpdate()
    {
        Move();
        Debug.Log(dashDir);
        
    }

    void Move()
    {
        dirX = Input.GetAxisRaw("Horizontal");
        dirY = Input.GetAxisRaw("Vertical");

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
        
        // 画面内制限
        currentPos.x = Mathf.Clamp(currentPos.x, -xLimit, xLimit);
        currentPos.y = Mathf.Clamp(currentPos.y, -yLimit, yLimit);
        
        // 正規化
        if (moveDir.magnitude >= 1)
        {
            moveDir.Normalize();
        }

        // ダッシュ
        dashCdTimer -= Time.deltaTime;
        if (Keyboard.current.spaceKey.isPressed)
        {
            if(dashCdTimer <= 0)
            {
                StartCoroutine(Dash());
                dashCdTimer = dashCd;
            }
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
}
