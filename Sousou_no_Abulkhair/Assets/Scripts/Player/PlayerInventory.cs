using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public Inventory inventory;
    [SerializeField] InventoryUI inventoryUI;

    public bool Add(InventoryInstance item)
    {
        bool ans = inventory.AddItem(item);
        inventoryUI.RefreshUI();
        return ans;
    }

    public bool Remove(Item item, int amount = 1)
    {
        bool ans = inventory.RemoveItem(item, amount);
        inventoryUI.RefreshUI();
        return ans;
    }
}
