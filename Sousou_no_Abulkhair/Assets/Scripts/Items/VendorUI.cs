using UnityEngine;

public class VendorUI : InventoryUI
{
    //public PlayerInventory heroInventory;

    // Переопределяем OnSlotClicked для покупки
    public override void OnSlotClicked(int index)
    {
        var slot = playerInventory.inventory.slots[index];
        Debug.Log(slot.item);
        Debug.Log("Покупка предмета...");
        if (slot == null || slot.item == null) return;

        if (vendor != null)
        {
            var item = slot.item;
            InventoryInstance itemInstance = new InventoryInstance(item, 1);
            if (vendor.BuyItem(item))
            {
                Debug.Log("Игрок купил: " + item.itemName);
            }
            else
            {
                Debug.Log("Инвентарь игрока полный!");
            }
        }

        RefreshUI();
    }
}

