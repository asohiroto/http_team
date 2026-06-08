using System.Collections;
using UnityEngine;

public class ClearCoinSpawner : MonoBehaviour
{
    public GameObject coinPrefab;

    // 生成する間隔（秒）
    private float spawnInterval = 0.1f;
    // 生成する高さのプラス分
    private float heightOffset = 5f;   

    // 時間調整
    private float duration = 10.0f;
    // -------------------------------------

    void Start()
    {
        StartCoroutine(SpawnRoutine());
    }

    IEnumerator SpawnRoutine()
    {
        float elapsedTime = 0f;

        // 指定時間のみ生成
        while (elapsedTime < duration)
        {
            if (coinPrefab == null || !coinPrefab)
            {
                yield break;
            }

            float randomX = Random.Range(-15f, 5f);
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

        Debug.Log(duration + "秒間の生成が終了しました！");
    }
}