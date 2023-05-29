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
    //[SerializeField] private GameObject _objectPoolPrefab;
    [SerializeField] private Vector2 _borders = new(10.0f, 10.0f);
    [SerializeField] private bool _considerTime;
    [SerializeField] private float _timeBetweenSpawn = 1.0f;
    [SerializeField] private bool _considerDistance;
    [SerializeField] private float _distanceBetweenSpawn = 5.0f;

    //private GenericObjectPool<T> _objectPool;
    private List<GameObject> _spawnedObjects;
    private Transform _parent;
    private Coroutine _spawnTimer;
    private float _distance;
    private float _time;
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

/*    private void Awake()
    {
        if (!TryGetComponent(out GenericObjectPool<T> objectPool))
        {
            throw new MissingComponentException($"{typeof(T).FullName} component is missing from SpawnerObject");
        }

        _objectPool = objectPool;
    }*/

    private void Update()
    {
        if (!_isInitialized)
        {
            return;
        }

        CheckBorderCrossing();
        CalculateTime();
        CalculateDistance();
    }

    private void InitializeSpawner()
    {
        _isInitialized = true;
        _spawnedObjects = new List<GameObject>();
        _parent = new GameObject(_parentName).transform;
        _cameraLastPosition = _cameraObject.transform.position;
        _distance = 0.0f;
        _time = 0.0f;
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
            yield return null;

            // Check position camera higher than specified point
            if (_cameraObject.transform.position.y < _startSpawningPoint.position.y)
            {
                continue;
            }

            // Check time between spawn
            if (_considerTime && (_time < _timeBetweenSpawn))
            {
                continue;
            }

            // Check distance between spawn
            if (_considerDistance && (_distance < _distanceBetweenSpawn))
            {
                continue;
            }

            SpawnObject();
            _time = 0.0f;
            _distance = 0.0f;
        }
    }

    private void SpawnObject()
    {
        int randomObjectIndex = Random.Range(0, _spawningPrefabs.Length);
        int randomSpawnPointIndex = Random.Range(0, _spawnPoints.Length);

        GameObject spawnedObject = Instantiate(_spawningPrefabs[randomObjectIndex], _parent);
        //GameObject spawnedObject = GenericObjectPool<T>.Instance.Get().gameObject;

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
            GameObject spawnedObject = _spawnedObjects[i];

            if (spawnedObject == null)
            {
                continue;
            }

            Vector3 objectPosition = spawnedObject.transform.position;

            if (Mathf.Abs(objectPosition.x) > _borders.x ||
                _cameraObject.transform.position.y - objectPosition.y > _borders.y)
            {
                Destroy(spawnedObject);
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
        Destroy(_parent.gameObject);
    }

    private void CalculateDistance()
    {
        if (_considerDistance)
        {
            _distance += _cameraObject.transform.position.y - _cameraLastPosition.y;
            _cameraLastPosition = _cameraObject.transform.position;
        }
    }

    private void CalculateTime()
    {
        if (_considerTime)
        {
            _time += Time.deltaTime;
        }
    }
}
