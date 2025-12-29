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

    public void ReturnSet(){
    Time.timeScale = 1f;               
    Cursor.visible = true;              
    Cursor.lockState = CursorLockMode.None;
    }
}

