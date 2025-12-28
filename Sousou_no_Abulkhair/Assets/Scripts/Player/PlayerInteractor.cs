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
        if (!Input.GetKeyDown(interactKey)) return;

        // Если уже взаимодействуем — закрываем по F
        if (currentInteracting != null)
        {
            EndInteraction();
            return;
        }

        // Если рядом есть интерактивный объект — открываем по F
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

        // Если игрок вышел из триггера во время взаимодействия — закрыть всё (на всякий)
        if (currentInteracting == obj)
            EndInteraction();
    }

    private void BeginInteraction(InteractableObject obj)
    {
        currentInteracting = obj;

        // Пауза мира
        prevTimeScale = Time.timeScale;
        prevFixedDeltaTime = Time.fixedDeltaTime;

        Time.timeScale = 0f;
        Time.fixedDeltaTime = 0f;

        if (pauseAudio)
        {
            wasAudioPaused = AudioListener.pause;
            AudioListener.pause = true;
        }

        // Отключаем управление/камера и т.п.
        foreach (var mb in disableWhileInteracting)
            if (mb != null) mb.enabled = false;

        foreach (var go in hideUiWhileInteracting)
            if (go != null) go.SetActive(false);

        // Курсор
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        // Открываем объект
        obj.Open();
    }

    public void EndInteraction()
    {
        if (currentInteracting == null) return;

        currentInteracting.Close();
        currentInteracting = null;

        // Возврат мира
        Time.timeScale = prevTimeScale;
        Time.fixedDeltaTime = prevFixedDeltaTime;

        if (pauseAudio)
            AudioListener.pause = wasAudioPaused;

        foreach (var mb in disableWhileInteracting)
            if (mb != null) mb.enabled = true;

        foreach (var go in hideUiWhileInteracting)
            if (go != null) go.SetActive(true);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // Если всё ещё стоим рядом с объектом — он сам может снова показать подсказку
        if (currentCandidate != null)
            currentCandidate.RefreshPrompt();
    }

    public bool IsInteracting => currentInteracting != null;
}
