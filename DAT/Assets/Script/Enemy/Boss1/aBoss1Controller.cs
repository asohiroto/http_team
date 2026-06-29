#if false

using UnityEngine;

public class Boss1Controller : MonoBehaviour
{
    [SerializeField] private float bossHp;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float dashSpeed;
    [SerializeField] private float stopDist;

    [Header("Attack")]
    [SerializeField] private float attack1Power;
    [SerializeField] private float attack2Power;
    [SerializeField] private float[] attackStartupDuration;
    [SerializeField] private float[] attackCooldownDuration;
    [SerializeField] private float attackOnColliderDuration;
    [SerializeField] private int nowAttackNum;
    [SerializeField] private float[] animStartupDuration;

    [Header("config")]
    [SerializeField] private GameObject playerObj;      // Playerオブジェクト
    [SerializeField] private GameObject col1;
    [SerializeField] private GameObject col2;
    private EnemyAnimation enemyAnim;

    [Header("Debug")]
    [SerializeField] private Vector2 myPos = Vector2.zero;
    [SerializeField] private Vector2 playerPos = Vector2.zero;
    [SerializeField] private Vector2 moveDir = Vector2.zero;
    [SerializeField] private float distSq;
    [SerializeField] private float stopDistSq;


    [Header("State")]
    [SerializeField] private float attackStateTimer = 0.0f;
    [SerializeField] private float animStartupTimer = 0.0f;
    [SerializeField] private bool isAttack = false;
    [SerializeField] private bool isFaceRight = false;
    private enum AttackData { Idle, Startup, Active, Cooldown }

    [SerializeField] private AttackData attackState;

    private enum AttackType { Attack1, Attack2, Summon }
    [SerializeField] private AttackType attackType;
    private enum CurrentAnim { Idle, Walk, Atk1, Atk2, Atk3, Down }

    [SerializeField] private CurrentAnim currentAnim;

    private void Start()
    {
        if (playerObj == null)
        {
            playerObj = GameObject.FindWithTag("Player");
        }

        stopDistSq = stopDist * stopDist;

        enemyAnim = GetComponent<EnemyAnimation>();
    }

    private void Update()
    {
        attackStateTimer -= Time.deltaTime;
        animStartupTimer -= Time.deltaTime;
    }

    private void FixedUpdate()
    {
        SetInfo();
        Move();
    }

    float GetSqrDistance(Vector2 a, Vector2 b)
    {
        return (b.x - a.x) * (b.x - a.x) + (b.y - a.y) * (b.y - a.y);
    }

    private void CheckIsRight()
    {
        if (playerPos.x - myPos.x > 0)
        {
            isFaceRight = true;
        }
        else
        {
            isFaceRight = false;
        }

    }

    private void SetInfo()
    {
        playerPos = playerObj.transform.position;
        myPos = this.transform.position;

        distSq = GetSqrDistance(playerPos, myPos);
        moveDir = (playerPos - myPos).normalized;

        if (isAttack) return;

        CheckIsRight();

    }

    private void Move()
    {
        if (stopDist * stopDist > distSq)
        {
            StartAttack();

            return;
        }

        SetNextAnim(CurrentAnim.Walk);
        transform.position = Vector2.MoveTowards(transform.position, playerPos, moveSpeed * Time.fixedDeltaTime);
    }

    private void StartAttack()
    {
        if (isAttack) return;
        isAttack = true;

        int len = attackCooldownDuration.Length - 1;
        attackType = (AttackType)Random.Range(0, len);

        float startup = attackStartupDuration[(int)attackType];
        float cooldown = attackCooldownDuration[(int)attackType];

        attackStateTimer = startup;

        UpdateAttackState(startup, attackOnColliderDuration, cooldown);

        // 攻撃処理呼び出し
        /*
        if (nowAttackNum == 0)
        {
            attackType = 0;
            Attack1();
        }
        else if (nowAttackNum == 1)
        {
            Attack2();
        }
        else
        {
            Attack3();
        }*/
        switch(attackType)
        {
            case AttackType.Attack1:

                break;

                Attack1();

                case AttackType.Attack2:

                Attack2();

                break;

                case AttackType.Summon:

                Attack3();

                break;
        }
    }


    private void UpdateAttackState(float startup, float onCol, float cooldown)
    {
        switch (attackState)
        {
            case AttackData.Startup:

                if (attackStateTimer < 0)
                {
                    attackStateTimer += onCol;

                    attackState = AttackData.Active;
                }

                break;

            case AttackData.Active:

                if (attackStateTimer < 0)
                {
                    OnAttackCollider();

                    attackStateTimer += cooldown;

                    attackState = AttackData.Cooldown;
                }

                break;

            case AttackData.Cooldown:

                if (attackStateTimer < 0)
                {
                    attackState = AttackData.Idle;

                    SetNextAnim(CurrentAnim.Idle);
                }

                break;
        }
    }


    private void SetNextAnim(CurrentAnim nextAnim)
    {
        if (currentAnim == nextAnim) return;

        currentAnim = nextAnim;

        switch (nextAnim)
        {
            case CurrentAnim.Idle:

                if (isFaceRight)
                {
                    enemyAnim.ChangeState(EnemyAnimState.Idle);
                }
                else
                {
                    enemyAnim.ChangeState(EnemyAnimState.IdleR);
                }

                break;

            case CurrentAnim.Walk:

                if (isFaceRight)
                {
                    enemyAnim.ChangeState(EnemyAnimState.Walk);
                }
                else
                {
                    enemyAnim.ChangeState(EnemyAnimState.WalkR);
                }

                break;

            case CurrentAnim.Atk1:

                if (isFaceRight)
                {
                    enemyAnim.ChangeState(EnemyAnimState.Atk1);
                }
                else
                {
                    enemyAnim.ChangeState(EnemyAnimState.Atk1R);
                }

                break;

            case CurrentAnim.Atk2:

                if (isFaceRight)
                {
                    enemyAnim.ChangeState(EnemyAnimState.Atk2);
                }
                else
                {
                    enemyAnim.ChangeState(EnemyAnimState.Atk2R);
                }

                break;

            case CurrentAnim.Atk3:

                if (isFaceRight)
                {
                    enemyAnim.ChangeState(EnemyAnimState.Atk3);
                }
                else
                {
                    enemyAnim.ChangeState(EnemyAnimState.Atk3R);
                }

                break;

            case CurrentAnim.Down:

                if (isFaceRight)
                {
                    enemyAnim.ChangeState(EnemyAnimState.Down);
                }
                else
                {
                    enemyAnim.ChangeState(EnemyAnimState.DownR);
                }

                break;
        }
    }

    private void Attack1()
    {
        SetNextAnim(CurrentAnim.Atk1);

        Debug.Log("1");
    }
    private void Attack2()
    {
        SetNextAnim(CurrentAnim.Atk2);

        Debug.Log("2");
    }
    private void Attack3()
    {
        SetNextAnim(CurrentAnim.Atk3);

        Debug.Log("3");
    }

    private void OnAttackCollider()
    {
        if (nowAttackNum == 0)
        {
            // col1呼び出し
            Debug.Log("col1");
        }
        else if (nowAttackNum == 1)
        {
            // col2呼び出し
            Debug.Log("col2");
        }
        else
        {
            Debug.Log("範囲外");
            return;
        }
    }

}
#endif