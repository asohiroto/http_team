using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private float speed = 0.1f;
    
    void Start()
    {
        
    }

    void FixedUpdate()
    {
        float dirX = Input.GetAxis("Horizontal");
        float dirY = Input.GetAxis("Vertical");

        Vector3 moveDir = new Vector3(dirX, dirY, 0);

        if(moveDir.magnitude >= 1)
        {
            moveDir.Normalize();
        }

        transform.position += moveDir * speed;
    }
}
