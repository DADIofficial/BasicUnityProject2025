using UnityEngine;


public enum PotionType
{
    Health,
    Mana,
    Stamina
}

[CreateAssetMenu(fileName = "PotionType", menuName = "Item/Potion")]
public class PotionItem : Item
{
    public PotionType potionType;
    public int restoreAmount;

    private void OnEnable()
    {
        itemType = ItemType.Potion;
    }
}
