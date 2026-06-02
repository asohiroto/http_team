using UnityEngine;
using UnityEngine.UIElements;

public class EneBoss2 : MonoBehaviour
{
    float speed = 0.1f;
    Vector3 moveDir;
    Vector3 currentPos;
    void Start()
    {
        currentPos = transform.position;
    }

    void FixedUpdate()
    {
        Move(new Vector3(5, 0, 0));
        transform.position = currentPos;
    }

    // 指定した座標に移動する
    void Move(Vector3 position)
    {
        if ((currentPos.x - position.x < 0.5f&& currentPos.x - position.x > -0.5f )
            && currentPos.y - currentPos.y < 0.5f && currentPos.y - currentPos.y > -0.5f)
        {
            return;
        }
        moveDir = position - currentPos;
        moveDir = moveDir.normalized;
        currentPos += moveDir * speed;
    }
}
