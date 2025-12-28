using UnityEngine;
using System.Collections.Generic;

public static class ItemDB
{
    private static ItemDatabase database;

    public static void Register(ItemDatabase db)
    {
        if (db == null)
        {
            UnityEngine.Debug.LogError("[ItemDB] Database is null");
            return;
        }

        database = db;
        database.Init();
    }

    public static Item Get(string id)
    {
        if (database == null)
        {
            UnityEngine.Debug.LogError("[ItemDB] Database not registered");
            return null;
        }

        return database.Get(id);
    }
}

