using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    public enum Music
    {
        MENU,
        GAME
    }

    public enum Sound
    {
        LOSS,
        STAR,
        FUEL,
        UPGRADE,
        CLICK,
        COLLISION
    }

    [SerializeField] private AudioSource _musicAudioSource;
    [SerializeField] private AudioSource _soundAudioSource;
    [SerializeField] private AudioClip _menuMusic;
    [SerializeField] private AudioClip _gameMusic;
    [SerializeField] private AudioClip _lossSound;
    [SerializeField] private AudioClip _fireSound;
    [SerializeField] private AudioClip[] _starCollectSounds;
    [SerializeField] private AudioClip _fuelCollectSound;
    [SerializeField] private AudioClip _clickSound;
    [SerializeField] private AudioClip _upgradeSound;
    [SerializeField] private AudioClip _collisionSound;

    private AudioControl _musicAudioControl;

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
            case Sound.LOSS:
                _soundAudioSource.PlayOneShot(_lossSound);
                break;
            case Sound.STAR:
                int randomIndex = Random.Range(0, _starCollectSounds.Length);
                _soundAudioSource.PlayOneShot(_starCollectSounds[randomIndex]);
                break;
            case Sound.FUEL:
                _soundAudioSource.PlayOneShot(_fuelCollectSound);
                break;
            case Sound.UPGRADE:
                _soundAudioSource.PlayOneShot(_upgradeSound);
                break;
            case Sound.CLICK:
                _soundAudioSource.PlayOneShot(_clickSound);
                break;
            case Sound.COLLISION:
                _soundAudioSource.PlayOneShot(_collisionSound);
                break;
        }
    }

    public void StopSound()
    {
        _soundAudioSource.Stop();
    }

    public void PlayRocketSound()
    {
        _soundAudioSource.clip = _fireSound;
        if (!_soundAudioSource.isPlaying || _soundAudioSource.clip != _fireSound)
        {
            _soundAudioSource.Play();
        }
    }

    public void StopRocketSound()
    {
        _soundAudioSource.clip = null;
    }

    private void Awake()
    {
        InitializeInstance();
        InitializeListeners();
        InitializeManager();
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
        GameManager.Instance.OnStartGame.AddListener(delegate { PlayMusic(Music.GAME); });
        GameManager.Instance.OnLossGame.AddListener(delegate { StopMusic(); StopRocketSound(); PlaySound(Sound.LOSS); });
        GameManager.Instance.OnRestartGame.AddListener(delegate { StopSound(); PlayMusic(Music.MENU); SetMusicVolume(1.0f); });
        GameManager.Instance.OnPauseGame.AddListener(delegate { SetMusicVolume(0.5f); });
        GameManager.Instance.OnUnpauseGame.AddListener(delegate { SetMusicVolume(1.0f); });
        GameManager.Instance.OnUpgradeMenu.AddListener(delegate { SetMusicVolume(0.5f); });
        GameManager.Instance.OnRevivePause.AddListener(delegate { SetMusicVolume(0.5f); StopRocketSound(); });
        GameManager.Instance.OnUpgradeSkill.AddListener((UpgradeManager.Upgrade upgrade) => PlaySound(Sound.UPGRADE));

        UIManager.Instance.OnButtonClick.AddListener(() => PlaySound(Sound.CLICK));
    }

    private void InitializeManager()
    {
        if (!_musicAudioSource.TryGetComponent(out AudioControl audioControl))
        {
            throw new MissingComponentException("AudioControl component is missing from MusicAudioSourceObject");
        }

        _musicAudioControl = audioControl;
    }

    private void SetMusicVolume(float value)
    {
        _musicAudioSource.volume = value * SaveDataManager.Instance.LoadAudio(_musicAudioControl.GetAudioName());
    }
}
