using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoseVariant : MonoBehaviour
{

    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private CanvasGroup canvasGroup1;
    [SerializeField] private Button quitButton;
    [SerializeField] private Button tryAgainButton;


    // [SerializeField] Player_battle player;

    [SerializeField] private PlayerInventory playerInventory;

    private void Awake()
    {
        Hide();
    }

    void Start()
    {
        quitButton.onClick.AddListener(OnQuit);
        tryAgainButton.onClick.AddListener(OnTryAgain);
    }

    void Update()
    {
        
    }

    public void Show()
    {
        canvasGroup.alpha = 1f;
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;

        canvasGroup1.alpha = 0f;
        canvasGroup1.interactable = false;
        canvasGroup1.blocksRaycasts = false;
    }

    public void Hide()
    {
        canvasGroup.alpha = 0f;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;

        canvasGroup1.alpha = 1f;
        canvasGroup1.interactable = true;
        canvasGroup1.blocksRaycasts = true;
    }

    private void OnTryAgain()
    {
        Debug.Log("[LoseVariant] Try Again");

        GameManager.Instance.RestoreBattleSnapshot(playerInventory);

        SceneManager.LoadScene("BattleScene");
    }

    private void OnQuit()
    {
        Debug.Log("[LoseVariant] Quit");

        GameManager.Instance.RestoreBattleSnapshot(playerInventory);

        SceneManager.LoadScene("MainLevel");
    }
}
