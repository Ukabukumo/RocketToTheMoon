using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Spawner : MonoBehaviour
{
    [SerializeField] private GameObject[] _spawnPoints;
    [SerializeField] private GameObject[] _spawningPrefabs;
    [SerializeField] private GameObject _cameraObject;
    [SerializeField] private RocketMovement _rocketMovement;
    [SerializeField] private Transform _startSpawningPoint;
    [SerializeField] private string _parentName = "Parent";
    [SerializeField] private float _timeBetweenSpawn = 1.0f;
    [SerializeField] private Vector2 _borders = new Vector2(10.0f, 10.0f);
    [SerializeField] private bool _considerDistance = false;
    [SerializeField] private float _distanceBetween = 5.0f;

    private List<GameObject> _spawnedObjects;
    private Transform _parent;
    private Coroutine _spawnTimer;
    private float _distance;
    private Vector3 _cameraLastPosition;
    private bool _isInitialized = false;

    public void StartSpawn()
    {
        InitializeSpawner();
        _spawnTimer = StartCoroutine(SpawnTimer());
    }

    public void StopSpawn()
    {
        DeinitializeSpawner();
        StopCoroutine(_spawnTimer);
    }

    private void Update()
    {
        if (_isInitialized)
        {
            CheckBorderCrossing();
            CalculateDistance();
        }
    }

    private void InitializeSpawner()
    {
        _isInitialized = true;
        _spawnedObjects = new List<GameObject>();
        _parent = new GameObject(_parentName).transform;
        _cameraLastPosition = _cameraObject.transform.position;
        _distance = 0.0f;
    }

    private void DeinitializeSpawner()
    {
        _isInitialized = false;
        ClearSpawnedObjects();
    }

    private IEnumerator SpawnTimer()
    {
        while (true)
        {
            // Check position camera higher than specified point
            if (_cameraObject.transform.position.y < _startSpawningPoint.position.y)
            {
                yield return null;
                continue;
            }

            yield return new WaitForSeconds(_timeBetweenSpawn);

            if (!_considerDistance || (_distance > _distanceBetween))
            {
                SpawnObject();
                _distance = 0.0f;
            }
        }
    }

    private void SpawnObject()
    {
        int randomObjectIndex = Random.Range(0, _spawningPrefabs.Length);
        int randomSpawnPointIndex = Random.Range(0, _spawnPoints.Length);

        GameObject spawnedObject = Instantiate(_spawningPrefabs[randomObjectIndex], _parent);

        float rocketCurrentSpeed = _rocketMovement.GetCurrentSpeed();
        Vector3 spawnPosition = new Vector3(_spawnPoints[randomSpawnPointIndex].transform.position.x,
                                            _spawnPoints[randomSpawnPointIndex].transform.position.y + 
                                            _cameraObject.transform.position.y + rocketCurrentSpeed,
                                            spawnedObject.transform.position.z);
        spawnedObject.transform.position = spawnPosition;

        _spawnedObjects.Add(spawnedObject);
    }

    private void CheckBorderCrossing()
    {
        for (int i = _spawnedObjects.Count - 1; i >= 0; --i)
        {
            GameObject enemy = _spawnedObjects[i];
            Vector3 enemyPosition = enemy.transform.position;

            if (Mathf.Abs(enemyPosition.x) > _borders.x ||
                _cameraObject.transform.position.y - enemyPosition.y > _borders.y)
            {
                Destroy(enemy);
                _spawnedObjects.RemoveAt(i);
            }
        }
    }

    private void ClearSpawnedObjects()
    {
        foreach (GameObject spawnedObject in _spawnedObjects)
        {
            Destroy(spawnedObject);
        }

        _spawnedObjects.Clear();
    }

    private void CalculateDistance()
    {
        _distance += _cameraObject.transform.position.y - _cameraLastPosition.y;
        _cameraLastPosition = _cameraObject.transform.position;
    }
}
