using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

[DisallowMultipleComponent]
public class DualPlaylistMusicPlayer : MonoBehaviour
{
    // ВАЖНО: Third добавлен в конец, чтобы не сломать уже сохранённые значения в сценах/префабах
    public enum PlaylistId { First, Second, Third }

    [System.Serializable]
    public class ScenePlaylistRule
    {
        [Tooltip("Имя сцены (как в Build Settings). Регистр важен.")]
        public string sceneName;

        public PlaylistId playlist = PlaylistId.First;

        [Tooltip("Перезапускать трек/плейлист при входе в эту сцену даже если плейлист уже активен.")]
        public bool restartEvenIfSame = false;
    }

    [Header("Audio")]
    [SerializeField] private AudioSource audioSource;

    [Header("Playlists (WAV -> AudioClip)")]
    [SerializeField] private AudioClip[] playlistFirst;
    [SerializeField] private AudioClip[] playlistSecond;
    [SerializeField] private AudioClip[] playlistThird;

    [Header("Scene -> Playlist rules")]
    [Tooltip("Если сцена есть в списке — включаем указанный плейлист. Если нет — ничего не меняем (или применяем default ниже, если включено).")]
    [SerializeField] private ScenePlaylistRule[] sceneRules;

    [SerializeField] private bool useDefaultPlaylistWhenNoRule = false;
    [SerializeField] private PlaylistId defaultPlaylist = PlaylistId.First;

    [Header("State")]
    [SerializeField] private PlaylistId activePlaylist = PlaylistId.First;

    [Header("Behaviour")]
    [SerializeField] private bool persistBetweenScenes = true;
    [SerializeField] private bool avoidImmediateRepeat = true;

    private const string PREF_KEY = "MUSIC_VOLUME";
    public float Volume => audioSource != null ? audioSource.volume : 1f;

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

        if (PlayerPrefs.HasKey(PREF_KEY))
            audioSource.volume = Mathf.Clamp01(PlayerPrefs.GetFloat(PREF_KEY, audioSource.volume));
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
        ApplyPlaylistForScene(SceneManager.GetActiveScene(), allowRestartEvenIfSame: true);
        RestartFromActivePlaylist();
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        ApplyPlaylistForScene(scene, allowRestartEvenIfSame: false);
    }

    private void ApplyPlaylistForScene(Scene scene, bool allowRestartEvenIfSame)
    {
        bool foundRule = TryGetRuleForScene(scene.name, out var rule);

        if (!foundRule)
        {
            if (!useDefaultPlaylistWhenNoRule) return;

            if (activePlaylist != defaultPlaylist)
                SetPlaylist(defaultPlaylist, restart: true);

            return;
        }

        bool same = (activePlaylist == rule.playlist);
        bool shouldRestart = (!same) || (rule.restartEvenIfSame && allowRestartEvenIfSame);

        if (!same)
        {
            activePlaylist = rule.playlist;
            _lastIndex = -1;
        }

        if (shouldRestart)
            RestartFromActivePlaylist();
    }

    private bool TryGetRuleForScene(string sceneName, out ScenePlaylistRule rule)
    {
        if (sceneRules != null)
        {
            for (int i = 0; i < sceneRules.Length; i++)
            {
                var r = sceneRules[i];
                if (r != null && !string.IsNullOrWhiteSpace(r.sceneName) && r.sceneName == sceneName)
                {
                    rule = r;
                    return true;
                }
            }
        }

        rule = null;
        return false;
    }

    public void SetPlaylist(PlaylistId playlist, bool restart = true)
    {
        activePlaylist = playlist;
        _lastIndex = -1;

        if (restart)
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
        return activePlaylist switch
        {
            PlaylistId.First => playlistFirst,
            PlaylistId.Second => playlistSecond,
            PlaylistId.Third => playlistThird,
            _ => playlistFirst
        };
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

    public void SetVolume(float value01)
    {
        if (audioSource == null) return;

        value01 = Mathf.Clamp01(value01);
        audioSource.volume = value01;

        PlayerPrefs.SetFloat(PREF_KEY, value01);
        PlayerPrefs.Save();
    }
}
