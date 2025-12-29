using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class InventoryButton : MonoBehaviour, IPointerClickHandler
{
    public int index;
    public Image icon;

    public InventoryUI inventoryUI;

    /*public void OnClick()
    {
        //Debug.Log("Surprise, Motherfucker! You clicked on slot: " + index);
        inventoryUI.OnSlotClicked(index);
    }*/

    public void OnPointerClick(PointerEventData eventData)
    {
        inventoryUI.OnSlotClicked(index, eventData.button);
    }
}
