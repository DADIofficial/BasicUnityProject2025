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

    private bool isAttacking = false;

    void Start()
    {
        anim = GetComponent<Animator>();
        anim.SetBool("IsAttacking", false);
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

        yield return new WaitForSeconds(attackDelay);

        anim.SetBool("IsAttacking", true);

        yield return new WaitForSeconds(attackDuration);

        anim.SetBool("IsAttacking", false);

        // Включаем движение обратно
        if (movementScript != null)
            movementScript.enabled = true;

        isAttacking = false;
    }
}
