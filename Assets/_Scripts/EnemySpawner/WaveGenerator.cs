using UnityEngine;

public class WaveGenerator : MonoBehaviour
{
    [SerializeField] private WaveData _baseWaveTemplate;
    [SerializeField] private float _difficultyMultiplier = 1.2f;
    [SerializeField] private float _minSpawnDelay = 0.3f;
    [SerializeField] private int _wavesToMinDelay = 20;

    public WaveData GenerateWave(int waveNumber)
    {
        if (_baseWaveTemplate == null)
        {
            Debug.LogError("BaseWaveTemplate не назначен!");
            return null;
        }

        WaveData newWave = ScriptableObject.CreateInstance<WaveData>();

        int baseEnemies = _baseWaveTemplate.EnemyCount;
        float baseDelay = _baseWaveTemplate.TimeBetweenSpawns;

        int finalEnemies = CalculateEnemyCount(baseEnemies, waveNumber);
        float finalDelay = CalculateSpawnDelay(baseDelay, waveNumber);

        newWave.SetName($"Wave {waveNumber}");
        newWave.SetEnemyCount(finalEnemies);
        newWave.SetSpawnDelay(finalDelay);

        return newWave;
    }

    protected virtual int CalculateEnemyCount(int baseCount, int waveNumber)
    {
        return Mathf.RoundToInt(baseCount * Mathf.Pow(_difficultyMultiplier, waveNumber - 1));
    }

    protected virtual float CalculateSpawnDelay(float baseDelay, int waveNumber)
    {
        if (_wavesToMinDelay <= 1)
            return baseDelay;

        float progress = Mathf.Clamp01((float)(waveNumber - 1) / (_wavesToMinDelay - 1));

        float delay = Mathf.Lerp(baseDelay, _minSpawnDelay, progress);

        return delay;
    }
}
