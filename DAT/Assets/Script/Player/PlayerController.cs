using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField]private float speed = 0.1f;
    [SerializeField] private float dashSpeed = 1000.0f;
    private bool onDash;


    float dirX;
    float dirY;
    [SerializeField]float xLimit = 8.5f;
    [SerializeField]float yLimit = 4.7f;
    Vector3 currentPos;
    Vector3 moveDir;
    Rigidbody2D rb;

    float dashCd;
    float dashCdTimer;

    


    void Start()
    {
        currentPos = transform.position;
        onDash = false;
        rb = GetComponent<Rigidbody2D>();
        
    }

    void FixedUpdate()
    {
        dashCdTimer -= Time.deltaTime;
        Move();
        if (Keyboard.current.spaceKey.isPressed)
        {
            Debug.Log("space");
            Dash();
        }
    }

    void Move()
    {
        dirX = Input.GetAxisRaw("Horizontal");
        dirY = Input.GetAxisRaw("Vertical");

        moveDir = new Vector3(dirX, dirY, 0);

        // 画面内制限
        if (transform.position.x >= xLimit)
        {
            currentPos.x = xLimit;
        }

        if (transform.position.x <= -xLimit)
        {
            currentPos.x = -xLimit;
        }

        if (transform.position.y >= yLimit)
        {
            currentPos.y = yLimit;
        }

        if (transform.position.y <= -yLimit)
        {
            currentPos.y = -yLimit;
        }

        // 正規化
        if (moveDir.magnitude >= 1)
        {
            moveDir.Normalize();
        }
            //currentPos += moveDir * speed;
        
        transform.position = currentPos;
    }

    void Dash()
    {
        if (dashCdTimer > 0)
        {
            return;
        }
        else
        {
            dashCdTimer = dashCd;
        }

        onDash = true;
        
    }
}
