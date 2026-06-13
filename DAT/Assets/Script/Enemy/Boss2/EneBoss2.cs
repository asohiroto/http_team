using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using System;
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
    int flashRangeFrame = 10;
    bool isTransparent = false;
    // ミサイル攻撃の変数-------------------------------
    enum ShotState { Aim, Shot} // ミサイル攻撃の状態を管理する
    ShotState shotState;
    [SerializeField] GameObject missilePrefab;
    [SerializeField] GameObject missileRangePrefab;
    [SerializeField] GameObject missileEffectPrefab;
    GameObject missileObj = null;
    GameObject RangeObj = null;
    Vector3 missileLandingPos;
    Vector3 missilePos;
    int missileWaitingFrame = 120;
    bool isShot = false;
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

        // ターゲットポジションを少しディレイをかける
        if(pastPositions.Count >= frameDelay)
        {
            targetPos = pastPositions.Peek();
        }
        Missile();
    }

    void ActionManager()
    {
        //Move(playerObj.transform.position);
        //Jump();
        //Stamp();
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

    // ミサイルを一発発射する処理
    void Missile()
    {
        
        switch (shotState)
        {
            // プレイヤーを狙う処理
            case ShotState.Aim:
                frameTimer++;
                if (!isAttackReach) // 一度だけ生成するための条件処理
                {
                    // 着弾地点を決定
                    missileLandingPos = playerObj.transform.position;
                    // 攻撃範囲を表示するプレハブを生成
                    RangeObj = Instantiate(missileRangePrefab);
                    RangeObj.transform.position = missileLandingPos;
                    isAttackReach = true;
                }
                if(frameTimer >= attackWaitingFrame) shotState = ShotState.Shot;
                break;
            // ミサイルを発射する処理（一発だけ）
            case ShotState.Shot:
                if (!isShot) // 一度だけ生成するための条件処理
                {
                    // ミサイルを生成
                    missileObj = Instantiate(missilePrefab);
                    // ミサイルのポジションを設定
                    missilePos = transform.position;
                    missileObj.transform.position = missilePos;
                    isShot = true;
                }
                if (endMove || missileObj == null) return;
                // 到着したかどうかの判定
                if (Math.Abs(missilePos.x - missileLandingPos.x) < 0.05f && 
                    Math.Abs(missilePos.y - missileLandingPos.y) < 0.05f)
                {
                    endMove = true;
                    Instantiate(missileEffectPrefab);
                    Destroy(missileObj);
                    Destroy(RangeObj);
                    return;
                }
                // ミサイルから着弾点までのベクトルを計算
                moveDir = missileLandingPos - missilePos;
                // 方向ベクトルの正規化
                moveDir = moveDir.normalized;
                // ミサイルの移動処理
                missilePos += moveDir * 0.1f;
                missileObj.transform.position = missilePos;
                break;
        }
    }

    // スタンプ攻撃の処理
    void Stamp()
    {
        switch(stampState)
        {
            // 空中に飛び上がるまでの処理
            case StampState.Jump:
                if (currentPos.y <= jumpHeight)
                {
                    // ジャンプの高さまで飛び上がる
                    currentPos += Vector3.up * jumpSpeed;
                }
                else
                {
                    // ジャンプ終了後、エイム状態へ遷移
                    stampState = StampState.Aim;
                }
                break;
            // プレイヤーを狙っているときの処理
            case StampState.Aim:
                frameTimer++;
                if (!isAttackReach) // 一度だけ生成するための条件処理
                {
                    // 攻撃範囲を表示するプレハブを生成
                    stampAttackReachObj = Instantiate(stampAttackReach);
                    // 攻撃警告を表示するプレハブを生成
                    cautionObj = Instantiate(cautionEffectPrefab);
                    isAttackReach = true;
                }
                // プレイヤーを追いかけるための処理
                if (stampAttackReachObj != null && followPlayer)
                {
                    // 攻撃範囲を表示するエフェクトをターゲットポジションまで移動させる
                    stampAttackReachObj.transform.position = targetPos;
                    // 警告エフェクトを当たり判定の右上に
                    cautionObj.transform.position = targetPos + new Vector3(2.5f, 2.5f, 0);
                    // 点滅処理用の関数
                    if(frameTimer % flashRangeFrame == 0)
                    {
                        if(!isTransparent)isTransparent = true;
                        else if (isTransparent) isTransparent = false;
                    }
                }
                // プレイヤーを追いかけるのをやめて攻撃位置を決定する
                if (frameTimer >= attackFrame)
                {
                    // 点滅処理後、警告マークが消えないようにする
                    isTransparent = false;
                    // プレイヤーを追いかけるのをやめる
                    followPlayer = false;
                    // 攻撃開始時間になったらStampState.Stampへ遷移する
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
            // スタンプ攻撃の処理
            case StampState.Stamp:
                // スタンプ攻撃がおわったら行動を終了
                if (stampAttackReachObj == null || cautionObj == null)
                {
                    return;
                }
                // ｘ軸の位置を攻撃範囲がある位置に移動させる
                currentPos.x = stampAttackReachObj.transform.position.x;
                // 着地位置に到達するまで下向きに移動させる
                if (currentPos.y >= stampAttackReachObj.transform.position.y + landingPosAdj)
                {
                    // ジャンプスピード分下向きに速さを加える
                    currentPos.y -= jumpSpeed;
                }
                else
                {
                    // エフェクトの生成
                    Instantiate(rangeAttackObj, transform);
                    // 攻撃範囲と警告マークを消す
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

