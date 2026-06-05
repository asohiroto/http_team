using System;
using System.Collections;
using NUnit.Framework;
using Unity.VisualScripting;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Rendering;
using UnityEngine.UIElements;

public class EneBoss2 : MonoBehaviour
{
    [SerializeField] float speed = 0.1f;
    Vector3 moveDir;
    Vector3 currentPos;
    bool endMove = false;
    [SerializeField] public int attackPower = 0;
    [SerializeField] float attackWaitingTime = 0.1f;
    [SerializeField] float showAttackRangeTime = 3.0f;
    bool isAttackWaiting = false;
    float flashTime = 0.1f;
    // 範囲攻撃の変数
    [SerializeField] float attackTime = 1.0f;
    [SerializeField] GameObject rangeAttackObj;
    [SerializeField] Transform rangeAttackReach;
    SpriteRenderer spriteRenderer;
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        currentPos = transform.position;
        rangeAttackReach = transform.GetChild(0); // 範囲攻撃のリーチを取得
        rangeAttackReach.gameObject.SetActive(false); //範囲攻撃のリーチを非表示にする
    }

    void FixedUpdate()
    {
        Move(new Vector3(5, 0, 0));
        transform.position = currentPos;
    }

    // 指定した座標に移動する
    void Move(Vector3 targetPos)
    {
        if (currentPos.x - targetPos.x < 0.5f && currentPos.x - targetPos.x > -0.5f
            && currentPos.y - currentPos.y < 0.5f && currentPos.y - currentPos.y > -0.5f)
        {
            if (endMove) return;
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
        isAttackWaiting = true;
        StartCoroutine(ChangeColor());
        //攻撃待機時間
        yield return new WaitForSeconds(attackWaitingTime);
        // 攻撃予測線を出す
        rangeAttackReach.gameObject.SetActive(true);
        yield return new WaitForSeconds(showAttackRangeTime);
        // 攻撃を行う
        GameObject obj = Instantiate(rangeAttackObj, this.transform);
        obj.transform.position = transform.position;
        rangeAttackReach.gameObject.SetActive(false);
        isAttackWaiting = false;
        // 攻撃終了
        yield return new WaitForSeconds(attackTime);
        Destroy(obj);
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

