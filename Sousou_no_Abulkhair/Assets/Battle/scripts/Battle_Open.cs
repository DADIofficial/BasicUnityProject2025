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
        // 🔥 берём ID САМОГО СЕБЯ
        enemyWorldID = GetComponent<EnemyWorldID>();

        // игрок — глобальный
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (triggered) return;

        if (other.gameObject.layer == LayerMask.NameToLayer("PlayerWeapon"))
        {
            triggered = true;

            var save = GameManager.Instance.saveData;
            var playerScript = player.GetComponent<Player>();

            save.playerPosition = player.position;
            save.playerRotation = player.rotation;
            save.hasSavedPosition = true;

            save.health = playerScript.health;
            save.mana = playerScript.mana;
            save.stamina = 100;

            GameManager.Instance.currentEnemyID = enemyWorldID.id;

            StartCoroutine(LoadBattleScene());
        }
    }


    private IEnumerator LoadBattleScene()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        yield return new WaitForSeconds(1f);

        SceneManager.LoadScene("BattleScene");
    }
}
