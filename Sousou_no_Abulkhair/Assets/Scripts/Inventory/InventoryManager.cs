using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    [SerializeField] private GameObject InventoryMenu;
    [SerializeField] private GameObject QuestLogMenu;
    [SerializeField] private InventoryUI inventoryUI;
    [SerializeField] private Inventory inventory;
    [SerializeField] private PlayerInventory playerInventory;

    private bool menuActivated = false;
    private bool questLogActivated = false;

    private float _prevTimeScale = 1f;
    private float _prevFixedDeltaTime = 0.02f;

    private bool _prevCursorVisible;
    private CursorLockMode _prevCursorLock;

    private PlayerInteractor _playerInteractor;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            menuActivated = !menuActivated;
            if (menuActivated) OpenInventory();
            else CloseInventory();
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            questLogActivated = !questLogActivated;
            if (QuestLogMenu != null)
                QuestLogMenu.SetActive(questLogActivated);
        }
    }

    private void OpenInventory()
    {
        if (InventoryMenu != null)
            InventoryMenu.SetActive(true);

        _playerInteractor = GetPlayerInteractor();

        if (_playerInteractor != null && _playerInteractor.IsInteracting)
            _playerInteractor.EndInteraction();

        if (_playerInteractor != null)
        {
            _playerInteractor.SetInputBlocked(true);
            _playerInteractor.PushExternalLock();
        }

        _prevCursorVisible = Cursor.visible;
        _prevCursorLock = Cursor.lockState;

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        _prevTimeScale = Time.timeScale;
        _prevFixedDeltaTime = Time.fixedDeltaTime;

        Time.timeScale = 0f;
        Time.fixedDeltaTime = 0f;
    }

    private void CloseInventory()
    {
        if (InventoryMenu != null)
            InventoryMenu.SetActive(false);

        Time.timeScale = _prevTimeScale;
        Time.fixedDeltaTime = _prevFixedDeltaTime;

        Cursor.visible = _prevCursorVisible;
        Cursor.lockState = _prevCursorLock;

        if (_playerInteractor != null)
        {
            _playerInteractor.PopExternalLock();
            _playerInteractor.SetInputBlocked(false);
        }

        _playerInteractor = null;
    }

    private PlayerInteractor GetPlayerInteractor()
    {
        if (PlayerInteractor.Instance != null)
            return PlayerInteractor.Instance;

        return FindFirstObjectByType<PlayerInteractor>(FindObjectsInactive.Include);
    }

    public void AddItem(string itemName, Sprite itemIcon, string itemDescription)
    {
        inventoryUI.RefreshUI();
    }

    public void RemoveItemById(string itemId, int amount = 1)
    {
        inventory.RemoveItemById(itemId, amount);
        inventoryUI.RefreshUI();
    }

    public Dictionary<string, int> GetItemCounts()
    {
        return inventory.GetItemCounts();
    }
}
