using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Image _healthImg;

    public void UpdateHealth(float maxHealth, float currentHealth)
    {
        _healthImg.fillAmount = currentHealth / maxHealth;
    }
}
