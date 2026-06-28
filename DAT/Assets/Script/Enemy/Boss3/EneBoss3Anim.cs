using Unity.VisualScripting;
using UnityEngine;

public class EneBoss3Anim : MonoBehaviour
{
    // EneBoss3内変数
    EneBoss3 enemyCtrl;
    // 1フレーム前の状態を取得する
    EneBoss3.State lastState;
    SpriteRenderer spriteRenderer;
    // アニメーションの画像
    [SerializeField] Sprite[] walkAnim;
    [SerializeField] Sprite[] leaveAnim;
    [SerializeField] Sprite[] spawnAnim;
    [SerializeField] Sprite[] idleAnim;
    [SerializeField] Sprite[] deathAnim;
    [SerializeField] Sprite[] dashAnim;
    [SerializeField] Sprite[] meleeAnim;
    enum TeleportState{Leave, Spawn, End};
    TeleportState teleportState;
    // アニメーションの最大数
    int walkAnimMax;
    int leaveAnimMax;
    int spawnAnimMax;
    int idleAnimMax;
    int deathAnimMax;
    int dashAnimMax;
    int meleeAnimMax;
    // 現在のコマ数
    int animIdx = 0;
    // フレームを数える変数
    int frameTimer = 0;
    // アニメーション1コマあたりのコマ数
    int teleAnimFrame = 5;
    int animFrame = 5;
    // テレポートのスポーン状態の途中で敵ボスを出現させるための変数
    GameObject childObj;
    int spawnChildAnim = 2;
    bool isWalkLast = false;
    bool isDashLast = false;
    bool isAttackLast = false;

    void Start()
    {
        enemyCtrl = GetComponent<EneBoss3>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        // アニメーションの最大数の取得
        walkAnimMax = walkAnim.Length;
        leaveAnimMax = leaveAnim.Length;
        spawnAnimMax = spawnAnim.Length;
        idleAnimMax = idleAnim.Length;
        deathAnimMax = deathAnim.Length;
        dashAnimMax = dashAnim.Length;
        meleeAnimMax = meleeAnim.Length;
        // 子オブジェクトの取得
        childObj = transform.GetChild(0).gameObject;
    }

    void FixedUpdate()
    {
        frameTimer++;
        CheckChangeState();

        if (enemyCtrl.state == EneBoss3.State.Teleport)
        {
            if (frameTimer >= teleAnimFrame)
            {
                switch (teleportState)
                {
                    //移動前のアニメーション
                    case TeleportState.Leave:
                        childObj.SetActive(false);
                        if (animIdx < leaveAnimMax)
                        {
                            spriteRenderer.sprite = leaveAnim[animIdx];
                            animIdx++;
                        }
                        else
                        {
                            animIdx = 0;
                            //状態を変える
                            teleportState = TeleportState.Spawn;
                        }
                        
                        break;
                    // 移動後のアニメーション
                    case TeleportState.Spawn:
                        if (animIdx < spawnAnimMax)
                        {
                            spriteRenderer.sprite = spawnAnim[animIdx];
                            animIdx++;
                            if (animIdx >= spawnAnimMax - spawnChildAnim) childObj.SetActive(true);
                        }
                        else
                        {   
                            animIdx = 0;
                            spriteRenderer.sprite = walkAnim[0];
                            childObj.SetActive(false);
                            teleportState = TeleportState.End;
                        }
                        break;
                    // アニメーション終了後の処理
                    case TeleportState.End:
                        spriteRenderer.sprite = walkAnim[0];
                        enemyCtrl.endTeleport = true;
                        Debug.Log("End!!");
                        break;
                }
                frameTimer = 0;
            }
        }
        else if(enemyCtrl.isAttackAnim)
        {
            if(frameTimer >= animFrame)
            {
                if (animIdx < meleeAnimMax)
                {
                    spriteRenderer.sprite = meleeAnim[animIdx];
                    animIdx++;
                }
                else
                {
                    animIdx = 0;
                    //状態を変える
                    enemyCtrl.isAttackAnim = false;
                }
                frameTimer = 0;
            }
        }
        else if(enemyCtrl.isWalk)
        {
            Animation(walkAnimMax, walkAnim);
        }
        else if(enemyCtrl.isDash)
        {
            Animation(dashAnimMax, dashAnim);
        }
        else
        {
            Animation(idleAnimMax, idleAnim);
        }
    }

    void Animation(int animMax, Sprite[] animSprite)
    {
        if(frameTimer >= animFrame)
            {
                if(animIdx < animMax)
                {
                    spriteRenderer.sprite = animSprite[animIdx];
                    animIdx++;
                }
                else
                {
                    animIdx = 0;
                }
                frameTimer = 0;
            }
    }

    void CheckChangeState()
    {
        if(lastState != enemyCtrl.state)
        {
            animIdx = 0;
            frameTimer = 0;
            teleportState = TeleportState.Leave;
        }

        if(isWalkLast != enemyCtrl.isWalk 
        || isDashLast != enemyCtrl.isDash
        || isAttackLast != enemyCtrl.isAttackAnim)
        {
            animIdx = 0;
            frameTimer = 0;
        }

        lastState = enemyCtrl.state;
        isWalkLast = enemyCtrl.isWalk;
        isDashLast = enemyCtrl.isDash;
        isAttackLast = enemyCtrl.isAttackAnim;
    }
}
