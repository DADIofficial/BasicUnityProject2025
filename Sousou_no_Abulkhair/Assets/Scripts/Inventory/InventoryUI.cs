using System.Collections.ObjectModel;
#if UNITY_EDITOR
using UnityEditor.Rendering.Universal;
#endif
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    public Inventory inventory;
    [SerializeField] public PlayerInventory playerInventory;
    public InventoryButton slotPrefab;    
    public Transform slotParent;         
    public Vendor vendor;

    public ChestRuntime currentChest;

    private Transform[] waypoints;

    public Inventory ActiveInventory =>
        vendor != null ? vendor.VendorInventory :
        currentChest != null ? currentChest.inventory :
        playerInventory.inventory;



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
        var slot = Player.instance.playerInventory.inventory.slots[index];
        Debug.Log(slot);
        if (slot == null || slot.item == null) return;

        if (button == PointerEventData.InputButton.Left)
        {
            if (vendor == null)
            {
                playerInventory.RemoveBySlotIndex(index);


                if (currentChest != null &&
                    currentChest.inventory.IsEmpty())
                {
                    GameManager.Instance.OnChestLooted(
                        currentChest.chestIndex
                    );

                    currentChest = null;
                }
            }
            else
            {
                vendor.SellItem(index);
            }
        }
        else if (button == PointerEventData.InputButton.Right)
        {
            
            var item = ActiveInventory.slots[index]?.item;

            if (item is PotionItem potion)
            {
                // Debug.Log("hp/mana/stamina");
                GameManager.Instance.UsePotion(potion);

                playerInventory.RemoveBySlotIndex(index, true);

                RefreshUI();
                return;
            }


            if (item.itemType == ItemType.Potion)
            {
                int Id = int.Parse(item.itemId);
                GameManager.Instance.ChangeMagic(Id);

                playerInventory.RemoveBySlotIndex(index, true);

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
