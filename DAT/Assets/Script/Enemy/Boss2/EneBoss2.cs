using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UIElements;

public class EneBoss2 : MonoBehaviour
{
    [SerializeField]float speed = 0.1f;
    Vector3 moveDir;
    Vector3 currentPos;
    bool endMove = false;
    [SerializeField]public int attackPower = 0;
    [SerializeField]float attackWaitingTime = 1.0f;
    [SerializeField]float showAttackRangeTime = 1.0f;
    // 範囲攻撃の変数
    [SerializeField]float rangeAttackRadius = 1.0f;
    [SerializeField]float attackTime = 1.0f;
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
            StartCoroutine(Stanp());
            endMove = true;
            return;
        }
        moveDir = targetPos - currentPos;
        moveDir = moveDir.normalized;
        currentPos += moveDir * speed;
    }

    IEnumerator Stanp()
    {
        yield return new WaitForSeconds(attackWaitingTime);

        GameObject obj = Instantiate(rangeAttackObj, this.transform);
        obj.transform.position = transform.position;
        yield return new WaitForSeconds(attackTime);
        Destroy(obj);
    }

    void DrawCircle()
    {
        
    }
}
