using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private float speed = 0.1f;
    float dirX;
    float dirY;
    float xLimit = 8.5f;
    float yLimit = 4.7f;
    Vector3 currentPos;


    void Start()
    {
        currentPos = transform.position;
    }

    void FixedUpdate()
    {
        Move();
    }

    void Move()
    {
        dirX = Input.GetAxis("Horizontal");
        dirY = Input.GetAxis("Vertical");

        Vector3 moveDir = new Vector3(dirX, dirY, 0);

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
}
