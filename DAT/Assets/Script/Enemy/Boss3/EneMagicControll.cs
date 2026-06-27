using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class EneMagicControll: MonoBehaviour
{
    GameObject playerObj;
    float moveSpeed = 0.1f;
    Vector3 moveDir;
    float angle;
    float screenXLimit = 9.0f;
    float screenYLimit = 6.0f;
    Vector3 currentPos;
    void Start()
    {
        playerObj = GameObject.Find("Player");
        // プレイヤーから自分の位置のベクトルを出す
        moveDir = playerObj.transform.position - transform.position;
        moveDir = moveDir.normalized;
        // オブジェクトの向きを決定する
        if(moveDir != Vector3.zero) // 不自然な挙動をしないようにVector3.zeroのパターンをはじく
        {
            angle = Mathf.Atan2(moveDir.y, moveDir.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
        currentPos = transform.position;
    }

    void FixedUpdate()
    {
        // 動かす処理
        currentPos += moveDir * moveSpeed;
        // 画面外に出たときオブジェクトを消す
        if(Mathf.Abs(currentPos.x) > screenXLimit || 
        Mathf.Abs(currentPos.y) > screenYLimit)
        {
            Destroy(gameObject);
        }
        transform.position = currentPos;
    }
}
