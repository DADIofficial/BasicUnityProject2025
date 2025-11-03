using System.Collections.ObjectModel;
using UnityEditor.Rendering.Universal;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    public PlayerInventory playerInventory;
    public Transform slotParent;
    private Transform[] waypoints;

    void Start()
    {
        RefreshUI();
    }

    public void RefreshUI()
    {
        slotParent = GetComponent<Transform>();
        waypoints = new Transform[transform.childCount];
        for (int i = 0; i < transform.childCount; i++)
        {
            waypoints[i] = slotParent.GetChild(i);
        }
        Debug.Log(playerInventory);
        int j = 0; 
        foreach (InventoryInstance i in playerInventory.inventory.slots) {
            if(i == null)
            {
                Debug.Log("No slots in inventory");
                return;
            }
            if(i.item == null)
            {
                Debug.Log("Empty slot found");
                j++;
                continue;
            }
            waypoints[j].GetComponent<Image>().sprite = i.item.icon;
            j++;
        }
    }
}
