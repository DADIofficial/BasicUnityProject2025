using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class Battle_Open : MonoBehaviour
{
    private EnemyWorldID enemyWorldID;
    private Transform player;
    private bool triggered;

    public float EnemyHP = 100f;
    public int EnemyAttack = 5;

    private void Awake()
    {
        enemyWorldID = GetComponent<EnemyWorldID>();
        if (enemyWorldID == null)
        {
            Debug.LogError("[Battle_Open] EnemyWorldID missing!");
            return;
        }

        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        if (player == null)
        {
            Debug.LogError("[Battle_Open] Player not found!");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (triggered)
            return;

        var playerScript1 = Player.instance;

        if (!playerScript1.IsAttacking)
            return;

        if (other != playerScript1.GetCurrentWeaponCollider())
            return;


        triggered = true;

        var save = GameManager.Instance.saveData;
        var playerScript = player.GetComponent<Player>();

        // ✅ только вход в бой
        save.battleEntryEnemyPosition = transform.position;

        save.playerPosition = player.position;
        save.playerRotation = player.rotation;
        save.hasSavedPosition = true;

        save.health = playerScript.health;
        save.mana = playerScript.mana;
        save.stamina = 100;

        save.enemyHP = EnemyHP;
        save.enemyAttack = EnemyAttack;


        save.weaponID = int.Parse(playerScript.GetCurrentWeaponID());
        // save.magicID = int.Parse(playerScript.GetCurrentMagicID());


        GameManager.Instance.currentEnemyID = enemyWorldID.id;

        // чтобы не триггерилось повторно
        GetComponent<Collider>().enabled = false;

        GameManager.Instance.SaveBattleSnapshot(
            player.GetComponent<PlayerInventory>()
        );


        StartCoroutine(LoadBattleScene());
    }

    private IEnumerator LoadBattleScene()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        yield return new WaitForSeconds(1f);

        SceneManager.LoadScene("BattleScene");
    }
}
