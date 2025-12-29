using UnityEngine;

public class MenuResumeButton : MonoBehaviour
{
    [SerializeField] private MenuPauseController menuController;

    
    public void ReturnToGame()
    {
        if (menuController == null)
            menuController = FindObjectOfType<MenuPauseController>();

        if (menuController != null)
            menuController.CloseMenu();
        else
            Debug.LogWarning("MenuReturnButton: �� ������ MenuPauseController �� �����.");
    }
}
