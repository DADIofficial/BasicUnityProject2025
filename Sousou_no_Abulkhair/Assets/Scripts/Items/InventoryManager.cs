using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    [SerializeField] private GameObject InventoryMenu;
    [SerializeField] private GameObject QuestLogMenu;
    [SerializeField] private InventoryUI inventoryUI;
    [SerializeField] private Inventory inventory;
    private bool menuActivated = false;
    private bool questLogActivated = false;

    void Start()
    {
        
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.E))
        {
            //Debug.Log("E key pressed - toggling inventory menu");
            menuActivated = !menuActivated;
            InventoryMenu.SetActive(menuActivated);
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            //Debug.Log("C key pressed - toggling inventory menu");
            questLogActivated = !questLogActivated;
            QuestLogMenu.SetActive(questLogActivated);
        }
    }

    public void AddItem(string itemName, Sprite itemIcon, string itemDescription)
    {
        // Implementation for adding item to inventory
        //Debug.Log($"Adding item: {itemName}");
        inventoryUI.RefreshUI();
    }

    public void RemoveItemById(string itemId, int amount = 1)
    {
        inventory.RemoveItemById(itemId, amount);
        inventoryUI.RefreshUI();
    }

    public Dictionary<string, int> GetItemCounts()
    {
        return inventory.GetItemCounts();
    }
}
