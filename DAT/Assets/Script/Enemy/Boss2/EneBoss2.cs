using System.Collections;
using UnityEngine;
public class EneBoss2 : MonoBehaviour
{
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
    [SerializeField] float attackTime = 1.0f;
    [SerializeField] GameObject rangeAttackObj;
    [SerializeField] GameObject stampAttackReach;
    Vector3 stampPos;
    float jumpHeight = 20.0f; // 範囲攻撃の高さ
    float jumpSpeed = 1.0f;
    SpriteRenderer spriteRenderer;
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        currentPos = transform.position;
        playerObj = GameObject.Find("Player");
    }

    void FixedUpdate()
    {
        //Move(playerObj.transform.position);
        transform.position = currentPos;
        ActionManager();
    }

    void ActionManager()
    {
        //Move(playerObj.transform.position);
        Jump();
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

    /*IEnumerator Stamp(Vector3 targetPos)
    {
        if (currentPos.y <= jumpHeight)
        {
            currentPos += Vector3.up * jumpSpeed;
        }
        else
        {
            GameObject reachObj = Instantiate(stampAttackReach);
        }
    }*/

    void Jump()
    {
        if(currentPos.y <= jumpHeight)
        {
            currentPos += Vector3.up * jumpSpeed;
        }
        else
        {
            
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

