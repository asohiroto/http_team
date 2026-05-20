using System.Collections;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.InputSystem.XR;

public class WeakTorcher : MonoBehaviour
{
    // このスクリプトで外部に渡す変数は管理したくない
    // 敵の種類によってスクリプト名が変わる -> 参照が大変になる

    [SerializeField] private float dirX = 0f;
    [SerializeField] private float dirY = 0f;
    [SerializeField] private bool isLookRight = true;     // 右を向いているか(アセットの画像が右向きのため)
    [SerializeField] private float lookDir = 0f;        // 0 上, 1 右, 2 下, 3 左   // 仮の変数です
    [SerializeField] private Vector3 lookPos = Vector2.zero;    // 攻撃時に使用する

    [Header("Animation")]
    [SerializeField] private Sprite[] idleSpr;          // 待機時
    [SerializeField] private Sprite[] moveSpr;          // 移動時
    [SerializeField] private Sprite[] horAttackSpr;     // 横方向の攻撃
    [SerializeField] private Sprite[] upperAttackSpr;     // 上方向の攻撃
    [SerializeField] private Sprite[] lowerAttackSpr;     // 下方向の攻撃

    // 方向の変数名は統一しときます......

    [SerializeField] private int timer;

    private bool isMove = false;
    [SerializeField] private bool isAttack = false;

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
    private void CheckAttack()
    {
        if (enemyCtrl.CanAttack)
        {
            if (isAttack || enemyCtrl.IsAttack) return;
            StartCoroutine(Attack());
        }
    }

    private void Animation()
    {
        if (isAttack)
        {

        }
        else if (isMove)
        {

        }
        else
        {
            IdelAnimation();
        }
    }

    // Update is called once per frame
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
        enemyCtrl.IsAttack = true;
        GameObject obj = Instantiate(enemyCtrl.AttackCol, this.transform);
        obj.transform.position = lookPos * enemyCtrl.AttackDist * 0.5f + enemyCtrl.transform.position;     // 攻撃距離に合わせる

        yield return new WaitForSeconds(enemyCtrl.AttackSec);
        Destroy(obj);

        isAttack = false;
        enemyCtrl.IsAttack = false;
    }
}
