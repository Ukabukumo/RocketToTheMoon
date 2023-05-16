using UnityEngine;
using UnityEngine.UI;

public class AudioControl : MonoBehaviour
{
    [SerializeField] private Slider _audioSlider;
    [SerializeField] private string _audioName;

    private AudioSource _audioSource;

    private void Awake()
    {
        InitializeAudio();
    }

    private void InitializeAudio()
    {
        _audioSource = GetComponent<AudioSource>();
        _audioSlider.onValueChanged.AddListener(delegate { ChangeVolumeValue(); });

        if (PlayerPrefs.HasKey(_audioName))
        {
            _audioSlider.value = PlayerPrefs.GetFloat(_audioName);
        }
        else
        {
            _audioSlider.value = 0.5f;
        }
    }

    private void ChangeVolumeValue()
    {
        _audioSource.volume = _audioSlider.value;
        PlayerPrefs.SetFloat(_audioName, _audioSlider.value);
    }
}