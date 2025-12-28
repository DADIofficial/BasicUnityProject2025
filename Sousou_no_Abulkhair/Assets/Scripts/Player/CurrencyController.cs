using TMPro;
using UnityEngine;

public class CurrencyController : MonoBehaviour
{
    public int coins = 0;
    public TextMeshProUGUI currencyText;
    [SerializeField] private Player player;

    public static CurrencyController Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        UpdateUI();
    }


    public void AddLeaves(int amount)
    {
        //Debug.Log("coins");
        player.leaves += amount;
        UpdateUI();
    }

    private void UpdateUI()
    {
        coins = player.leaves;
        currencyText.text = $"{coins}";
    }
}
