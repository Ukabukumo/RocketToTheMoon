using System;
using UnityEngine;
using TMPro;

public class FPSCounter : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _fpsText;
    [SerializeField] private float _maxUpdateTime = 0.5f;

    private float _fps;
    private float _updateTime = 0.0f;

    private void Update()
    {
        _updateTime += Time.deltaTime;
        if (_updateTime > _maxUpdateTime)
        {
            _updateTime = 0.0f;
            _fps = 1.0f / Time.deltaTime;
            _fpsText.text = Convert.ToString((int)_fps);
        }
    }
}
