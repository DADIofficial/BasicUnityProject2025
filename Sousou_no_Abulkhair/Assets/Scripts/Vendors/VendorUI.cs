using UnityEngine;
using UnityEngine.EventSystems;

public class VendorUI : InventoryUI
{
    //public PlayerInventory heroInventory;

    void Start()
    {
        RefreshUI();
    }

    public override void OnSlotClicked(int index, PointerEventData.InputButton button)
    {
        var slot = playerInventory.inventory.slots[index];
        Debug.Log(slot.item);
        if (slot == null || slot.item == null) return;

        if (vendor != null)
        {
            var item = slot.item;
            InventoryInstance itemInstance = new InventoryInstance(item, 1);
            if (vendor.BuyItem(item, index))
            {
                
            }
        }

        RefreshUI();
    }
}

