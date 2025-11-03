using UnityEngine;

public class Vendor : MonoBehaviour
{

    [SerializeField] private GameObject ShopMenu;
    [SerializeField] private GameObject InventoryMenu;
    private bool menuActivated = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.gameObject.tag);
        if (other.gameObject.CompareTag("Player"))
        {
            menuActivated = true;
            ShopMenu.SetActive(menuActivated);
            InventoryMenu.SetActive(true);
        }
    }
  
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            menuActivated = false;
            ShopMenu.SetActive(menuActivated);
            InventoryMenu.SetActive(false);
        }
    }
}
