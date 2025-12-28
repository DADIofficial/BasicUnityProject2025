using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class Battle_Open : MonoBehaviour
{
    private EnemyWorldID enemyWorldID;
    private Transform player;
    private bool triggered;

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

        if (other.gameObject.layer != LayerMask.NameToLayer("PlayerWeapon"))
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

        save.weaponID = int.Parse(playerScript.GetCurrentWeaponID());

        GameManager.Instance.currentEnemyID = enemyWorldID.id;

        // чтобы не триггерилось повторно
        GetComponent<Collider>().enabled = false;

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
