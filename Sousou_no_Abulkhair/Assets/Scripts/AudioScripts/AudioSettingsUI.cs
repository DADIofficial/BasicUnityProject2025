using UnityEngine;
using UnityEngine.UI;

public class AudioSettingsUI : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private Scrollbar musicScrollbar;
    [SerializeField] private Scrollbar sfxScrollbar;

    [Header("Managers")]
    [SerializeField] private DualPlaylistMusicPlayer musicManager;
    [SerializeField] private SFXManager sfxManager;

    private void OnEnable()
    {
        if (musicManager != null && musicScrollbar != null)
            musicScrollbar.value = musicManager.Volume;

        if (sfxManager != null && sfxScrollbar != null)
            sfxScrollbar.value = sfxManager.Volume;

        // подписываемся на изменения
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
