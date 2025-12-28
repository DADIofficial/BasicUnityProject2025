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

    public int coef;

    private bool menuActivated = false;

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
            currencyText.text = $"{player.leaves}";
        }
    }
}
