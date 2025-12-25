using UnityEngine;

public class InventoryButton : MonoBehaviour
{
    public int index;

    public InventoryUI inventoryUI;

    public void OnClick()
    {
        //Debug.Log("Surprise, Motherfucker! You clicked on slot: " + index);
        inventoryUI.OnSlotClicked(index);
    }
}
