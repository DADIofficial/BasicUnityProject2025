using System.Collections.ObjectModel;
using UnityEditor.Rendering.Universal;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    public PlayerInventory playerInventory;
    public PlayerInventory customerInventory;
    public InventoryButton inventoryButton;
    public Vendor vendor;
    public Transform slotParent;
    private Transform[] waypoints;

    void Start()
    {
        RefreshUI();
    }

    public void RefreshUI()
    {
        slotParent = GetComponent<Transform>();
        waypoints = new Transform[transform.childCount];
        for (int i = 0; i < transform.childCount; i++)
        {
            waypoints[i] = slotParent.GetChild(i);
            InventoryButton slotUI = waypoints[i].GetComponent<InventoryButton>();
            slotUI.index = i;
            slotUI.inventoryUI = this;

            // Очищаем изображение по умолчанию
            waypoints[i].GetComponent<Image>().sprite = null;
        }

        for (int i = 0; i < playerInventory.inventory.slots.Count; i++)
        {
            var slot = playerInventory.inventory.slots[i];
            if (slot != null && slot.item != null)
            {
                waypoints[i].GetComponent<Image>().sprite = slot.item.icon;
            }
        }
    }


    public void OnSlotClicked(int index)
    {
        var slots = playerInventory.inventory.slots;

        // Check if slot is empty
        if (slots[index] == null || slots[index].item == null)
        {
            //Debug.Log("Slot is empty: " + index);
            return;
        }

        // Get the item
        var item = slots[index].item;
        //Debug.Log("Clicked on item: " + item);
        //Debug.Log("Attempting to buy item: " + item.itemName);
        vendor.BuyItem(item);
    }
}
