#if false

using UnityEngine;

public class Boss1Controller : MonoBehaviour
{
    [SerializeField] private float bossHp;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float dashSpeed;
    [SerializeField] private float stopDist;

    [Header("Attack")]
    [SerializeField] private float[] attackPower;
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
    [SerializeField] private float dist;
    private float stopDistSq;


    [Header("State")]
    [SerializeField] private float timer = 0.0f;
    [SerializeField] private float animStartupTimer = 0.0f;
    [SerializeField] private bool isAttack = false;
    [SerializeField] private bool isRight = false;
    private enum AttackState { Startup, OnCollider, Cooldown, Init }

    [SerializeField] private AttackState attackState;
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
        timer -= Time.deltaTime;
        animStartupTimer -= Time.deltaTime;
    }

    private void FixedUpdate()
    {
        CheckIsRight();
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
            isRight = true;
        }
        else
        {
            isRight = false;
        }

    }

    private void SetInfo()
    {
        playerPos = playerObj.transform.position;
        myPos = this.transform.position;

        dist = GetSqrDistance(playerPos, myPos);
        moveDir = (playerPos - myPos).normalized;

    }

    private void Move()
    {
        if (stopDist > dist)
        {
            AttackSetup();

            return;
        }

        SetNextAnim(CurrentAnim.Walk);
        transform.position = Vector2.MoveTowards(transform.position, playerPos, moveSpeed * Time.fixedDeltaTime);
    }

    private void AttackSetup()
    {
        if (isAttack) return;
        isAttack = true;

        int len = attackPower.Length - 1;
        nowAttackNum = Random.Range(0, len);

        float startup = attackStartupDuration[nowAttackNum];
        float cooldown = attackCooldownDuration[nowAttackNum];

        timer = startup;

        ChangeAttackState(startup, attackOnColliderDuration, cooldown);

        // 攻撃処理呼び出し
        if (nowAttackNum == 0)
        {
            Attack1();
        }
        else if (nowAttackNum == 1)
        {
            Attack2();
        }
        else
        {
            Attack3();
        }
    }


    private void ChangeAttackState(float startup, float onCol, float cooldown)
    {
        switch (attackState)
        {
            case AttackState.Startup:

                if (timer < 0)
                {
                    timer += onCol;

                    attackState = AttackState.OnCollider;
                }

                break;

            case AttackState.OnCollider:

                if (timer < 0)
                {
                    OnAttackCollider();

                    timer += cooldown;

                    attackState = AttackState.Cooldown;
                }

                break;

            case AttackState.Cooldown:

                if (timer < 0)
                {
                    attackState = AttackState.Init;

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

                if (isRight)
                {
                    enemyAnim.ChangeState(EnemyAnimState.Idle);
                }
                else
                {
                    enemyAnim.ChangeState(EnemyAnimState.IdleR);
                }

                break;

            case CurrentAnim.Walk:

                if (isRight)
                {
                    enemyAnim.ChangeState(EnemyAnimState.Walk);
                }
                else
                {
                    enemyAnim.ChangeState(EnemyAnimState.WalkR);
                }

                break;

            case CurrentAnim.Atk1:

                if (isRight)
                {
                    enemyAnim.ChangeState(EnemyAnimState.Atk1);
                }
                else
                {
                    enemyAnim.ChangeState(EnemyAnimState.Atk1R);
                }

                break;

            case CurrentAnim.Atk2:

                if (isRight)
                {
                    enemyAnim.ChangeState(EnemyAnimState.Atk2);
                }
                else
                {
                    enemyAnim.ChangeState(EnemyAnimState.Atk2R);
                }

                break;

            case CurrentAnim.Atk3:

                if (isRight)
                {
                    enemyAnim.ChangeState(EnemyAnimState.Atk3);
                }
                else
                {
                    enemyAnim.ChangeState(EnemyAnimState.Atk3R);
                }

                break;

            case CurrentAnim.Down:

                if (isRight)
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