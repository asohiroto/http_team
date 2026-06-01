using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    [SerializeField] private Vector3 lookPos = Vector2.zero;    // 攻撃時に使用する
    [SerializeField] private float playerDist = 0f;                 // PlayerとEnemyの距離
    [SerializeField] private bool isFindPlayer = false;
    [SerializeField] private bool isLostPlayer = false;
    [SerializeField] private bool isChasePlayer = false;
    [SerializeField] private bool canAttack = false;
    [SerializeField] private bool isAttack = false;
    [SerializeField] private bool isAttackCool = false;
    [SerializeField] private bool isStop = false;
    [SerializeField] private bool isLookRight = false;
    public bool IsAttack = false;   // いい方法が思いつかなかったので
    private bool playing = false;
    [SerializeField] private float cdTimer = 0f;

    [Header("Config")]
    [SerializeField] private float takeDamageDist = 1f; // Player からの攻撃をくらう距離
    [SerializeField] private GameObject player;         // Player オブジェクト
    [SerializeField] private GameObject coinPrefab;     // Coin オブジェクト
    [SerializeField] private GameObject coinParent;     // Coin のドロップ時の親オブジェクト
    [SerializeField] GameObject attackCol;              // 攻撃の当たり判定(プレハブ)
    EnemyAnimation enemyAnim;
    GameObject obj;

    public enum EnemyMoveState { Idle, Walk, SideAttack, LowerAttack, UpperAttack, AttackCool }

    [SerializeField]
    private EnemyMoveState currentMoveState = EnemyMoveState.Idle;


    public float AttackDist => attackDist;
    public int AttackPower => attackPower;
    public float AttackSec => attackSec;
    public float AttackCd => attackCd;
    public Vector2 NowDir => nowDir;
    public bool CanAttack => canAttack;     // 攻撃可能か　読み取り専用
    public bool IsMove => isChasePlayer;
    public GameObject AttackCol => attackCol;

    Component attack;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerDist = findDist + 1;   // 0で開始すると値が代入されるまでの間に動いてしまうため

        // 見失う距離が発見距離よりも短い場合、見失う距離を発見距離と同じ大きさにします。
        if (loseDist < findDist)  loseDist = findDist;

        enemyAnim = GetComponent<EnemyAnimation>();
        attack = GetComponent<EnemyAttack>();

        playing = true;

        if (player == null)
        {
            player = GameObject.FindWithTag("Player");
        }
        if (coinParent == null)
        {
            coinParent = GameObject.FindWithTag("DropItems");
        }

        // HPが0のとき、スポーンさせない <- これいる？　検討中    // 必ず一番最後に処理
    }

    private void FixedUpdate()
    {
        CheckDist();
        Attack();
        AttackCool();
        ChangeAnimation();
        LookPlayer();
        CheckLookDir();
        LookHor();
        ChasePlayer();
        //Animation();
        //AttackAnim();
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

            // コインのスポーンはできる
            // スポーン位置がthis.transformの位置とズレている -> Prefabのミス
            GameObject dropCoin = Instantiate(coinPrefab, this.transform);
            // ドロップ時に他のオブジェクトの子オブジェクトにします
            dropCoin.transform.SetParent(coinParent.transform);
            
            Destroy(this.gameObject);
        }
    }

    // Playerとの距離を取得
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

    void ChangeAnimation()
    {
        switch (currentMoveState)
        {
            case EnemyMoveState.Idle:
                enemyAnim.ChangeState(EnemyAnimState.Idle);
                break;

            case EnemyMoveState.Walk:
                enemyAnim.ChangeState(EnemyAnimState.Walk);
                break;

            case EnemyMoveState.SideAttack:
                
                enemyAnim.ChangeState(EnemyAnimState.SideAttack);
                break;

            case EnemyMoveState.LowerAttack:
                enemyAnim.ChangeState(EnemyAnimState.LowerAttack);
                break;

            case EnemyMoveState.UpperAttack:
                enemyAnim.ChangeState(EnemyAnimState.UpperAttack);
                break;

            case EnemyMoveState.AttackCool:
                break;
        }
    }

    void LookPlayer()
    {
        // これを列挙型にしたら楽じゃない？
        // アニメーションの呼び出しもSwitch文でできそう
        isFindPlayer = findDist > playerDist;   // 発見距離内か
        isLostPlayer = loseDist < playerDist;   // 見失ったか
        canAttack = attackDist > playerDist;    // 攻撃範囲内か
        isStop = stopDist > playerDist;         // 止まる距離、攻撃中か

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
        
        if (attackDist > playerDist)
        {
            // 仮
            if (isAttack) return;
            isAttack = true;

            //currentMoveState = EnemyMoveState.Attack;
            if (lookPos.y == 0)         // 横向き
            {
                currentMoveState = EnemyMoveState.SideAttack;
            }
            else if (lookPos.y == 1)     // 上向き
            {
                currentMoveState = EnemyMoveState.UpperAttack;
            }
            else if (lookPos.y == -1)    // 下向き
            {
                currentMoveState = EnemyMoveState.LowerAttack;
            }
        }
        else if ((!isLostPlayer || alwaysFindPlayer) && !isStop)
        {
            if (isFindPlayer || alwaysFindPlayer)
            {
                currentMoveState = EnemyMoveState.Walk;
            }
        }
        else
        {
            currentMoveState = EnemyMoveState.Idle;
        }
        
    }

    public void IsAttackFalse()
    {
        isAttack = false;
        isAttackCool = false;
    }

    void Attack()
    {
        // isAttackがfalseなら返す
        if (!isAttack) return;
        // クールダウン中なら返す
        if (isAttackCool) return;
        isAttackCool = true;
        obj = Instantiate(attackCol, this.transform);      // プレハブ呼び出し
        obj.transform.position = AttackDist * 0.5f * lookPos + transform.position;     // 攻撃距離に合わせる

    }

    public void FinishAnim()
    {
        if (currentMoveState == EnemyMoveState.SideAttack
            || currentMoveState == EnemyMoveState.LowerAttack
            || currentMoveState == EnemyMoveState.UpperAttack)
        {
            Destroy(obj);
            currentMoveState = EnemyMoveState.AttackCool;
        }
    }

    // 攻撃のクールダウン処理
    void AttackCool()
    {
        if (!isAttack) return;
        cdTimer -= Time.deltaTime;

    }

    void CheckLookDir()
    {
        float dirX = nowDir.x;
        float dirY = nowDir.y;

        // 絶対値を取った現在向いている方向
        float ABSdirX = Mathf.Abs(nowDir.x);
        float ABSdirY = Mathf.Abs(nowDir.y);

        if (dirX > 0)   // 右向き
        {
            isLookRight = true;
        }
        else            // 左向き
        {
            isLookRight = false;
        }

        if (ABSdirY > ABSdirX)  // 上下方向
        {
            lookPos.x = 0;
            if (dirY > 0)       // 上向き
            {
                lookPos.y = 1;
            }
            else if (dirY < 0)  // 下向き
            {
                lookPos.y = -1;
            }
        }
        else                    // 左右方向
        {
            lookPos.y = 0;
            if (dirX > 0)
            {
                lookPos.x = 1;  // 右向き
            }
            else if (dirX < 0)
            {
                lookPos.x = -1; // 左向き
            }
        }
    }

    // 左右を向く
    private void LookHor()
    {
        if (!isChasePlayer) return;
        if (isLookRight)
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        else
        {
            transform.rotation = Quaternion.Euler(0, 180, 0);
        }
    }

    void ChasePlayer()
    {
        // 追跡状態じゃないなら返す
        if (!isChasePlayer) return;
        // 移動中ではないなら返す
        if (currentMoveState != EnemyMoveState.Walk) return;

        transform.position = Vector2.MoveTowards(transform.position, new Vector2(playerPos.x, playerPos.y), e_moveSpeed * Time.fixedDeltaTime);
    }

    void Animation()
    {
        if (canAttack) return;
        if (!isChasePlayer)
        {
            enemyAnim.ChangeState(EnemyAnimState.Idle);
        }
        if (isChasePlayer)
        {
            enemyAnim.ChangeState(EnemyAnimState.Walk);
        }
    }
    // Animationとひとまとめにしたい
    void AttackAnim()
    {
        if (!canAttack) return;
        if (lookPos.y == 0)         // 横向き
        {
            enemyAnim.ChangeState(EnemyAnimState.SideAttack);
        }
        else if (lookPos.x == 1)     // 上向き
        {
            enemyAnim.ChangeState(EnemyAnimState.UpperAttack);
        }
        else if (lookPos.x == -1)    // 下向き
        {
            enemyAnim.ChangeState(EnemyAnimState.LowerAttack);
        }
    }


    // デバッグ用処理
    private void OnDrawGizmos()
    {
        // セグメント数
        int seg = 16;
        float r = 0;

        if (playing && !alwaysFindPlayer)
        {
            if (isChasePlayer || canAttack)
            {
                Gizmos.color = Color.red;
                r = loseDist;
            }
            else if (!isChasePlayer)
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
}
