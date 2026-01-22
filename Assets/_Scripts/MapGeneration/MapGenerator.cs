using UnityEngine;
using System.Collections.Generic;

public class MapGenerator : MonoBehaviour
{
    [SerializeField]
    private Transform _tilePrefab;
    [SerializeField]
    private Transform _obstaclePrefab;
    [SerializeField]
    private Vector2 _mapSize;
    [SerializeField, Range(0,1)]
    private float _outlinePercent;
    [SerializeField]
    private int _seed = 10;
    [SerializeField]
    private int _obstacleCount = 10;

    private List<Coord> _allTileCoords;
    private Queue<Coord> _shuffledTileCoords;
    private void Start()
    {
        GenerateMap();
    }

    public Coord GetRandomCoord()
    {
        Coord randomCoord = _shuffledTileCoords.Dequeue();
        _shuffledTileCoords.Enqueue(randomCoord);
        return randomCoord;
    }

    public void GenerateMap()
    {
        _allTileCoords = new List<Coord>();
        for (int x = 0; x < _mapSize.x; x++)
        {
            for (int y = 0; y < _mapSize.y; y++)
            {
                _allTileCoords.Add(new Coord(x, y));
            }
        }

        _shuffledTileCoords = new Queue<Coord>(Utility.ShuffleArray(_allTileCoords.ToArray(), _seed));

        string holderName = "Generated Map";

        if (transform.Find(holderName))
        {
            DestroyImmediate(transform.Find(holderName).gameObject);
        }

        Transform mapHolder = new GameObject(holderName).transform;
        mapHolder.parent = transform;

        for (int x = 0; x < _mapSize.x; x++)
        {
            for (int y = 0; y < _mapSize.y; y++)
            {
                Vector3 tilePosition = CoordToPosition(x, y);
                Transform newTile = Instantiate(_tilePrefab, tilePosition, Quaternion.Euler(Vector3.right*90));
                newTile.localScale = Vector3.one * (1- _outlinePercent);
                newTile.parent = mapHolder;
            }
        }

       
        
        for (int i = 0; i < _obstacleCount; i++)
        {
            Coord randomCoord = GetRandomCoord();
            Vector3 obstaclePosition = CoordToPosition(randomCoord.x, randomCoord.y);
            Transform newObstacle = Instantiate(_obstaclePrefab, obstaclePosition + Vector3.up * 0.5f, Quaternion.identity);
            newObstacle.parent = mapHolder;
        }
    }

    private Vector3 CoordToPosition(int x, int y)
    {
        return new Vector3(-_mapSize.x / 2 + 0.5f + x, 0, -_mapSize.y / 2 + 0.5f + y);
    }

    public struct Coord
    {
        public int x;
        public int y;

        public Coord(int X, int Y)
        {
            x = X; 
            y = Y;
        }
    }
}
