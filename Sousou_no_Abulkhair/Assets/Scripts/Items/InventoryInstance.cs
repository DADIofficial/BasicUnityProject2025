using NUnit.Framework;
using UnityEngine;
using UnityEngine.UIElements;

[System.Serializable]
public class InventoryInstance
{
    public Item item;
    public int count;

    public void InventorySlot(Item item, int count = 1)
    {
        this.item = item;
        this.count = count;
    }
}
