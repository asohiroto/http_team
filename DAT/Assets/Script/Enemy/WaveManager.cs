using System.Collections.Generic;
using UnityEngine;

public enum WaveState { Shop, Wave1, Wave2, Boss, Interval, Init }
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

    private float waveTimer;


    private WaveState currentWaveState;
    private WaveData currentWaveData;

    void Start()
    {
        foreach (var data in waveManagers)
        {
            if (!waveDictionary.ContainsKey(data.Wave))
            {
                waveDictionary.Add(data.Wave, data);
            }
        }

        currentWaveState = WaveState.Init;
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

        currentWaveState = newState;

        if (waveDictionary.TryGetValue(newState, out var foundData))
        {
            currentWaveData = foundData;

            // インターバル中なら飛ばす
            if (currentWaveData.Wave == WaveState.Interval) return;

        }
    }

    private void WaveManagement()
    {
        // ウェーブ終了
        if (waveTimer > 0) return;

        // インターバル終了時のみ実行        <- どう実装する？
        if (currentWaveState == WaveState.Interval)
        {
            switch (currentWaveState)
            {
                case WaveState.Wave1:

                    ChangeWaveState(WaveState.Wave2);

                    break;

                case WaveState.Wave2:

                    ChangeWaveState(WaveState.Boss);

                    break;


            }
        }

    }

}