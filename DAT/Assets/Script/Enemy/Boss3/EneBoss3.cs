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
    public enum State { Idle, Beam, Portal, Teleport };
    public State state = State.Teleport;
    public State lastState = 0;
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
    [SerializeField]float beamPosAdjX = 15f;
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
    [SerializeField] GameObject leavePrefab;
    [SerializeField] GameObject spawnPrefab;
    GameObject leaveObj;
    GameObject spawnObj;
    int teleFrameTimer = 0;
    int leaveWaitFrame = 80;
    int spawnCdFrame = 75;
    Vector3[] telePosPattern; // テレポートの座標をいくつか事前に準備
    Vector3 telePos; // 決定したてれーポートの座標
    float teleRightPos = 8.0f;
    float teleLeftPos = -8.0f;
    float teleTopPos = 3.4f;
    float teleMiddlePos = 1.1f;
    float teleBottomPos = -0.8f;

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
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        ActionManager();
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
                    state = (State)Enum.ToObject(typeof(State), UnityEngine.Random.Range(0, Enum.GetNames(typeof(State)).Length));
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
                    beamPos = new Vector3(currentPos.x, playerObj.transform.position.y + UnityEngine.Random.Range(-1.5f, 1.5f));
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
                    /*beamObj = Instantiate(beamPrefab);
                    beamObj.transform.position = currentPos + new Vector3(beamPosAdjX * dir, 0, 0);
                    beamObj.transform.rotation = Quaternion.Euler(0, transform.rotation.y, beamRotAdjZ);
                    isBeam = true;*/
                    beamObj = Instantiate(beamPrefab);

                    // 【修正点①】dirの正負で位置のズレ（オフセット）を計算
                    // dir = 1（左向き）のときはマイナス方向、dir = -1（右向き）のときはプラス方向
                    float offsetX = (dir == 1) ? -beamPosAdjX : beamPosAdjX;
                    beamObj.transform.position = currentPos + new Vector3(offsetX, 0, 0);

                    // 【修正点②】transform.rotation.y を直接使わず、dirからオイラー角を組み立てる
                    // 左向き(dir=1)ならY軸180度、右向き(dir=-1)ならY軸0度
                    float rotY = (dir == 1) ? 180f : 0f;
                    beamObj.transform.rotation = Quaternion.Euler(0, rotY, beamRotAdjZ);
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
            endMove = true;
            return;
        }
        moveDir = targetPos - currentPos;
        moveDir = moveDir.normalized;
        currentPos += moveDir * moveSpeed;
    }
}
