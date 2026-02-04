using System;
using System.Collections;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public event Action EnemyDeath;

    [SerializeField] private Enemy _enemyPrefab;
    [SerializeField] private float _spawnDelay = 1f;
    [SerializeField] private float _tileFlashSpeed = 4f;
    [SerializeField] private float _campThresholdDistance = 1.5f;
    [SerializeField] private float _timeBetweenCampingChecks = 2;
    [SerializeField] private MapSpawner _mapSpawner;

    private LevelMapData _mapData;
    private bool _hasMapData;
    private Entity _playerEntity;
    private Transform _playerTransform;
    private ITargetable _target;

    private float _nextCampCheckTime;
    private Vector3 _campPositionOld;
    private bool _isCamping;

    private bool _isDisabled;

    private void Start()
    {
        InitializePlayer();

        if (_mapSpawner == null) _mapSpawner = FindFirstObjectByType<MapSpawner>();
    }

    private void Update()
    {
        if (!_isDisabled)
        {
            CampingCheck();
        }
    }

    public void OnMapGenerated(LevelMapData data)
    {
        _mapData = data;
        _hasMapData = true;

        ResetPlayerPosition();
    }

    private IEnumerator SpawnEnemySequence(Coord spawnCoord)
    {
        Vector3 spawnPosition = _mapData.grid.CoordToWorld(spawnCoord);
 
        Transform tileTransform = _mapSpawner.GetTileAt(spawnCoord.x, spawnCoord.y);

        Renderer tileRenderer = tileTransform.GetComponent<Renderer>();
        Material tileMat = tileRenderer.material;

        Color initialColour = tileMat.color;
        Color flashColour = Color.red;
        float spawnTimer = 0;

        while (spawnTimer < _spawnDelay)
        {
            tileMat.color = Color.Lerp(initialColour, flashColour, Mathf.PingPong(spawnTimer * _tileFlashSpeed, 1));
            spawnTimer += Time.deltaTime;
            yield return null;
        }

        tileMat.color = initialColour;

        Enemy spawnedEnemy = Instantiate(_enemyPrefab, spawnPosition + Vector3.up, Quaternion.identity);

        if (_target != null)
            spawnedEnemy.SetTarget(_target);

        spawnedEnemy.OnDeath += OnEnemyDeath;
    }

    public void SpawnEnemy()
    {
        if (!_hasMapData) return;

        Coord spawnCoord = _mapData.GetRandomOpenTile();

        if (_isCamping)
        {
            spawnCoord = _mapData.grid.WorldToCoord(_playerTransform.position);
        }

        StartCoroutine(SpawnEnemySequence(spawnCoord));
    }

    private void OnEnemyDeath(Entity entity)
    {
        entity.OnDeath -= OnEnemyDeath;

        EnemyDeath?.Invoke();
    }

    private void CampingCheck()
    {
        if (Time.time > _nextCampCheckTime)
        {
            _nextCampCheckTime = Time.time + _timeBetweenCampingChecks;

            _isCamping = (Vector3.Distance(_playerTransform.position, _campPositionOld) < _campThresholdDistance);
            _campPositionOld = _playerTransform.position;
        }
    }

    private void InitializePlayer()
    {
        _playerEntity = FindFirstObjectByType<PlayerHealth>();
        if (_playerEntity != null) _target = _playerEntity.GetComponent<ITargetable>();
        _playerTransform = _playerEntity.transform;

        _nextCampCheckTime = _timeBetweenCampingChecks + Time.time;
        _campPositionOld = _playerTransform.position;

        _playerEntity.OnDeath += OnPlayerDeath;
    }

    private void OnPlayerDeath(Entity entity)
    {
        _isDisabled = true;
    }

    private void ResetPlayerPosition()
    {
        if (!_hasMapData) return;

        Coord centerCoord = _mapData.center;

        Vector3 centerWorldPos = _mapData.grid.CoordToWorld(centerCoord);

        _playerTransform.position = centerWorldPos + Vector3.up * 3f;
    }
}
