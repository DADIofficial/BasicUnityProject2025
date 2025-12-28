using UnityEngine;
using System.Collections.Generic;
using System.Collections; 
using UnityEngine.SceneManagement;



public class Battle_manager : MonoBehaviour
{
    public Player_battle player;
    public List<Battle_Enemy> BattleEnemies = new List<Battle_Enemy>();

    void Start(){}

    void Update(){}

    public void EnemiesAttack()
    {
        StartCoroutine(EnemyAttackSequence());
    }

    private IEnumerator EnemyAttackSequence()
    {
        foreach (var enemy in BattleEnemies)
        {
            if (enemy != null && enemy.Health > 0)
            {
                enemy.Enemy_Attack(); 
                yield return new WaitForSeconds(4f);
            }
        }

        if (AreAllEnemiesDead())
        {
            Debug.Log("Win");

            var save = GameManager.Instance.saveData;
            var battlePlayer = player;

            save.health = Mathf.RoundToInt(battlePlayer.hp);
            save.mana = Mathf.RoundToInt(battlePlayer.mana);
            save.stamina = 100;

            if (save.battleEntryEnemyPosition == Vector3.zero)
            {
                Debug.LogError("[BattleManager] battleEntryEnemyPosition is ZERO");
            }
            else
            {
                save.lastKilledEnemyPosition = save.battleEntryEnemyPosition;
                save.lastKilledEnemyId = GameManager.Instance.currentEnemyID;
            }

            GameManager.Instance.KillEnemy(GameManager.Instance.currentEnemyID);
            GameManager.Instance.currentEnemyID = null;

            SceneManager.LoadScene("MainLevel");
            yield break;
        }


        player.PlayerMode();
    }


    private bool AreAllEnemiesDead()
    {
        foreach (var enemy in BattleEnemies)
        {
            if (enemy != null && enemy.Health > 0)
                return false;
        }
        return true;
    }
}
