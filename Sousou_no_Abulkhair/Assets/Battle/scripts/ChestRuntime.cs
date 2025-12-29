using UnityEngine;
using System.Collections.Generic;


public class ChestRuntime : MonoBehaviour
{
    public int chestIndex;
    public Inventory inventory;

    [HideInInspector] public Vector3 startPosition;
    [HideInInspector] public Quaternion startRotation;

    [SerializeField] private List<ChestItemSaveData> initialItems = new();

    [SerializeField] private Vendor ven;


    public bool IsInUse { get; private set; }

    private void Awake()
    {
        startPosition = transform.position;
        startRotation = transform.rotation;

        initialItems.Clear();

        foreach (var slot in inventory.slots)
        {
            if (slot == null || slot.item == null)
                continue;

            initialItems.Add(new ChestItemSaveData
            {
                itemId = slot.item.itemId,
                count = 1 // 🔑 ВСЕГДА 1, потому что объект уникален
            });
        }


        Debug.Log($"[ChestRuntime] Awake chest {chestIndex}, items = {inventory.GetItemCounts().Count}");

        IsInUse = false;
        gameObject.SetActive(false);
    }

    public void TeleportTo(Vector3 position)
    {
        if (position == Vector3.zero)
        {
            Debug.LogError($"[ChestRuntime] Teleport aborted: ZERO position (chest {chestIndex})");
            return;
        }

        transform.position = position;
        transform.rotation = startRotation;

        IsInUse = true;
        gameObject.SetActive(true);

        Debug.Log($"[ChestRuntime] Chest {chestIndex} teleported to {position}");
    }


    public void ReturnToStart()
    {
        transform.position = startPosition;
        transform.rotation = startRotation;

        IsInUse = false;
        gameObject.SetActive(false);

        ven.ClosingShop();
    }


    public void OpenChest(InventoryUI inventoryUI)
    {
        inventoryUI.currentChest = this;
    }

    public void CloseChest(InventoryUI inventoryUI)
    {
        if (inventoryUI.currentChest == this)
            inventoryUI.currentChest = null;
    }

    public void RestoreInitialInventory()
    {
        inventory.SetFromSave(initialItems);
        Debug.Log($"[ChestRuntime] Restored initial inventory, items = {initialItems.Count}");
    }



}
