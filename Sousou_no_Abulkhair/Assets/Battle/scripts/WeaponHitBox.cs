using UnityEngine;

public class WeaponHitBox : MonoBehaviour
{
    private Collider col;

    private void Awake()
    {
        col = GetComponent<Collider>();
        col.enabled = false; 
    }

    public void EnableHitbox()
    {
        col.enabled = true;
    }

    public void DisableHitbox()
    {
        col.enabled = false;
    }
}
