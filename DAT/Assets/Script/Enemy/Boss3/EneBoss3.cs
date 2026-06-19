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
    enum State { Idle, Beam, Portal, Attack3 };
    State state = 0;
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
    int portalWaitFrame = 20;
    int portalMax = 4;
    int portalIdx = 0;
    Vector3 portalPos;
    bool genePortal = false;
    void Start()
    {
        playerObj = GameObject.Find("Player");
        playerCtrl = playerObj.GetComponent<PlayerController>();
        InitVariable();
        currentPos = transform.position;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Portal();
        transform.position = currentPos;
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
                    portalPos = new Vector3(UnityEngine.Random.Range(-8, 8), UnityEngine.Random.Range(-3, 4), 0);
                    getPos = true;
                }
                else
                {
                    Move(portalPos, 0.5f);
                }
                if(endMove)
                {
                    if(portalIdx < portalMax) portalState = PortalState.Set;
                }
                
                break;
            case PortalState.Set:
                if(!genePortal)
                {
                    genePortal = true;
                    Instantiate(portalPrefab);
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
                    portalState = PortalState.Run;
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
