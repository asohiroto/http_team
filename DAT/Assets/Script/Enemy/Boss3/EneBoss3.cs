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
    public State state = 0;
    public State lastState = 0;
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
    enum TeleportState { Wait, Leave, Spawn};
    TeleportState teleportState = TeleportState.Wait;
    [SerializeField] GameObject leavePrefab;
    [SerializeField] GameObject spawnPrefab;
    GameObject leaveObj;
    GameObject spawnObj;
    int teleportFrameTimer = 0;
    int leaveWaitFrame = 10;

    void Start()
    {
        playerObj = GameObject.Find("Player");
        playerCtrl = playerObj.GetComponent<PlayerController>();
        InitVariable();
        currentPos = transform.position;
        portalPosAll = new Vector3[portalMax];
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Portal();
        transform.position = currentPos;
    }

    void ActionManager()
    {
        switch(state)
        {
            case State.Idle:
            break;
            case State.Beam:
            break;
            case State.Portal:
            break;
            case State.Teleport:
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
    }

    void Teleport()
    {
        
    }

    void Beam()
    {
        switch(beamState)
        {
            case BeamState.Walk:
                Debug.Log("walk");
                if (!getPos)
                {
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
                    //beamRangeObj.transform.rotation = Quaternion.Euler(0, 0, beamRotAdjZ);
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
                    beamObj = Instantiate(beamPrefab, transform);
                    beamObj.transform.rotation = Quaternion.Euler(0, 0, beamRotAdjZ);
                    isBeam = true;
                }
                if(beamFrameTimer >= destroyRangeDelay)
                {
                    Destroy(beamRangeObj);
                    beamFrameTimer = 0;
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
                    // ポータルの位置を先にすべて決める
                    for(int i = 0; i < portalMax; i++)
                    {
                        int attempts = 0;
                        bool isSetPos = false;

                        while(attempts < 15)
                        {
                            attempts++;
                            // ポータルの位置の仮決定
                            portalPosAll[i] = new Vector3(Random.Range(-screenWidth, screenWidth + 1), Random.Range(-screenBottom, screenTop + 1), 0);
                            
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
                    getPos = false;
                    if(portalIdx < portalMax) portalState = PortalState.Set;
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
