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
    enum TeleportState{Leave, Spawn, End};
    TeleportState teleportState;
    // アニメーションの最大数
    int walkAnimMax;
    int leaveAnimMax;
    int spawnAnimMax;
    int idleAnimMax;
    int deathAnimMax;
    // 現在のコマ数
    int animIdx = 0;
    // フレームを数える変数
    int frameTimer = 0;
    // アニメーション1コマあたりのコマ数
    int animFrame = 10;

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
    }

    void Update()
    {
        frameTimer++;
        CheckChangeState();
        if (frameTimer >= animFrame)
        {
            animIdx++;
            frameTimer = 0;
            switch(teleportState)
            {
                //移動前のアニメーション
                case TeleportState.Leave:
                if(animIdx >= leaveAnimMax)
                {
                    //状態を変える
                    animIdx = 0;
                    teleportState = TeleportState.Spawn;
                }
                spriteRenderer.sprite = leaveAnim[animIdx];
                break;
                // 移動後のアニメーション
                case TeleportState.Spawn:
                if(animIdx >= spawnAnimMax)
                {
                    teleportState = TeleportState.End;
                }
                spriteRenderer.sprite = spawnAnim[animIdx];
                break;
            }
        }
    }

    void CheckChangeState()
    {
        if(lastState != enemyCtrl.state)
        {
            animIdx = 0;
            animFrame = 0;
        }

        lastState = enemyCtrl.state;
    }
}
