using TMPro;
using UnityEngine;

public class Vendor : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private GameObject shopMenu;
    [SerializeField] private GameObject inventoryMenu;
    [SerializeField] private TextMeshProUGUI currencyText;

    [Header("References")]
    [SerializeField] private PlayerInventory playerInventory;
    [SerializeField] private InventoryUI inventoryUI;
    [SerializeField] private VendorUI vendorUI;
    [SerializeField] private PlayerInventory vendorInventory;

    public Inventory VendorInventory => vendorInventory.inventory;


    public int coef;

    private bool menuActivated = false;

    public enum VendorMode
    {
        Vendor,
        Chest
    }

    public VendorMode mode;


    private void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        UpdateCurrencyText();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            vendorUI.RefreshUI();
            inventoryUI.vendor = this;
            vendorUI.playerInventory = this.vendorInventory;
            vendorUI.inventory = this.vendorInventory.inventory;
            vendorUI.vendor = this;
            vendorUI.RefreshUI();
            OpenShop();

            if (mode == VendorMode.Chest)
            {
                var chest = GetComponent<ChestRuntime>();
                inventoryUI.currentChest = chest;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            inventoryUI.vendor = null;
            vendorUI.playerInventory = null;
            vendorUI.inventory = null;
            vendorUI.vendor = null;
            CloseShop();

            if (mode == VendorMode.Chest)
            {
                if (inventoryUI.currentChest == GetComponent<ChestRuntime>())
                    inventoryUI.currentChest = null;
            }

            
        }
    }

    private void OpenShop()
    {
        menuActivated = true;
        Cursor.visible = true;
        shopMenu.SetActive(true);
        inventoryMenu.SetActive(true);
    }

    private void CloseShop()
    {
        menuActivated = false;
        Cursor.visible = false;
        shopMenu.SetActive(false);
        inventoryMenu.SetActive(false);
    }

    public void ClosingShop()
    {
        menuActivated = false;
        Cursor.visible = false;
        shopMenu.SetActive(false);
        inventoryMenu.SetActive(false);

        inventoryUI.vendor = null;
        vendorUI.playerInventory = null;
        vendorUI.inventory = null;
        vendorUI.vendor = null;

        if (mode == VendorMode.Chest)
            {
                if (inventoryUI.currentChest == GetComponent<ChestRuntime>())
                    inventoryUI.currentChest = null;
            }

        CloseShop();
    }

    public bool BuyItem(Item item, int index)
    {
        if (item == null) return false;
        Debug.Log(Player.instance);
        Debug.Log(item);
        if (Player.instance.leaves >= coef * item.price)
        {
            bool added = Player.instance.playerInventory.Add(new InventoryInstance(item, 1));
            if (added)
            {
                Player.instance.leaves -= (coef * item.price);
                vendorInventory.RemoveBySlotIndex(index);


                // 🔑 ТОЛЬКО если сейчас открыт сундук
                if (inventoryUI.currentChest != null &&
                    inventoryUI.currentChest.inventory.IsEmpty())
                {
                    GameManager.Instance.OnChestLooted(
                        inventoryUI.currentChest.chestIndex
                    );

                    inventoryUI.currentChest = null; // защита
                }



                UpdateCurrencyText();
                Debug.Log($"{item.itemName}");

                return true;
            }
            else
            {
                Debug.Log("  !");
                //return false;
            }
        }
        Debug.Log(" Leaves!");
        return false;
    }


    public void SellItem(int index)
    {
        var item = Player.instance.playerInventory.inventory.slots[index]?.item;
        if (item == null) return;

        Player.instance.leaves += (coef * item.price / 100);
        Player.instance.playerInventory.RemoveBySlotIndex(index);
        vendorInventory.Add(new InventoryInstance(item, 1));
        UpdateCurrencyText();

        Debug.Log($"�������: {item.itemName}");
    }

    private void UpdateCurrencyText()
    {
        if (currencyText != null)
        {
            currencyText.text = $"Leaves: {Player.instance.leaves}";
        }
    }
}