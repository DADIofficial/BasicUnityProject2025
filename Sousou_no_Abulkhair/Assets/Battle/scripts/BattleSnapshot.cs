using UnityEngine;
using System.Collections.Generic;


[System.Serializable]
public class BattleSnapshot
{
    public int health;
    public int mana;
    public int stamina;

    public int weaponID;
    public int magicID;

    public float enemyHP;
    public int enemyAttack;

    public List<InventorySlotSave> inventorySlots;
}

[System.Serializable]
public class InventorySlotSave
{
    public string itemId;
}

