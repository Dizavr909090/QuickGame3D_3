using UnityEngine;

[CreateAssetMenu(fileName = "WaveData", menuName = "Waves/WaveData")]
public class WaveData : ScriptableObject
{
    [SerializeField] private string _waveName = "Wave";
    [SerializeField] private int _enemyCount = 10;
    [SerializeField] private float _timeBetweenSpawns = 1f;

    public string WaveName => _waveName;
    public int EnemyCount => _enemyCount;
    public float TimeBetweenSpawns => _timeBetweenSpawns;

    public void SetName(string name) => _waveName = name;
    public void SetEnemyCount(int count) => _enemyCount = count;
    public void SetSpawnDelay(float delay) => _timeBetweenSpawns = delay;

}
