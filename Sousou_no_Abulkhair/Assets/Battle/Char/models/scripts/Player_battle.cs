using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using UnityEngine.UI;



public class Player_battle : MonoBehaviour
{
    [SerializeField] private CanvasGroup BattleCanva; 
    [SerializeField] private Button magicButton;

    public DamageSystem damageSystem;

    public Battle_manager battleManager;

    public Animator animator;

    private Transform currentTargetPoint;
    private int currentEnemyID = -1;

    private Vector3 StartPos;
    private Quaternion StartRot;

    private float delay;
    private bool Basic = false;
    private bool Ench = false;

    public bool IsAttacking { get; private set; } = false;



    [SerializeField] private player_parameters parameters;
    [SerializeField] private Collider BattleEnemCol; 

    public float hp;
    public float mana;
    public float stamina;
    private float MaxHP;
    private float Maxstamina;
    private float MaxMana;

    private const float MAX_VALUE = 100f;


    // [SerializeField] private Collider playerCollider; 


    void Start()
    {
        var save = GameManager.Instance.saveData;

        hp = save.health;
        mana = save.mana;
        stamina = save.stamina;


        MaxHP = MAX_VALUE;
        MaxMana = MAX_VALUE;
        Maxstamina = MAX_VALUE;

        StartPos = transform.position;
        StartRot = transform.rotation;

        StartCoroutine(InitUI());

    }

    void Update(){}

    public void SetTarget(Transform targetPoint, int enemyID)
    {
        currentTargetPoint = targetPoint;
        currentEnemyID = enemyID;

        // Debug.Log($"Target selected: EnemyID {enemyID}");
    }

    private IEnumerator InitUI()
    {
        yield return null; // ⬅ ждём 1 кадр

        parameters.UpdateHP(MaxHP, hp);
        parameters.UpdateMana(MaxMana, mana);
        parameters.UpdateStamina(Maxstamina, stamina);

        Debug.Log(
            $"[UI INIT]\nHP: {hp}/{MaxHP}\nMana: {mana}/{MaxMana}\nStamina: {stamina}/{Maxstamina}"
        );
    }


    public void Basic_Attack()
    {
        if (IsAttacking) return;
        damageSystem.BasicDamage();
        animator.SetTrigger("Basic_attack");
        IsAttacking = true;
        Basic = true;
        TryTeleportToTarget();
    }

    public void Ench_Attack()
    {
        if (IsAttacking) return;

        stamina -= 15;
        stamina = Mathf.Clamp(stamina, 0, Maxstamina);
        parameters.UpdateStamina(Maxstamina, stamina);

        damageSystem.EnchDamage();
        animator.SetTrigger("Ench");
        IsAttacking = true;
        Ench = true;
        TryTeleportToTarget();
    }

    public void Magic_Attack()
    {
        if (IsAttacking) return;

        if (mana <= 0)
        {
            Debug.Log("Not enough mana");
            return;
        }

        mana -= 25;
        mana = Mathf.Clamp(mana, 0, MaxMana);
        parameters.UpdateMana(MaxMana, mana);


        Debug.Log("magic attack");
        IsAttacking = true;
        StartCoroutine(MDamageforAll());
        animator.SetTrigger("magic_attack");

        
    }

    private void TryTeleportToTarget()
    {
        BattleCanva.GetComponent<CanvasGroup>().interactable = false;
        BattleCanva.GetComponent<CanvasGroup>().blocksRaycasts = false;

        if (currentTargetPoint == null)
        {
            Debug.LogWarning("No target selected!");
            StartCoroutine(TryTeleportToStart());
            return;
        }

        Vector3 direction = -currentTargetPoint.forward;
        direction.y = 0;
        direction.Normalize();

        Vector3 teleportPos = currentTargetPoint.position - direction * 1.5f;
        teleportPos.y -= 0;

        Quaternion lookRot = Quaternion.LookRotation(direction);
        lookRot *= Quaternion.Euler(0, 60f, 0);

        transform.position = teleportPos;
        transform.rotation = lookRot;

        StartCoroutine(TryTeleportToStart());

    }

    private IEnumerator TryTeleportToStart()
    {
        if(Basic){
            yield return new WaitForSeconds(3f);
            Basic = false;
        }
        else if(Ench){
            yield return new WaitForSeconds(6f);
            Ench = false;
        }
        transform.position = StartPos ;
        transform.rotation = StartRot;  

        yield return new WaitForSeconds(2f);
        battleManager.EnemiesAttack();
    }

    public void PlayerMode(){
        BattleCanva.GetComponent<CanvasGroup>().interactable = true;
        BattleCanva.GetComponent<CanvasGroup>().blocksRaycasts = true;
        IsAttacking = false; 
    }


    private IEnumerator MDamageforAll()
    {
        BattleCanva.GetComponent<CanvasGroup>().interactable = false;
        BattleCanva.GetComponent<CanvasGroup>().blocksRaycasts = false;
        yield return new WaitForSeconds(3f);

        foreach (var enemy in battleManager.BattleEnemies)
        {
            if (enemy != null && enemy.Health > 0)
            {
                enemy.Health -= damageSystem.MagicDamage();
                enemy.UpdateHealthBar();

                if (enemy.Health <= 0)
                    enemy.Die();

            }
        }

        yield return new WaitForSeconds(2f);
        battleManager.EnemiesAttack();
    }


    private void UpdateMagicButton()
    {
        if (magicButton == null) return;

        magicButton.interactable = mana <= 0;
    }




    private void OnTriggerEnter(Collider other){


        if (other.CompareTag("battleEnemWeap"))
        {

            if (parameters != null){
                hp -= 5;
                hp = Mathf.Clamp(hp, 0, MaxHP);
                parameters.UpdateHP(MaxHP, hp);
            }

            if (hp <=0){
                Debug.Log("you die");
                
            }

        }

    }




    

}

