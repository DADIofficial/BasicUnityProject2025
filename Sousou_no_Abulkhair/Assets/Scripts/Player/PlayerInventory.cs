using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;


public class PlayerInventory : MonoBehaviour
{
    public Inventory inventory;

    [SerializeField] private InventoryUI inventoryUI;
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

    private Item getItemById(string itemId) {
        foreach (var slot in inventory.slots) {
            if(slot?.item.itemId == itemId) {
                return slot?.item;
            }
        }
        return null;
    }

    public void RemoveItemById(string itemId, int amount = 1)
    {
        var item = getItemById(itemId);
        if(item == null)
            return;

        if (item is WeaponItem weaponItem)
        {
            if (Player.instance.currentWeaponItem == weaponItem)
            {
                Player.instance.ChangeWeapon(null);
            }
        }
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

    public static event System.Action OnInventoryChanged;

    public void RemoveBySlotIndex(int index)
    {
        var item = inventory.slots[index].item;

        if (item is WeaponItem weaponItem)
        {
            if (Player.instance.currentWeaponItem == weaponItem)
            {
                Player.instance.ChangeWeapon(null);
            }
        }

        inventory.RemoveBySlotIndex(index);
        QuestController.instance.CheckInventoryForQuests();

        OnInventoryChanged?.Invoke();
    }
    
    public bool IsInInventory(Item item)
    {
        return inventory.IsInInventory(item);
    }
}
