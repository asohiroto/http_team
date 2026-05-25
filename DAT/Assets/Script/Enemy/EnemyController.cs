using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyState
{
    Idle,
    Move,
    Attack,
}

public class EnemyController : MonoBehaviour
{
    [Header("Enemy")]
    [SerializeField] private float eHp = 100f;

    [Header("Behavior")]
    [SerializeField] private float findDist = 4f;    // player発見距離
    [SerializeField] private float loseDist = 5f;    // player追跡可能距離(見失う距離)
    [SerializeField] private bool alwaysFindPlayer = false; // playerを常に発見
    [SerializeField] private float e_moveSpeed = 1f; // 移動速度
    [SerializeField] private float stopDist = 0.7f;    // 停止位置
    [SerializeField] private float attackDist = 1f;  // 攻撃可能な距離
    [SerializeField] private int attackPower = 30;   // 攻撃力
    [SerializeField] private float attackSec = 0.6f;    // 攻撃の時間    // フレームカウントではないので注意
    [SerializeField] private float attackCd = 1.4f;    // 攻撃のクールダウン


    [Header("State")]
    [SerializeField] private Vector2 ePos = Vector2.zero;      // Enemy(このオブジェクト)の座標
    [SerializeField] private Vector2 playerPos = Vector2.zero; // Playerの座標
    [SerializeField] private Vector2 nowDir = Vector2.zero; // 現在の移動方向
    [SerializeField] private float playerDist = 0f;                 // PlayerとEnemyの距離
    [SerializeField] private bool isFindPlayer = false;
    [SerializeField] private bool isLostPlayer = false;
    [SerializeField] private bool isChasePlayer = false;
    [SerializeField] private bool canAttack = false;
    [SerializeField] private bool isStop = false;
    public bool IsAttack = false;   // いい方法が思いつかなかったので

    [Header("Config")]
    [SerializeField] private float takeDamageDist = 1f; // Player からの攻撃をくらう距離
    [SerializeField] private Transform player;          // Player オブジェクト
    [SerializeField] GameObject attackCol;              // 攻撃の当たり判定(プレハブ)


    public float AttackDist => attackDist;
    public int AttackPower => attackPower;
    public float AttackSec => attackSec;
    public float AttackCd => attackCd;
    public Vector2 NowDir => nowDir;
    public bool CanAttack => canAttack;     // 攻撃可能か　読み取り専用
    public bool IsStop => isStop;
    public bool IsMove => isChasePlayer;
    public float Distance => playerDist;
    public bool CanTakeDamage => playerDist < takeDamageDist;
    public GameObject AttackCol => attackCol;

    Component attack;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerDist = findDist + 1;   // 0で開始すると値が代入されるまでの間に動いてしまうため

        // 見失う距離が発見距離よりも短い場合、見失う距離を発見距離と同じ大きさにします。
        if (loseDist < findDist)  loseDist = findDist;

        attack = GetComponent<EnemyAttack>();

        // HPが0のとき、スポーンさせない <- これいる？　検討中    // 必ず一番最後に処理
    }

    private void FixedUpdate()
    {
        CheckDist();
        LookPlayer();
        ChasePlayer();
    }

    // EnemyDamaged
    public void EnemyDamaged(int dmg)
    {
        // HPをdmg分減らす
        eHp -= dmg;
        Debug.Log(this.gameObject.name + "HP" + dmg + "減少");

        // 死亡チェック
        EnemyDie();
    }

    void EnemyDie()
    {
        // HPが0以下
        if (eHp <= 0)
        {
            Debug.Log("HPが0になった" + this.gameObject.name);
            // 以降の処理は後で追加
            Destroy(this.gameObject);
        }
    }

    void CheckDist()
    {
        playerPos = player.transform.position;
        ePos = transform.position;
        playerDist = Vector2.Distance(ePos, playerPos);
        //playerDist = (playerPos - ePos).magnitude;

        // EnemyからPlayerへのベクトル
        nowDir = playerPos - ePos;
        nowDir.Normalize();
    }

    void LookPlayer()
    {
        isFindPlayer = findDist > playerDist;   // 発見距離内か
        isLostPlayer = loseDist < playerDist;   // 見失ったか
        canAttack = attackDist > playerDist;    // 攻撃範囲内か
        isStop = stopDist > playerDist && IsAttack;         // 止まる距離、攻撃中か

        // 発見距離内なら移動開始
        if ((!isLostPlayer || alwaysFindPlayer) && !isStop)
        {
            if (isFindPlayer || alwaysFindPlayer)
            {
                isChasePlayer = true;
            }
        }
        else
        {
            isChasePlayer = false;
        }
    }
    void ChasePlayer()
    {
        // 追跡状態じゃないなら返す
        if (!isChasePlayer) return;
        transform.position = Vector2.MoveTowards(transform.position, new Vector2(playerPos.x, playerPos.y), e_moveSpeed * Time.deltaTime);
    }

    private void OnDrawGizmos()
    {
        // セグメント数
        int seg = 32;
        float r;

        if (isChasePlayer)
        {
            Gizmos.color = Color.red;
            r = loseDist;
        }
        else
        {
            Gizmos.color = Color.yellow;
            r = findDist;
        }

        List<Vector3> vertices = new();

        for (int i = 0; i < seg; i++)
        {
            float angle = Mathf.PI * 2f * i / seg;

            float x = Mathf.Cos(angle) * r;
            float y = Mathf.Sin(angle) * r;
            
            Vector3 a = new Vector3(x, y, 0);
            Vector3 b = new Vector3(ePos.x, ePos.y, 0);

            Vector3 pos = a + b;

            vertices.Add(pos);
        }

        foreach (var v in vertices)
        {
            Gizmos.DrawSphere(v, 0.05f);
        }
    }
}
