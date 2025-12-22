using TMPro;
using UnityEngine;

public class Vendor : MonoBehaviour
{

    [SerializeField] private GameObject ShopMenu;
    [SerializeField] private GameObject InventoryMenu;
    [SerializeField] private PlayerInventory playerInventory;
    [SerializeField] private Player player;
    [SerializeField] TextMeshProUGUI currencyText;
    private bool menuActivated = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Cursor.lockState = CursorLockMode.None;
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.gameObject.tag);
        if (other.gameObject.CompareTag("Player"))
        {
            menuActivated = true;
            Cursor.visible = true;
            ShopMenu.SetActive(menuActivated);
            InventoryMenu.SetActive(true);
        }
    }

    private void OnTriggerStay(Collider other)
    {

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            menuActivated = false;
            Cursor.visible = false;
            ShopMenu.SetActive(menuActivated);
            InventoryMenu.SetActive(false);
        }
    }

    public void BuyItem(Item item)
    {
        InventoryInstance instance = new InventoryInstance(item, 1);
        if (player.leaves >= item.price && playerInventory.Add(instance))
        {
            player.leaves -= item.price;

            currencyText.text = $"Leaves: {player.leaves}";

            Debug.Log("Куплен: " + item.itemName);
        }
        else
        {
            Debug.Log("Недостаточно золота");
        }
    }

    public void SellItem(Item item)
    {
        player.leaves += item.price / 100;
        playerInventory.Remove(item);

        Debug.Log("Продан: " + item.itemName);
    }
}
