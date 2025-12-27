using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class SaveData
{
    public Vector3 playerPosition;
    public Quaternion playerRotation;
    public bool hasSavedPosition = false;

    public int health;
    public int mana;
    public int stamina;

    public List<string> killedEnemies = new List<string>();
}
