using UnityEngine;

public class PlayerSpawn : MonoBehaviour
{
    [SerializeField] private Transform defaultSpawnPoint;

    private void Start()
    {
    
        var save = GameManager.Instance.saveData;
        var player = GetComponent<Player>();

        if (save != null && save.hasSavedPosition)
        {
            transform.position = save.playerPosition;
            transform.rotation = save.playerRotation;

                player.health = save.health;
                player.mana = save.mana;
                player.stamina = save.stamina;
        }
        else if (defaultSpawnPoint != null)
        {
            transform.position = defaultSpawnPoint.position;
            transform.rotation = defaultSpawnPoint.rotation;
        }
    }
}
