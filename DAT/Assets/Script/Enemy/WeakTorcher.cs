using System.Collections;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.InputSystem.Interactions;
using UnityEngine.InputSystem.XR;

public class WeakTorcher : MonoBehaviour
{
    // このスクリプトで外部に渡す変数は管理したくない
    // 敵の種類によってスクリプト名が変わる -> 参照が大変になる

    [SerializeField] private float dirX = 0f;
    [SerializeField] private float dirY = 0f;
    [SerializeField] private bool isLookRight = true;     // 右を向いているか(アセットの画像が右向きのため)
    [SerializeField] private Vector3 lookPos = Vector2.zero;    // 攻撃時に使用する

    [Header("Animation")]
    [SerializeField] private Sprite[] idleSpr;          // 待機時
    [SerializeField] private Sprite[] moveSpr;          // 移動時
    [SerializeField] private Sprite[] horAttackSpr;     // 横方向の攻撃
    [SerializeField] private Sprite[] upperAttackSpr;     // 上方向の攻撃
    [SerializeField] private Sprite[] lowerAttackSpr;     // 下方向の攻撃
    [SerializeField] private Sprite[] anime;

    // 方向の変数名は統一しときます......

    [SerializeField] private int timer;

    [SerializeField] private bool isMove = false;
    [SerializeField] private bool isAttack = false;
    [SerializeField] private bool isAttackAnime = false;

    SpriteRenderer spr;
    EnemyController enemyCtrl;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        this.spr = GetComponent<SpriteRenderer>();
        enemyCtrl = GetComponent<EnemyController>(); 
    }
        

    private void FixedUpdate()
    {
        CheckLookDir();
        CheckAttack();
        Animation();
        LookHor();
    }

     private void CheckLookDir()
    {
        // dirX, dirY に NowDirを代入
        dirX = enemyCtrl.NowDir.x;
        dirY = enemyCtrl.NowDir.y;

        // 絶対値
        float ABSdirX = Mathf.Abs(dirX);
        float ABSdirY = Mathf.Abs(dirY);

        // 左右のチェック
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
            else if (dirY < 0)
            {
                lookPos.y = -1;
            }
            else
            {
                lookPos.y = 0;
            }
        }
        else                    // 左右方向
        {
            lookPos.y = 0;
            if (dirX > 0)    // 右向き
            {
                lookPos.x = 1;
            }
            else if (dirX < 0)
            {
                lookPos.x = -1;
            }
            else
            {
                lookPos.x = 0;
            }
        }
    }
    private void LookHor()
    {
        if (isLookRight)
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        else
        {
            transform.rotation = Quaternion.Euler(0, 180, 0);
        }
    }
    private void CheckAttack()
    {
        if (enemyCtrl.CanAttack)
        {
            if (isAttack) return;
            StartCoroutine(Attack());
        }
    }

    private void Animation()
    {
        timer++;
        isMove = enemyCtrl.IsMove;     //　移動中か

        if (isAttackAnime)
        {
            if(lookPos.y != 0)      // 上下を向いている
            {
                if (lookPos.y > 0)  // 上
                {
                    UpperAttackAnimation();
                }
                if (lookPos.y < 0)  // 下
                {
                    LowerAttackAnimation();
                }
            }
            else                    // 左右を向いている
            {
                HorAttackAnimation();
            }
        }
        else if (isMove)
        {
            int count = moveSpr.Length;

            spr.sprite = moveSpr[timer / 5 % count];
        }
        else
        {
            // staySpr の枚数カウント
            int count = idleSpr.Length;

            spr.sprite = idleSpr[timer / 5 % count];
        }
    }

    private void UpperAttackAnimation()
    {
        int count = upperAttackSpr.Length;

        for (int i = 0; i < count; i++)
        {
            spr.sprite = upperAttackSpr[timer / 5 % count];
        }
        
    }

    private void LowerAttackAnimation()
    {
        if (!isAttackAnime) return;
        int count = lowerAttackSpr.Length;
        int i = 0;
        int sprNum = 0;
        sprNum = i / 5% count;
        spr.sprite = lowerAttackSpr[sprNum];
        i++;
    }

    private void HorAttackAnimation()
    {
        int count = horAttackSpr.Length;

        spr.sprite = horAttackSpr[timer / 5 % count];
    }

    private void IdelAnimation()
    {
        // staySpr の枚数カウント
        int count = idleSpr.Length;

        this.spr.sprite = idleSpr[timer / 5 % count];
        timer++;
    }

    IEnumerator Attack()
    {
        isAttack = true;
        isAttackAnime = true;
        enemyCtrl.IsAttack = true;
        float attackTime = enemyCtrl.AttackSec / 2;     // 攻撃のアニメーションと合わせるため
        yield return new WaitForSeconds(attackTime);

        GameObject obj = Instantiate(enemyCtrl.AttackCol, this.transform);      // プレハブ呼び出し
        obj.transform.position = enemyCtrl.AttackDist * 0.5f * lookPos + enemyCtrl.transform.position;     // 攻撃距離に合わせる

        EnemyAttack attack = obj.GetComponent<EnemyAttack>();

        yield return new WaitForSeconds(attackTime);
        Destroy(obj);
        isAttackAnime = false;

        yield return new WaitForSeconds(enemyCtrl.AttackCd);

        isAttack = false;
        enemyCtrl.IsAttack = false;
    }
}
