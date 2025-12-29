using UnityEngine;
using System.Collections;

public class dooropen : MonoBehaviour
{
    public Transform doorPivot;
    public float openAngle = -90f;
    public float speed = 2f;

    private bool open;
    private Coroutine closeCoroutine;

    void Update()
    {
        float targetAngle = open ? openAngle : 0f;
        Quaternion targetRot = Quaternion.Euler(0, 0, targetAngle);

        doorPivot.localRotation =
            Quaternion.Lerp(doorPivot.localRotation, targetRot, Time.deltaTime * speed);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        if (closeCoroutine != null)
        {
            StopCoroutine(closeCoroutine);
            closeCoroutine = null;
        }

        open = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        closeCoroutine = StartCoroutine(CloseWithDelay());
    }

    private IEnumerator CloseWithDelay()
    {
        yield return new WaitForSeconds(2f);
        open = false;
        closeCoroutine = null;
    }
}
