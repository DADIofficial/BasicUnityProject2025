using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class Idle_Walk_TransitAnimation : MonoBehaviour
{
    private Animator anim;
    void Start()
    {
        anim = GetComponent<Animator>();
        anim.SetBool("IsAttacking", false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            anim.SetBool("IsAttacking", true);
            StartCoroutine(Stop());
        }
    }
    private IEnumerator Stop()
    {
        yield return new WaitForSeconds(3f);
        anim.SetBool("IsAttacking", false);
    }
}
