using UnityEngine;

[CreateAssetMenu(fileName = "WeaponItem", menuName = "Item/WeaponItem")]
public class WeaponItem : Item
{
    public string weaponId;
    public GameObject weaponPrefab;
    //public bool isConsumable = false;

    private void OnValidate()
    {
        if (string.IsNullOrEmpty(weaponId))
            weaponId = System.Guid.NewGuid().ToString();
    }

    public override void OnRightClick(Player player)
    {
        Player.instance.EquipWeapon(weaponPrefab);
    }
}
