using UnityEngine;

public class MenuResumeButton : MonoBehaviour
{
    [SerializeField] private MenuPauseController menuController;

    // Вызывай этот метод в Button -> OnClick()
    public void ReturnToGame()
    {
        if (menuController == null)
            menuController = FindObjectOfType<MenuPauseController>();

        if (menuController != null)
            menuController.CloseMenu();
        else
            Debug.LogWarning("MenuReturnButton: Не найден MenuPauseController на сцене.");
    }
}
