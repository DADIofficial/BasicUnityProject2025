using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class MenuPauseController : MonoBehaviour
{
    [Header("Menu")]
    [SerializeField] private GameObject menuCanvasRoot;
    [SerializeField] private KeyCode toggleKey = KeyCode.Tab;

    [Header("Disable player control")]
    [SerializeField] private Behaviour[] disableBehaviours;
    [SerializeField] private GameObject[] disableObjects;

    [Header("Canvas control")]
    [SerializeField] private GameObject[] excludeCanvasRoots;

    [Header("Cursor")]
    [SerializeField] private bool showCursorInMenu = true;

    [Header("Pause time")]
    [SerializeField] private bool pauseTime = true;
    [SerializeField] private bool pauseAudio = false;

    private readonly List<GameObject> _disabledCanvasRoots = new();
    private readonly List<Behaviour> _disabledBehaviours = new();
    private readonly List<GameObject> _disabledObjects = new();

    private bool _menuOpen;

    private bool _prevCursorVisible;
    private CursorLockMode _prevCursorLock;

    private float _prevTimeScale = 1f;
    private float _prevFixedDeltaTime = 0.02f;
    private bool _prevAudioPaused;

    private void Awake()
    {
        if (menuCanvasRoot == null)
        {
            Debug.LogError($"{nameof(MenuPauseController)}: �� �������� menuCanvasRoot.");
            enabled = false;
            return;
        }

        if (menuCanvasRoot.activeSelf)
            menuCanvasRoot.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(toggleKey))
        {
            if (_menuOpen) CloseMenu();
            else OpenMenu();
        }
    }

    public void OpenMenu()
    {
        if (_menuOpen) return;
        _menuOpen = true;

        DisableOtherCanvases();
        menuCanvasRoot.SetActive(true);
        DisableControls();

        // ----- PAUSE TIME -----
        if (pauseTime)
        {
            _prevTimeScale = Time.timeScale;
            _prevFixedDeltaTime = Time.fixedDeltaTime;

            Time.timeScale = 0f;
            Time.fixedDeltaTime = 0f;
        }

        if (pauseAudio)
        {
            _prevAudioPaused = AudioListener.pause;
            AudioListener.pause = true;
        }

        // ----- CURSOR -----
        if (showCursorInMenu)
        {
            _prevCursorVisible = Cursor.visible;
            _prevCursorLock = Cursor.lockState;

            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
    }

    public void CloseMenu()
    {
        if (!_menuOpen) return;
        _menuOpen = false;

        menuCanvasRoot.SetActive(false);

        RestoreControls();
        RestoreCanvases();

        
        if (pauseTime)
        {
            Time.timeScale = _prevTimeScale;
            Time.fixedDeltaTime = _prevFixedDeltaTime;
        }

        if (pauseAudio)
        {
            AudioListener.pause = _prevAudioPaused;
        }

        
        if (showCursorInMenu)
        {
            Cursor.visible = _prevCursorVisible;
            Cursor.lockState = _prevCursorLock;
        }
    }

    private void DisableOtherCanvases()
    {
        _disabledCanvasRoots.Clear();

        var canvases = FindObjectsOfType<Canvas>(true);

        foreach (var c in canvases)
        {
            if (c == null) continue;

            GameObject root = c.rootCanvas != null ? c.rootCanvas.gameObject : c.gameObject;

            if (root == null) continue;
            if (root == menuCanvasRoot) continue;
            if (IsExcluded(root)) continue;

            if (root.activeSelf)
            {
                root.SetActive(false);
                _disabledCanvasRoots.Add(root);
            }
        }
    }

    private void RestoreCanvases()
    {
        for (int i = 0; i < _disabledCanvasRoots.Count; i++)
        {
            var go = _disabledCanvasRoots[i];
            if (go != null) go.SetActive(true);
        }
        _disabledCanvasRoots.Clear();
    }

    private void DisableControls()
    {
        _disabledBehaviours.Clear();
        _disabledObjects.Clear();

        if (disableBehaviours != null)
        {
            foreach (var b in disableBehaviours)
            {
                if (b == null) continue;
                if (!b.enabled) continue;

                b.enabled = false;
                _disabledBehaviours.Add(b);
            }
        }

        if (disableObjects != null)
        {
            foreach (var go in disableObjects)
            {
                if (go == null) continue;
                if (!go.activeSelf) continue;

                go.SetActive(false);
                _disabledObjects.Add(go);
            }
        }
    }

    private void RestoreControls()
    {
        for (int i = 0; i < _disabledBehaviours.Count; i++)
        {
            var b = _disabledBehaviours[i];
            if (b != null) b.enabled = true;
        }
        _disabledBehaviours.Clear();

        for (int i = 0; i < _disabledObjects.Count; i++)
        {
            var go = _disabledObjects[i];
            if (go != null) go.SetActive(true);
        }
        _disabledObjects.Clear();
    }

    private bool IsExcluded(GameObject root)
    {
        if (excludeCanvasRoots == null) return false;
        foreach (var ex in excludeCanvasRoots)
            if (ex == root) return true;
        return false;
    }
}

