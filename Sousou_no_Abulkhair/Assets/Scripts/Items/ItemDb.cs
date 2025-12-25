using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemDb", menuName = "Scriptable Objects/ItemDb")]
public class ItemDb : ScriptableObject
{
    public List<Item> items;

    private Dictionary<string, Item> itemById;

    public void Init()
    {
        itemById = new Dictionary<string, Item>();

        foreach (var item in items)
        {
            if (item == null || string.IsNullOrEmpty(item.itemId))
                continue;

            if (!itemById.ContainsKey(item.itemId))
                itemById.Add(item.itemId, item);
            else
                Debug.LogError($"Duplicate Item ID: {item.itemId}");
        }
    }

    public Item GetItemById(string id)
    {
        if (itemById == null)
            Init();

        itemById.TryGetValue(id, out Item item);
        return item;
    }
}
