using UnityEngine;
using UnityEngine.UI;

public class InventoryButton : MonoBehaviour
{
    public int index;
    public Image icon;

    public InventoryUI inventoryUI;

    public void OnClick()
    {
        Debug.Log("Surprise, Motherfucker! You clicked on slot: " + index);
        inventoryUI.OnSlotClicked(index);
    }
}
