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
        questInfo.SetActive(true);
        playerInside = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        Cursor.visible = false;
        questInfo.SetActive(false);
        playerInside = false;
    }

    public void AcceptQuest()
    {
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

        QuestController.instance.HandInQuest(quest.questId);
        UpdateQuestText();
    }
}
