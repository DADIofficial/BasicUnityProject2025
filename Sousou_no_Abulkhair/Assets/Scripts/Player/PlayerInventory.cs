using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;


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

    public void RemoveItemById(string itemId, int amount = 1)
    {
        inventory.RemoveItemById(itemId, amount);
        QuestController.instance.CheckInventoryForQuests();
        inventoryUI.RefreshUI();
    }

    public int GetAmountOfFreeSlots()
    {
        return inventory.GetFreeSlots();
    }

    public bool HasKey(string keyId, out InventoryInstance instance)
    {
        foreach (var slot in inventory.slots)
        {
            if (slot?.item is KeyItem key && key.keyId == keyId)
            {
                instance = slot;
                return true;
            }
        }

        instance = null;
        return false;
    }

    public void RemoveBySlotIndex(int index)
    {
        inventory.RemoveBySlotIndex(index);
        QuestController.instance.CheckInventoryForQuests();
        inventoryUI.RefreshUI();
    }
}
