using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public Inventory inventory;

    public void Add(InventoryInstance item)
    {
        inventory.AddItem(item);
    }
}
