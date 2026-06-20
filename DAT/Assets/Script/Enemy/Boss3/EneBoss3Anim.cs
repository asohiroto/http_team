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
    }
    void Update()
    {
        
    }

    void CheckChangeState()
    {
        if(lastState != enemyCtrl.state)
        {
            
        }
    }
}
