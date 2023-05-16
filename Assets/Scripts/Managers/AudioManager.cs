using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    public enum Music
    {
        MENU,
        GAME,
        LOSS
    }

    public enum Sound
    {
        FIRE,
        COIN
    }

    [SerializeField] private AudioSource _musicAudioSource;
    [SerializeField] private AudioSource _soundAudioSource;
    [SerializeField] private AudioClip _menuMusic;
    [SerializeField] private AudioClip _gameMusic;
    [SerializeField] private AudioClip _lossMusic;
    [SerializeField] private AudioClip _fireSound;
    [SerializeField] private AudioClip _collisionSound;
    [SerializeField] private AudioClip _coinCollectSound;
    [SerializeField] private AudioClip _fuelCollectSound;
    [SerializeField] private AudioClip _buttonPressSound;
    [SerializeField] private AudioClip _loadSound;

    public void PlayMusic(Music music)
    {
        switch(music)
        {
            case Music.MENU:
                _musicAudioSource.clip = _menuMusic;
                break;
            case Music.GAME:
                _musicAudioSource.clip = _gameMusic;
                break;
            case Music.LOSS:
                _musicAudioSource.clip = _lossMusic;
                break;
        }

        _musicAudioSource.time = 0.0f;
        _musicAudioSource.Play();
    }

    public void StopMusic()
    {
        _musicAudioSource.Stop();
    }

    public void PlaySound(Sound sound)
    {
        switch(sound)
        {
            case Sound.FIRE:
                _soundAudioSource.clip = _fireSound;
                if (!_soundAudioSource.isPlaying || _soundAudioSource.clip != _fireSound)
                {
                    _soundAudioSource.Play();
                }
                break;
            case Sound.COIN:
                _soundAudioSource.PlayOneShot(_coinCollectSound);
                break;
        }
    }

    public void StopSound()
    {
        _soundAudioSource.Stop();
    }

    private void Awake()
    {
        InitializeInstance();
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
        GameManager.Instance.OnStartGame.AddListener(() => PlayMusic(Music.GAME));
        GameManager.Instance.OnLossGame.AddListener(delegate { PlayMusic(Music.LOSS); StopSound(); });
        GameManager.Instance.OnRestartGame.AddListener(() => PlayMusic(Music.MENU));
    }
}
