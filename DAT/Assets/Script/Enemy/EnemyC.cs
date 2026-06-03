using System;
using UnityEngine;

public class EnemyC : MonoBehaviour
{
    [Header("Enemy Stats")]
    [SerializeField] private int enemyHp;       // 体力
    [SerializeField] private float moveSpeed;   // 移動速度
    [SerializeField] private int attackPower;   // 攻撃力

    [Header("Enemy Settings")]
    // コード内では2乗した状態で使用する
    [SerializeField] private bool alwaysFindPlayer = false;     // どの距離でもPlayerを発見する
    [SerializeField] private float findDist;                    // Playerを発見する距離
    [SerializeField] private float lostDistf;                   // Playerを見失う距離
    [SerializeField] private float stopDistf;                   // この距離で立ち止まる
    [SerializeField] private float attackRange;                 // 攻撃のレンジ
    [SerializeField] private float attackStartupDuration;       // 攻撃の予備動作の時間
    [SerializeField] private float attackCooldownDuration;      // 攻撃のクールダウンの時間


    [Header("Debug")]
    [SerializeField] private Vector2 enemyPos = Vector2.zero;   // このオブジェクトの座標
    [SerializeField] private Vector2 playerPos = Vector2.zero;  // Playerの座標
    [SerializeField] private Vector2 moveDir = Vector2.zero;    // 正規化された、現在の移動方向
    [SerializeField] private Vector2 lookDir = Vector2.zero;    // moveDirを -1, 0, 1 に限定 攻撃時に使用
    [SerializeField] private float sqrDistanceToPlayer;         // 2乗したPlayerとの距離
    [SerializeField] private float isFindPlayer;                // Playerを発見中
    [SerializeField] private float isLostPlayer;                // Playerを見失っている
    [SerializeField] private float isChasePlayer;               // Playerを追跡中
    [SerializeField] private float isStop;
    [SerializeField] private float isLookRight;
    [SerializeField] private float playing;// state に initを追加して判定
    [SerializeField] private float canAttack;                   // 攻撃可能範囲内
    [SerializeField] private float isAttack;
    [SerializeField] private float attackStartupTimer;          // 予備動作用のカウントダウン
    [SerializeField] private float attackCooldownTimer;         // クールダウン用のカウントダウン
    
     
    [Header("Object References")]
    [SerializeField] private GameObject playerObj;      // Playerオブジェクト
    [SerializeField] private GameObject coinParentObj;  // Coin のプレハブ
    [SerializeField] private GameObject coinPrefab;     // Coin を格納する親オブジェクト
    [SerializeField] private GameObject attackCol;      // 攻撃用のコライダー(プレハブ)
    private EnemyAnimation enemyAnim;
    private GameObject hitObj;

    private enum EnemyAiState       // Enemyの状態
    { 
        Idle,
        Move,
        AttackStartup,
        SideAttack,
        LowerAttack,
        UpperAttack,
        AttackCool,
        KnockBack,
        Init
    }
    [SerializeField] private EnemyAiState enemyAiState = EnemyAiState.Init;

    private enum EnemyMovePattern
    {
        Stay,
        Patrol,
        Walk,
        Dash,
        Hop,
        Jump,
        Init
    }
    [SerializeField] private EnemyMovePattern enemyMovePattern = EnemyMovePattern.Init;


    void Start()
    {
        
    }


    void Update()
    {
        UpdateTimers();
    }

    void FixedUpdate()
    {
        
    }

    public void EnemyDamaged(int dmg)
    {
        enemyHp -= dmg;
    }

    public void OnAttackAnimationFinished()
    {

    }

    float GetSqrDistance (Vector2 a, Vector2 b)
    {
        /*
        float distX = b.x - a.x;
        float distY = b.y - a.y;
        float dist = distX * distX + distY * distY;
        return dist; 
        */
        return (b.x -  a.x) * (b.x - a.x) + (b.y - a.y) * (b.y - a.y);
    }

    /// <summary>
    /// 経過時間の管理用
    /// </summary>
    void UpdateTimers()
    {
        attackStartupTimer -= Time.deltaTime;
        attackCooldownTimer -= Time.deltaTime;
    }

    void Move()
    {
        if (enemyAiState != EnemyAiState.Move) return;
    }

    /// <summary>
    /// 攻撃可能かを確認し、攻撃処理を行う
    /// </summary>
    void TryAttack()
    { }

    void UpdateAiState()
    { }

    void ChangeAnimState()
    { }

    void Die()
    { }

    void LookToPlayer()
    { }
}
