using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelData", menuName = "Game/Level Data")]
public class LevelData : ScriptableObject
{
    [SerializeField] private List<WaveData> waves = new List<WaveData>();

    public IReadOnlyList<WaveData> Waves => waves;
}