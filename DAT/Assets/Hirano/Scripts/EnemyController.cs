using Unity.VisualScripting;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [Header("Behavior")]
    [SerializeField] private float findDist = 0f;
    [SerializeField] private float loseDist = 0f;
    [SerializeField] private float e_moveSpeed = 0f;
    [SerializeField] private float attackDist = 0f;
    [SerializeField] private float attackSec = 0f;




    // karino hensuu
    // 座標が１増えるごとの割り
    public float distRate = 100;

    [Header("State")]
    public bool isFindPlayer = false;
    [SerializeField] private float playerDist = 0f;

    [SerializeField] private Vector2 ePos = new Vector2(0, 0);
    [SerializeField] private Vector2 playerPos = new Vector2(0, 0);
    [SerializeField] private float dist = 0f;


private Rigidbody2D rb;
    [SerializeField] private GameObject player;





    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        
        

    }

    // Update is called once per frame
    void Update()
    {
        CheckDist();
    }

    private void FixedUpdate()
    {
        
    }

    void CheckDist()
    {
        playerPos = player.transform.position;
        this.ePos = transform.position;
        this.dist = Vector2.Distance(ePos, playerPos) / distRate;
    }

    void LookPlayer()
    {
        this.dist = Vector2.Distance(this.ePos, playerPos);
    }
}
