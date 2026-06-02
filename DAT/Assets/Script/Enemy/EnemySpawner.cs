using System.Runtime.CompilerServices;
using System.Threading;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private float spawnIntervalSec = 2.0f;
    [SerializeField] private int maxEnemy = 10;
    [SerializeField] private int enemyCount = 0;

    [SerializeField] private GameObject WeakTorcher;
    // ほかの敵も追加していく


    float timer = 0;
    float count = 0;

    public int dirX;
    public int sponeX;
    public int sponeY;
    public int tempX;
    public int tempY;

    void FixedUpdate()
    {
        timer += 0.02f;

        if (timer >= spawnIntervalSec)
        {
            // 敵のスポーン上限
            if (maxEnemy < enemyCount) return;
            timer = 0;
            dirX = Random.Range(-1, 2);
            tempY = Random.Range(0, 2); // 0か1

            tempX = Random.Range(10, 15);
            sponeX = tempX * dirX;
            sponeY = Random.Range(6, 10);

            GameObject newObj = Instantiate(WeakTorcher, this.transform);

            enemyCount++;
            // x座標 10～15 * (1 or -1)
            // 秒数 % 2 で 1, -1を出す
            // y座標 6～10
            if (dirX == 0)
            {
                newObj.transform.localPosition = new Vector3(sponeX, sponeY, 0);
            }
            else
            {
                newObj.transform.localPosition = new Vector3(sponeX, sponeY * tempY, 0);
            }
        }
    }

    public void DestiriyEnemy()
    {
        enemyCount--;
    }
}
