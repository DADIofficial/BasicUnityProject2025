using UnityEngine;

[CreateAssetMenu(fileName = "KeyItem", menuName = "Item/KeyItem")]
public class KeyItem : Item
{
    public string doorID;

    private void OnEnable()
    {
        itemType = ItemType.Key;
    }
}
