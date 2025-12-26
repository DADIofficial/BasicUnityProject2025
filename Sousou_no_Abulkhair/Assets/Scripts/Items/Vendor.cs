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
    [SerializeField] private Player player;

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
            OpenShop();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
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

    /// <summary>
    /// Покупка предмета игроком
    /// </summary>
    public bool BuyItem(Item item)
    {
        if (item == null) return false;

        if (player.leaves >= item.price)
        {
            bool added = playerInventory.Add(new InventoryInstance(item, 1));
            if (added)
            {
                player.leaves -= item.price;
                UpdateCurrencyText();
                Debug.Log($"Куплено: {item.itemName}");
                return true;
            }
            else
            {
                Debug.Log("Инвентарь игрока полный!");
                //return false;
            }
        }
        Debug.Log("Недостаточно Leaves!");
        return false;
    }

    /// <summary>
    /// Продажа предмета игроком
    /// </summary>
    public void SellItem(Item item)
    {
        if (item == null) return;

        player.leaves += item.price / 100;
        playerInventory.Remove(item);
        UpdateCurrencyText();

        Debug.Log($"Продано: {item.itemName}");
    }

    /// <summary>
    /// Обновление текста валюты игрока
    /// </summary>
    private void UpdateCurrencyText()
    {
        if (currencyText != null)
        {
            currencyText.text = $"Leaves: {player.leaves}";
        }
    }
}
