using UnityEngine;

public class LevelInitializer : MonoBehaviour
{
    [SerializeField] private MapGenerator _mapGenerator;
    [SerializeField] private Spawner _enemySpawner;
    [SerializeField] private WaveGenerator _waveGenerator;
    [SerializeField] private WaveSequencer _waveSequencer;

    private void Awake()
    {
        _mapGenerator.MapGenerated += _enemySpawner.OnMapGenerated;

        _waveSequencer.NewWave += OnNewWaveRequested;

        _waveSequencer.SpawnRequested += _enemySpawner.SpawnEnemy;

        _enemySpawner.EnemyDeath += _waveSequencer.RecordEnemyDeath;
    }

    private void OnNewWaveRequested(int waveNumber)
    {
        WaveData newWave = _waveSequencer.GetManualWave(waveNumber);

        if (newWave == null)
        {
            newWave = _waveGenerator.GenerateWave(waveNumber);
        }

        _waveSequencer.SetWave(newWave);
        _mapGenerator.OnNewWave(waveNumber);
    }
}
