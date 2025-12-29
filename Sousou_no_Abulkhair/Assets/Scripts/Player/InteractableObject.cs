using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider))]
public class InteractableObject : MonoBehaviour
{
    [Header("Prompt canvas (shows on trigger enter)")]
    [SerializeField] private GameObject promptCanvas; 

    [Header("Actions")]
    [SerializeField] private UnityEvent onOpen;   
    [SerializeField] private UnityEvent onClose;  

    [Header("Behavior")]
    [SerializeField] private bool hidePromptWhileOpen = true;

    private bool playerInside;
    private bool isOpen;

    private void Reset()
    {
        
        var col = GetComponent<Collider>();
        col.isTrigger = true;
    }

    private void Start()
    {
        if (promptCanvas != null)
            promptCanvas.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        playerInside = true;

        var interactor = other.GetComponent<PlayerInteractor>();
        if (interactor != null)
            interactor.SetCandidate(this);

        if (!isOpen && promptCanvas != null)
            promptCanvas.SetActive(true);
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        playerInside = false;

        if (promptCanvas != null)
            promptCanvas.SetActive(false);

        var interactor = other.GetComponent<PlayerInteractor>();
        if (interactor != null)
            interactor.ClearCandidate(this);
    }

    public void Open()
    {
        isOpen = true;

        if (hidePromptWhileOpen && promptCanvas != null)
            promptCanvas.SetActive(false);

        onOpen?.Invoke();
    }

    public void Close()
    {
        isOpen = false;

        onClose?.Invoke();

       
        RefreshPrompt();
    }

    public void RefreshPrompt()
    {
        if (promptCanvas == null) return;

        if (playerInside && !isOpen)
            promptCanvas.SetActive(true);
        else
            promptCanvas.SetActive(false);
    }
}

