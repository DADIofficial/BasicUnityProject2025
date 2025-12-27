using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

[DisallowMultipleComponent]
public class DualPlaylistMusicPlayer : MonoBehaviour
{
    public enum PlaylistId { First, Second }

    [Header("Audio")]
    [SerializeField] private AudioSource audioSource;

    [Header("Playlists (WAV -> AudioClip)")]
    [SerializeField] private AudioClip[] playlistFirst;
    [SerializeField] private AudioClip[] playlistSecond;

    [Header("State")]
    [SerializeField] private PlaylistId activePlaylist = PlaylistId.First;

    [Header("Behaviour")]
    [SerializeField] private bool persistBetweenScenes = true;
    [SerializeField] private bool autoSwitchPlaylistOnSceneLoad = true;
    [SerializeField] private bool avoidImmediateRepeat = true;

    private static DualPlaylistMusicPlayer _instance;
    private Coroutine _playLoop;
    private int _lastIndex = -1;

    private void Awake()
    {

        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }
        _instance = this;

        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();

        if (audioSource == null)
        {
            Debug.LogError($"{nameof(DualPlaylistMusicPlayer)}: На объекте нет AudioSource.");
            enabled = false;
            return;
        }

        audioSource.loop = false;

        if (persistBetweenScenes)
            DontDestroyOnLoad(gameObject);
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void Start()
    {
        RestartFromActivePlaylist();
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (!autoSwitchPlaylistOnSceneLoad) return;

        SwitchPlaylistAndRestart();
    }

    public void SetPlaylist(PlaylistId playlist, bool restart = true)
    {
        activePlaylist = playlist;
        _lastIndex = -1;

        if (restart)
            RestartFromActivePlaylist();
    }

    public void SwitchPlaylistAndRestart()
    {
        activePlaylist = (activePlaylist == PlaylistId.First) ? PlaylistId.Second : PlaylistId.First;
        _lastIndex = -1;
        RestartFromActivePlaylist();
    }

    public void RestartFromActivePlaylist()
    {
        if (_playLoop != null)
            StopCoroutine(_playLoop);

        PlayRandomFromActivePlaylist();
        _playLoop = StartCoroutine(PlayLoop());
    }

    public void StopMusic()
    {
        if (_playLoop != null)
        {
            StopCoroutine(_playLoop);
            _playLoop = null;
        }
        audioSource.Stop();
    }


    private IEnumerator PlayLoop()
    {
        while (true)
        {
            while (audioSource != null && audioSource.isPlaying)
                yield return null;

            if (!isActiveAndEnabled || audioSource == null)
                yield break;

            PlayRandomFromActivePlaylist();
            yield return null;
        }
    }

    private void PlayRandomFromActivePlaylist()
    {
        var list = GetActiveList();
        if (list == null || list.Length == 0)
        {
            Debug.LogWarning($"{nameof(DualPlaylistMusicPlayer)}: В активном плейлисте нет треков.");
            return;
        }

        int idx = PickIndex(list.Length);
        _lastIndex = idx;

        audioSource.clip = list[idx];
        audioSource.Play();
    }

    private AudioClip[] GetActiveList()
    {
        return (activePlaylist == PlaylistId.First) ? playlistFirst : playlistSecond;
    }

    private int PickIndex(int length)
    {
        if (length <= 1 || !avoidImmediateRepeat)
            return Random.Range(0, length);

        int idx;
        do { idx = Random.Range(0, length); }
        while (idx == _lastIndex);

        return idx;
    }
}

