using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;
using System.Collections;

public class BackgroundManager : MonoBehaviour
{
    public static BackgroundManager Instance;

    [SerializeField] private GameObject backgroundPrefab;
    [SerializeField] private Camera cameraObject;

    private List<GameObject> _backgroundObjects;
    private Coroutine _backgroundGenerator;

    private void Awake()
    {
        InitializeInstance();
    }

    private void Start()
    {
        InitializeListeners();
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

    private void InitializeListeners()
    {
        GameManager.Instance.OnRestartGame.AddListener(() => ReinitializeBackground());
    }

    private void InitializeBackground()
    {
        ClearBackground();
        _backgroundObjects = new List<GameObject>();
        _backgroundGenerator = StartCoroutine(BackgroundGenerator());
    }

    private void DeinitializeBackground()
    {
        if (_backgroundGenerator != null)
        {
            StopCoroutine(_backgroundGenerator);
        }
    }

    private void ReinitializeBackground()
    {
        DeinitializeBackground();
        InitializeBackground();
    }

    private void GenerateBackground()
    {
        if (_backgroundObjects.Count < 2)
        {
            CreateBackgroundObject();
            return;
        }

        Vector3 lastButOneBackgroundPosition = _backgroundObjects[_backgroundObjects.Count - 2].transform.position;
        Vector3 secondBackgroundPosition = _backgroundObjects[1].transform.position;

        if (cameraObject.transform.position.y > lastButOneBackgroundPosition.y)
        {
            CreateBackgroundObject();
        }

        if (cameraObject.transform.position.y > secondBackgroundPosition.y)
        {
            RemoveBackgroundObject(0);
        }
    }

    private void CreateBackgroundObject()
    {
        GameObject bacgroundObject = Instantiate(backgroundPrefab);

        if (_backgroundObjects.Count == 0)
        {
            //bacgroundObject.transform.position = new Vector3(0.0f, 0.0f, 0.0f);
            _backgroundObjects.Add(bacgroundObject);
            return;
        }

        Transform endPoint = _backgroundObjects[_backgroundObjects.Count - 1].transform.Find("End");
        Transform beginPoint = bacgroundObject.transform.Find("Begin");

        Vector3 newBackgroundPosition = new Vector3(bacgroundObject.transform.position.x,
                                                    endPoint.position.y - beginPoint.position.y,
                                                    bacgroundObject.transform.position.z);

        bacgroundObject.transform.position = newBackgroundPosition;
        _backgroundObjects.Add(bacgroundObject);
        return;
    }

    private void RemoveBackgroundObject(int index)
    {
        Destroy(_backgroundObjects[index]);
        _backgroundObjects.RemoveAt(index);
    }

    private void ClearBackground()
    {
        if (_backgroundObjects != null && _backgroundObjects.Count > 0)
        {
            foreach (GameObject backgroundObject in _backgroundObjects)
            {
                Destroy(backgroundObject);
            }

            _backgroundObjects.Clear();
        }
    }

    private IEnumerator BackgroundGenerator()
    {
        while (true)
        {
            GenerateBackground();

            yield return null;
        }
    }
}
