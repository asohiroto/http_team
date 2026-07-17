using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using System;
public class EneBoss2 : MonoBehaviour
{
    EnemyHpManager eneHp;
    int frameTimer = 0;
    float damageFXTime = 0.2f;
    enum State { Idle, Melee, Stamp, Missile }
    State state = State.Idle;
    [SerializeField] float speed = 0.1f;
    Vector3 moveDir;
    int dir; // 敵のX方向の向きを決定する変数（-1：左向き、1：右向き）
    bool isAttack; // 敵ボスがアタック中かどうか
    Vector3 currentPos;
    bool endMove = false;
    GameObject playerObj;
    PlayerController playerCtrl;
    [SerializeField] public int attackPower = 0;
    bool isAttackWaiting = false;
    bool isGetPosition = false;
    float idleFrame = 30;
    float flashTime = 0.1f;
    // 範囲攻撃の変数---------------------------------
    enum StampState { Jump, Aim, Stamp } // スタンプ攻撃の状態を管理する
    StampState stampState;
    [SerializeField] GameObject rangeAttackObj;
    [SerializeField] GameObject stampAttackReach;
    [SerializeField] GameObject cautionEffectPrefab;
    GameObject stampAttackReachObj = null;
    GameObject cautionObj = null;
    bool isAttackReach = false;
    int stampFrameTimer = 0;
    float landingPosAdj = 1.0f;
    float jumpHeight = 20.0f; // 範囲攻撃の高さ
    float jumpSpeed = 1.0f;
    [SerializeField] int frameDelay = 10; // 何フレーム前の値をとるか
    Queue<Vector3> pastPositions = new Queue<Vector3>();
    Vector3 targetPos = Vector3.zero;
    int attackFrame = 100;
    int attackWaitingFrame = 15;
    bool followPlayer = false;
    int flashRangeFrame = 10;
    bool isTransparent = false;
    Vector3 cautionDir = Vector3.zero; // 警告のポジションを設定するための変数 
    // ミサイル攻撃の変数-------------------------------
    [SerializeField] GameObject missilePrefab;
    const int missileMax = 10; // 0~missileMax-1までミサイルを生成する
    GameObject[] missileObj = new GameObject[10];
    MissileManager[] missile = new MissileManager[10];
    int missileIdx = 0;
    int missileSpanFrame = 20;
    int missileFrameTimer = 0;
    // 近接攻撃の変数----------------------------------
    enum MeleeState { Walk, Wait, Attack }
    MeleeState meleeState;
    [SerializeField] GameObject meleePrefab;
    [SerializeField] GameObject meleeRangePrefab;
    GameObject meleeObj;
    GameObject meleeRangeObj;
    int meleeWaitingFrame = 45;
    int meleeFrameTimer = 0;
    bool isMeleeAttack = false;
    bool isMeleeRange = false;
    bool secondAttack = false;
    int secondAttackFrame = 15;
    float meleeRangeDistance = 2.4f; // 攻撃範囲を表示する際の距離
    float meleePlayerDistance = 1.5f; // 近接攻撃をする際にとるプレイヤーとの距離
    Vector3 attackDir = Vector3.left;
    Vector3 meleeTargetPos = Vector3.zero;

    SpriteRenderer spriteRenderer;

    // 衝突時の変数-----------------------------------
    BoxCollider2D boxCol;

    // SEの変数--------------------------------------
    [SerializeField] GameObject sePrefab;
    GameObject seObj;
    SEManager se;
    float colDamage; // 敵ボスとプレイヤーが衝突したときに与えるダメージ
    void Start()
    {
        eneHp = GetComponent<EnemyHpManager>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        currentPos = transform.position;
        playerObj = GameObject.Find("Player");
        playerCtrl = playerObj.GetComponent<PlayerController>();
        seObj = Instantiate(sePrefab);
        se = seObj.GetComponent<SEManager>();
        followPlayer = true; // 仮
    }

    void FixedUpdate()
    {
        transform.position = currentPos;
        // frameDelayフレーム前のプレイヤーの位置情報を取得する
        pastPositions.Enqueue(playerCtrl.currentPos);
        if (pastPositions.Count > frameDelay)
        {
            pastPositions.Dequeue();
        }

        // ターゲットポジションを少しディレイをかける
        if (pastPositions.Count >= frameDelay)
        {
            targetPos = pastPositions.Peek();
        }

        // 行動を管理する関数
        ActionManager();
        // x方向の向きを管理
        if (!isAttack)
        {
            if (currentPos.x >= playerCtrl.currentPos.x) dir = 1;
            else dir = -1;

            transform.rotation = Quaternion.Euler(0, 180 * dir, 0);
        }
        transform.position = currentPos;
        CheckDie();
        CheckDamage();
    }

    // 敵ボスそのものの当たり判定
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player") && state != State.Stamp)
        {
            playerCtrl.Damaged(colDamage);
        }
    }

    // 行動を管理する関数（ランダムで攻撃を抽選し、攻撃を行う）
    void ActionManager()
    {
        switch (state)
        {
            case State.Idle:
                frameTimer++;
                if (frameTimer >= idleFrame)
                {
                    // 初期化
                    InitAttackBool();
                    frameTimer = 0;

                    // ランダムな状態を取得する
                    state = (State)Enum.ToObject(typeof(State), UnityEngine.Random.Range(0, Enum.GetNames(typeof(State)).Length));
                    //state = State.Melee;
                }
                break;
            case State.Melee:
                MeleeAttack();
                break;
            case State.Stamp:
                Stamp();
                break;
            case State.Missile:
                MissileGene();
                break;
        }
    }

    // アタックで使ったブール変数をまとめて初期化
    // アタックで使った変数をまとめて初期化
    void InitAttackBool()
    {
        isAttack = false;
        endMove = false;
        isAttackWaiting = false;
        isGetPosition = false;
        isAttackReach = false;
        followPlayer = true;
        isMeleeAttack = false;
        isMeleeRange = false;
        secondAttack = false;
        secondAttackFrame = 0;
        meleeState = MeleeState.Walk;
        stampState = StampState.Jump;
        stampFrameTimer = 0;
        missileIdx = 0;
    }

    // 指定した座標に移動する
    // 指定した座標に移動させる関数
    void Move(Vector3 targetPos, float moveSpeed)
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
        currentPos += moveDir * moveSpeed;
    }

    void MeleeAttack()
    {
        switch (meleeState)
        {
            // プレイヤーの近くまで移動する状態
            case MeleeState.Walk:
                if (!isGetPosition) // 一度だけ行うための条件処理
                {
                    // どこまで移動するかを取得
                    meleeTargetPos = GetClosePos();
                    isGetPosition = true;
                    isMeleeRange = false;
                    isMeleeAttack = false;
                }
                // 移動が終わった時、WaitStateへ
                if (endMove)
                {
                    meleeState = MeleeState.Wait;
                }
                else
                {
                    // 実際に移動する処理
                    Move(meleeTargetPos, speed * 2);
                }
                break;
            // 攻撃を開始するまでの待機時間
            case MeleeState.Wait:
                meleeFrameTimer++;
                if (!isMeleeRange) // 一度だけ生成するための条件処理
                {
                    // 攻撃範囲を表示するプレハブを生成
                    meleeRangeObj = Instantiate(meleeRangePrefab, transform);
                    meleeRangeObj.transform.position = transform.position + attackDir * meleeRangeDistance * dir;
                    if (dir < 0) meleeRangeObj.transform.rotation = Quaternion.Euler(0, 180, 45);
                    isMeleeRange = true;
                    isAttack = true;
                }
                // 攻撃待機時間が終われば攻撃状態に遷移する
                else if (meleeFrameTimer >= meleeWaitingFrame - secondAttackFrame)
                {
                    meleeFrameTimer = 0;
                    int idx = UnityEngine.Random.Range(0, 100);
                    if (idx <= 50 || secondAttack)
                    {
                        meleeState = MeleeState.Attack;
                    }
                    else
                    {
                        InitAttackBool();
                        Destroy(meleeRangeObj);
                        // 2度目の攻撃なら待ち時間減少
                        secondAttackFrame = 30;
                        secondAttack = true;
                        meleeState = MeleeState.Walk;
                    }
                    //meleeState = MeleeState.Attack;
                }
                break;
            // 攻撃をする状態
            case MeleeState.Attack:
                if (!isMeleeAttack)
                {
                    meleeObj = Instantiate(meleePrefab, transform);
                    meleeObj.transform.position = transform.position + attackDir * meleeRangeDistance * dir;
                    if (dir < 0) meleeObj.transform.rotation = Quaternion.Euler(0, 180, 90);
                    Destroy(meleeRangeObj);
                    isMeleeAttack = true;
                    se.PlaySE(0);
                    state = State.Idle;
                }
                break;
        }
    }

    // ミサイルを発射する処理（MissileManagerを生成する）
    void MissileGene()
    {
        missileFrameTimer++;
        if (missileIdx >= missileMax)
        {
            // ミサイル処理を終了する
            state = State.Idle;
        }
        else if (missileFrameTimer >= missileSpanFrame)
        {
            missileObj[missileIdx] = Instantiate(missilePrefab, transform);
            missile[missileIdx] = missileObj[missileIdx].GetComponent<MissileManager>();
            missileIdx++;
            missileFrameTimer = 0;
            se.PlaySE(1);
        }
    }

    // スタンプ攻撃の処理
    void Stamp()
    {
        isAttack = true;
        switch (stampState)
        {
            // 空中に飛び上がるまでの処理
            case StampState.Jump:
                if (currentPos.y <= jumpHeight)
                {
                    if(!isAttackReach)
                    {
                        se.PlaySE(4);
                        isAttackReach = true;
                    }
                    // ジャンプの高さまで飛び上がる
                    currentPos += Vector3.up * jumpSpeed;
                }
                else
                {
                    isAttackReach = false;
                    // ジャンプ終了後、エイム状態へ遷移
                    stampState = StampState.Aim;
                }
                break;
            // プレイヤーを狙っているときの処理
            case StampState.Aim:
                stampFrameTimer++;
                if (!isAttackReach) // 一度だけ生成するための条件処理
                {
                    // 攻撃範囲を表示するプレハブを生成
                    stampAttackReachObj = Instantiate(stampAttackReach);
                    // 攻撃警告を表示するプレハブを生成
                    cautionObj = Instantiate(cautionEffectPrefab);

                    // 警告を表示する場所をプレイヤーのポジションから決定
                    if (2.5f >= playerCtrl.currentPos.x) cautionDir.x = 1;
                    else cautionDir.x = -1;
                    if (0 >= playerCtrl.currentPos.y) cautionDir.y = 1; // 上方向にいるとき
                    else cautionDir.y = -1; // 下方向にいるとき
                    isAttackReach = true;
                }
                // プレイヤーを追いかけるための処理
                if (stampAttackReachObj != null && followPlayer)
                {
                    // 攻撃範囲を表示するエフェクトをターゲットポジションまで移動させる
                    stampAttackReachObj.transform.position = targetPos;
                    // 警告エフェクトを当たり判定の右上に
                    cautionObj.transform.position = targetPos + new Vector3(2.5f * cautionDir.x, 2.5f * cautionDir.y, 0);
                    // 点滅処理用の関数
                    if (stampFrameTimer % flashRangeFrame == 0)
                    {
                        if (!isTransparent) isTransparent = true;
                        else if (isTransparent) isTransparent = false;
                    }
                }
                // プレイヤーを追いかけるのをやめて攻撃位置を決定する
                if (stampFrameTimer >= attackFrame)
                {
                    // 点滅処理後、警告マークが消えないようにする
                    isTransparent = false;
                    // プレイヤーを追いかけるのをやめる
                    followPlayer = false;
                    // 攻撃開始時間になったらStampState.Stampへ遷移する
                    if (stampFrameTimer >= attackFrame + attackWaitingFrame)
                    {
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
                    // 着磁後にSEを再生
                    se.PlaySE(3);
                    state = State.Idle;
                }
                break;
        }
    }

    // 近くまで移動するための座標を取得する
    Vector3 GetClosePos()
    {
        Vector3 posAdj = Vector3.zero; // ポジションを調整するための変数
        // プレイヤーが右側にいるとき若干左の値を返す
        if (playerCtrl.currentPos.x >= currentPos.x) posAdj = Vector3.left * meleePlayerDistance;
        else posAdj = Vector3.right * meleePlayerDistance;
        // プレイヤーが左側にいるとき若干右の値を返す
        return playerCtrl.currentPos + posAdj;
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

    void CheckDie()
    {
        if (eneHp.GetCurrentHp() <= 0)
        {
            if (stampAttackReachObj != null) Destroy(stampAttackReachObj);
            if (cautionObj != null) Destroy(cautionObj);
            if (meleeObj != null) Destroy(meleeObj);
            if (meleeRangeObj != null) Destroy(meleeRangeObj);
            if (cautionObj != null) Destroy(cautionObj);
            for(int i = 0; i < missileMax; i++)
            {
                if (missileObj[i] == null) continue;
                missile[i].DestroyMissile();
            }

            GameObject.FindWithTag("EnemySpawner").
            GetComponent<WaveManager>().
            NotifyBossDefeated();

            Destroy(gameObject);
        }
    }

    void CheckDamage()
    {
        if (eneHp.TakeDamage())
        {
            StartCoroutine(BackDamageColor());
        }
    }

    IEnumerator BackDamageColor()
    {
        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(damageFXTime);
        spriteRenderer.color = Color.white;
    }
}