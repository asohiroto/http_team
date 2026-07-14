using System;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [Header("Enemy Stats")]
    [SerializeField] private float enemyHp;       // 体力
    [SerializeField] private float moveSpeed;   // 移動速度
    [SerializeField] private int attackPower;   // 攻撃力
    [SerializeField] private EnemyType enemyType;   // 敵の種類

    [Header("Enemy Settings")]
    // コード内では2乗した状態で使用する
    [SerializeField] private bool alwaysFindPlayer = false;     // どの距離でもPlayerを発見する
    [SerializeField] private float findDist;                    // Playerを発見する距離
    [SerializeField] private float lostDist;                   // Playerを見失う距離
    [SerializeField] private float stopDist;                   // この距離で立ち止まる
    [SerializeField] private float attackRange;                 // 攻撃のレンジ
    [SerializeField] private float attackStartupDuration;       // 攻撃の予備動作の時間
    [SerializeField] private float attackCooldownDuration;      // 攻撃のクールダウンの時間
    [SerializeField] private float attackOnColliderDuration;      // 攻撃呼び出しのタイミング (現在のフレームを確認するのがめんどくさいのでゴリ押し。規模的に問題ないと思う)


    [Header("Debug")]
    [SerializeField] private Vector2 enemyPos = Vector2.zero;       // このオブジェクトの座標
    [SerializeField] private Vector2 playerPos = Vector2.zero;      // Playerの座標
    [SerializeField] private Vector2 targetDir = Vector2.zero;      // 正規化された、現在の移動方向
    [SerializeField] private Vector2 targetDirSign = Vector2.zero;  // targetDirを -1, 0, 1 に限定 攻撃時に使用
    float AbsDirX;
    float AbsDirY;
    [SerializeField] private float sqrDistPlayer;       // 2乗したPlayerとの距離
    [SerializeField] private float attackAngle;         // 攻撃の向き
    [SerializeField] private bool wasAttack;
    [SerializeField] private float attackStartupTimer;              // 予備動作用のカウントダウン
    [SerializeField] private float attackCooldownTimer;             // クールダウン用のカウントダウン
    [SerializeField] private float attackOnColliderTimer;           // 攻撃呼び出し用のカウントダウン
    
     
    [Header("Object References")]
    [SerializeField] private GameObject playerObj;      // Playerオブジェクト
    [SerializeField] private GameObject DropParentObj;  // DropItem を格納する親オブジェクト
    [SerializeField] private GameObject DropItemPrefab; // DropItem のプレハブ
    [SerializeField] private GameObject attackCol;      // 攻撃用のコライダー(プレハブ)
    private EnemyAnimation enemyAnim;
    private EnemyHpManager hpManager;
    private SEManager se;
    private EnemySpawner enemySpawner;
    [SerializeField] private GameObject attackObj;
    [SerializeField] private GameObject arrowObj;

    private enum EnemyType  // 敵の種類 インスペクターで設定
    {
        Torcher,
        Archer
    }

    private enum EnemyAiState       // Enemyの状態
    { 
        Idle,
        Move,
        AttackStartup,
        Attack,
        AttackCooldown,
        KnockBack,
        Init
    }
    [SerializeField] private EnemyAiState enemyAiState = EnemyAiState.Init;
#if true
    // Move拡張用
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
#endif

    void Awake()
    {
        if (playerObj == null)
        {
            playerObj = GameObject.FindWithTag("Player");
        }

        if (DropParentObj  == null)
        {
            DropParentObj = GameObject.FindWithTag("DropItems");
        }

        enemyAnim = GetComponent<EnemyAnimation>();

        // 親オブジェクト(EnemySpawner)を取得
        GameObject parentObj = transform.parent.gameObject;

        enemySpawner = parentObj.GetComponent<EnemySpawner>();

        hpManager = GetComponent<EnemyHpManager>();

        se = GetComponent<SEManager>();

    }

    void Start()
    {
        if (alwaysFindPlayer)
        {
            findDist = float.PositiveInfinity;
        }
        if (lostDist < findDist)
        {
            lostDist = findDist;
        }
        if (stopDist > attackRange)
        {
            stopDist = attackRange;
        }

        enemyAiState = EnemyAiState.Idle;
    }


    void Update()
    {
        UpdateTimers();
    }

    void FixedUpdate()
    {
        UpdateHp();
        UpdatePositionInfo();
        UpdateTargetInfo();
        UpdateAiState();
        UpdateAnimState();
        Die();
        TryAttack();
        MoveEnemy();
        FlipEnemy();
    }

    /// <summary>
    /// 敵を消す(ドロップなし)
    /// </summary>
    public void Delete()
    {
        Destroy(this.gameObject);

    }

    /// <summary>
    /// Enemyの被ダメージ処理
    /// </summary>
    /// <param name="dmg"></param>
    public void EnemyDamaged(float dmg)
    {
        enemyAiState = EnemyAiState.KnockBack;
        enemyHp -= dmg;
    }


    private void UpdateHp()
    {
        enemyHp = hpManager.GetCurrentHp();

        // ノックバック処理
        if (hpManager.TakeDamage())
        {
            enemyAiState = EnemyAiState.KnockBack;
        }
    }

    public void OnAttackAnimationFinished()
    {
        if (enemyAiState == EnemyAiState.Attack)
        {
            attackCooldownTimer = attackCooldownDuration;

            enemyAiState = EnemyAiState.AttackCooldown;
        }
    }

    /// <summary>
    /// Enemyの攻撃力
    /// </summary>
    /// <returns>攻撃力を返す</returns>
    public int GetAttackPower()
    {
        return attackPower;
    }

    /// <summary>
    /// 二つのベクトル間の距離を2乗した値を取得
    /// </summary>
    /// <returns>ベクトル間の距離を2乗した値</returns>
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
        attackOnColliderTimer -= Time.deltaTime;
    }

    /// <summary>
    /// 座標をVector2クラスに保持
    /// </summary>
    void UpdatePositionInfo()
    {
        playerPos = playerObj.transform.position;
        enemyPos = this.transform.position;
    }

    /// <summary>
    /// Playerの位置、方向取得
    /// </summary>
    void UpdateTargetInfo()
    {
        sqrDistPlayer = GetSqrDistance(playerPos, enemyPos);
        targetDir = (playerPos - enemyPos).normalized;
        
        AbsDirX = Mathf.Abs(targetDir.x);
        AbsDirY = Mathf.Abs(targetDir.y);

        if (AbsDirX < AbsDirY)  // 上下
        {
            targetDirSign.x = 0f;

            if (targetDir.y < 0f)
            {
                targetDirSign.y = -1f;  // 下
            }
            else if (targetDir.y > 0f)
            {
                targetDirSign.y = 1f;   // 上
            }
            else
            {
                targetDirSign.y = 0f;
            }
        }
        else                            // 左右
        {
            targetDirSign.y = 0f;

            if (targetDir.x > 0f)
            {
                targetDirSign.x = 1f;   // 右
            }
            else if (targetDir.x < 0f)
            {
                targetDirSign.x = -1f;  // 左
            }
            else
            {
                targetDirSign.x = 0f;
            }
        }
    }

    /// <summary>
    /// Enemyの行動状態更新
    /// </summary>
    void UpdateAiState()
    {
        /*
        if (attackStartupTimer > 0) return;
        if (enemyAiState == EnemyAiState.Attack) return;
        if (attackCooldownTimer > 0) return;
        if (enemyAiState == EnemyAiState.KnockBack) return;
        */

        // アイドル時と移動時以外飛ばす
        if (enemyAiState != EnemyAiState.Idle 
            && enemyAiState != EnemyAiState.Move) return;

        bool isStop = sqrDistPlayer < stopDist * stopDist;
        bool isFind = sqrDistPlayer < findDist * findDist;
        bool isLost = sqrDistPlayer > lostDist * lostDist;

        if (isStop)
        {
            enemyAiState = EnemyAiState.AttackStartup;
            attackStartupTimer = attackStartupDuration;
        }
        else if (!isLost)
        {
            if (isFind)
            {
                enemyAiState = EnemyAiState.Move;
            }
        }
        else
        {
            enemyAiState = EnemyAiState.Idle;
        }
    }

    /// <summary>
    /// アニメーションの更新
    /// </summary>
    void UpdateAnimState()
    {
        switch (enemyAiState)
        {
            case EnemyAiState.Idle:

                enemyAnim.ChangeState(EnemyAnimState.Idle);
                
                break;

            case EnemyAiState.Move:
                
                enemyAnim.ChangeState(EnemyAnimState.Walk);
                
                break;

            case EnemyAiState.Attack:
                
                if (AbsDirX < AbsDirY)  // 上下
                {
                    if (targetDir.y < 0f)   // 下
                    {
                        enemyAnim.ChangeState(EnemyAnimState.LowerAttack);
                    }
                    if (targetDir.y > 0f)   // 上
                    {
                        enemyAnim.ChangeState(EnemyAnimState.UpperAttack);
                    }
                }
                else                    // 左右
                {
                    enemyAnim.ChangeState(EnemyAnimState.SideAttack);
                }
                
                break;

            case EnemyAiState.KnockBack:
                
                enemyAnim.StartBlink();
                
                Vector3 force = new Vector3 (targetDir.x, targetDir.y, 0) * -1 * 0.75f + this.transform.position;
                this.transform.position = force;
                // 2重で飛ばされるのを防止したい

                enemyAiState = EnemyAiState.Idle;
                break;

            default:
                
                break;

        }
    }

    /// <summary>
    /// 死亡判定、死亡後処理
    /// </summary>
    void Die()
    {
        if (enemyHp > 0f) return;

        GameObject dropItem = Instantiate(DropItemPrefab, this.transform);
        dropItem.transform.SetParent(DropParentObj.transform);

        enemySpawner.DestroyEnemy(this.gameObject);

        Destroy(this.gameObject);
    }

    /// <summary>
    /// 攻撃可能かを確認し、攻撃処理
    /// </summary>
    void TryAttack()
    { 
        switch(enemyAiState)
        {
            case EnemyAiState.AttackStartup:

                enemyAnim.ChangeState(EnemyAnimState.Idle);

                if (attackStartupTimer <= 0)
                {
                    attackOnColliderTimer = attackOnColliderDuration;

                    enemyAiState = EnemyAiState.Attack;
                }

                break;

            case EnemyAiState.Attack:

                if (wasAttack) return;

                if (attackOnColliderTimer <= 0)
                {
                    wasAttack = true;

                    StartAttack();
                }
                break;
            
            case EnemyAiState.AttackCooldown:

                Destroy(attackObj);
                wasAttack = false;
                

                enemyAnim.ChangeState(EnemyAnimState.Idle);
                
                if (attackCooldownTimer <= 0)
                {
                    enemyAiState = EnemyAiState.Idle;
                }

                break;
        }
    }

    /// <summary>
    /// 攻撃種類の選択
    /// </summary>
    void StartAttack()
    {
        //Debug.Log("StartAttack");
        switch (enemyType)
        {
            case EnemyType.Torcher:

                TorcherAttack();

                break;

            case EnemyType.Archer:

                ArcherAttack();

                break;
        }
    }

    /// <summary>
    /// WeackTocher用
    /// 攻撃処理
    /// </summary>
    void TorcherAttack()
    {
        Debug.Log("TorcherAttack");

        attackObj = Instantiate(attackCol, this.transform);

        attackObj.GetComponent<EnemyAttack>().SetAttackPower(attackPower);

        attackObj.transform.position = attackRange * 0.5f * targetDirSign + this.enemyPos;     // 攻撃距離に合わせる
    }

    /// <summary>
    /// WeakArcher用
    /// 攻撃処理
    /// </summary>
    void ArcherAttack()
    {
        Debug.Log("ArcherAttack");

        se.PlaySE(0);

        arrowObj = Instantiate(attackCol, transform.position, Quaternion.identity);

        arrowObj.GetComponent<ArrowController>().SetArrowAttack(targetDir, playerPos, attackPower);
    }

    /// <summary>
    /// 敵の移動処理
    /// </summary>
    void MoveEnemy()
    {
        if (enemyAiState != EnemyAiState.Move) return;

        transform.position = Vector2.MoveTowards(transform.position, playerPos, moveSpeed * Time.fixedDeltaTime);
    }

    /// <summary>
    /// 移動時にプレイヤーの方向を向く
    /// </summary>
    void FlipEnemy()
    {
        if (enemyAiState != EnemyAiState.Move) return;
        
        if (targetDir.x < 0)    // 左向き
        {
            this.transform.rotation = Quaternion.Euler(0, 180, 0);
        }
        else
        {
            this.transform.rotation = Quaternion.Euler(0, 0, 0);
        }
    }
}
