using UnityEngine;
using System.Collections;

public class Door : MonoBehaviour
{
    [Header("Permission")]
    [SerializeField] private bool isOpen = false;
    [SerializeField] private string requiredKeyId;

    [Header("Door movement")]
    [SerializeField] private Transform doorPivot;
    [SerializeField] private float openAngle = 90f;
    [SerializeField] private float speed = 2f;

    private bool openVisual = false;
    private Coroutine moveCoroutine;
    private Coroutine closeDelayCoroutine;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        CancelCloseDelay();

        if (isOpen)
        {
            OpenVisual();
        }
    }


    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        closeDelayCoroutine = StartCoroutine(CloseWithDelay());
    }

    public bool TryOpen(PlayerInventory inventory)
    {
        if (isOpen)
        {
            OpenVisual();
            return true;
        }

        if (inventory == null)
            return false;

        if (!string.IsNullOrEmpty(requiredKeyId))
        {
            if (!inventory.HasKey(requiredKeyId, out InventoryInstance slot))
                return false;

            int slotIndex = inventory.inventory.slots.IndexOf(slot);
            if (slotIndex == -1)
                return false;

            // inventory.RemoveBySlotIndex(slotIndex);
        }

        isOpen = true;
        OpenVisual();
        return true;
    }



    private void OpenVisual()
    {
        if (openVisual)
            return;

        openVisual = true;
        StartMove(openAngle);
    }

    private void CloseVisual()
    {
        if (!openVisual)
            return;

        openVisual = false;
        StartMove(0f);
    }

    private IEnumerator CloseWithDelay()
    {
        yield return new WaitForSeconds(2f);
        CloseVisual();
        closeDelayCoroutine = null;
    }

    private void CancelCloseDelay()
    {
        if (closeDelayCoroutine != null)
        {
            StopCoroutine(closeDelayCoroutine);
            closeDelayCoroutine = null;
        }
    }

    private void StartMove(float targetAngle)
    {
        if (moveCoroutine != null)
            StopCoroutine(moveCoroutine);

        moveCoroutine = StartCoroutine(RotateRoutine(targetAngle));
    }

    private IEnumerator RotateRoutine(float targetAngle)
    {
        Quaternion startRot = doorPivot.localRotation;
        Quaternion targetRot = Quaternion.Euler(0, 0, targetAngle);

        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime * speed;
            doorPivot.localRotation = Quaternion.Slerp(startRot, targetRot, t);
            yield return null;
        }

        doorPivot.localRotation = targetRot;
        moveCoroutine = null;
    }
}
