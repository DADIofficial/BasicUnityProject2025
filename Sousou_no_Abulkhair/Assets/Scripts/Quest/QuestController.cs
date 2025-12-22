using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using static Quest;

public class QuestController : MonoBehaviour
{
    public static QuestController instance;
    // public Quest testQuest;
    public List<QuestProgress> activateQuests = new();
    private QuestUI questUI;


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
        if (IsQuestActive(quest.questId)) return;
        activateQuests.Add(new QuestProgress(quest));
        questUI.UpdateQuestUI();
    }

    public bool IsQuestActive(string questId)
    {
        return activateQuests.Exists(q => q.QuestId == questId);
    }
}
