using UnityEngine;

[CreateAssetMenu(fileName = "QuestGiver", menuName = "Quest/QuestGiver")]
public class QuestGiver : ScriptableObject
{
    public string giverId;
    public string giverName;
    public string questId; 

    [TextArea] public string offerText;
    [TextArea] public string inProgressText;
    [TextArea] public string readyToTurnInText;
    [TextArea] public string completedText;
}
