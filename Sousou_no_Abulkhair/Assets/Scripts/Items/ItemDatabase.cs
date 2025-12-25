using UnityEngine;

public class ItemDatabase : MonoBehaviour
{
    public static ItemDatabase instance;

    [SerializeField] private ItemDb database;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        database.Init();
    }

    public Item GetItem(string id)
    {
        return database.GetItemById(id);
    }
}
