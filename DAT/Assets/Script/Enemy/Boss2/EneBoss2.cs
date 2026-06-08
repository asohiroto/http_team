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
    // 範囲攻撃の変数
    enum StampState { Jump, Aim, Stamp} // スタンプ攻撃の状態を管理する
    StampState stampState;
    [SerializeField] float attackTime = 1.0f;
    [SerializeField] GameObject rangeAttackObj;
    [SerializeField] GameObject stampAttackReach;
    GameObject stampAttackReachObj = null;
    bool isAttackReach = false;
    float landingPosAdj = 1.0f;
    float jumpHeight = 20.0f; // 範囲攻撃の高さ
    float jumpSpeed = 1.0f;
    [SerializeField] int frameDelay = 10; // 何フレーム前の値をとるか
    Queue<Vector3> pastPositions = new Queue<Vector3>();
    Vector3 targetPos = Vector3.zero;
    int attackFrame = 60;
    int attackWaitingFrame = 10;
    bool followPlayer;


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
                if (!isAttackReach)
                {
                    stampAttackReachObj = Instantiate(stampAttackReach);
                    isAttackReach = true;
                }
                if (stampAttackReachObj != null && followPlayer)stampAttackReachObj.transform.position = targetPos;
                if (frameTimer >= attackFrame)
                {
                    followPlayer = false;
                    if(frameTimer >= attackFrame + attackWaitingFrame)
                    {
                        frameTimer = 0;
                        stampState = StampState.Stamp;
                    }
                }
            break;
            case StampState.Stamp:
                currentPos.x = stampAttackReachObj.transform.position.x;
                if (currentPos.y >= stampAttackReachObj.transform.position.y + landingPosAdj)
                {
                    currentPos.y -= jumpSpeed;
                    Instantiate(rangeAttackObj, transform);
                }
                else
                {
                    Destroy(stampAttackReachObj);
                }
            break;
        }
    }

    void Jump()
    {
        if(currentPos.y <= jumpHeight)
        {
            currentPos += Vector3.up * jumpSpeed;
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

