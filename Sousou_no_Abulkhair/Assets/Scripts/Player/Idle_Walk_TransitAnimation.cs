using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class Idle_Walk_TransitAnimation : MonoBehaviour
{
    private Animator anim;
    public float attackDelay = 0.2f;
    public float attackDuration = 1.0f;

    void Start()
    {
        anim = GetComponent<Animator>();
        anim.SetBool("IsAttacking", false);
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            StartCoroutine(AttackRoutine());
        }
    }

    IEnumerator AttackRoutine()
    {
        yield return new WaitForSeconds(attackDelay);

        anim.SetBool("IsAttacking", true);

        yield return new WaitForSeconds(attackDuration);

        anim.SetBool("IsAttacking", false);
    }
}
