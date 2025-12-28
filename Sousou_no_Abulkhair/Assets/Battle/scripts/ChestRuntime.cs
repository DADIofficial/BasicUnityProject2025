using UnityEngine;

public class ChestRuntime : MonoBehaviour
{
    public int chestIndex;
    public Inventory inventory;

    [HideInInspector] public Vector3 startPosition;
    [HideInInspector] public Quaternion startRotation;

    public bool IsInUse { get; private set; }

    private void Awake()
    {
        startPosition = transform.position;
        startRotation = transform.rotation;

        IsInUse = false;
        gameObject.SetActive(false);
    }

    public void TeleportTo(Vector3 position)
    {
        if (position == Vector3.zero)
        {
            Debug.LogError($"[ChestRuntime] Teleport aborted: ZERO position (chest {chestIndex})");
            return;
        }

        transform.position = position;
        transform.rotation = startRotation;

        IsInUse = true;
        gameObject.SetActive(true);

        Debug.Log($"[ChestRuntime] Chest {chestIndex} teleported to {position}");
    }

    public void ReturnToStart()
    {
        transform.position = startPosition;
        transform.rotation = startRotation;

        IsInUse = false;
        gameObject.SetActive(false);
    }
}
