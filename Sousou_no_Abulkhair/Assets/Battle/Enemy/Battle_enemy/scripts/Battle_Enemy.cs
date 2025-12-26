using UnityEngine;
using UnityEngine.Events;
using System.Collections;

[System.Serializable]
public class EnemyClickedEvent : UnityEvent<int> { }




public class Battle_Enemy : MonoBehaviour
{   
    public Animator animator;

    public DamageSystem d;

    [SerializeField] private Collider playerWeaponBattle; 
    public Transform playerPoint;
    
    [SerializeField] public float Health; 
    [SerializeField] private HealthBar healthBar;

    private float MaxHP;
    private Vector3 StartPos;
    private Quaternion StartRot;

    public bool EnemyAttacking { get; private set; } = false;

    [SerializeField] private Collider enemyCollider; 


    void Start()
    {
        MaxHP = Health;

        enemyCollider.isTrigger = true;
        StartPos = transform.position;
        StartRot = transform.rotation;

    }

    void Update(){}

    public int enemyID;
    public Transform targetPoint;

    public Player_battle player;

    public EnemyClickedEvent OnEnemyClicked;

    void OnMouseDown()
    {
        if (player != null && player.IsAttacking)
            return;

        player.SetTarget(targetPoint, enemyID);
    }
    
    private void OnTriggerEnter(Collider other){

        if (other.CompareTag("PlayerWeapon"))
        {

            Health -= d.Damage;

            Debug.Log("Enemy "+ Health);

            if (healthBar != null)
                UpdateHealthBar();

            if(Health <=0){
                StartCoroutine(KillEnemy());
            }

        }

    }

    public void UpdateHealthBar()
    {
        if (healthBar != null)
            healthBar.UpdateHealth(MaxHP, Health);
    }

    public void Die()
    {
        StartCoroutine(KillEnemy());
    }


    private IEnumerator KillEnemy(){
        yield return new WaitForSeconds(3f);
        Destroy(gameObject);
    }

    
    public void Enemy_Attack()
    {
        if (EnemyAttacking) return;
        animator.SetTrigger("Enemy_attack");
        EnemyAttacking = true;
        enemyCollider.isTrigger = false;
        TeleportToP();
    }


    private void TeleportToP()
    {

        Vector3 direction = -playerPoint.forward;
        direction.y = 0;
        direction.Normalize();

        direction = Quaternion.Euler(0, -60f, 0) * direction;

        Vector3 teleportPos = playerPoint.position - direction * 1f;
        teleportPos.y -= 0;

        Quaternion lookRot = Quaternion.LookRotation(direction);

        transform.position = teleportPos;
        transform.rotation = lookRot;

        StartCoroutine(TeleportToS());

    }

    private IEnumerator TeleportToS()
    {
        yield return new WaitForSeconds(3f);

        transform.position = StartPos ;
        transform.rotation = StartRot;  
        EnemyAttacking = false;
        enemyCollider.isTrigger = true;
    }
}
