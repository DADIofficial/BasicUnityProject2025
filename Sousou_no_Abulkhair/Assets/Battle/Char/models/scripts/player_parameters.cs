using UnityEngine;
using System.Collections;
using UnityEngine.UI;


public class player_parameters : MonoBehaviour
{
    [SerializeField] private Image _HPImg;
    [SerializeField] private Image _ManaImg;
    [SerializeField] private Image _StaminaImg;

    void Start(){}
    void Update(){}

    public void UpdateHP(float maxHP, float currentHP)
    {
        Debug.Log(currentHP);
        _HPImg.fillAmount = currentHP / maxHP;
    }

    public void UpdateMana(float maxMana, float currentMana)
    {
        _ManaImg.fillAmount = currentMana / maxMana;
    }

    public void UpdateStamina(float maxStamina, float currentStamina)
    {
        _StaminaImg.fillAmount = currentStamina / maxStamina;
    }

}
