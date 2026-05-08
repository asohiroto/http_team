using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField]private float speed = 0.1f;
    [SerializeField]private float dashMulti = 5.0f;
    private bool onDash;


    float dirX;
    float dirY;
    [SerializeField]float xLimit = 8.5f;
    [SerializeField]float yLimit = 4.7f;
    Vector3 currentPos;
    Vector3 moveDir;


    void Start()
    {
        currentPos = transform.position;
        onDash = false;
    }

    void FixedUpdate()
    {
        Move();
        Dash();
    }

    void Move()
    {
        dirX = Input.GetAxis("Horizontal");
        dirY = Input.GetAxis("Vertical");

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

        currentPos += moveDir * speed;
        transform.position = currentPos;

        

    }

    void Dash()
    {

        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            Debug.Log("space");
            currentPos += moveDir * dashMulti * speed;
            onDash = true;
        }
    }
}
