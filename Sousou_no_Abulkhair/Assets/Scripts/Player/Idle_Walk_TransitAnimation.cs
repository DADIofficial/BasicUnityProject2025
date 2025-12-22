using UnityEngine;
using System.Collections;

public class Idle_Walk_TransitAnimation : MonoBehaviour
{
    private Animator anim;

    [Header("Attack timings")]
    public float attackDelay = 0.1f;     // задержка перед началом удара
    public float attackDuration = 0.5f;  // длительность анимации удара

    [Header("Movement")]
    // Сюда в инспекторе перетаскиваешь скрипт, который отвечает за движение
    public MonoBehaviour movementScript;

    private bool isAttacking = false;

    void Start()
    {
        anim = GetComponent<Animator>();
        anim.SetBool("IsAttacking", false);
    }

    void Update()
    {
        // Не даём бить, пока уже идёт атака
        if (Input.GetMouseButtonDown(0) && !isAttacking)
        {
            StartCoroutine(AttackRoutine());
        }
    }

    IEnumerator AttackRoutine()
    {
        isAttacking = true;

        // Отключаем движение, чтобы персонаж стоял
        if (movementScript != null)
            movementScript.enabled = false;

        // Ждём задержку до начала удара
        yield return new WaitForSeconds(attackDelay);

        anim.SetBool("IsAttacking", true);

        // Ждём пока проигрывается анимация удара
        yield return new WaitForSeconds(attackDuration);

        anim.SetBool("IsAttacking", false);

        // Включаем движение обратно
        if (movementScript != null)
            movementScript.enabled = true;

        isAttacking = false;
    }
}
