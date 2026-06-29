using System.Collections;
using UnityEngine;

public class EneBoss3Attack : MonoBehaviour
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
    BoxCollider2D boxCol;

    // アニメーション用の変数
    int frameTimer = 0;
    int animMax;
    [SerializeField]int animFrame = 5;
    int animIdx = 0;
    [SerializeField] bool isLoop;
    [SerializeField] bool isBreakOnCol; // オブジェクトがプレイヤーと衝突したとき消去するかどうか

    // アタックエフェクトのアニメーションと当たり判定の管理------------------

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        enemyObj = GameObject.Find("EneBoss3");
        eneCtrl = enemyObj.GetComponent<EneBoss3>();
        boxCol = GetComponent<BoxCollider2D>();
        boxCol.enabled = false;
        animMax = attackSprite.Length;
        spriteRenderer.sprite = attackSprite[0];
        boxCol.enabled = false;
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


    // BoxColliderのオンオフを管理する
    void CollisionManager()
    {
        if(frameTimer < startColFrame && startColFrame != 0)
        {
            boxCol.enabled = false;
        }
        else if(frameTimer >= startColFrame && frameTimer < endColFrame)
        {
            boxCol.enabled = true;
        }
        else if(frameTimer >= endColFrame)
        {
            boxCol.enabled = false;
        }
    }

    void Animation()
    {
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

