using Unity.VisualScripting;
using UnityEngine;

public class EneBoss3Anim : MonoBehaviour
{
    
    // EneBoss3内変数
    // public enum State { Idle, Beam, Portal, Attack3 };
    //public State state = 0;
    EneBoss3 enemyCtrl;
    // 1フレーム前の状態を取得する
    EneBoss3.State lastState;
    SpriteRenderer spriteRenderer;
    // アニメーションの画像
    [SerializeField] Sprite[] walkAnim;
    [SerializeField] Sprite[] teleportAnim;
    [SerializeField] Sprite[] idleAnim;
    [SerializeField] Sprite[] deathAnim;
    // アニメーションの最大数
    int walkAnimMax;
    int teleportAnimMax;
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
        teleportAnimMax = teleportAnim.Length;
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
            if(frameTimer >= animFrame)
            {

            }
            switch (enemyCtrl.state)
            {
                case EneBoss3.State.Idle:
                    
                    break;
                case EneBoss3.State.Beam:
                    break;
                case EneBoss3.State.Teleport:
                    if (frameTimer >= teleportAnimMax)
                    {

                    }
                    break;
                case EneBoss3.State.Portal:
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
