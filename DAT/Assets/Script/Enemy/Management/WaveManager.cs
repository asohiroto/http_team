using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum WaveState
{
    Shop,
    Wave1,
    Wave2,
    Boss,
    Interval,
    LevelClearWait,
}

[System.Serializable]
public struct WaveData
{
    public WaveState Wave;

    [Tooltip("このWaveで出す敵の固定順。最後まで行ったら先頭に戻る")]
    public GameObject[] SpawnOrder;

    [Tooltip("敵を出す間隔")]
    public float SpawnInterval;

    [Tooltip("画面内に同時存在できる敵の最大数")]
    public int MaxAliveEnemy;

    [Tooltip("このWaveの継続時間")]
    public float WaveDuration;
}

public class WaveManager : MonoBehaviour
{
    [Header("Level Data")]
    [SerializeField] private LevelData[] levelDatas;
    [SerializeField] private int currentLevelIndex = 0;

    [Header("Level Clear")]
    [SerializeField] private float nextLevelWaitSec = 3.0f;

    [Header("Wave State")]
    [SerializeField] private float waveTimer;
    [SerializeField] private WaveState currentWaveState;
    [SerializeField] private WaveState nextWaveState;
    [SerializeField] private WaveData currentWaveData;

    private EnemySpawner enemySpawner;

    private Dictionary<WaveState, WaveData> waveDictionary = new Dictionary<WaveState, WaveData>();

    private void Start()
    {
        enemySpawner = GetComponent<EnemySpawner>();

        LoadLevel(currentLevelIndex);

        currentWaveState = WaveState.Shop;

        ChangeWaveState(WaveState.Wave1);
    }

    private void Update()
    {
        waveTimer -= Time.deltaTime;
#if false
        // デバッグ用
        if (Input.GetKey(KeyCode.P) && waveTimer > 0.1f)
        {
            if (currentWaveState == WaveState.LevelClearWait) return;

            waveTimer = 0.1f;

            if (currentWaveState == WaveState.Boss)
            {
                ChangeWaveState(WaveState.LevelClearWait);
            }
        }
#endif
    }

    private void FixedUpdate()
    {
        WaveManagement();
    }

    private void LoadLevel(int levelIndex)
    {
        waveDictionary.Clear();

        LevelData levelData = levelDatas[levelIndex];

        foreach (var data in levelData.Waves)
        {
            if (!waveDictionary.ContainsKey(data.Wave))
            {
                waveDictionary.Add(data.Wave, data);
            }
        }
    }

    public float GetTimeLeft()
    {
        return waveTimer;
    }

    public WaveState GetCurrentWaveState()
    {
        return currentWaveState;
    }

    public int GetCurrentLevelIndex()
    {
        return currentLevelIndex;
    }

    public void ChangeWaveState(WaveState newState)
    {
        if (currentWaveState == newState) return;

        nextWaveState = newState;

        if (currentWaveState != WaveState.Interval &&
            newState != WaveState.Interval &&
            newState != WaveState.LevelClearWait)
        {
            newState = WaveState.Interval;
        }

        currentWaveState = newState;

        if (newState == WaveState.LevelClearWait)
        {
            waveTimer = nextLevelWaitSec;
            enemySpawner.ClearField();
            return;
        }

        if (!waveDictionary.TryGetValue(newState, out var foundData)) return;

        currentWaveData = foundData;
        waveTimer = currentWaveData.WaveDuration;

        enemySpawner.UpdateSpawner(
            currentWaveData.SpawnInterval,
            currentWaveData.MaxAliveEnemy,
            currentWaveData.SpawnOrder
        );

        enemySpawner.ClearField();
    }

    private void WaveManagement()
    {
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

            case WaveState.LevelClearWait:
                if (currentLevelIndex == levelDatas.Length) // 最後のボスなら
                {
                    SceneManager.LoadScene("ClearScene");   // クリアシーンに遷移
                    break;
                }

                GoToNextLevel();
                break;
        }
    }

    /// <summary>
    /// Boss撃破時にBoss側から呼ぶ
    /// </summary>
    public void NotifyBossDefeated()
    {
        if (currentWaveState != WaveState.Boss) return;

        ChangeWaveState(WaveState.LevelClearWait);
    }

    private void GoToNextLevel()
    {
        currentLevelIndex++;

        if (currentLevelIndex >= levelDatas.Length)
        {
            enemySpawner.ClearField();
            return;
        }

        if (levelDatas[currentLevelIndex] == null)
        {
            return;
        }

        LoadLevel(currentLevelIndex);

        currentWaveState = WaveState.Shop;

        ChangeWaveState(WaveState.Wave1);
    }
}