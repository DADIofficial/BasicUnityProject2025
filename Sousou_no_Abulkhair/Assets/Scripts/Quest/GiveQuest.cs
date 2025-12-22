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
        if (!playerInside) return;
        QuestController.instance.AcceptQuest(quest);
        questInfo.SetActive(false);
        Cursor.visible = false;
        questText.text = questGiver.inProgressText;
    }
}
