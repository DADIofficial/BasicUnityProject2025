using System.ComponentModel;
using UnityEngine;

public enum ItemType
{
    Key, 
    Potion, 
    Weapon
}

[CreateAssetMenu(fileName = "Item", menuName = "Scriptable Objects/Item")]
public class Item : ScriptableObject
{
    public string itemId;
    public string itemName;
    public int price;
    public Sprite icon;
    public string description;
    public ItemType itemType;

    public virtual void OnRightClick(Player player)
    {
        
    }

}
