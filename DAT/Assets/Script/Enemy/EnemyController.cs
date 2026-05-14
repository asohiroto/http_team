using Unity.VisualScripting;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{

    // 敵の判別名(仮)
    /*
        弱い敵、近距離 WeakMeleeEnemy 
        弱い敵、遠距離 WeakRengedEnemy
    */
    [Header("Enemy")]
    [SerializeField] private float eHp = 100f;


    [Header("Behavior")]
    [SerializeField] private float findDist = 0f;       // player発見距離
    [SerializeField] private float loseDist = 0f;       // player追跡可能距離(見失う距離)
    [SerializeField] private float e_moveSpeed = 0f;    // 移動速度
    [SerializeField] private float attackDist = 0f;     // 攻撃可能な距離
    [SerializeField] private float attackSec = 0f;      // 攻撃のクールダウン




    // 仮の変数
    public float distRate = 100f;                        // 座標が１増えるごとの割り

    [Header("State")]
    public bool isFindPlayer = false;
    [SerializeField] private Vector2 ePos = new Vector2(0, 0);      // Enemy(このオブジェクト)の座標
    [SerializeField] private Vector2 playerPos = new Vector2(0, 0); // Playerの座標
    [SerializeField] private float playerDist = 0f;                 // PlayerとEnemyの距離

    [Header("Config")]
    [SerializeField] private GameObject playerObject;

    private Rigidbody2D rb;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        playerDist = findDist + 1;   // 0で開始すると値が代入されるまでの間に動いてしまうため

        // 見失う距離が発見距離よりも短い場合、見失う距離を発見距離と同じ大きさにします。
        if (loseDist != 0f && loseDist < findDist || findDist == 0)  loseDist = findDist;

        // 必ず一番最後に処理
        // HPが0のとき、スポーンさせない <- これいるか？
        EnemyDamaged(20, 0);   // 被ダメージテスト
        EnemyDamaged(50, 0);
        EnemyDamaged(50, 0);
    }

    // Update is called once per frame
    void Update()
    {
        CheckDist();
    }

    private void FixedUpdate()
    {
        LookPlayer();
    }

    public void EnemyDamaged(float dmg, float dist)
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
        }
    }

    void CheckDist()
    {
        playerPos = playerObject.transform.position;
        ePos = transform.position;
        playerDist = Vector2.Distance(ePos, playerPos) * distRate;
    }

    void LookPlayer()
    {
        // findDistが0 or distがfindDistより小さいなら移動開始
        if (findDist == 0f || findDist > playerDist)
        {
            // loseDistが0 and distがloseDistより大きいなら移動停止
            if (loseDist != 0f && loseDist < playerDist) return;

            // 攻撃可能な距離で止まる
            if (attackDist > playerDist) return;

            isFindPlayer = true;
            chasePlayer();
        }
        // 発見状態からloseDistの外に出るまで
        else if (isFindPlayer && findDist <= playerDist && loseDist > playerDist) chasePlayer();
        else isFindPlayer = false;
    }

    void chasePlayer()
    {
        transform.position = Vector2.MoveTowards(transform.position, new Vector2(playerPos.x, playerPos.y), e_moveSpeed * Time.deltaTime);
    }

    // いらないかも？
    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("オブジェクト名：" + other.gameObject.name + " タグ：" + other.gameObject.tag);

        // 衝突した相手がプレイヤーのタグか確認
        if (other.CompareTag("Player"))
        {
            // ダメージ処理を呼ぶ
            Debug.Log("Damaged!");
        }
    }

}
