using NUnit.Framework;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEditor;
using UnityEngine;
using static Quest;

public class QuestController : MonoBehaviour
{
    public static QuestController instance;
    // public Quest testQuest;
    public List<QuestProgress> activateQuests = new();
    [SerializeField] private InventoryManager inventory;
    private QuestUI questUI;

    public List<string> handinQuestIDs = new();

    private void Start()
    {
        // activateQuests.Add(new QuestProgress(testQuest));
    }

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        questUI = Object.FindFirstObjectByType<QuestUI>();
    }

    public void AcceptQuest(Quest quest)
    {
        if (IsQuestActive(quest.questId) || IsQuestHandedIn(quest.questId)) return;
        activateQuests.Add(new QuestProgress(quest));
        CheckInventoryForQuests();
        questUI.UpdateQuestUI();
    }

    public bool IsQuestActive(string questId)
    {
        return activateQuests.Exists(q => q.QuestId == questId);
    }

    public void CheckInventoryForQuests()
    {
        Dictionary<string, int> itemCounts = inventory.GetItemCounts();
        foreach(QuestProgress quest in activateQuests)
        {
            foreach(QuestObjective questObjective in quest.objectives)
            {
                if (questObjective.type != ObjectiveType.Collect) continue;

                int newAmount = itemCounts.TryGetValue(questObjective.objectiveId, out int count) ? Mathf.Min(count, questObjective.requiredAmount) : 0;
                
                if(questObjective.currentAmount != newAmount)
                {
                    questObjective.currentAmount = newAmount;
                }
            }
        }
        questUI.UpdateQuestUI();
    }

    public bool RemoveRequiredItemsFromInventory(string questId)
    {
        QuestProgress quest = activateQuests.Find(q => q.QuestId == questId);
        if (quest == null) return false;

        Dictionary<int, int> requiredItems = new();
        foreach(QuestObjective objective in quest.objectives)
        {
            if(objective.type == ObjectiveType.Collect && int.TryParse(objective.objectiveId, out int itemID))
            {
                requiredItems[itemID] = objective.requiredAmount;
            }
        }
        Dictionary<string, int> itemCounts = inventory.GetItemCounts();
        foreach(var item in requiredItems)
        {
            if(itemCounts.GetValueOrDefault(item.Key.ToString()) < item.Value)
            {
                return false;
            }
        }
        
        foreach(var itemRequirement in requiredItems)
        {
            inventory.RemoveItemById(itemRequirement.Key.ToString(), itemRequirement.Value);
        }

        return true;
    }

    public bool IsQuestCompleted(string questId)
    {
        QuestProgress quest = activateQuests.Find(q => q.QuestId == questId);
        return quest != null && quest.objectives.TrueForAll(o => o.IsCompleted());
    }

    public void HandInQuest(string questId)
    {
        if(!RemoveRequiredItemsFromInventory(questId))
        {
            return;
        }

        QuestProgress quest = activateQuests.Find(q => q.QuestId == questId);
        if(quest != null)
        {
            handinQuestIDs.Add(questId);
            activateQuests.Remove(quest);
            questUI.UpdateQuestUI();
        }
    }

    public bool IsQuestHandedIn(string questId)
    {
        return handinQuestIDs.Contains(questId);
    }
}
