using System;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] private Wave[] _waves;
    [SerializeField] private Enemy _enemyPrefab;

    private ITargetable _target;

    private Wave _currentWave;
    private int _currentWaveNumber;

    private int _enemiesRemainingToSpawn;
    private int _enemiesRemainingAlive;
    private float _nextSpawnTime;

    private void Start()
    {
        var player = GameObject.FindGameObjectWithTag("Player");
        if (player != null) _target = player.GetComponent<ITargetable>();

        NextWave();
    }

    private void Update()
    {
        SpawnEnemy();
        
    }

    private void SpawnEnemy()
    {
        if (_enemiesRemainingToSpawn > 0 && Time.time > _nextSpawnTime)
        {
            _enemiesRemainingToSpawn--;
            _nextSpawnTime = Time.time + _currentWave._timeBetweenSpawns;

            Enemy spawnedEnemy = Instantiate(_enemyPrefab, transform.position, Quaternion.identity);
            if (_target != null)
                spawnedEnemy.SetTarget(_target);
            
            spawnedEnemy.OnDeath += OnEnemyDeath;
        }
    }

    private void NextWave()
    {
        _currentWaveNumber++;
        
        if (_currentWaveNumber - 1 < _waves.Length)
        {
            print("Wave: " + _currentWaveNumber);
            _currentWave = _waves[_currentWaveNumber - 1];

            _enemiesRemainingToSpawn = _currentWave._enemyCount;
            _enemiesRemainingAlive = _enemiesRemainingToSpawn;
        }
    }

    private void OnEnemyDeath(Entity entity)
    {
        entity.OnDeath -= OnEnemyDeath;

        _enemiesRemainingAlive --;
        if (_enemiesRemainingAlive == 0)
        {
            NextWave();
        }
    }

    [Serializable]
    private class Wave
    {
        public int _enemyCount;
        public float _timeBetweenSpawns;
    }
}
