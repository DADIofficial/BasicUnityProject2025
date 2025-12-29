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

    [Header("SFX")]
    [SerializeField] private SFXManager sfxManager; // можно оставить пустым — возьмёт из сцены

    private bool isAttacking = false;

    private void Awake()
    {
        if (anim == null)
            anim = GetComponent<Animator>();

        if (sfxManager == null)
            sfxManager = FindFirstObjectByType<SFXManager>(FindObjectsInactive.Include);
    }

    void Start()
    {
        if (anim != null)
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
        {
            isAttacking = false;
            yield break;
        }

        if (movementScript != null)
            movementScript.enabled = false;

        yield return new WaitForSeconds(attackDelay);


        if (sfxManager != null)
            sfxManager.PlaySFX(SFXType.Attack);
        else
            Debug.LogWarning($"{nameof(Idle_Walk_TransitAnimation)}: SFXManager не найден в сцене.");

        player.StartAttack();
        player.EnableWeaponHitbox();

        if (anim != null)
            anim.SetBool("IsAttacking", true);

        yield return new WaitForSeconds(attackDuration);

        if (anim != null)
            anim.SetBool("IsAttacking", false);

        player.DisableWeaponHitbox();
        player.EndAttack();

        if (movementScript != null)
            movementScript.enabled = true;

        isAttacking = false;
    }
}
