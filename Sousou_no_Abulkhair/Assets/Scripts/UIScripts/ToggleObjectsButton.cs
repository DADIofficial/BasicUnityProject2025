using UnityEngine;

public class ToggleObjectsButton : MonoBehaviour
{
    [SerializeField] private GameObject[] targets;

    [SerializeField] private bool useActiveSelf = true;

    public void Toggle()
    {
        if (targets == null) return;

        foreach (var go in targets)
        {
            if (go == null) continue;

            bool current = useActiveSelf ? go.activeSelf : go.activeInHierarchy;
            go.SetActive(!current);
        }
    }
}

