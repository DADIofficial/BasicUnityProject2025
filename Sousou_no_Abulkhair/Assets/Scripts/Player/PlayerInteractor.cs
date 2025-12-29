using UnityEngine;

public class PlayerInteractor : MonoBehaviour
{
    public static PlayerInteractor Instance { get; private set; }

    [Header("Input")]
    [SerializeField] private KeyCode interactKey = KeyCode.F;

    [Header("What to disable while paused (optional)")]
    [SerializeField] private MonoBehaviour[] disableWhileInteracting;
    [SerializeField] private GameObject[] hideUiWhileInteracting;

    [Header("Pause options")]
    [SerializeField] private bool pauseAudio = false;

    private InteractableObject currentCandidate;
    private InteractableObject currentInteracting;

    private float prevTimeScale;
    private float prevFixedDeltaTime;
    private bool wasAudioPaused;

    private bool inputBlocked;

    private int externalLocks;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Update()
    {
        if (inputBlocked) return;
        if (!Input.GetKeyDown(interactKey)) return;

        if (currentInteracting != null)
        {
            EndInteraction();
            return;
        }

        if (currentCandidate != null)
        {
            BeginInteraction(currentCandidate);
        }
    }

    public void SetCandidate(InteractableObject obj)
    {
        currentCandidate = obj;
    }

    public void ClearCandidate(InteractableObject obj)
    {
        if (currentCandidate == obj) currentCandidate = null;

        if (currentInteracting == obj)
            EndInteraction();
    }

    private void BeginInteraction(InteractableObject obj)
    {
        currentInteracting = obj;

        prevTimeScale = Time.timeScale;
        prevFixedDeltaTime = Time.fixedDeltaTime;

        Time.timeScale = 0f;
        Time.fixedDeltaTime = 0f;

        if (pauseAudio)
        {
            wasAudioPaused = AudioListener.pause;
            AudioListener.pause = true;
        }

        DisableControlledStuff();

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        obj.Open();
    }

    public void EndInteraction()
    {
        if (currentInteracting == null) return;

        currentInteracting.Close();
        currentInteracting = null;

        Time.timeScale = prevTimeScale;
        Time.fixedDeltaTime = prevFixedDeltaTime;

        if (pauseAudio)
            AudioListener.pause = wasAudioPaused;

        if (externalLocks <= 0)
            EnableControlledStuff();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        if (currentCandidate != null)
            currentCandidate.RefreshPrompt();
    }

    public void SetInputBlocked(bool blocked)
    {
        inputBlocked = blocked;
    }

    public void PushExternalLock()
    {
        externalLocks++;
        if (externalLocks == 1)
        {
            DisableControlledStuff();
        }
    }

    public void PopExternalLock()
    {
        externalLocks = Mathf.Max(0, externalLocks - 1);

        if (externalLocks == 0 && currentInteracting == null)
        {
            EnableControlledStuff();
        }
    }

    private void DisableControlledStuff()
    {
        foreach (var mb in disableWhileInteracting)
            if (mb != null) mb.enabled = false;

        foreach (var go in hideUiWhileInteracting)
            if (go != null) go.SetActive(false);
    }

    private void EnableControlledStuff()
    {
        foreach (var mb in disableWhileInteracting)
            if (mb != null) mb.enabled = true;

        foreach (var go in hideUiWhileInteracting)
            if (go != null) go.SetActive(true);
    }

    public bool IsInteracting => currentInteracting != null;
}
