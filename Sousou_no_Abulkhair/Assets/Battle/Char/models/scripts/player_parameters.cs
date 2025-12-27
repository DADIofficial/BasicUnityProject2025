using UnityEngine;
using System.Collections;
using UnityEngine.UI;


public class player_parameters : MonoBehaviour
{
    [SerializeField] private Image _HPImg;
    [SerializeField] private Image _ManaImg;
    [SerializeField] private Image _StaminaImg;

    void Start(){
        
    }
    void Update(){}

    public void UpdateHP(float maxHP, float currentHP)
    {
        _HPImg.fillAmount = maxHP <= 0 ? 0 : currentHP / maxHP;
    }

    public void UpdateMana(float maxMana, float currentMana)
    {
        _ManaImg.fillAmount = maxMana <= 0 ? 0 : currentMana / maxMana;
    }

    public void UpdateStamina(float maxStamina, float currentStamina)
    {
        _StaminaImg.fillAmount = maxStamina <= 0 ? 0 : currentStamina / maxStamina;
    }


}
