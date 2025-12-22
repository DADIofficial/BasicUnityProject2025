using NUnit.Framework;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static Quest;
using static QuestController;

public class QuestUI : MonoBehaviour
{
    public Transform questListContent;
    public GameObject questEntryPrefab;
    public GameObject objectiveTextPrefab;

    private List<QuestProgress> activateQuests;
    void Start()
    {
        activateQuests = QuestController.instance.activateQuests;
        UpdateQuestUI();
    }

    public void UpdateQuestUI()
    {
        foreach(Transform child in questListContent)
        {
            Destroy(child.gameObject);
        }

        foreach(var quest in activateQuests)
        {
            GameObject entry = Instantiate(questEntryPrefab, questListContent);
            TMP_Text questNameText = entry.transform.Find("QuestNameText").GetComponent<TMP_Text>();
            Transform objectiveList = entry.transform.Find("ObjectiveList");

            questNameText.text = quest.quest.questName;

            foreach(var objective in quest.objectives)
            {
                Debug.Log($"objective: {objective}");
                GameObject objTextGO = Instantiate(objectiveTextPrefab, objectiveList);
                TMP_Text objText = objTextGO.GetComponent<TMP_Text>();
                objText.text = $"{objective.description}: {objective.currentAmount}/{objective.requiredAmount}";
            }
        }
    }
}
