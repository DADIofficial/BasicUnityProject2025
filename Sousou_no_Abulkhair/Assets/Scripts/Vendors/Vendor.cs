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

    private void Start()
    {
        // �����: �� ������� ������ � Start, ����� �� �������� FPS-���
        CloseShopUIOnly();
        UpdateCurrencyText();
    }

    // ���������� �� InteractableObject -> onOpen
    public void OpenVendor()
    {
        inventoryUI.vendor = this;

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        if (shopMenu != null) shopMenu.SetActive(true);
        if (inventoryMenu != null) inventoryMenu.SetActive(true);

        UpdateCurrencyText();
    }

    // ���������� �� InteractableObject -> onClose
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

    /// <summary> ������� �������� ������� </summary>
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

            Debug.Log("��������� ������ ������!");
            return false;
        }

        Debug.Log("������������ Leaves!");
        return false;
    }

    /// <summary> ������� �������� ������� </summary>
    public void SellItem(int index)
    {
        var slot = playerInventory.inventory.slots[index];
        var item = slot?.item;
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
            currencyText.text = $"{player.leaves}";
    }

    // (�����������) ������ "�������" � UI ��������
    public void CloseByButton()
    {
        if (PlayerInteractor.Instance != null)
            PlayerInteractor.Instance.EndInteraction();
        else
            CloseVendor();
    }
}

