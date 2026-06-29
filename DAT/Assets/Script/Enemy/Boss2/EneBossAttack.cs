using System.Collections;
using UnityEngine;

public class EneBossAttack : MonoBehaviour
{
    EneBoss2 eneCtrl;
    PlayerController playerCtrl;
    GameObject enemyObj;
    GameObject playerObj;

    [SerializeField] Sprite[] attackSprite;
    SpriteRenderer spriteRenderer;
    public int attackDamage;

    [SerializeField] int startColFrame;
    [SerializeField] int endColFrame;
    CircleCollider2D circleCol;
    // アニメーション用の変数
    int frameTimer = 0;
    int animMax;
    [SerializeField]int animFrame = 5;
    int animIdx = 0;
    [SerializeField] bool isLoop;
    [SerializeField] bool isBreakOnCol; // オブジェクトがプレイヤーと衝突したとき消去するかどうか

    // アタックエフェクトのアニメーションと当たり判定の管理------------------
    // オブジェクトごとに設定できる項目は、アタックのダメージ、ループするかどうか、衝突したとき消去するかどうか
    // 当たり判定を出すタイミング、消すタイミング

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        enemyObj = GameObject.Find("Boss2");
        eneCtrl = enemyObj.GetComponent<EneBoss2>();
        circleCol = GetComponent<CircleCollider2D>();
        circleCol.enabled = false;
        animMax = attackSprite.Length;    
    }

    void FixedUpdate()
    {
        frameTimer++;
        Animation();
        CollisionManager();
    }

    void OnTriggerStay2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            playerObj = col.gameObject;
            playerCtrl = playerObj.GetComponent<PlayerController>();
            playerCtrl.Damaged(attackDamage);
            if(isBreakOnCol) Destroy(gameObject);
        }
    }

    void CollisionManager()
    {
        if(frameTimer < startColFrame && startColFrame != 0)
        {
            circleCol.enabled = false;
        }
        else if(frameTimer >= startColFrame && frameTimer < endColFrame)
        {
            circleCol.enabled = true;
        }
        else if(frameTimer >= endColFrame)
        {
            circleCol.enabled = false;
        }
    }


    void Animation()
    {
        if(animMax == 0) return;
        if(frameTimer % animFrame == 0)
        {
            if(animIdx < animMax - 1)
            {
                animIdx++;
            }
            else
            {
                animIdx = 0;
                if(!isLoop)
                {
                    Destroy(gameObject);
                }
            }
            spriteRenderer.sprite = attackSprite[animIdx];
        }
    }
}

