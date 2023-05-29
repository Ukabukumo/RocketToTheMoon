using UnityEngine;
using UnityEngine.UI;

public class AudioControl : MonoBehaviour
{
    [SerializeField] private Slider _audioSlider;
    [SerializeField] private string _audioName;

    private AudioSource _audioSource;

    public string GetAudioName()
    {
        return _audioName;
    }

    private void Start()
    {
        InitializeAudio();
    }

    private void InitializeAudio()
    {
        _audioSource = GetComponent<AudioSource>();
        _audioSlider.onValueChanged.AddListener(delegate { ChangeVolumeValue(); });
        _audioSlider.value = SaveDataManager.Instance.LoadAudio(_audioName);
        ChangeVolumeValue();
    }

    private void ChangeVolumeValue()
    {
        _audioSource.volume = _audioSlider.value;
        SaveDataManager.Instance.SaveAudio(_audioName, _audioSlider.value);
    }
}