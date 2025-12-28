using System;
using UnityEditor.UIElements;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player instance;
    public int health = 100;
    public int stamina = 100;
    public int mana = 100;
    public int leaves = 10;
    [SerializeField] private float speed = 5.0f;
    public PlayerInventory playerInventory;
    [Header("Weapon")]
    public Transform weaponSocket;
    public WeaponItem currentWeaponItem;

    private GameObject currentWeaponObject;

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        EquipWeapon(currentWeaponItem);
    }

    public void EquipWeapon(WeaponItem weaponItem)
    {
        // ������� ������ ������
        if (currentWeaponObject != null)
            Destroy(currentWeaponObject);

        currentWeaponItem = weaponItem;

        if (weaponItem == null) return;
        if(!playerInventory.IsInInventory(weaponItem))
        {
            currentWeaponItem = null;
            return;
        }

        currentWeaponObject = Instantiate(
            weaponItem.weaponPrefab,
            weaponSocket
        );

        currentWeaponObject.transform.localPosition = Vector3.zero;
        currentWeaponObject.transform.localRotation = Quaternion.identity;
        int index = GetCurrentWeaponIndex();
        Debug.Log("Current weapon index in slot: " + index);
    }

    public void ChangeWeapon(WeaponItem newWeapon)
    {
        currentWeaponItem = newWeapon;
        EquipWeapon(newWeapon);
        //int index = GetCurrentWeaponIndex();
        //Debug.Log("Current weapon index in slot: " + index);
    }

    public int GetCurrentWeaponIndex()
    {
        if (currentWeaponItem == null) return -1;
        for (int i = 0; i < playerInventory.inventory.slots.Count; i++)
        {
            var slot = playerInventory.inventory.slots[i];
            if (slot != null && slot.item == currentWeaponItem)
            {
                return i;
            }
        }
        return -1;
    }

    // public void UseItem(Item item)
    // {
    //     if(item.itemType == ItemType.Potion)
    //     {
    //         PotionItem potion = (PotionItem)item;
    //         if(potion.potionType == PotionType.Health)
    //         {
    //             health += potion.restoreAmount;
    //             Debug.Log("Health: " + health);
    //         } else if(potion.potionType == PotionType.Mana)
    //         {
    //             mana += potion.restoreAmount;
    //         } else if(potion.potionType == PotionType.Stamina)
    //         {
    //             stamina += potion.restoreAmount;
    //         }
    //         playerInventory.Remove(item, 1);
    //     }
    // }

    public Collider GetCurrentWeaponCollider()
    {
        if (currentWeaponObject == null)
            return null;

        return currentWeaponObject.GetComponentInChildren<Collider>(true);
    }

    public string GetCurrentWeaponID()
    {
        if (currentWeaponItem == null)
            return "1";

        return currentWeaponItem.itemId;
    }



}
