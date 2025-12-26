using UnityEngine;
using System.Collections.Generic;
using System.Collections; 



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
