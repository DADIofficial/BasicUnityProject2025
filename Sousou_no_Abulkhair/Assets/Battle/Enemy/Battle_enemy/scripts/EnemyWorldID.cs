using UnityEngine;

public class EnemyWorldID : MonoBehaviour
{
    public string id;

#if UNITY_EDITOR
    private void OnValidate()
    {
        if (string.IsNullOrEmpty(id))
        {
            id = System.Guid.NewGuid().ToString();
            UnityEditor.EditorUtility.SetDirty(this);
        }
    }
#endif


    private void Awake()
    {
        if (GameManager.Instance != null &&
            GameManager.Instance.IsEnemyKilled(id))
        {
            Destroy(gameObject);
        }
    }
}
