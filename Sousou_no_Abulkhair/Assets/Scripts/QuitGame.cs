using UnityEngine;
using UnityEngine.SceneManagement;

public class QuitGame : MonoBehaviour
{
    [Header("Optional hotkey")]
    [SerializeField] private bool enableHotkey = false;
    [SerializeField] private KeyCode hotkey = KeyCode.Escape;



    public void Quit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        // В билде закрывает приложение
        Application.Quit();
#endif
    }

    public void LoadSceneByName(string sceneName)
    {
        if (string.IsNullOrWhiteSpace(sceneName))
        {
            Debug.LogWarning("LoadSceneByName: sceneName is empty.");
            return;
        }

        SceneManager.LoadScene(sceneName);
    }
}

