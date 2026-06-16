using NUnit.Framework.Constraints;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class CardAttackReach : MonoBehaviour
{
    // 敵オブジェクトのリスト（コリジョン内にいるエネミーを管理する）
    List<GameObject> enemyObj;
    GameObject playerObj;
    PlayerController playerController;

    // プレイヤーから敵へのベクトル
    Vector3 playerToEnemy = Vector3.zero;
    public Vector3 playerToEnemyNol = Vector3.zero;

    // プレイヤーから敵へのベクトルの最小値をいれる（最小値を更新していくため、あえて大きい数字を入れている）
    public Vector3 playerToEnemyMin = new Vector3(100, 100);

    public Vector3 mousePosScreen = Vector3.zero;
    public Vector3 mousePosWorld = Vector3.zero;

    void Start()
    {
        enemyObj = new List<GameObject>();
        playerObj = transform.parent.gameObject;
        playerController = playerObj.GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        // マウスのスクリーン座標を取得
        mousePosScreen.x = Mouse.current.position.x.ReadValue();
        mousePosScreen.y = Mouse.current.position.y.ReadValue();
        mousePosScreen.z = -Camera.main.transform.position.z;

        // スクリーン座標をワールド座標に変換
        mousePosWorld = Camera.main.ScreenToWorldPoint(mousePosScreen);

        if (enemyObj.Count <= 0)
        {
            playerToEnemy = mousePosWorld;
            playerToEnemyNol = playerToEnemy.normalized;
        }
        else
        {
            playerToEnemyMin = new Vector3(100, 100); // 最小値を更新していくため、あえて大きな数字を入れる

            for (int i = 0; i < enemyObj.Count; i++)
            {
                if (enemyObj[i] == null) return;

                playerToEnemy = enemyObj[i].transform.position - playerObj.transform.position;

                if (playerToEnemyMin.magnitude > playerToEnemy.magnitude)
                {
                    playerToEnemyMin = playerToEnemy;
                }
            }

            playerToEnemyNol = playerToEnemyMin.normalized; // 一番近いエネミーへの単位ベクトル
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Enemy"))
        {
            if (!enemyObj.Contains(col.gameObject))
            {
                enemyObj.Add(col.gameObject);

            }
        }
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if (col.CompareTag("Enemy"))
        {
            if (enemyObj.Contains(col.gameObject))
            {
                enemyObj.Remove(col.gameObject);
            }
        }
    }
}

