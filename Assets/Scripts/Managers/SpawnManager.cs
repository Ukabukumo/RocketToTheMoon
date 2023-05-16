using UnityEngine;
using UnityEngine.Events;

public class SpawnManager : MonoBehaviour
{
    public static SpawnManager Instance { get; private set; }
    //public UnityEvent StartSpawnEvent = new UnityEvent();
    //public UnityEvent StopSpawnEvent = new UnityEvent();

    [SerializeField] private Spawner[] _spawners;

    private void Awake()
    {
        InitializeInstance();
    }

    private void Start()
    {
        CreateListeners();
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

    private void CreateListeners()
    {
        GameManager.Instance.OnStartGame.AddListener(() => StartSpawn());
        GameManager.Instance.OnLossGame.AddListener(() => StopSpawn());
        GameManager.Instance.OnRevive.AddListener(() => RestartSpawn());
        //StartSpawnEvent.AddListener(StartSpawn);
        //StopSpawnEvent.AddListener(StopSpawn);
    }

    private void StartSpawn()
    {
        foreach (Spawner spawner in _spawners)
        {
            spawner.StartSpawn();
        }
    }

    private void StopSpawn()
    {
        foreach (Spawner spawner in _spawners)
        {
            spawner.StopSpawn();
        }
    }

    private void RestartSpawn()
    {
        StopSpawn();
        StartSpawn();
    }
}
