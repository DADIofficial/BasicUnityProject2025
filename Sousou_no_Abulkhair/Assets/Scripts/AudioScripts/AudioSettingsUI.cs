using UnityEngine;
using UnityEngine.UI;

public class AudioSettingsUI : MonoBehaviour
{
    [SerializeField] private Scrollbar musicScrollbar;
    [SerializeField] private Scrollbar sfxScrollbar;

    private DualPlaylistMusicPlayer musicManager;
    private SFXManager sfxManager;

    private void Awake()
    {
        musicManager = FindFirstObjectByType<DualPlaylistMusicPlayer>(FindObjectsInactive.Include);
        sfxManager = FindFirstObjectByType<SFXManager>(FindObjectsInactive.Include);

        if (musicManager == null)
            Debug.LogWarning($"{nameof(AudioSettingsUI)}: Не найден DualPlaylistMusicPlayer в сцене.");

        if (sfxManager == null)
            Debug.LogWarning($"{nameof(AudioSettingsUI)}: Не найден SFXManager в сцене.");
    }

    private void OnEnable()
    {
        if (musicManager != null && musicScrollbar != null)
            musicScrollbar.value = musicManager.Volume;

        if (sfxManager != null && sfxScrollbar != null)
            sfxScrollbar.value = sfxManager.Volume;

        if (musicScrollbar != null && musicManager != null)
            musicScrollbar.onValueChanged.AddListener(musicManager.SetVolume);

        if (sfxScrollbar != null && sfxManager != null)
            sfxScrollbar.onValueChanged.AddListener(sfxManager.SetVolume);
    }

    private void OnDisable()
    {
        if (musicScrollbar != null && musicManager != null)
            musicScrollbar.onValueChanged.RemoveListener(musicManager.SetVolume);

        if (sfxScrollbar != null && sfxManager != null)
            sfxScrollbar.onValueChanged.RemoveListener(sfxManager.SetVolume);
    }
}

