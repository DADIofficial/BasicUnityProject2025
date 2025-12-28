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



    [SerializeField] public player_parameters parameters;
    [SerializeField] private Collider BattleEnemCol; 

    public float hp;
    public float mana;
    public float stamina;
    public float MaxHP;
    public float Maxstamina;
    public float MaxMana;

    private int lastHp;
    private int lastMana;
    private int lastStamina;


    private const float MAX_VALUE = 100f;

    [Header("Weapon")]
    [SerializeField] private Transform weaponSocket;
    [SerializeField] private List<WeaponItem> allWeapons;
    private GameObject currentWeaponObject;
    private WeaponItem currentWeaponItem;

    private Collider currentWeaponCollider;



    // [SerializeField] private Collider playerCollider; 


    void Awake()
    {
        GameManager.Instance.RegisterBattlePlayer(this);
    }


    void Start()
    {
        var save = GameManager.Instance.saveData;

        hp = save.health;
        mana = save.mana;
        stamina = save.stamina;


        MaxHP = MAX_VALUE;
        MaxMana = MAX_VALUE;
        Maxstamina = MAX_VALUE;

        lastHp = (int)hp;
        lastMana = (int)mana;
        lastStamina = (int)stamina;

        StartPos = transform.position;
        StartRot = transform.rotation;

        EquipWeaponByID(save.weaponID);
        damageSystem.InitFromSave(save.weaponID);


        StartCoroutine(InitUI());

    }

    void Update(){
        
        }

    public void SetMagic(){
        var save = GameManager.Instance.saveData;
        damageSystem.InitFromSaveMagic(save.magicID);

        
    }

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

        if (currentWeaponCollider != null)
            currentWeaponCollider.enabled = true;

        damageSystem.BasicDamage();
        animator.SetTrigger("Basic_attack");
        IsAttacking = true;
        Basic = true;
        TryTeleportToTarget();
    }

    public void Ench_Attack()
    {
        if (IsAttacking) return;

        if (currentWeaponCollider != null)
            currentWeaponCollider.enabled = true;

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

        if (currentWeaponCollider != null)
            currentWeaponCollider.enabled = false;

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


    private void EquipWeaponByID(int weaponID)
    {
        // удаляем старое оружие
        if (currentWeaponObject != null)
            Destroy(currentWeaponObject);

        currentWeaponItem = null;

        // ищем WeaponItem по ID
        foreach (var weapon in allWeapons)
        {
            if (int.TryParse(weapon.itemId, out int id) && id == weaponID)
            {
                currentWeaponItem = weapon;
                break;
            }
        }

        if (currentWeaponItem == null)
        {
            Debug.LogError($"Weapon with ID {weaponID} not found");
            return;
        }

        // создаём prefab
        currentWeaponObject = Instantiate(
            currentWeaponItem.weaponPrefab,
            weaponSocket
        );


        currentWeaponCollider =
            currentWeaponObject.GetComponentInChildren<Collider>(true);

        currentWeaponObject.transform.localPosition = Vector3.zero;
        currentWeaponObject.transform.localRotation = Quaternion.identity;
    }

    public void OnPotionUsed(PotionItem potion)
    {
        switch (potion.potionType)
        {
            case PotionType.Health:
                hp += potion.restoreAmount;
                hp = Mathf.Clamp(hp, 0, MaxHP);
                parameters.UpdateHP(MaxHP, hp);
                break;

            case PotionType.Mana:
                mana += potion.restoreAmount;
                mana = Mathf.Clamp(mana, 0, MaxMana);
                parameters.UpdateMana(MaxMana, mana);
                break;

            case PotionType.Stamina:
                stamina += potion.restoreAmount;
                stamina = Mathf.Clamp(stamina, 0, Maxstamina);
                parameters.UpdateStamina(Maxstamina, stamina);
                break;
        }
    }


    private void SyncFromSave()
    {

        
        var save = GameManager.Instance.saveData;

        if (save.health != lastHp)
        {
            hp = save.health;
            parameters.UpdateHP(MaxHP, hp);
            lastHp = save.health;
        }

        if (save.mana != lastMana)
        {
            Debug.Log("mana");
            mana = save.mana;
            parameters.UpdateMana(MaxMana, mana);
            lastMana = save.mana;
        }

        if (save.stamina != lastStamina)
        {
            Debug.Log("stamina");
            stamina = save.stamina;
            parameters.UpdateStamina(Maxstamina, stamina);
            lastStamina = save.stamina;
        }
    }





    

}

