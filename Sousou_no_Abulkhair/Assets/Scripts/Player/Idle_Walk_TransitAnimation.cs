using UnityEngine;
using System.Collections;

public class Idle_Walk_TransitAnimation : MonoBehaviour
{
    [SerializeField] private Animator anim;

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

        var player = Player.instance;
        if (player == null)
            yield break;

        // 🔒 блокируем движение
        if (movementScript != null)
            movementScript.enabled = false;

        // ⏳ задержка перед ударом
        yield return new WaitForSeconds(attackDelay);

        // ⚔️ НАЧАЛО АТАКИ
        player.StartAttack();
        player.EnableWeaponHitbox();
        anim.SetBool("IsAttacking", true);

        // 🩸 окно удара
        yield return new WaitForSeconds(attackDuration);

        // ❌ КОНЕЦ АТАКИ
        anim.SetBool("IsAttacking", false);
        player.DisableWeaponHitbox();
        player.EndAttack();

        // 🔓 возвращаем движение
        if (movementScript != null)
            movementScript.enabled = true;

        isAttacking = false;
    }
}
