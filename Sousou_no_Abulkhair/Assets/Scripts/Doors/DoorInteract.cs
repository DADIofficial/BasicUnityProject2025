using UnityEngine;

public class DoorInteract : MonoBehaviour
{
    [SerializeField] private Door door;
    [SerializeField] private PlayerInventory inventory;

    private bool playerInside;

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
                playerInside = true;
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player"))
                playerInside = false;
        }

        private void Update()
        {
            if (!playerInside)
                return;

            if (Input.GetKeyDown(KeyCode.E))
            {
                door.TryOpen(inventory);
            }
        }
}
