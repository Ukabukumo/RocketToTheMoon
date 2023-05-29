using UnityEngine;
using System.Collections.Generic;

public abstract class GenericObjectPool<T> : MonoBehaviour where T : Component
{
    public static GenericObjectPool<T> Instance { get; private set; }

    [SerializeField] private T _objectPrefab;

    private Queue<T> _objects = new();

    private void Awake()
    {
        InitializeInstance();
    }

    private void InitializeInstance()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    public T Get()
    {
        if (_objects.Count == 0)
        {
            AddObjects(1);
        }

        return _objects.Dequeue();
    }

    public void ReturnToPool(T objectToReturn)
    {
        objectToReturn.gameObject.SetActive(false);
        _objects.Enqueue(objectToReturn);
    }

    private void AddObjects(int count)
    {
        for (int i = 0; i < count; ++i)
        {
            var newObject = Instantiate(_objectPrefab);
            newObject.gameObject.SetActive(false);
            _objects.Enqueue(newObject);
        }
    }
}
