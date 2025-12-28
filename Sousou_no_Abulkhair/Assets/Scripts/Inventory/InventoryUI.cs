using System.Collections.ObjectModel;
using UnityEditor.Rendering.Universal;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    public Inventory inventory;
    [SerializeField] public PlayerInventory playerInventory;
    public InventoryButton slotPrefab;    // ������ ������ �����
    public Transform slotParent;          // Parent ��� ������
    public Vendor vendor;

    private Transform[] waypoints;

    void Start()
    {
        //CreateSlots();
        RefreshUI();
    }

    private void OnEnable()
    {
        PlayerInventory.OnInventoryChanged += RefreshUI;
    }


    public void RefreshUI()
    {
        if (playerInventory == null || playerInventory.inventory == null) return;

        slotParent = transform;
        int childCount = slotParent.childCount;
        waypoints = new Transform[childCount];

        for (int i = 0; i < childCount; i++)
        {
            waypoints[i] = slotParent.GetChild(i);
            InventoryButton slotUI = waypoints[i].GetComponent<InventoryButton>();

            if (slotUI != null)
            {
                slotUI.index = i;
                slotUI.inventoryUI = this;
            }

            Image slotImage = waypoints[i].GetComponent<Image>();
            if (slotImage != null)
                slotImage.sprite = null;
        }

        for (int i = 0; i < playerInventory.inventory.slots.Count && i < waypoints.Length; i++)
        {
            var slot = playerInventory.inventory.slots[i];
            if (slot != null && slot.item != null)
            {
                Image slotImage = waypoints[i].GetComponent<Image>();
                if (slotImage != null)
                    slotImage.sprite = slot.item.icon;
            }
        }
    }

    public virtual void OnSlotClicked(int index, PointerEventData.InputButton button)
    {
        var slot = inventory.slots[index];
        Debug.Log(slot);
        if (slot == null || slot.item == null) return;
        // Debug.Log("Surprise, Motherfucker! Inventory UI click");
        /*switch (inventory.inventoryType)
        {
            case InventoryType.Player:
            case InventoryType.Chest:
                inventory.RemoveBySlotIndex(index);
                break;

            case InventoryType.Vendor:
                if (vendor != null)
                {
                    vendor.BuyItem(slot.item);
                }
                break;
        }*/


        if (button == PointerEventData.InputButton.Left)
        {
            if (vendor == null)
            {
                playerInventory.RemoveBySlotIndex(index);
            }
            else
            {
                vendor.SellItem(index);
            }
        }
        else if (button == PointerEventData.InputButton.Right)
        {
            
            var item = playerInventory.inventory.slots[index]?.item;

            if (item is PotionItem potion)
            {
                // Debug.Log("hp/mana/stamina");
                GameManager.Instance.UsePotion(potion);

                inventory.RemoveBySlotIndex(index);

                RefreshUI();
                return;
            }


            if (item.itemType == ItemType.Potion)
            {
                int Id = int.Parse(item.itemId);
                GameManager.Instance.ChangeMagic(Id);

                inventory.RemoveBySlotIndex(index);

                RefreshUI();
                return;
            }


            if (item.itemType != ItemType.Weapon)
                return;
            item?.OnRightClick(Player.instance);

            

            // RightClickAction(index);
        }

        RefreshUI();
    }
}
