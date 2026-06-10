using System.Collections;
using UnityEngine;
using System.Collections.Generic;
public class EneBoss2 : MonoBehaviour
{
    int frameTimer = 0;

    enum State{Idle, Walk, Stamp}
    [SerializeField] float speed = 0.1f;
    Vector3 moveDir;
    Vector3 currentPos;
    bool endMove = false;
    GameObject playerObj;
    [SerializeField] public int attackPower = 0;
    [SerializeField] float attackWaitingTime = 0.1f;
    [SerializeField] float showAttackRangeTime = 3.0f;
    bool isAttackWaiting = false;
    float flashTime = 0.1f;
    // 範囲攻撃の変数---------------------------------
    enum StampState { Jump, Aim, Stamp} // スタンプ攻撃の状態を管理する
    StampState stampState;
    [SerializeField] float attackTime = 1.0f;
    [SerializeField] GameObject rangeAttackObj;
    [SerializeField] GameObject stampAttackReach;
    [SerializeField] GameObject cautionEffectPrefab;
    SpriteRenderer stampSpr;
    GameObject stampAttackReachObj = null;
    GameObject cautionObj = null;
    bool isAttackReach = false;
    float landingPosAdj = 1.0f;
    float jumpHeight = 20.0f; // 範囲攻撃の高さ
    float jumpSpeed = 1.0f;
    [SerializeField] int frameDelay = 10; // 何フレーム前の値をとるか
    Queue<Vector3> pastPositions = new Queue<Vector3>();
    Vector3 targetPos = Vector3.zero;
    int attackFrame = 120;
    int attackWaitingFrame = 5;
    bool followPlayer;
    int flashRangeFrame = 20;
    bool isTransparent = false;
    // ミサイル攻撃の変数-------------------------------
    enum ShotState { Wait, Shot} // ミサイル攻撃の状態を管理する
    ShotState shotState;
    [SerializeField] GameObject missilePrefab;

    SpriteRenderer spriteRenderer;
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        currentPos = transform.position;
        playerObj = GameObject.Find("Player");
        followPlayer = true; // 仮
    }

    void FixedUpdate()
    {
        transform.position = currentPos;
        // frameDelayフレーム前のプレイヤーの位置情報を取得する
        pastPositions.Enqueue(playerObj.transform.position);
        if(pastPositions.Count > frameDelay)
        {
            pastPositions.Dequeue();
        }

        if(pastPositions.Count >= frameDelay)
        {
            targetPos = pastPositions.Peek();
        }
        ActionManager();
    }

    void ActionManager()
    {
        //Move(playerObj.transform.position);
        //Jump();
        Stamp();
    }

    // 指定した座標に移動する
    void Move(Vector3 targetPos)
    {
        if (endMove) return;
        if (currentPos.x - targetPos.x < 0.5f && currentPos.x - targetPos.x > -0.5f
            && currentPos.y - targetPos.y < 0.5f && currentPos.y - targetPos.y > -0.5f)
        {
            Debug.Log("移動完了！");
            //StartCoroutine(Stamp(playerObj.transform.position));
            endMove = true;
            return;
        }
        moveDir = targetPos - currentPos;
        moveDir = moveDir.normalized;
        currentPos += moveDir;
    }

    // スタンプ攻撃の処理
    void Stamp()
    {
        switch(stampState)
        {
            case StampState.Jump:
                if (currentPos.y <= jumpHeight)
                {
                    currentPos += Vector3.up * jumpSpeed;
                }
                else 
                {
                    // ジャンプ終了後、エイム状態へ遷移
                    stampState = StampState.Aim;
                }
                break;
            case StampState.Aim:
                frameTimer++;
                if (!isAttackReach)　// 一度だけ生成する
                {
                    stampAttackReachObj = Instantiate(stampAttackReach);
                    //stampSpr = stampAttackReachObj.GetComponent<SpriteRenderer>();
                    cautionObj = Instantiate(cautionEffectPrefab);
                    isAttackReach = true;
                }
                if (stampAttackReachObj != null && followPlayer)
                {
                    stampAttackReachObj.transform.position = targetPos;
                    // 警告エフェクトを当たり判定の右上に
                    cautionObj.transform.position = targetPos + new Vector3(3, 3, 0);
                    // 点滅処理する状態かどうか
                    if(frameTimer % flashRangeFrame == 0)
                    {
                        if(!isTransparent)isTransparent = true;
                        else if (isTransparent) isTransparent = false;
                        flashRangeFrame--;
                    }
                }
                if (frameTimer >= attackFrame)
                {
                    isTransparent = false;
                    followPlayer = false;
                    if(frameTimer >= attackFrame + attackWaitingFrame)
                    {
                        frameTimer = 0;
                        stampState = StampState.Stamp;
                    }
                }

                // 点滅処理の本体
                if (!isTransparent) cautionObj.SetActive(true);
                else if (isTransparent) cautionObj.SetActive(false);
            break;
            case StampState.Stamp:
                // スタンプ攻撃がおわったら行動を終了
                if (stampAttackReachObj == null || cautionObj == null)
                {
                    return;
                }
                currentPos.x = stampAttackReachObj.transform.position.x;
                if (currentPos.y >= stampAttackReachObj.transform.position.y + landingPosAdj)
                {
                    currentPos.y -= jumpSpeed;
                    Instantiate(rangeAttackObj, transform);
                }
                else
                {
                    Destroy(stampAttackReachObj);
                    Destroy(cautionObj);
                }
            break;
        }
    }

    

    IEnumerator ChangeColor()
    {
        while (isAttackWaiting)
        {
            spriteRenderer.color = Color.red;
            yield return new WaitForSeconds(flashTime);
            spriteRenderer.color = Color.white;
            yield return new WaitForSeconds(flashTime);
        }
        spriteRenderer.color = Color.white;
    }
}

