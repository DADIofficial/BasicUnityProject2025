using UnityEngine;

[CreateAssetMenu(fileName = "KeyItem", menuName = "Item/KeyItem")]
public class KeyItem : Item
{
    public string keyId;
    //public bool isConsumable = false;

    private void OnValidate()
    {
        if (string.IsNullOrEmpty(keyId))
            keyId = System.Guid.NewGuid().ToString();
    }
}
