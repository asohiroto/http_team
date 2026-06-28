using System.Collections.Generic;
using UnityEngine;

public enum WaveState { Shop, Wave1, Wave2, Boss, Interval,}
[System.Serializable]
public struct WaveData
{
    public WaveState Wave;
    public GameObject[] Enemys;
    public float SpawnInterval;
    public int MaxSpawnEnemy;
    public float WaveDuration;
}
public class WaveManager : MonoBehaviour
{
    [SerializeField] private List<WaveData> waveManagers = new List<WaveData>();
    private Dictionary<WaveState, WaveData> waveDictionary = new Dictionary<WaveState, WaveData>();

    [SerializeField] private float waveTimer;


    [SerializeField] private WaveState currentWaveState;
    [SerializeField] private WaveState nextWaveState;    // 次のウェーブ(current == next なら インターバルをはさむ)
    [SerializeField] private WaveData currentWaveData;

    private EnemySpawner enemySpawner;

    void Start()
    {
        foreach (var data in waveManagers)
        {
            if (!waveDictionary.ContainsKey(data.Wave))
            {
                waveDictionary.Add(data.Wave, data);
            }
        }

        enemySpawner = GetComponent<EnemySpawner>();

        currentWaveState = WaveState.Shop;

        ChangeWaveState(WaveState.Wave1);
    }

    void Update()
    {
        waveTimer -= Time.deltaTime;
    }

    private void FixedUpdate()
    {
        WaveManagement();
    }

    /// <summary>
    /// 残り時間を取得
    /// </summary>
    /// <returns>ウェーブの残り秒数</returns>
    public float GetTimeLeft()
    {
        return waveTimer;
    }

    public void ChangeWaveState(WaveState newState)
    {
        if (currentWaveState == newState) return;

        // 次のウェーブを保持しておく
        nextWaveState = newState;

        // インターバル終了後じゃないなら
        if (currentWaveState != WaveState.Interval && newState != WaveState.Interval)
        {
            newState = WaveState.Interval;  // 次をインターバルに
        }

        // 現在の状態を確定
        currentWaveState = newState;

        if (waveDictionary.TryGetValue(newState, out var foundData))
        {
            currentWaveData = foundData;

            // 時間を設定
            waveTimer = currentWaveData.WaveDuration;

            enemySpawner.UpdataSpawner(
                currentWaveData.SpawnInterval,
                currentWaveData.MaxSpawnEnemy,
                currentWaveData.Enemys);

            enemySpawner.ClearField();
            Debug.Log("ウェーブを変更します" + newState);
        }
    }

    private void WaveManagement()
    {
        // ウェーブ終了
        if (waveTimer > 0) return;


        switch (currentWaveState)
        {
            case WaveState.Interval:

                ChangeWaveState(nextWaveState);

                break;

            case WaveState.Wave1:

                ChangeWaveState(WaveState.Wave2);

                break;

            case WaveState.Wave2:

                ChangeWaveState(WaveState.Boss);

                break;


        }
    }

}

