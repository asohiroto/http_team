using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [Header("Enemy")]
    [SerializeField] private float eHp = 100f;


    [Header("Behavior")]
    [SerializeField] private float findDist = 3f;    // player発見距離
    [SerializeField] private float loseDist = 4f;    // player追跡可能距離(見失う距離)
    [SerializeField] private float e_moveSpeed = 1f; // 移動速度
    [SerializeField] private float stopDist = 1f;    // 停止位置
    [SerializeField] private float attackDist = 2f;  // 攻撃可能な距離
    [SerializeField] private float attackSec = 0f;   // 攻撃のクールダウン


    [Header("State")]
    [SerializeField] private Vector2 ePos = new Vector2(0, 0);      // Enemy(このオブジェクト)の座標
    [SerializeField] private Vector2 playerPos = new Vector2(0, 0); // Playerの座標
    [SerializeField] private float playerDist = 0f;                 // PlayerとEnemyの距離
    public bool isFindPlayer = false;
    [SerializeField] private bool canAttack = false;


    [Header("Config")]
    [SerializeField] private Transform player;

    public bool CanAttack => canAttack;     // 攻撃可能か　読み取り専用

    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerDist = findDist + 1;   // 0で開始すると値が代入されるまでの間に動いてしまうため

        // 見失う距離が発見距離よりも短い場合、見失う距離を発見距離と同じ大きさにします。
        if (loseDist != 0f && loseDist < findDist || findDist == 0)  loseDist = findDist;


        // HPが0のとき、スポーンさせない <- これいる？　検討中    // 必ず一番最後に処理
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        CheckDist();
        LookPlayer();
    }

    public void EnemyDamaged(float dmg)
    {
        // HPをdmg分減らす
        eHp -= dmg;
        Debug.Log(this.gameObject.name + "HP" + dmg + "減少");

        // 死亡チェック
        EnemyDied();
    }

    void EnemyDied()
    {
        // HPが0以下
        if (eHp <= 0)
        {
            Debug.Log("HPが0になった" + this.gameObject.name);
            // 以降の処理は後で追加
        }
    }

    void CheckDist()
    {
        playerPos = player.transform.position;
        ePos = transform.position;
        playerDist = Vector2.Distance(ePos, playerPos);
    }

    void LookPlayer()
    {
        // findDistが0 or distがfindDistより小さいなら移動開始
        if (findDist == 0f || findDist > playerDist)
        {
            // loseDistが0 and distがloseDistより大きいなら移動停止
            if (loseDist != 0f && loseDist < playerDist) return;

            // 攻撃可能な距離の半分で止まる
            if (stopDist > playerDist) return;

            isFindPlayer = true;
            ChasePlayer();
        }
        // 発見状態からloseDistの外に出るまで
        else if (isFindPlayer && findDist <= playerDist && loseDist > playerDist)
        {
            ChasePlayer();
        }
        else
        {
            isFindPlayer = false;
        }

        if (stopDist < playerDist)
        {
            canAttack = true;
        }
        else canAttack = false;
    }
    void ChasePlayer()
    {
        transform.position = Vector2.MoveTowards(transform.position, new Vector2(playerPos.x, playerPos.y), e_moveSpeed * Time.deltaTime);
    }
}
