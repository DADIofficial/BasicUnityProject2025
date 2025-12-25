using UnityEngine;

public class DoorInteract : MonoBehaviour
{
    [SerializeField] private Door door;
    [SerializeField] private PlayerInventory inventory;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            door.TryOpen(inventory);
        }
    }
}
