using UnityEngine;
using System.Collections;

public class dooropen : MonoBehaviour
{
    public Transform doorPivot;
    public float openAngle = -90f;
    public float speed = 2f;

    private bool open;
    private Coroutine closeCoroutine;
    private Collider triggerCol;

    private void Awake()
    {
        triggerCol = GetComponent<Collider>();
    }

    void Update()
    {
        float targetAngle = open ? openAngle : 0f;
        Quaternion targetRot = Quaternion.Euler(0, 0, targetAngle);

        doorPivot.localRotation =
            Quaternion.Lerp(doorPivot.localRotation, targetRot, Time.deltaTime * speed);
    }

    public void SetInitialState(bool opened)
    {
        open = opened;

        // сразу ставим нужный поворот, без анимации
        float angle = open ? openAngle : 0f;
        doorPivot.localRotation = Quaternion.Euler(0, 0, angle);
    }


    // 🔓 Вызывается ТОЛЬКО из Door
    public void Open()
    {
        if (closeCoroutine != null)
        {
            StopCoroutine(closeCoroutine);
            closeCoroutine = null;
        }

        open = true;
    }

    // 🔒 Вызывается ТОЛЬКО из Door
    public void Close()
    {
        open = false;
    }

    // 🔑 Включаем / выключаем триггер
    public void SetTriggerActive(bool active)
    {
        if (triggerCol != null)
            triggerCol.enabled = active;
    }

    // ⚠️ Эти методы теперь просто поддержка,
    // они не решают, можно ли открываться
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        // open = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        closeCoroutine = StartCoroutine(CloseWithDelay());
    }

    private IEnumerator CloseWithDelay()
    {
        yield return new WaitForSeconds(2f);
        open = false;
        closeCoroutine = null;
    }
}
