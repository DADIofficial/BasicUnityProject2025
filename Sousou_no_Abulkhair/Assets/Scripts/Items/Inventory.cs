using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Inventory", menuName = "Scriptable Objects/Inventory")]
public class Inventory : ScriptableObject
{
    public int width = 2;
    public int height = 5;

    public List<InventoryInstance> slots = new List<InventoryInstance>();
    public static QuestController instance;

    private void OnEnable()
    {
        int totalSlots = width * height;
        if (slots.Count != totalSlots)
        {
            slots.Clear();
            for (int i = 0; i < totalSlots; i++)
                slots.Add(new InventoryInstance());
        }
    }

    public bool AddItem(InventoryInstance itemToAdd)
    {
        int maxItems = width * height;
        for(int i = 0; i < slots.Count; i++)
        {
            if (slots[i].count == 0) {
                slots[i] = itemToAdd;
                return true;
            }
        }
        if(slots.Count < maxItems)
        {
            slots.Add(itemToAdd);
            //instance.CheckInventoryForQuests();
            return true;
        }
        Debug.Log("Inventory is full");
        return false;
    }

    public bool RemoveItem(Item itemToRemove, int amount = 1)
    {
        for (int i = 0; i < slots.Count; i++)
        {
            if (slots[i].count > 0 && slots[i].item == itemToRemove)
            {
                slots[i].count -= amount;

                if (slots[i].count <= 0)
                {
                    slots[i].item = null;
                    slots[i].count = 0;
                }

                //instance.CheckInventoryForQuests();
                return true;
            }
        }

        Debug.Log("Item not found in inventory");
        return false;
    }

    public Dictionary<string, int> GetItemCounts()
    {
        Dictionary<string, int> itemCounts = new Dictionary<string, int>();
        foreach (var slot in slots)
        {
            if (slot == null || slot.item == null) continue;
            if (itemCounts.ContainsKey(slot.item.itemId))
                itemCounts[slot.item.itemId] += slot.count;
            else
                itemCounts[slot.item.itemId] = slot.count;
        }
        return itemCounts;
    }

    public void RemoveItemById(string itemId, int amount = 1)
    {
        int j = amount;
        for (int i = 0; i < slots.Count; i++)
        {
            if (slots[i].count > 0 && slots[i].item.itemId == itemId)
            {
                slots[i].item = null;
                slots[i].count = 0;
                j--;
                if (j == 0)
                    break;
            }
        }

        Debug.Log("Item not found in inventory");
    }
}
