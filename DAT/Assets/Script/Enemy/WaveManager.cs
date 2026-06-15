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
    public int WaveDuration;
}
public class WaveManager : MonoBehaviour
{
    [SerializeField] private List<WaveData> waveManagers = new List<WaveData>();
    private Dictionary<WaveState, WaveData> waveDictionary = new Dictionary<WaveState, WaveData>();

    private float waveTimer;


    private WaveState currentWaveState;
    private WaveData currentWaveData;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        foreach (var data in waveManagers)
        {
            if (!waveDictionary.ContainsKey(data.Wave))
            {
                waveDictionary.Add(data.Wave, data);
            }
        }

    }

    // Update is called once per frame
    void Update()
    {
        waveTimer -= Time.deltaTime;
    }

    public void ChangeWaveState(WaveState newState)
    {
        if (currentWaveState == newState) return;

        currentWaveState = newState;

        if (waveDictionary.TryGetValue(newState, out var foundData) )
        {
            currentWaveData = foundData;



            // インターバルなら飛ばす
            if (currentWaveData.Wave == WaveState.Interval) return;

        }
    }
}
