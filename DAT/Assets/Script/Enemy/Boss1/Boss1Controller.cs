#if true
using UnityEngine;

public class Boss1Controller : MonoBehaviour
{
    private enum AttackType
    {
        None,
        Attack1,
        Attack2,
        Summon
    }

    private enum AttackState
    {
        Idle,
        Startup,
        Active,
        Cooldown
    }

    private enum CurrentAnim
    {
        None,
        Idle,
        Walk,
        Attack1,
        Attack2,
        Attack3,
        Down
    }

    // 攻撃ごとに時間を設定
    [System.Serializable]
    private struct AttackTiming
    {
        // 予備動作
        public float startupDuration;

        // 実行時間
        public float activeDuration;

        // 後隙
        public float cooldownDuration;
    }

    // 基本設定

    [Header("Boss Status")]
    [SerializeField] private float bossHp;

    [SerializeField] private float moveSpeed;

    [Tooltip("プレイヤーとの距離がこの値以下になると攻撃を開始する")]
    [SerializeField] private float attackStartDistance;

    // 攻撃設定
    [Header("Attack 1")]
    [SerializeField] private int attack1Power;
    [SerializeField] private AttackTiming attack1Timing;

    [Header("Attack 2")]
    [SerializeField] private int attack2Power;
    [SerializeField] private AttackTiming attack2Timing;

    [Header("Attack 3 : 召喚")]
    [SerializeField] private AttackTiming summonTiming;

    [Tooltip("召喚するプレハブ")]
    [SerializeField] private GameObject minionPrefab;
    [SerializeField] private int summonCount = 3;
    [SerializeField] private float minionDist;  // minion同士の間隔

    // 攻撃用のコライダー
    [Header("Attack Hitbox Prefab")]
    [SerializeField] private GameObject attack1HitboxPrefab;
    [SerializeField] private GameObject attack2HitboxPrefab;

    [Header("Attack Spawn Points")]
    [SerializeField] private Vector2 attack1SpawnPoint;
    [SerializeField] private Vector2 attack2SpawnPoint;


    [Header("References")]
    [SerializeField] private GameObject playerObj;
    private EnemyAnimation enemyAnim;


    [Header("State")]
    [SerializeField] private AttackType currentAttack = AttackType.None;
    [SerializeField] private AttackState attackState = AttackState.Idle;
    [SerializeField] private CurrentAnim currentAnim = CurrentAnim.None;
    [SerializeField] private GameObject activeAttackHitbox;

    [SerializeField] private bool isFaceRight = true;
    [SerializeField] private float attackStateTimer;

    [Header("Debug")]
    [SerializeField] private Vector2 bossPos;
    [SerializeField] private Vector2 playerPos;
    [SerializeField] private float distanceSq;
    [SerializeField] private bool isDie = false;

    private float attackStartDistanceSq;
    private bool currentAnimFacingRight;

    [SerializeField] private EnemyHpManager hpManager;
    [SerializeField] private EnemySpawner enemySpawner;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        enemyAnim = GetComponent<EnemyAnimation>();

        
        // 親オブジェクト(EnemySpawner)を取得
        GameObject parentObj = transform.parent.gameObject;
        enemySpawner = parentObj.GetComponent<EnemySpawner>();
        hpManager = GetComponent<EnemyHpManager>();

        if (playerObj == null)
        {
            playerObj = GameObject.FindWithTag("Player");
        }


        attackStartDistanceSq = attackStartDistance * attackStartDistance;

        DestroyActiveAttackHitbox();

        UpdateTargetInfo();
        UpdateFaceDir();

        SetAnimation(CurrentAnim.Idle);
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        UpdateTargetInfo();
        HpManage();

        switch (attackState)
        {
            case AttackState.Idle:

                UpdateIdle();

                break;

            // 攻撃処理時のみ移動しない
            case AttackState.Startup:
            case AttackState.Active:
            case AttackState.Cooldown:
                UpdateAttackState();
                break;
        }
    }

    /// <summary>
    /// プレイヤーオブジェクトを取得
    /// </summary>
    private void InitializePlayerReference()
    {
        if (playerObj != null) return;

        playerObj = GameObject.FindWithTag("Player");
    }

    /// <summary>
    /// ボス、プレイヤーの位置と距離を更新
    /// </summary>
    private void UpdateTargetInfo()
    {
        // スプライトのズレ分 y座標を動かす
        bossPos = transform.position;
        playerPos = new Vector2(playerObj.transform.position.x, playerObj.transform.position.y - 0.5f);

        distanceSq = GetSqDistance(bossPos, playerPos);
    }

    /// <summary>
    /// 2点間の距離の二乗を返す。
    /// </summary>
    private float GetSqDistance(Vector2 a, Vector2 b)
    {
        float x = b.x - a.x;
        float y = b.y - a.y;

        return x * x + y * y;
    }

    /// <summary>
    /// プレイヤーの方向を確認
    /// </summary>
    private void UpdateFaceDir()
    {
        //if (playerPos.x >= bossPos.x) isFaceRight = true;

        isFaceRight = playerPos.x >= bossPos.x;
    }

    /* 攻撃処理 */

    /// <summary>
    /// 攻撃処理時　以外の行動
    /// </summary>
    private void UpdateIdle()
    {
        UpdateFaceDir();

        if (CanAttack())
        {
            // 攻撃処理
            StartAttack();

            return;
        }

        if (currentAnim == CurrentAnim.Down) return;

        // 移動処理
        MoveToPlayer();
    }

    /// <summary>
    /// 攻撃可能か
    /// </summary>
    /// <returns></returns>
    private bool CanAttack()
    {
        return distanceSq <= attackStartDistanceSq;
    }

    private void MoveToPlayer()
    {
        // アニメーションの変更
        SetAnimation(CurrentAnim.Walk);

        // 移動
        transform.position = Vector2.MoveTowards(
            transform.position,
            playerPos,
            moveSpeed * Time.fixedDeltaTime);
    }

    /* 攻撃処理 */

    /// <summary>
    /// 攻撃をランダムに選択し、行う
    /// </summary>
    private void StartAttack()
    {
        // 攻撃の種類をランダムに設定
        currentAttack = SelectRandomAttack();

        switch (currentAttack)
        {
            case AttackType.Attack1:

                StartAttack1();

                break;

            case AttackType.Attack2:

                StartAttack2();

                break;

            case AttackType.Summon:

                StartSummonAttack();

                break;

            default:

                FinishAttack();

                break;
        }
    }

    private AttackType SelectRandomAttack()
    {
        int randomValue = Random.Range(0, 3);

        switch (randomValue)
        {
            case 0:

                return AttackType.Attack1;

            case 1:

                return AttackType.Attack2;

            case 2:

                return AttackType.Summon;

            default:

                return AttackType.Attack1;
        }

    }

    /* 攻撃のアニメーション、攻撃力の設定 */
    private void StartAttack1()
    {
        // アニメーションの変更
        SetAnimation(CurrentAnim.Attack1);

        ChangeAttackState(AttackState.Startup, attack1Timing.startupDuration);

    }

    private void StartAttack2()
    {
        // アニメーションの変更
        SetAnimation(CurrentAnim.Attack2);

        ChangeAttackState(AttackState.Startup, attack2Timing.startupDuration);

    }

    private void StartSummonAttack()
    {
        SetAnimation(CurrentAnim.Attack3);

        ChangeAttackState(AttackState.Startup, summonTiming.startupDuration);

    }

    private void UpdateAttackState()
    {
        attackStateTimer -= Time.fixedDeltaTime;

        if (attackStateTimer > 0.0f) return;

        switch (attackState)
        {
            case AttackState.Startup:

                BeginActiveState();

                break;

            case AttackState.Active:

                BeginCooldownState();

                break;

            case AttackState.Cooldown:

                FinishAttack();

                break;
        }
    }

    /// <summary>
    /// 攻撃状態の変更
    /// </summary>
    /// <param name="nextState"></param>
    /// <param name="duration"></param>
    private void ChangeAttackState(AttackState nextState, float duration)
    {
        attackState = nextState;
        attackStateTimer = duration;
    }

    /// <summary>
    /// 攻撃開始
    /// </summary>
    private void BeginActiveState()
    {
        ActivateCurrentAttackEffect();

        ChangeAttackState(AttackState.Active, GetCurrentAttackTiming().activeDuration);
    }

    private void BeginCooldownState()
    {
        DestroyActiveAttackHitbox();

        ChangeAttackState(AttackState.Cooldown, GetCurrentAttackTiming().cooldownDuration);
    }

    /// <summary>
    /// 現在の攻撃に対応する時間設定を返す。
    /// </summary>
    private AttackTiming GetCurrentAttackTiming()
    {
        switch (currentAttack)
        {
            case AttackType.Attack1:
                return attack1Timing;

            case AttackType.Attack2:
                return attack2Timing;

            case AttackType.Summon:
                return summonTiming;

            default:
                return default;
        }
    }

    /// <summary>
    /// 攻撃の当たり判定のプレハブを生成
    /// </summary>
    private void ActivateCurrentAttackEffect()
    {
        switch (currentAttack)
        {
            case AttackType.Attack1:
                ActivateAttack1();
                break;

            case AttackType.Attack2:
                ActivateAttack2();
                break;

            case AttackType.Summon:
                SummonMinions();
                break;
        }
    }

    /* プレハブ生成 */

    private void ActivateAttack1()
    {
        // 左右を設定
        Vector2 spawnPos = Vector2.zero;
        if (!isFaceRight)
        {
            spawnPos = new Vector2(-attack1SpawnPoint.x, attack1SpawnPoint.y);
        }
        else
        {
            spawnPos = attack1SpawnPoint;
        }

        // プレハブ生成
        activeAttackHitbox = Instantiate(
            attack1HitboxPrefab,
            bossPos + spawnPos,
            Quaternion.identity);

        EnemyAttack attack = activeAttackHitbox.GetComponent<EnemyAttack>();

        attack.SetAttackPower(attack1Power);
    }

    private void ActivateAttack2()
    {
        // 左右を設定
        Vector2 spawnPos = Vector2.zero;
        if (!isFaceRight)
        {
            spawnPos = new Vector2(-attack2SpawnPoint.x, attack2SpawnPoint.y);
        }
        else
        {
            spawnPos = attack2SpawnPoint;
        }

        // プレハブ生成
        activeAttackHitbox = Instantiate(
            attack2HitboxPrefab,
            bossPos + spawnPos,
            Quaternion.identity);

        EnemyAttack attack = activeAttackHitbox.GetComponent<EnemyAttack>();

        attack.SetAttackPower(attack2Power);
    }

    private void SummonMinions()
    {
        Vector2 spawnPos = Vector2.zero;

        if (isFaceRight)    // 右
        {
            spawnPos = new Vector2(10.0f, minionDist);
        }
        else
        {
            spawnPos = new Vector2(-10.0f, minionDist);
        }

        for (int i = 0; i < summonCount; i++)
        {
            GameObject newObj = Instantiate(minionPrefab, this.transform);


            newObj.transform.localScale = Vector3.one;

            newObj.transform.position = spawnPos;


            spawnPos.y -= minionDist;
        }


    }

    /// <summary>
    /// 当たり判定のプレハブを削除
    /// </summary>
    private void DestroyActiveAttackHitbox()
    {
        Destroy(activeAttackHitbox);
        activeAttackHitbox = null;
    }


    private void FinishAttack()
    {
        DestroyActiveAttackHitbox();

        currentAttack = AttackType.None;
        attackState = AttackState.Idle;
        attackStateTimer = 0.0f;

        UpdateFaceDir();
        SetAnimation(CurrentAnim.Idle);
    }

    /* アニメーション */

    /// <summary>
    /// アニメーションを変更
    /// </summary>
    /// <param name="nextAnim"></param>
    private void SetAnimation(CurrentAnim nextAnim)
    {
        if (isDie) return;

        bool isSameAnim = currentAnim == nextAnim;
        bool isSameDir = currentAnimFacingRight == isFaceRight;

        if (isSameAnim && isSameDir) return;

        currentAnim = nextAnim;
        currentAnimFacingRight = isFaceRight;

        EnemyAnimState nextState = GetEnemyAnimState(nextAnim, isFaceRight);

        enemyAnim.ChangeState(nextState);

    }

    /// <summary>
    /// アニメーションの変更先を決定
    /// </summary>
    /// <param name="anim"></param>
    /// <param name="facingRight"></param>
    /// <returns></returns>
    private EnemyAnimState GetEnemyAnimState(CurrentAnim anim, bool facingRight)
    {
        switch (anim)
        {
            case CurrentAnim.Idle:

                if (facingRight)
                {
                    return EnemyAnimState.Idle;
                }
                else
                {
                    return EnemyAnimState.IdleR;
                }

            case CurrentAnim.Walk:

                if (facingRight)
                {
                    return EnemyAnimState.Walk;
                }
                else
                {
                    return EnemyAnimState.WalkR;
                }

            case CurrentAnim.Attack1:

                if (facingRight)
                {
                    return EnemyAnimState.Atk1;
                }
                else
                {
                    return EnemyAnimState.Atk1R;
                }

            case CurrentAnim.Attack2:

                if (facingRight)
                {
                    return EnemyAnimState.Atk2;
                }
                else
                {
                    return EnemyAnimState.Atk2R;
                }

            case CurrentAnim.Attack3:

                if (facingRight)
                {
                    return EnemyAnimState.Atk3;
                }
                else
                {
                    return EnemyAnimState.Atk3R;
                }

            case CurrentAnim.Down:
                
                if (facingRight)
                {
                    return EnemyAnimState.Down;
                }
                else
                {
                    return EnemyAnimState.DownR;
                }

            default:

                if (facingRight)
                {
                    return EnemyAnimState.Idle;
                }
                else
                {
                    return EnemyAnimState.IdleR;
                }

        }

    }

    private void HpManage()
    {
        if (isDie) return;

        bossHp = hpManager.GetCurrentHp();

        if (hpManager.TakeDamage())
        {
            enemyAnim.StartBlink();
        }

        if (bossHp <= 0)
        {
            Down();
        }
    }

    /// <summary>
    /// すべてをリセットし、ダウン状態に
    /// </summary>
    private void Down()
    {
        DestroyActiveAttackHitbox();

        currentAttack = AttackType.None;
        attackState = AttackState.Idle;

        attackStateTimer = 0.0f;

        SetAnimation(CurrentAnim.Down);

        isDie = true;
    }

    public void OnAnimationFinished(EnemyAnimState finishedState)
    {
        switch (finishedState)
        {
            case EnemyAnimState.Atk1:
            case EnemyAnimState.Atk1R:
            case EnemyAnimState.Atk2:
            case EnemyAnimState.Atk2R:
            case EnemyAnimState.Atk3:
            case EnemyAnimState.Atk3R:

                SetAnimation(CurrentAnim.Idle);

                break;

            case EnemyAnimState.Down:
            case EnemyAnimState.DownR:

                float tmpTimer = attackStateTimer;
                attackStateTimer -= Time.fixedDeltaTime;

                if (tmpTimer > attackStateTimer + 1.0f)
                {
                    Destroy(gameObject);
                }

                break;
        }
    }


}
#endif