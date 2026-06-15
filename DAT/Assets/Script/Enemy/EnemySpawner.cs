using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private float spawnIntervalSec = 2.0f;
    [SerializeField] private int maxEnemy = 10;
    [SerializeField] private int enemyCount = 0;

    [SerializeField] private GameObject WeakTorcher;
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
        timer += Time.fixedDeltaTime;

        if (timer >= spawnIntervalSec)
        {
            // 敵のスポーン上限
            if (maxEnemy < enemyCount) return;
            timer = 0;

            spawnPos = SetSpawnPos();

            GameObject newObj = Instantiate(WeakTorcher, this.transform);

            newObj.transform.localPosition = spawnPos;

            enemyCount++;
        }
    }

    public void SpawnEnemy()
    {
        spawnPos = SetSpawnPos();

        GameObject newObj = Instantiate(WeakTorcher, this.transform);

        newObj.transform.localPosition = spawnPos;

        enemyCount++;
    }
    public void DestroyEnemy()
    {
        enemyCount--;
    }

    public void UpdataSpawner()
    {

    }

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
}
