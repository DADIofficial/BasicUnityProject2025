using System.Collections.ObjectModel;
using UnityEditor.Rendering.Universal;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    public PlayerInventory playerInventory;
    public PlayerInventory customerInventory;
    public InventoryButton inventoryButton;
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
        }
        //Debug.Log(playerInventory);
        int j = 0; 
        foreach (InventoryInstance i in playerInventory.inventory.slots) {
            if(i == null)
            {
                //Debug.Log("No slots in inventory");
                return;
            }
            if(i.item == null)
            {
                //Debug.Log("Empty slot found");
                j++;
                continue;
            }
            waypoints[j].GetComponent<Image>().sprite = i.item.icon;
            j++;
        }
    }

    public void OnSlotClicked(int index)
    {
        var slots = playerInventory.inventory.slots;

        // Check if slot is empty
        if (slots[index] == null || slots[index].item == null)
        {
            Debug.Log("Slot is empty: " + index);
            return;
        }

        // Get the item
        var item = slots[index].item;
    }
}
