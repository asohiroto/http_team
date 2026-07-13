using System.Collections;
using UnityEngine;

public class EneBoss3Slash : MonoBehaviour
{
    EneBoss3 eneCtrl;
    PlayerController playerCtrl;
    GameObject enemyObj;
    GameObject playerObj;

    [SerializeField] Sprite[] attackSprite;
    SpriteRenderer spriteRenderer;
    [SerializeField] int attackDamage;

    [SerializeField] int startColFrame;
    [SerializeField] int endColFrame;
    PolygonCollider2D polyCol;

    // アニメーション用の変数
    int frameTimer = 0;
    int animMax;
    [SerializeField] int animFrame = 5;
    int animIdx = 0;
    [SerializeField] bool isLoop;
    [SerializeField] bool isBreakOnCol; // オブジェクトがプレイヤーと衝突したとき消去するかどうか

    // アタックエフェクトのアニメーションと当たり判定の管理------------------

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        enemyObj = GameObject.Find("EneBoss3(Clone)");
        eneCtrl = enemyObj.GetComponent<EneBoss3>();
        polyCol = GetComponent<PolygonCollider2D>();
        polyCol.enabled = false;
        animMax = attackSprite.Length;
        if(animMax != 0) spriteRenderer.sprite = attackSprite[0];
        polyCol.enabled = false;
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
            if (isBreakOnCol) Destroy(gameObject);
        }
    }


    // BoxColliderのオンオフを管理する
    void CollisionManager()
    {
        if (frameTimer < startColFrame && startColFrame != 0)
        {
            polyCol.enabled = false;
        }
        else if (frameTimer >= startColFrame && frameTimer < endColFrame)
        {
            polyCol.enabled = true;
        }
        else if (frameTimer >= endColFrame)
        {
            polyCol.enabled = false;
        }
    }

    void Animation()
    {
        if (animMax == 0) return;
        if (frameTimer % animFrame == 0)
        {
            if (animIdx < animMax - 1)
            {
                animIdx++;
            }
            else
            {
                animIdx = 0;
                if (!isLoop)
                {
                    Destroy(gameObject);
                }
            }
            spriteRenderer.sprite = attackSprite[animIdx];
        }
    }
}


