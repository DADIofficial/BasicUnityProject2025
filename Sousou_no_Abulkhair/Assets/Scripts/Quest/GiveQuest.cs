using TMPro;
using UnityEngine;

public class GiveQuest : MonoBehaviour
{
    public Quest quest;
    public QuestGiver questGiver;

    [SerializeField] private GameObject questInfo;
    [SerializeField] private TMP_Text questText;

    private void Start()
    {
        if (questInfo != null) questInfo.SetActive(false);
        if (questText != null) questText.text = questGiver.offerText;
    }

    
    public void OpenQuestUI()
    {
        if (questInfo != null) questInfo.SetActive(true);

        UpdateQuestText();


        QuestUI.instance.Show(this);
    }

    
    public void CloseQuestUI()
    {
        if (questInfo != null) questInfo.SetActive(false);
    }

    public void AcceptQuest()
    {
        if (QuestController.instance.IsQuestHandedIn(quest.questId)) return;

        QuestController.instance.AcceptQuest(quest);

        if (questInfo != null) questInfo.SetActive(false);
        if (questText != null) questText.text = questGiver.inProgressText;

        
        if (PlayerInteractor.Instance != null)
            PlayerInteractor.Instance.EndInteraction();
    }

    public void HandInQuest()
    {
        if (!QuestController.instance.IsQuestCompleted(quest.questId)) return;

        if (RewardsController.instance.GiveQuestReward(quest))
            QuestController.instance.HandInQuest(quest.questId);
        else
        {
            Debug.Log("Inventory is full, error quest hand in");
            return;
        }

        UpdateQuestText();
    }

    private void UpdateQuestText()
    {
        if (questText == null) return;

        if (QuestController.instance.IsQuestHandedIn(quest.questId))
            questText.text = questGiver.completedText;
        else if (QuestController.instance.IsQuestCompleted(quest.questId))
            questText.text = questGiver.readyToTurnInText;
        else if (QuestController.instance.IsQuestActive(quest.questId))
            questText.text = questGiver.inProgressText;
        else
            questText.text = questGiver.offerText;
    }
}
