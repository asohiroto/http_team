#if false
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
        Idle,
        Walk,
        Attack1,
        Attack2,
        Attack3,
        Down
    }

    // 攻撃ごとに時間を設定
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
    [SerializeField] private float attack1Power;
    [SerializeField] private AttackTiming attack1Timing;

    [Header("Attack 2")]
    [SerializeField] private float attack2Power;
    [SerializeField] private AttackTiming attack2Timing;

    [Header("Attack 3 : 召喚")]
    [SerializeField] private AttackTiming summonTiming;

    [Tooltip("召喚するプレハブ")]
    [SerializeField] private GameObject minionPrefab;
    [SerializeField] private int summonCount;

    // 攻撃用のコライダー
    [Header("Attack Hitbox Prefab")]
    [SerializeField] private GameObject attack1Hitbox;
    [SerializeField] private GameObject attack2Hitbox;

    [Header("Attack Spawn Points")]
    [SerializeField] private Transform attack1SpawnPoint;
    [SerializeField] private Transform attack2SpawnPoint;


    [Header("References")]
    [SerializeField] private GameObject playerObj;
    private EnemyAnimation enemyAnim;


    [Header("State")]
    [SerializeField] private AttackType currentAttack = AttackType.None;
    [SerializeField] private AttackState attackState = AttackState.Idle;
    [SerializeField] private CurrentAnim currentAnim = CurrentAnim.Idle;
    [SerializeField] private GameObject activeAttackHitbox;

    [SerializeField] private bool isFaceRight = true;
    [SerializeField] private float attackStateTimer;

    [Header("Debug")]
    [SerializeField] private Vector2 bossPos;
    [SerializeField] private Vector2 playerPos;
    [SerializeField] private float distanceSq;

    private float attackStartDistanceSq;
    private bool currentAnimFacingRigh;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        enemyAnim = GetComponent<EnemyAnimation>();


    }

    // Update is called once per frame
    void Update()
    {

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
        bossPos = transform.position;
        playerPos = playerObj.transform.position;

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

    private void UpdateIdle()
    {
        UpdateFaceDir();

        if (CanAttack())
        {
            // 攻撃処理

            return;
        }

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

    /* 攻撃のアニメーション、威力の設定 */
    private void StartAttack1()
    {
        // アニメーションの変更

    }

    private void StartAttack2()
    {
        // アニメーションの変更

    }

    private void StartSummonAttack()
    {

    }

    private void UpdateAttackState()
    {
        attackStateTimer -= Time.fixedDeltaTime;

        if (attackStateTimer > 0.0f) return;

        switch (attackState)
        {
            case AttackState.Startup:

                break;

            case AttackState.Active:

                break;

            case AttackState.Cooldown:

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


    private void BeginActiveState()
    {
        //

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

    }

    private void ActivateAttack2()
    {

    }

    private void SummonMinions()
    {

    }

    /// <summary>
    /// 当たり判定のプレハブを削除
    /// </summary>
    private void DeactivateCurrentAttackEffect()
    {
        switch (currentAttack)
        {
            // プレハブ削除処理
        }
    }
}
#endif