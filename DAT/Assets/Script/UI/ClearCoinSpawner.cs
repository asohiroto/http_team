using System.Collections;
using UnityEngine;

public class ClearCoinSpawner : MonoBehaviour
{
    public GameObject coinPrefab;

    // 生成する間隔（秒）
    private float spawnInterval = 0.15f;
    // 生成する高さのプラス分
    private float heightOffset = 5f;   

    // 時間調整
   // private float duration = 90.0f;
    // -------------------------------------

    void Start()
    {
        StartCoroutine(SpawnRoutine());
    }

    IEnumerator SpawnRoutine()
    {
        float elapsedTime = 0f;

        // 指定時間のみ生成
        while (true)
        {
            if (coinPrefab == null || !coinPrefab)
            {
                yield break;
            }

            float randomX = Random.Range(-14f, 4f);
            Vector3 spawnPosition = new Vector3(
                transform.position.x + randomX,
                transform.position.y + heightOffset,
                transform.position.z
            );

            Instantiate(coinPrefab, spawnPosition, Quaternion.identity);

            yield return new WaitForSeconds(spawnInterval);

            // 待った時間を経過時間に足す
            elapsedTime += spawnInterval;
        }
    }
}