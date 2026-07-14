using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Spawn Settings")]
    [SerializeField] private float spawnIntervalSec = 2.0f;
    [SerializeField] private int maxAliveEnemy = 10;

    [Header("Debug")]
    [SerializeField] private int enemyCount = 0;
    [SerializeField] private int enemyVariations = 0;
    [SerializeField] private int totalSpawnCount = 0;
    [SerializeField] private Vector2 spawnPos = Vector2.zero;
    [SerializeField] private bool wasBossSpawned = false;

    [Header("Runtime Lists")]
    [SerializeField] private List<GameObject> spawnOrder = new List<GameObject>();
    [SerializeField] private List<GameObject> field = new List<GameObject>();

    private WaveManager waveManager;

    private float timer = 0;

    private void Start()
    {
        waveManager = GetComponent<WaveManager>();
        timer = spawnIntervalSec;
    }

    private void Update()
    {
        timer += Time.deltaTime;
    }

    private void FixedUpdate()
    {
        SpawnEnemy();
    }

    public void AddField(GameObject newObj)
    {
        field.Add(newObj);
    }

    /// <summary>
    /// 敵のスポーンカウントを減らす
    /// </summary>
    public void DestroyEnemy(GameObject deleteObj)
    {
        if (!field.Contains(deleteObj)) return;

        field.Remove(deleteObj);
        enemyCount--;
    }

    /// <summary>
    /// すべての敵を消す
    /// </summary>
    public void ClearField()
    {
        var deleteList = new List<GameObject>(field);

        foreach (var enemy in deleteList)
        {
            if (enemy == null) continue;

            var enemyController = enemy.GetComponent<EnemyController>();

            if (enemyController != null)
            {
                enemyController.Delete();
            }
            else
            {
                Destroy(enemy);
            }
        }

        enemyCount = 0;
        field.Clear();
    }

    /// <summary>
    /// スポーン条件の変更
    /// </summary>
    public void UpdateSpawner(float spawnInterval, int maxAlive, GameObject[] newSpawnOrder)
    {
        spawnIntervalSec = spawnInterval;
        maxAliveEnemy = maxAlive;

        spawnOrder.Clear();

        if (newSpawnOrder != null)
        {
            foreach (var enemyPrefab in newSpawnOrder)
            {
                if (enemyPrefab == null) continue;

                spawnOrder.Add(enemyPrefab);
            }
        }

        enemyVariations = spawnOrder.Count;

        // Waveごとに固定順の先頭から始める
        totalSpawnCount = 0;

        // Wave切り替え直後にすぐ出したい場合は spawnIntervalSec にする
        timer = spawnIntervalSec;

        wasBossSpawned = false;

    }

    /// <summary>
    /// スポーン位置の設定
    /// </summary>
    private Vector2 SetSpawnPos()
    {
        if (waveManager.GetCurrentWaveState() == WaveState.Boss)
        {
            return new Vector2(6.0f, 0.0f);
        }

        const int width = 9;
        const int height = 5;
        const int widthRange = 13;
        const int heightRange = 9;

        int dirX = Random.Range(0, 2);
        int dirY = Random.Range(0, 2);

        int spawnX = 0;
        int spawnY = 0;

        if (dirY == 1)
        {
            spawnX = Random.Range(0, widthRange);

            if (dirX == 0)
            {
                spawnX *= -1;
            }

            spawnY = Random.Range(height, heightRange);
        }
        else
        {
            spawnX = Random.Range(width, widthRange);

            if (dirX == 0)
            {
                spawnX *= -1;
            }

            spawnY = Random.Range(0, height);
        }

        return new Vector2(spawnX, spawnY);
    }

    /// <summary>
    /// スポーン処理
    /// </summary>
    private void SpawnEnemy()
    {
        if (enemyVariations <= 0) return;
        if (timer < spawnIntervalSec) return;
        if (enemyCount >= maxAliveEnemy) return;

        timer = 0;

        spawnPos = SetSpawnPos();

        int nextNum = totalSpawnCount % enemyVariations;
        GameObject nextSpawn = spawnOrder[nextNum];

        // ボスが召喚済みだったら
        if (wasBossSpawned) return;

        GameObject newObj;
        if (waveManager.GetCurrentWaveState() == WaveState.Boss)
        {
            newObj = Instantiate(nextSpawn, spawnPos, Quaternion.identity);
            wasBossSpawned = true;
        }
        else
        {
            newObj = Instantiate(nextSpawn, transform);
            newObj.transform.localPosition = spawnPos;
            wasBossSpawned = false;
        }

        field.Add(newObj);

        enemyCount++;
        totalSpawnCount++;
    }
}