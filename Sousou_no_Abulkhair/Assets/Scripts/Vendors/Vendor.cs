using TMPro;
using UnityEngine;

public class Vendor : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private GameObject shopMenu;
    [SerializeField] private GameObject inventoryMenu;
    [SerializeField] private TextMeshProUGUI currencyText;

    [Header("References")]
    [SerializeField] private InventoryUI inventoryUI;
    [SerializeField] private VendorUI vendorUI;
    [SerializeField] private PlayerInventory vendorInventory;

    public Inventory VendorInventory => vendorInventory.inventory;

    [Header("Prices")]
    public int coef = 1;

    public enum VendorMode { Vendor, Chest }
    public VendorMode mode;

    private bool menuActivated;

    private void Start()
    {
        CloseShopUIOnly();
        UpdateCurrencyText();
    }

    public void OpenVendorUI()
    {
        menuActivated = true;

        if (inventoryUI != null)
            inventoryUI.vendor = this;

        if (vendorUI != null)
        {
            vendorUI.vendor = this;
            vendorUI.playerInventory = this.vendorInventory;
            vendorUI.inventory = this.vendorInventory.inventory;
            vendorUI.RefreshUI();
        }

        if (mode == VendorMode.Chest && inventoryUI != null)
        {
            var chest = GetComponent<ChestRuntime>();
            inventoryUI.currentChest = chest;
        }

        if (shopMenu != null) shopMenu.SetActive(true);
        if (inventoryMenu != null) inventoryMenu.SetActive(true);

        UpdateCurrencyText();
    }

    public void CloseVendorUI()
    {
        menuActivated = false;

        CloseShopUIOnly();

        if (inventoryUI != null)
            inventoryUI.vendor = null;

        if (vendorUI != null)
        {
            vendorUI.playerInventory = null;
            vendorUI.inventory = null;
            vendorUI.vendor = null;
        }

        if (mode == VendorMode.Chest && inventoryUI != null)
        {
            var chest = GetComponent<ChestRuntime>();
            if (inventoryUI.currentChest == chest)
                inventoryUI.currentChest = null;
        }
    }

    public void ClosingShop()
    {
        // если у тебя пауза/время/курсор управляются PlayerInteractor'ом — закрывай через него
        if (PlayerInteractor.Instance != null && PlayerInteractor.Instance.IsInteracting)
            PlayerInteractor.Instance.EndInteraction();
        else
            CloseVendorUI();
    }

    public void CloseByButton()
    {
        ClosingShop();
    }

    private void CloseShopUIOnly()
    {
        if (shopMenu != null) shopMenu.SetActive(false);
        if (inventoryMenu != null) inventoryMenu.SetActive(false);
    }

    public bool BuyItem(Item item, int index)
    {
        if (item == null) return false;

        if (Player.instance.leaves >= coef * item.price)
        {
            bool added = Player.instance.playerInventory.Add(new InventoryInstance(item, 1));
            if (added)
            {
                Player.instance.leaves -= (coef * item.price);
                vendorInventory.RemoveBySlotIndex(index);

                if (inventoryUI != null &&
                    inventoryUI.currentChest != null &&
                    inventoryUI.currentChest.inventory.IsEmpty())
                {
                    GameManager.Instance.OnChestLooted(inventoryUI.currentChest.chestIndex);
                    inventoryUI.currentChest = null;
                }

                UpdateCurrencyText();
                Debug.Log($"Куплено: {item.itemName}");
                return true;
            }

            Debug.Log("Инвентарь игрока полный!");
            return false;
        }

        Debug.Log("Недостаточно Leaves!");
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
        Debug.Log($"Продано: {item.itemName}");
    }

    private void UpdateCurrencyText()
    {
        if (currencyText != null)
            currencyText.text = $"{Player.instance.leaves}";
    }
}
