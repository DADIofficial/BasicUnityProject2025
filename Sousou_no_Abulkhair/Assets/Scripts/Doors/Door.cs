using UnityEngine;

public class Door : MonoBehaviour
{
    [Header("Key settings")]
    [SerializeField] private string requiredKeyId;
    [SerializeField] private bool isOpen = false;

    [Header("Animation")]
    [SerializeField] private Animator animator;

    private static readonly int OpenHash = Animator.StringToHash("Open");

    public void TryOpen(PlayerInventory inventory)
    {   
        if (isOpen) return;

        if (inventory.HasKey(requiredKeyId, out InventoryInstance keyInstance))
        {
            OpenDoor();

            KeyItem key = keyInstance.item as KeyItem;
            if (key != null)
            {
                //inventory.Remove(keyInstance);
            }
        }
        else
        {
            Debug.Log("Door: Required key not found");
        }
    }

    private void OpenDoor()
    {
        isOpen = true;

        if (animator != null)
            animator.SetTrigger(OpenHash);

        GetComponent<Collider>().enabled = false;
    }
}
