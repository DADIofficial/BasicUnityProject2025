
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


    // ID врага, с которым начался бой
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

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "MainLevel")
        {
            chests = new List<ChestRuntime>(
                Object.FindObjectsByType<ChestRuntime>(
                    FindObjectsInactive.Include,
                    FindObjectsSortMode.None
                )
            );

            foreach (var chest in chests)
            {
                chest.ReturnToStart(); // 🔥 СБРОС
            }



            Debug.Log($"[GameManager] Found {chests.Count} chests in scene");

            RestoreChests();              // 1️⃣ сначала восстановление
            SpawnChestFromLastEnemy();    // 2️⃣ потом спавн нового
        }
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



    // public void RestoreChest(ChestRuntime chest, ChestSaveData data)
    // {
    //     chest.transform.position = data.position;
    //     chest.gameObject.SetActive(data.isActive);

    //     chest.inventory.Clear(); // твой метод очистки

    //     foreach (var itemData in data.items)
    //     {
    //         Item item = ItemDB.Get(itemData.itemId);
    //         chest.inventory.AddItem(new InventoryInstance(item, itemData.count));
    //     }
    // }

    public void RestoreChests()
    {
        foreach (var data in saveData.chests)
        {
            // сундук уже залутан — не восстанавливаем
            if (!data.isActive)
                continue;

            // защита от мусорных данных
            if (data.position == Vector3.zero)
            {
                Debug.LogWarning("[RestoreChests] Skipped chest with ZERO position");
                continue;
            }

            var chest = chests.Find(c => c.chestIndex == data.chestIndex);
            if (chest == null)
                continue;

            chest.TeleportTo(data.position);
            chest.inventory.SetFromSave(data.items);
        }
    }



    public void OnChestLooted(int chestIndex)
    {
        // помечаем в сейве как неактивный
        var data = saveData.chests.Find(c => c.chestIndex == chestIndex);
        if (data != null)
            data.isActive = false;

        // освобождаем сундук в пуле
        var chest = chests.Find(c => c.chestIndex == chestIndex);
        if (chest != null)
            chest.ReturnToStart();
    }



    public void SpawnChestFromLastEnemy()
    {
        var save = saveData;
        Debug.Log($"[SpawnChest] id={save.lastKilledEnemyId} pos={save.lastKilledEnemyPosition}");


        // нет убитого врага — нечего спавнить
        if (string.IsNullOrEmpty(save.lastKilledEnemyId))
            return;

        // позиция некорректна — сбрасываем
        if (save.lastKilledEnemyPosition == Vector3.zero)
        {
            Debug.LogError("[SpawnChest] lastKilledEnemyPosition is ZERO");
            save.lastKilledEnemyId = null;
            return;
        }

        // уже есть активный сундук на этой позиции
        if (save.chests.Exists(c =>
            c.isActive &&
            Vector3.Distance(c.position, save.lastKilledEnemyPosition) < 0.1f))
        {
            save.lastKilledEnemyId = null;
            save.lastKilledEnemyPosition = Vector3.zero;
            return;
        }

        // ищем свободный сундук в пуле
        var freeChests = chests.FindAll(c => c != null && !c.IsInUse);

        if (freeChests.Count == 0)
        {
            Debug.LogWarning("[SpawnChest] No free chest available");
            return;
        }

        // спавним сундук
        var chest = freeChests[Random.Range(0, freeChests.Count)];
        chest.TeleportTo(save.lastKilledEnemyPosition);

        // сохраняем в сейв
        save.chests.Add(new ChestSaveData
        {
            chestIndex = chest.chestIndex,
            position = save.lastKilledEnemyPosition,
            isActive = true,
            items = new List<ChestItemSaveData>()
        });

        // 🔑 ОБЯЗАТЕЛЬНО чистим
        save.lastKilledEnemyId = null;
        save.lastKilledEnemyPosition = Vector3.zero;
    }






}

