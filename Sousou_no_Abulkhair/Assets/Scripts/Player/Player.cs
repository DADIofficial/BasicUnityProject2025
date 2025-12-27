using UnityEditor.UIElements;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int health = 100;
    public int stamina = 100;
    public int mana = 100;
    public int leaves = 10;
    [SerializeField] private float speed = 5.0f;
    [SerializeField] private PlayerInventory playerInventory;
    [Header("Weapon")]
    public Transform weaponSocket;      // куда крепим
    public GameObject weaponPrefab;     // текущее оружие

    private GameObject currentWeapon;

    void Start()
    {
        EquipWeapon(weaponPrefab);
    }

    public void EquipWeapon(GameObject newWeapon)
    {
        // удалить старое оружие
        if (currentWeapon != null)
            Destroy(currentWeapon);

        if (newWeapon == null) return;

        // создать новое
        currentWeapon = Instantiate(newWeapon, weaponSocket);
        currentWeapon.transform.localPosition = Vector3.zero;
        currentWeapon.transform.localRotation = Quaternion.identity;
    }

    public void ChangeWeapon(GameObject newWeapon)
    {
        weaponPrefab = newWeapon;
        EquipWeapon(newWeapon);
    }

    public void UseItem(Item item)
    {
        if(item.itemType == ItemType.Potion)
        {
            PotionItem potion = (PotionItem)item;
            if(potion.potionType == PotionType.Health)
            {
                health += potion.restoreAmount;
                Debug.Log("Health: " + health);
            } else if(potion.potionType == PotionType.Mana)
            {
                mana += potion.restoreAmount;
            } else if(potion.potionType == PotionType.Stamina)
            {
                stamina += potion.restoreAmount;
            }
        }
        playerInventory.Remove(item, 1);
    }
}
