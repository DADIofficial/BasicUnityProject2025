using UnityEngine;
using System.Collections;

public class Idle_Walk_TransitAnimation : MonoBehaviour
{
    private Animator anim;

    [Header("Attack timings")]
    public float attackDelay = 0.1f;
    public float attackDuration = 0.5f;

    [Header("Movement")]
    public MonoBehaviour movementScript;

    [Header("Weapon")]
    [SerializeField] private Collider weaponCollider;

    private bool isAttacking = false;

    void Start()
    {
        anim = GetComponent<Animator>();
        anim.SetBool("IsAttacking", false);

        // МЕЧ ВСЕГДА ВЫКЛЮЧЕН ВНЕ АТАКИё
        if (weaponCollider != null)
            weaponCollider.enabled = false;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !isAttacking)
        {
            StartCoroutine(AttackRoutine());
        }
    }

    IEnumerator AttackRoutine()
    {
        isAttacking = true;

        if (movementScript != null)
            movementScript.enabled = false;

        // задержка перед ударом (wind-up)
        yield return new WaitForSeconds(attackDelay);

        // ВКЛЮЧАЕМ ХИТБОКС
        if (weaponCollider != null)
            weaponCollider.enabled = true;

        anim.SetBool("IsAttacking", true);

        // время активного удара
        yield return new WaitForSeconds(attackDuration);

        anim.SetBool("IsAttacking", false);

        // ВЫКЛЮЧАЕМ ХИТБОКС
        if (weaponCollider != null)
            weaponCollider.enabled = false;

        if (movementScript != null)
            movementScript.enabled = true;

        isAttacking = false;
    }
}
