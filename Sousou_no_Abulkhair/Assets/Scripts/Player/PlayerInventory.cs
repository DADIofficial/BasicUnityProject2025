using UnityEngine;

using static QuestController;

public class PlayerInventory : MonoBehaviour
{
    public Inventory inventory;
    [SerializeField] InventoryUI inventoryUI;
    // public static QuestController instance;

    public bool Add(InventoryInstance item)
    {
        bool ans = inventory.AddItem(item);
        QuestController.instance.CheckInventoryForQuests();
        inventoryUI.RefreshUI();
        return ans;
    }

    public bool Remove(Item item, int amount = 1)
    {
        bool ans = inventory.RemoveItem(item, amount);
        QuestController.instance.CheckInventoryForQuests();
        inventoryUI.RefreshUI();
        return ans;
    }
}
