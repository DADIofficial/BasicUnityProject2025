using UnityEngine;
using static Quest;

public class RewardsController : MonoBehaviour
{
    public static RewardsController instance;

    [SerializeField] private Player player;
    [SerializeField] private PlayerInventory inventory;

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    public bool GiveQuestReward(Quest quest)
    {
        if (quest == null || quest.questRewards == null)
        {
            Debug.Log("RewardsController: Quest or rewards are null");
            return false;
        }

        // 1. Считаем, сколько слотов нужно под предметы
        int requiredSlots = 0;

        foreach (var reward in quest.questRewards)
        {
            if (reward.type == RewardType.Item && reward.item != null)
            {
                requiredSlots += reward.amount;
            }
        }

        // 2. Проверяем, хватает ли места
        if (inventory.GetAmountOfFreeSlots() < requiredSlots)
        {
            Debug.Log("RewardsController: Not enough inventory space. Reward cancelled.");
            return false;
        }

        // 3. Если места хватает — выдаём ВСЁ
        foreach (var reward in quest.questRewards)
        {
            switch (reward.type)
            {
                case RewardType.Item:
                    GiveItemReward(reward.item, reward.amount);
                    break;

                case RewardType.Leaves:
                    GiveCurrencyReward(reward.amount);
                    break;

                case RewardType.Custom:
                    // кастомные награды
                    break;
            }
        }

        return true;
    }

    public bool GiveItemReward(Item item, int amount)
    {
        InventoryInstance inventoryInstance = new InventoryInstance(item, 1);
        if (inventory.GetAmountOfFreeSlots() < amount)
        {
            return false;
        }
        for (int i = 0; i < amount; i++)
        {
            inventory.Add(inventoryInstance);
        }
        return true;
    }

    public bool GiveCurrencyReward(int amount)
    {
        CurrencyController.Instance.AddLeaves(amount);
        return true;
    }
}
