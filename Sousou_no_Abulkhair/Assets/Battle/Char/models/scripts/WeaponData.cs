using UnityEngine;

[CreateAssetMenu(fileName = "WeaponData", menuName = "Scriptable Objects/WeaponData")]
public class WeaponData : ScriptableObject
{
    public WData[] weapons;
    public MData[] magics;

    public WData GetWeaponByID(int id)
    {
        for (int i = 0; i < weapons.Length; i++)
        {
            if (weapons[i].WID == id)
                return weapons[i];
        }

        Debug.LogWarning("Weapon ID not found: " + id);
        return null;
    }


    public MData GetMagicByID(int id)
    {
        for (int i = 0; i < magics.Length; i++)
        {
            if (magics[i].MID == id)
                return magics[i];
        }

        Debug.LogWarning("Magic ID not found: " + id);
        return null;
    }
}
