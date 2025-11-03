using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    [SerializeField] private GameObject InventoryMenu;
    private bool menuActivated = false;

    void Start()
    {
        
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("E key pressed - toggling inventory menu");
            menuActivated = !menuActivated;
            InventoryMenu.SetActive(menuActivated);
        }
    }

    public void AddItem(string itemName, Sprite itemIcon, string itemDescription)
    {
        // Implementation for adding item to inventory
        Debug.Log($"Adding item: {itemName}");
    }
}
