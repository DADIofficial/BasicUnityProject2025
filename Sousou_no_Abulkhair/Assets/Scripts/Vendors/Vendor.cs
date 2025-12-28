using TMPro;
using UnityEngine;

public class Vendor : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private GameObject shopMenu;        // UI магазина
    [SerializeField] private GameObject inventoryMenu;   // UI игрока при открытом магазине
    [SerializeField] private TextMeshProUGUI currencyText;

    [Header("References")]
    [SerializeField] private PlayerInventory playerInventory;
    [SerializeField] private InventoryUI inventoryUI;
    [SerializeField] private PlayerInventory vendorInventory;
    [SerializeField] private Player player;

    private void Start()
    {
        // ВАЖНО: не трогаем курсор в Start, иначе ты сломаешь FPS-лок
        CloseShopUIOnly();
        UpdateCurrencyText();
    }

    // Вызывается из InteractableObject -> onOpen
    public void OpenVendor()
    {
        inventoryUI.vendor = this;

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        if (shopMenu != null) shopMenu.SetActive(true);
        if (inventoryMenu != null) inventoryMenu.SetActive(true);

        UpdateCurrencyText();
    }

    // Вызывается из InteractableObject -> onClose
    public void CloseVendor()
    {
        inventoryUI.vendor = null;

        CloseShopUIOnly();
    }

    private void CloseShopUIOnly()
    {
        if (shopMenu != null) shopMenu.SetActive(false);
        if (inventoryMenu != null) inventoryMenu.SetActive(false);
    }

    /// <summary> Покупка предмета игроком </summary>
    public bool BuyItem(Item item, int index)
    {
        if (item == null) return false;

        if (player.leaves >= item.price)
        {
            bool added = playerInventory.Add(new InventoryInstance(item, 1));
            if (added)
            {
                player.leaves -= item.price;
                vendorInventory.RemoveBySlotIndex(index);
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

    /// <summary> Продажа предмета игроком </summary>
    public void SellItem(int index)
    {
        var slot = playerInventory.inventory.slots[index];
        var item = slot?.item;
        if (item == null) return;

        player.leaves += item.price / 100;
        playerInventory.RemoveBySlotIndex(index);
        vendorInventory.Add(new InventoryInstance(item, 1));
        UpdateCurrencyText();

        Debug.Log($"Продано: {item.itemName}");
    }

    private void UpdateCurrencyText()
    {
        if (currencyText != null)
            currencyText.text = $"{player.leaves}";
    }

    // (Опционально) кнопка "Закрыть" в UI магазина
    public void CloseByButton()
    {
        if (PlayerInteractor.Instance != null)
            PlayerInteractor.Instance.EndInteraction();
        else
            CloseVendor();
    }
}

