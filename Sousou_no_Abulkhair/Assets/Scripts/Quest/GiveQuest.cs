using TMPro;
using UnityEngine;

public class GiveQuest : MonoBehaviour
{
    public Quest quest; 
    public QuestGiver questGiver;
    [SerializeField] private GameObject questInfo;
    [SerializeField] private TMP_Text questText;

    private bool playerInside;

    private void Start()
    {
        questInfo.SetActive(false);
        playerInside = false;
        questText.text = questGiver.offerText;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        Cursor.visible = true;
        playerInside = true;

        QuestUI.instance.Show(this);

        questInfo.SetActive(true);
        questText.text = questGiver.offerText; 
        UpdateQuestText();
    }


    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        Cursor.visible = false;
        UpdateQuestText();
        questInfo.SetActive(false);
        playerInside = false;
    }

    public void AcceptQuest()
    {
        Debug.Log($"Accept pressed on NPC: {gameObject.name}, quest: {quest.questId}");
        if (!playerInside || QuestController.instance.IsQuestHandedIn(quest.questId)) return;
        QuestController.instance.AcceptQuest(quest);
        questInfo.SetActive(false);
        Cursor.visible = false;
        questText.text = questGiver.inProgressText;
    }

    private void UpdateQuestText()
    {
        if (QuestController.instance.IsQuestHandedIn(quest.questId))
        {
            questText.text = questGiver.completedText;
        }
        else if (QuestController.instance.IsQuestCompleted(quest.questId))
        {
            questText.text = questGiver.readyToTurnInText;
        }
        else if (QuestController.instance.IsQuestActive(quest.questId))
        {
            questText.text = questGiver.inProgressText;
        }
        /*else if (QuestController.instance.IsQuestHandedIn(quest.questId))
        {
            questText.text = questGiver.completedText;
        }*/
    }

    public void HandInQuest()
    {
        if (!playerInside) return;

        if (!QuestController.instance.IsQuestCompleted(quest.questId))
            return;

        //Debug.Log(quest);
        if (RewardsController.instance.GiveQuestReward(quest))
            QuestController.instance.HandInQuest(quest.questId);
        else
        {
            Debug.Log("Inventory is full, error quest hand in");
            return;
        }
        UpdateQuestText();
    }
}
