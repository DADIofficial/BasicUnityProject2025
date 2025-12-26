using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class Battle_Open : MonoBehaviour
{
    [SerializeField] private Collider playerWeapon; 
    [SerializeField] private string worldToLoad;
    void Update()
    {
    }

    private bool triggered = false;

    private void OnTriggerEnter(Collider other)
    {
        if (!triggered && other == playerWeapon)
        {
            Debug.Log("Enemy hit by assigned collider!");

            triggered = true;

            // DisablePlayerControl();

            // // Включаем курсор
            // Cursor.visible = true;
            // Cursor.lockState = CursorLockMode.None;

            // SceneManager.LoadScene("BattleScene");
            StartCoroutine(LoadBattleScene());
            
        }
        // StartCoroutine(close());
    }
    private IEnumerator LoadBattleScene()
    {
        // Отключаем управление
        DisablePlayerControl();

        // Включаем курсор
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        // Ждём перед загрузкой сцены (например, чтобы показать анимацию удара)
        yield return new WaitForSeconds(1f);

        // Загружаем новую сцену
        SceneManager.LoadScene("BattleScene");

        // После загрузки можно снова разрешить триггер
        StartCoroutine(ResetTrigger());
    }

    private IEnumerator ResetTrigger()
    {
        yield return new WaitForSeconds(3f);
        triggered = false;
    }

    private void DisablePlayerControl()
    {
        // Используем новый безопасный метод поиска
        // var playerController = FindFirstObjectByType<StarterAssets.ThirdPersonController>();
        // var playerInput = FindFirstObjectByType<UnityEngine.InputSystem.PlayerInput>();

        // if (playerController != null)
        //     playerController.enabled = false;

        // if (playerInput != null)
        //     playerInput.enabled = false;
    }
}
