using System;
using Unity.VisualScripting;
using UnityEngine;

public class EneBoss3 : MonoBehaviour
{
    public int attackPower = 1;

    GameObject playerObj;
    PlayerController playerCtrl;
    bool endMove;
    bool getPos;
    Vector3 currentPos;
    Vector3 moveDir;
    float walkSpeed = 0.1f;
    public bool isWalk = false; // アニメーションに用いる変数
    public enum State { Idle, Beam, Portal, Teleport , Melee};
    public State state = State.Idle;
    State lastAttack = State.Idle;
    public State lastState = 0;
    int stateCount = 0;
    // 状態管理に使う変数----------------------
    int frameTimer = 0;
    int idleFrame = 20;
    // Shotに使う変数---------------------------
    enum BeamState { Walk, Aim, Shot};
    BeamState beamState = BeamState.Walk;
    [SerializeField] GameObject beamPrefab;
    [SerializeField] GameObject beamRangePrefab;
    GameObject beamObj;
    GameObject beamRangeObj;
    Vector3 beamPos = Vector3.zero;
    float beamFlipX = 0;
    float beamRotAdjZ = -90;
    int beamFrameTimer = 0;
    int beamRangeFrame = 40;
    int destroyRangeDelay = 15;
    bool isBeamRange = false;
    bool isBeam = false;
    float beamPosAdjX = 8.4f;
    // Portalに使う関数---------------------------
    enum PortalState { Wait, Run, Set};
    PortalState portalState = PortalState.Run;
    [SerializeField] GameObject portalPrefab;
    int portalFrameTimer = 0;
    int portalWaitFrame = 10;
    int portalMax = 4;
    int portalIdx = 0;
    Vector3 portalPos;
    Vector3[] portalPosAll;
    bool genePortal = false;
    float runSpeed = 0.4f;
    float geneDistance = 2.0f;
    float portalSize = 1.0f;
    int screenBottom = -2;
    int screenTop = 4;
    int screenWidth = 8;
    // テレポートに使用する変数-------------------------
    enum TeleState { Wait, Leave, Spawn};
    TeleState teleState = TeleState.Leave;
    int teleFrameTimer = 0;
    int leaveWaitFrame = 80;
    int spawnCdFrame = 70;
    Vector3[] telePosPattern; // テレポートの座標をいくつか事前に準備
    Vector3 telePos; // 決定したてれーポートの座標
    float teleRightPos = 8.0f;
    float teleLeftPos = -8.0f;
    float teleTopPos = 3.4f;
    float teleMiddlePos = 1.1f;
    float teleBottomPos = -0.8f;
    // 近接攻撃に使う変数-----------------------------
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
    public bool isDash = false;
    float meleeRangeDistance = 2.4f; // 攻撃範囲を表示する際の距離
    float meleePlayerDistance = 1.5f; // 近接攻撃をする際にとるプレイヤーとの距離
    Vector3 attackDir = Vector3.right;
    Vector3 meleeTargetPos = Vector3.zero;
    float speed = 0.3f;

    // 方向の向きを管理する----------------------------
    int dir = 0;
    bool isAttack = false;

void Start()
    {
        playerObj = GameObject.Find("Player");
        playerCtrl = playerObj.GetComponent<PlayerController>();
        InitVariable();
        currentPos = transform.position;
        portalPosAll = new Vector3[portalMax];
        // テレポートのする場所のパターン
        telePosPattern = new Vector3[]
        {
            new Vector3(teleRightPos, teleTopPos, 0),
            new Vector3(teleRightPos, teleMiddlePos, 0),
            new Vector3(teleRightPos, teleBottomPos, 0),
            new Vector3(teleLeftPos, teleTopPos, 0),
            new Vector3(teleLeftPos, teleMiddlePos, 0),
            new Vector3(teleLeftPos, teleBottomPos, 0)
        };
        stateCount = Enum.GetNames(typeof(State)).Length;
        lastAttack = state;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        ActionManager();
        SaveLastAttackState();
        // x方向の向きを管理
        if (currentPos.x >= playerCtrl.currentPos.x)
        {
            // 左向き
            transform.rotation = Quaternion.Euler(0, 180, 0);
            dir = -1;
        }
        else
        {
            // 右向き
            transform.rotation = Quaternion.Euler(0, 0, 0);
            dir = 1;
        }

        transform.position = currentPos;
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
                    InitVariable();
                    frameTimer = 0;

                    // ランダムな状態を取得する
                    state = GetRandomState(lastAttack);
                }
                break;
            case State.Teleport:
                Teleport();
                break;
            case State.Portal:
                Portal();
                break;
            case State.Beam:
                Beam();
                break;
            case State.Melee:
                Melee();
                break;
        }
    }
    
    // 変数の初期化を行う
    void InitVariable()
    {
        getPos = false;
        endMove = false;
        isBeamRange = false;
        isBeam = false;
        beamFrameTimer = 0;
        portalFrameTimer = 0;
        genePortal = false;
        isAttack = false;
        portalIdx = 0;
        teleFrameTimer = 0;
        teleState = TeleState.Leave;
        portalState = PortalState.Run;
        beamState = BeamState.Walk;
        meleeState = MeleeState.Walk;
        isMeleeRange = false;
        isMeleeAttack = false;
        meleeFrameTimer = 0;
        isDash = false;
    }

    // 近接攻撃
    void Melee()
    {
        switch (meleeState)
        {
            // プレイヤーの近くまで移動する状態
            case MeleeState.Walk:
                if (!getPos) // 一度だけ行うための条件処理
                {
                    // どこまで移動するかを取得
                    meleeTargetPos = GetClosePos();
                    getPos = true;
                    isMeleeRange = false;
                    isMeleeAttack = false;
                    isDash = true;
                }
                // 移動が終わった時、WaitStateへ
                if (endMove)
                {
                    isDash = false;
                    meleeState = MeleeState.Wait;
                }
                else
                {
                    // 実際に移動する処理
                    Move(meleeTargetPos, speed);
                }
                break;
            // 攻撃を開始するまでの待機時間
            case MeleeState.Wait:
                meleeFrameTimer++;
                if (!isMeleeRange) // 一度だけ生成するための条件処理
                {
                    // 攻撃範囲を表示するプレハブを生成
                    meleeRangeObj = Instantiate(meleeRangePrefab);
                    meleeRangeObj.transform.position = transform.position + attackDir * meleeRangeDistance * dir;
                    // 左向き
                    if(dir == -1)
                    {
                        meleeRangeObj.transform.rotation = Quaternion.Euler(0, 0, 45);
                    }
                    // 右向き
                    else if(dir == 1)
                    {
                        meleeRangeObj.transform.rotation = Quaternion.Euler(0, 180, 45);
                    }
                    isMeleeRange = true;
                    isAttack = true;
                }
                // 攻撃待機時間が終われば攻撃状態に遷移する
                else if (meleeFrameTimer >= meleeWaitingFrame)
                {
                    meleeFrameTimer = 0;
                        meleeState = MeleeState.Attack;
                }
                break;
            // 攻撃をする状態
            case MeleeState.Attack:
                if (!isMeleeAttack)
                {
                    meleeObj = Instantiate(meleePrefab);
                    meleeObj.transform.position = transform.position + attackDir * meleeRangeDistance * dir;
                    // 左向き
                    if(dir == -1)
                    {
                        meleeRangeObj.transform.rotation = Quaternion.Euler(0, 0, 90);
                    }
                    // 右向き
                    else if(dir == 1)
                    {
                        meleeRangeObj.transform.rotation = Quaternion.Euler(0, 180, 90);
                    }
                    Destroy(meleeRangeObj);
                    isMeleeAttack = true;
                    state = State.Idle;
                }
                break;
        }
    }

    // 移動前アニメーション流す⇒移動⇒移動後アニメーション；この流れで行う
    void Teleport()
    {
        teleFrameTimer++;
        // 瞬間移動する処理
        switch(teleState)
        {
            case TeleState.Leave:
                // 一度だけ行う処理
                if (!getPos)
                {
                    // 瞬間移動する場所は画面右端左端の上側下側真ん中の合計6種類からランダムに選ばれる
                    int rand = UnityEngine.Random.Range(0, telePosPattern.Length);
                    telePos = telePosPattern[rand];
                    getPos = true;
                    state = State.Teleport;
                    Debug.Log(telePos);
                }
                // アニメーションが終わるまで待つ
                if (teleFrameTimer >= leaveWaitFrame)
                {
                    teleFrameTimer = 0;
                    currentPos = telePos;
                    teleState = TeleState.Spawn;
                } 
                break;
            case TeleState.Spawn:
                // アニメーションが終わるまで待つ
                if(teleFrameTimer > spawnCdFrame)
                {
                    state = State.Idle;
                }
                break;
        }
    }

    void Beam()
    {
        switch(beamState)
        {
            case BeamState.Walk:
                Debug.Log("walk");
                if (!getPos)
                {
                    state = State.Beam;
                    beamPos = new Vector3(currentPos.x, playerObj.transform.position.y);
                    if(currentPos.x > playerCtrl.currentPos.x)
                    {
                        beamFlipX = 0;
                    }
                    else if(currentPos.x <= playerCtrl.currentPos.x)
                    {
                        beamFlipX = 180;
                    }
                    getPos = true;
                }
                else
                {
                    Move(beamPos, walkSpeed);
                }
                if (endMove) beamState = BeamState.Aim;
            break;
            case BeamState.Aim:
                beamFrameTimer++;
                if(!isBeamRange)
                {
                    beamRangeObj = Instantiate(beamRangePrefab, transform);
                    isBeamRange = true;
                }
                else if(beamFrameTimer >= beamRangeFrame)
                {
                    beamFrameTimer = 0;
                    beamState = BeamState.Shot;
                }
        break;
            case BeamState.Shot:
                beamFrameTimer++;
                if(!isBeam)
                {
                    beamObj = Instantiate(beamPrefab);
                    beamObj.transform.position = currentPos + new Vector3(beamPosAdjX * dir, 0, 0);
                    // 左向き
                    if(dir == -1)
                    {
                        beamObj.transform.rotation = Quaternion.Euler(0, 0, beamRotAdjZ);
                    }
                    // 右向き
                    else if(dir == 1)
                    {
                        beamObj.transform.rotation = Quaternion.Euler(0, 180, beamRotAdjZ);
                    }
                    isBeam = true;
                }
                if(beamFrameTimer >= destroyRangeDelay)
                {
                    Destroy(beamRangeObj);
                    beamFrameTimer = 0;
                    state = State.Idle;
                }
            break;
        }
    }

void Portal()
    {
        switch(portalState)
        {
            case PortalState.Run:
                if(!getPos)
                {
                    state = State.Portal;
                    // ポータルの位置を先にすべて決める
                    for(int i = 0; i < portalMax; i++)
                    {
                        int attempts = 0;
                        bool isSetPos = false;

                        while(attempts < 15)
                        {
                            attempts++;
                            // ポータルの位置の仮決定
                            portalPosAll[i] = new Vector3(UnityEngine.Random.Range(-screenWidth, screenWidth + 1), UnityEngine.Random.Range(-screenBottom, screenTop + 1), 0);
                            
                            // ポータルの位置がプレイヤーの位置に近すぎるなら再抽選を行う
                            if(Mathf.Abs(portalPosAll[i].x - playerCtrl.currentPos.x) < geneDistance &&
                            Mathf.Abs(portalPosAll[i].y - playerCtrl.currentPos.y) < geneDistance)
                            {
                                continue;
                            }

                            // ポータルどうしが重なっていたら再抽選を行う
                            bool isOverlap = false;
                            for(int j = 0; j < i; j++)
                            {
                                if(Mathf.Abs(portalPosAll[i].x - portalPosAll[j].x) < portalSize &&
                                Mathf.Abs(portalPosAll[i].y - portalPosAll[j].y) < portalSize)
                                {
                                    isOverlap = true;
                                    break;
                                }
                            }

                            // 重なっていないならループから抜ける
                            if(!isOverlap)
                            {
                                isSetPos = true;
                                break;
                            }
                            
                        }
                        // 生成する座標が決まらなかった時の処理
                        if(!isSetPos)
                        {
                            portalPosAll[i] = new Vector3(5, -1, 0);
                        }
                    }
                    getPos = true;
                }
                else
                {
                    portalState = PortalState.Set;
                }
                
                break;
            case PortalState.Set:
                if(!genePortal)
                {
                    genePortal = true;
                    GameObject obj = Instantiate(portalPrefab);
                    obj.transform.position = portalPosAll[portalIdx];
                    portalIdx++;
                    portalState = PortalState.Wait;
                }
                break;
            case PortalState.Wait:
                portalFrameTimer++;
                if(portalFrameTimer >= portalWaitFrame)
                {
                    portalFrameTimer = 0;
                    genePortal = false;
                    endMove = false;
                    if (portalIdx < portalMax)
                    {
                        portalState = PortalState.Set;
                    }
                    else
                    {
                        state = State.Idle;
                    }
                }
                break;
        }
    }


    // 指定した座標に移動する
    // 指定した座標に移動させる関数
    void Move(Vector3 targetPos, float moveSpeed)
    {
        if (endMove) return;
        if (currentPos.x - targetPos.x < 0.5f && currentPos.x - targetPos.x > -0.5f
            && currentPos.y - targetPos.y < 0.5f && currentPos.y - targetPos.y > -0.5f)
        {
            isWalk = false;
            endMove = true;
            return;
        }
        moveDir = targetPos - currentPos;
        moveDir = moveDir.normalized;
        currentPos += moveDir * moveSpeed;
        isWalk = true;
    }

    // 最後に行った攻撃の種類を保存する
    void SaveLastAttackState()
    {
        if(state != State.Idle && lastAttack != state)
        {
            lastAttack = state;
        }
    }

    // 最後に行った攻撃以外の種類の行動をランダムに選ぶ
    State GetRandomState(State _lastAttack)
    {
        // 現在の状態を除いた数の範囲でランダムな値を決める
        int randomIdx = UnityEngine.Random.Range(0, stateCount - 1);

        // 選ばれた値が現在の状態のIdx以上なら値を＋１してずらす
        if(randomIdx >= (int)_lastAttack)
        {
            randomIdx++;
        }
        // 状態を返す
        return (State)randomIdx;
    }


    Vector3 GetClosePos()
    {
        Vector3 posAdj = Vector3.zero; // ポジションを調整するための変数
        // プレイヤーが右側にいるとき若干左の値を返す
        if (playerCtrl.currentPos.x >= currentPos.x) posAdj = Vector3.left * meleePlayerDistance;
        else posAdj = Vector3.right * meleePlayerDistance;
        // プレイヤーが左側にいるとき若干右の値を返す
        return playerCtrl.currentPos + posAdj;
    }
}
