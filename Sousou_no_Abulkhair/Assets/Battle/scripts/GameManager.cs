
using UnityEngine;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public SaveData saveData = new SaveData();

    // ID врага, с которым начался бой
    public string currentEnemyID;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public bool IsEnemyKilled(string id)
    {
        return saveData.killedEnemies.Contains(id);
    }

    public void KillEnemy(string id)
    {
        if (!saveData.killedEnemies.Contains(id))
            saveData.killedEnemies.Add(id);
    }
}

