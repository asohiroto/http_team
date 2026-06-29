using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private float spawnIntervalSec = 2.0f;
    [SerializeField] private int maxEnemy = 10;
    [SerializeField] private int enemyCount = 0;
    [SerializeField] private int enemyVariations = 0;
    [SerializeField] private int totalSpawnCount = 0;

    [SerializeField] private GameObject WeakTorcher;

    [SerializeField] private List<GameObject> enemyList = new List<GameObject>(); 
    [SerializeField] private List<GameObject> field = new List<GameObject>(); 
    // ほかの敵も追加していく


    float timer = 0;

    [SerializeField] Vector2 spawnPos = Vector2.zero;

    private void Start()
    {
        timer = spawnIntervalSec;
    }

    private void Update()
    {
        timer += Time.deltaTime;
    }

    void FixedUpdate()
    {
        SpawnEnemy();

    }
    /// <summary>
    /// 敵のスポーンカウントを減らす
    /// </summary>
    /// <param name="deleteObj">削除したオブジェクト</param>
    public void DestroyEnemy(GameObject deleteObj)
    {
        field.Remove(deleteObj);

        enemyCount--;

        return;
    }

    /// <summary>
    /// すべての敵を消す
    /// </summary>
    public void ClearField()
    {
        foreach (var data in field)
        {
            data.GetComponent<EnemyController>().Delete();
        }

        enemyCount = 0;
        field.Clear();
    }

    /// <summary>
    /// スポーン条件の変更
    /// </summary>
    /// <param name="spawnInterval">スポーンの間隔</param>
    /// <param name="spawnMax">スポーン上限</param>
    public void UpdataSpawner(float spawnInterval, int spawnMax, GameObject[] enemys)
    {
        spawnIntervalSec = spawnInterval;
        maxEnemy = spawnMax;
        enemyVariations = enemys.Length;

        // リストを空にする
        enemyList.Clear();

        // リストに敵の一覧を追加
        foreach (var data in enemys)
        {
            enemyList.Add(data);
        }
    }
    // スポーン位置の設定
    private Vector2 SetSpawnPos()
    {
        const int width = 9;
        const int height = 5;
        const int widthRangw = 13;
        const int heightRange = 9;

        int dirX = Random.Range(0, 2);
        int dirY = Random.Range(0, 2);

        int spawnX = 0;
        int spawnY = 0;

        if (dirY == 1)  // 上側にスポーン
        {
            spawnX = Random.Range(0, widthRangw) * dirX;
            spawnY = Random.Range(height, heightRange);
        }
        else if (dirY == 0)
        {
            spawnX = Random.Range(width, widthRangw);
            if (dirX == 0) spawnX *= -1;   // 0なら左
            else spawnX *= 1;   // それ以外は右にスポーン

            spawnY = Random.Range(0, height);
        }
        return new Vector2(spawnX, spawnY);
    }
    // スポーン処理
    private void SpawnEnemy()
    {
        if (timer < spawnIntervalSec) return;

        // 敵のスポーン上限未満
        if (maxEnemy > enemyCount)
        {
            timer = 0;

            spawnPos = SetSpawnPos();

            // リストの長さに制限
            int nextNum = totalSpawnCount % enemyVariations;

            GameObject nextSpawn = enemyList[nextNum];

            GameObject newObj = Instantiate(WeakTorcher, this.transform);

            newObj.transform.localPosition = spawnPos;

            field.Add(newObj);

            enemyCount++;
            totalSpawnCount++;
        }

    }
}
