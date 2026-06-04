using System;
using UnityEngine;

public class EneBossShot : MonoBehaviour
{
    float speed = 0.1f;
    Vector3 moveDir;
    void Start()
    {
        
    }

    void FixedUpdate()
    {
        transform.position += speed * Vector3.right;
    }
}
