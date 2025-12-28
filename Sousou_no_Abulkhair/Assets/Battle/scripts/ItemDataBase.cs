using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "Database/Item Database")]
public class ItemDatabase : ScriptableObject
{
    public List<Item> items = new();

    private Dictionary<string, Item> lookup;

    public void Init()
    {
        lookup = new Dictionary<string, Item>();

        foreach (var item in items)
        {
            if (item == null) continue;

            if (lookup.ContainsKey(item.itemId))
            {
                Debug.LogError($"[ItemDatabase] Duplicate itemId: {item.itemId}");
                continue;
            }

            lookup[item.itemId] = item;
        }

        Debug.Log($"[ItemDatabase] Initialized with {lookup.Count} items");
    }

    public Item Get(string id)
    {
        if (lookup == null)
        {
            Debug.LogError("[ItemDatabase] Not initialized");
            return null;
        }

        if (!lookup.TryGetValue(id, out var item))
        {
            Debug.LogError($"[ItemDatabase] Item '{id}' not found");
            return null;
        }

        return item;
    }
}
