using UnityEditor.UIElements;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int health = 100;
    public int stamina = 100;
    public int mana = 100;
    public int leaves = 10;
    [SerializeField] private float speed = 5.0f;
    [SerializeField] private GameObject weapon = null;

    public void UseItem(Item item)
    {
        if(item.itemType == ItemType.Potion)
        {
            PotionItem potion = (PotionItem)item;
            if(potion.potionType == PotionType.Health)
            {
                health += potion.restoreAmount;
                Debug.Log("Health: " + health);
            } else if(potion.potionType == PotionType.Mana)
            {
                mana += potion.restoreAmount;
            } else if(potion.potionType == PotionType.Stamina)
            {
                stamina += potion.restoreAmount;
            }
        }
    }
}
