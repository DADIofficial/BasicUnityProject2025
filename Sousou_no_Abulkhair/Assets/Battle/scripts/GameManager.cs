
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField] private ItemDatabase itemDatabase;

    public Player_battle BattlePlayer { get; private set; }

    public SaveData saveData = new SaveData();
    
    private List<ChestRuntime> chests = new();

    private BattleSnapshot battleSnapshot;



    public string currentEnemyID;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            ItemDB.Register(itemDatabase);
        }
        else
        {
            Destroy(gameObject);
        }
    }


    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    public void OnChestLooted(int chestIndex)
    {

        var data = saveData.chests.Find(c => c.chestIndex == chestIndex);
        if (data != null)
            data.isActive = false;


        var chest = chests.Find(c => c.chestIndex == chestIndex);
        if (chest != null)
        {
            chest.inventory.Clear();
            chest.RestoreInitialInventory();
            chest.ReturnToStart(); 
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name != "MainLevel")
            return;

        chests = new List<ChestRuntime>(
            Object.FindObjectsByType<ChestRuntime>(
                FindObjectsInactive.Include,
                FindObjectsSortMode.None
            )
        );


        foreach (var chest in chests)
        {
            chest.ReturnToStart();
        }

  
        saveData.chests.RemoveAll(c => !c.isActive);

        Debug.Log($"[GameManager] Found {chests.Count} chests in scene");

        RestoreChests();          
        SpawnChestFromLastEnemy(); 
    }





    

    public void RegisterBattlePlayer(Player_battle player)
    {
        BattlePlayer = player;
    }


    public bool IsEnemyKilled(string id)
    {
        return saveData.killedEnemies.Contains(id);
    }

    public void KillEnemy(string id)
    {
        if (string.IsNullOrEmpty(id))
        {
            Debug.LogError("[KillEnemy] EMPTY ID");
            return;
        }

        Debug.Log($"[KillEnemy] {id}");

        if (!saveData.killedEnemies.Contains(id))
            saveData.killedEnemies.Add(id);
    }



    public void UsePotion(PotionItem potion)
    {
        switch (potion.potionType)
        {
            case PotionType.Health:
                RestoreHealth(potion.restoreAmount);
                BattlePlayer.OnPotionUsed(potion);
                break;

            case PotionType.Mana:
                RestoreMana(potion.restoreAmount);
                BattlePlayer.OnPotionUsed(potion);
                break;

            case PotionType.Stamina:
                RestoreStamina(potion.restoreAmount);
                BattlePlayer.OnPotionUsed(potion);
                break;
        }
    }



    public void RestoreHealth(int amount)
    {
        Debug.Log("health");
        saveData.health = Mathf.Clamp(saveData.health + amount, 0, 100);
    }

    public void RestoreMana(int amount)
    {
        Debug.Log("mana");
        saveData.mana = Mathf.Clamp(saveData.mana + amount, 0, 100);
    }

    public void RestoreStamina(int amount)
    {
        Debug.Log("stamina");
        saveData.stamina = Mathf.Clamp(saveData.stamina + amount, 0, 100);
    }



    public void ChangeMagic(int ID)
    {
        saveData.magicID = ID;
        BattlePlayer.SetMagic();
        Debug.Log($"[GameManager] Magic changed to ID {ID}");

    }






    public void SpawnChestFromLastEnemy()
    {
        var save = saveData;
        Debug.Log($"[SpawnChest] id={save.lastKilledEnemyId} pos={save.lastKilledEnemyPosition}");

        if (string.IsNullOrEmpty(save.lastKilledEnemyId))
            return;

        if (save.lastKilledEnemyPosition == Vector3.zero)
            return;

        // проверка: сундук уже есть
        if (save.chests.Exists(c =>
            c.isActive &&
            Vector3.Distance(c.position, save.lastKilledEnemyPosition) < 0.1f))
        {
            save.lastKilledEnemyId = null;
            save.lastKilledEnemyPosition = Vector3.zero;
            return;
        }

        var freeChests = chests.FindAll(c => !c.IsInUse);
        if (freeChests.Count == 0)
        {
            Debug.LogWarning("[SpawnChest] No free chest available");
            return;
        }

        var freeChest = freeChests[Random.Range(0, freeChests.Count)];

        // 🔑 восстановили стартовый лут
        freeChest.RestoreInitialInventory();
        freeChest.TeleportTo(save.lastKilledEnemyPosition);

        // 🔑 сохранили в SaveData
        var chestSave = new ChestSaveData
        {
            chestIndex = freeChest.chestIndex,
            position = save.lastKilledEnemyPosition,
            isActive = true
        };

        foreach (var pair in freeChest.inventory.GetItemCounts())
        {
            chestSave.items.Add(new ChestItemSaveData
            {
                itemId = pair.Key,
                count = pair.Value
            });
        }

        save.chests.Add(chestSave);

        save.lastKilledEnemyId = null;
        save.lastKilledEnemyPosition = Vector3.zero;
    }

    public void RestoreChests()
    {
        foreach (var data in saveData.chests)
        {
            if (!data.isActive)
                continue;

            var chest = chests.Find(c => c.chestIndex == data.chestIndex);
            if (chest == null)
                continue;

            chest.TeleportTo(data.position);
            chest.inventory.SetFromSave(data.items);
        }
    }



    public void SaveBattleSnapshot(PlayerInventory playerInventory)
    {
        var inv = playerInventory.inventory;

        battleSnapshot = new BattleSnapshot
        {
            health = saveData.health,
            mana = saveData.mana,
            stamina = saveData.stamina,

            weaponID = saveData.weaponID,
            magicID = saveData.magicID,

            enemyHP = saveData.enemyHP,
            enemyAttack = saveData.enemyAttack,

            inventorySlots = new List<InventorySlotSave>()
        };

        foreach (var slot in inv.slots)
        {
            if (slot == null || slot.item == null)
            {
                battleSnapshot.inventorySlots.Add(null);
                continue;
            }

            battleSnapshot.inventorySlots.Add(new InventorySlotSave
            {
                itemId = slot.item.itemId
            });
        }

        Debug.Log("[BattleSnapshot] Saved inventory slots = " + battleSnapshot.inventorySlots.Count);
    }


    public void RestoreBattleSnapshot(PlayerInventory playerInventory)
    {
        if (battleSnapshot == null)
        {
            Debug.LogError("[BattleSnapshot] No snapshot");
            return;
        }

        saveData.health = battleSnapshot.health;
        saveData.mana = battleSnapshot.mana;
        saveData.stamina = battleSnapshot.stamina;

        saveData.weaponID = battleSnapshot.weaponID;
        saveData.magicID = battleSnapshot.magicID;

        var inv = playerInventory.inventory;
        inv.Clear();

        for (int i = 0; i < battleSnapshot.inventorySlots.Count; i++)
        {
            var slotSave = battleSnapshot.inventorySlots[i];
            if (slotSave == null)
                continue;

            Item item = ItemDB.Get(slotSave.itemId);
            inv.slots[i] = new InventoryInstance(item, 1);
        }

        Debug.Log("[BattleSnapshot] Restored inventory");
    }








}

