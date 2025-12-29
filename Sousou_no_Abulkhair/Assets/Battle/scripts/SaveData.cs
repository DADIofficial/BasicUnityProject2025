using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class SaveData
{
    public Vector3 playerPosition;
    public Quaternion playerRotation;
    public bool hasSavedPosition = false;

    public int health;
    public int mana;
    public int stamina;

    public int weaponID;
    public int magicID;

    public List<string> killedEnemies = new List<string>();

    public string lastKilledEnemyId;

    public Vector3 battleEntryEnemyPosition;
    public Vector3 lastKilledEnemyPosition;

    public List<ChestSaveData> chests = new();

    public float enemyHP;
    public int enemyAttack;
    
}

[System.Serializable]
public class ChestSaveData
{
    public int chestIndex;
    public Vector3 position;
    public bool isActive;
    public List<ChestItemSaveData> items = new();
}

[System.Serializable]
public class ChestItemSaveData
{
    public string itemId;
    public int count;
}

