using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Quest", menuName = "Scriptable Objects/Quest")]
public class Quest : ScriptableObject
{
    public string questId;
    public string questName;
    public string description;
    public List<QuestObjective> objectives;

    [Header("Quest Giver")]
    public string giverId;         
    public bool turnInAtGiver = true;

    private void OnValidate()
    {
        if (string.IsNullOrEmpty(questId))
        {
            questId = System.Guid.NewGuid().ToString();
        }
    }

    [System.Serializable]
    public class QuestObjective
    {
        public string objectiveId;  // enemy, item, trigger, NPC id
        public string description;
        public ObjectiveType type;
        public int requiredAmount;
        public int currentAmount;

        public bool IsCompleted()
        {
            return currentAmount >= requiredAmount;
        }
    }

    public enum ObjectiveType
    {
        Kill,
        Collect,
        TalkTo,
        ReachLocation,
        Custom
    }

    [System.Serializable]
    public class QuestProgress
    {
        public Quest quest;
        public List<QuestObjective> objectives;

        public QuestProgress(Quest quest)
        {
            this.quest = quest;
            objectives = new List<QuestObjective>();
            foreach (var obj in quest.objectives)
            {
                objectives.Add(new QuestObjective
                {
                    objectiveId = obj.objectiveId,
                    description = obj.description,
                    type = obj.type,
                    requiredAmount = obj.requiredAmount,
                    currentAmount = 0
                });
            }
        }

        public bool IsQuestCompleted()
        {
            foreach (var obj in objectives)
            {
                if (!obj.IsCompleted())
                {
                    return false;
                }
            }
            return true;
        }
        
        public string QuestId => quest.questId;
    }

    [System.Serializable]
    public class QuestReward
    {
        public int leaves;
        public List<Item> items;
    }
}
