using System;
using UnityEngine;

public class WaveSequencer : MonoBehaviour
{
    public event Action SpawnRequested;
    public event Action<int> NewWave;

    [SerializeField] private WaveData[] _manualWaves;

    private WaveData _currentWave;
    private int _currentWaveNumber;
    private int _enemiesRemainingToSpawn;
    private int _enemiesRemainingAlive;
    private float _nextSpawnTime;

    private void Start()
    {
        _currentWaveNumber = 1;

        NewWave?.Invoke(_currentWaveNumber);
    }

    private void Update()
    {
        ProcessSpawning();
    }

    public WaveData GetManualWave(int number)
    {
        int index = number - 1;

        if (_manualWaves != null && index < _manualWaves.Length)
            return _manualWaves[index];

        return null;
    }

    public void SetWave(WaveData wave)
    {
        _currentWave = wave;
        _enemiesRemainingToSpawn = _currentWave.EnemyCount;
        _enemiesRemainingAlive = _enemiesRemainingToSpawn;
    }

    public void RecordEnemyDeath()
    {
        _enemiesRemainingAlive--;

        if (_enemiesRemainingAlive == 0)
        {
            StartNextWave();
        }
    }

    private void ProcessSpawning()
    {
        if (_currentWave == null) return;

        if (_enemiesRemainingToSpawn > 0 && Time.time > _nextSpawnTime)
        {
            _enemiesRemainingToSpawn--;
            _nextSpawnTime = Time.time + _currentWave.TimeBetweenSpawns;

            SpawnRequested?.Invoke();
        }
    }

    private void StartNextWave()
    {
        _currentWave = null;
        _currentWaveNumber++;
        NewWave?.Invoke(_currentWaveNumber);
    }   
}
