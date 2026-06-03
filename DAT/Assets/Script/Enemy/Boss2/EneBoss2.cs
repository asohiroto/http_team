using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UIElements;

public class EneBoss2 : MonoBehaviour
{
    float speed = 0.1f;
    Vector3 moveDir;
    Vector3 currentPos;
    bool endMove = false;
    // 範囲攻撃の変数
    int startRangeAttackFrame = 5;
    int endRangeAttackFrame = 5;
    [SerializeField]GameObject rangeAttackObj;
    void Start()
    {
        currentPos = transform.position;
    }

    void FixedUpdate()
    {
        Move(new Vector3(5, 0, 0));
        transform.position = currentPos;
    }

    // 指定した座標に移動する
    void Move(Vector3 targetPos)
    {
        if (currentPos.x - targetPos.x < 0.5f&& currentPos.x - targetPos.x > -0.5f
            && currentPos.y - currentPos.y < 0.5f && currentPos.y - currentPos.y > -0.5f)
        {
            if(endMove) return;
            Debug.Log("移動完了！");
            Stanp();
            endMove = true;
            return;
        }
        moveDir = targetPos - currentPos;
        moveDir = moveDir.normalized;
        currentPos += moveDir * speed;
    }

    void Stanp()
    {
        Instantiate(rangeAttackObj, this.transform);
    }
}
