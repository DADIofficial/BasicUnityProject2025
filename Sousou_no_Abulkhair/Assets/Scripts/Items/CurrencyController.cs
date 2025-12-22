using TMPro;
using UnityEngine;

public class CurrencyController : MonoBehaviour
{
    public int coins = 0;
    public TextMeshProUGUI currencyText;
    [SerializeField] private Player player;

    private void Start()
    {
        UpdateUI();
    }

    private void UpdateUI()
    {
        coins = player.leaves;
        currencyText.text = $"Leaves: {coins}";
    }
}
