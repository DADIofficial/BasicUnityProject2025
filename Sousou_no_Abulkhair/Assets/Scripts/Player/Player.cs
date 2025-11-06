using UnityEngine;

public class Player : MonoBehaviour
{
    public int health = 100;
    public int stamina = 100;
    public int mana = 100;
    public int leaves = 10;
    [SerializeField] private float speed = 5.0f;
    [SerializeField] private GameObject weapon = null;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
