using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using System.Collections.Generic;



public class DamageSystem : MonoBehaviour
{
    
    public int PlayerDamage;
    public int CritRate;
    public float CritDamage;
    public int WeaponID;
    public int MagicID;
    public float Damage = 0;
    private float WCoff;
    private float MCoff;
    public WeaponData AttackDatas;


    void Start()
    {
        WData Wdata = AttackDatas.GetWeaponByID(WeaponID);
        WCoff = Wdata.weaponCoff;

        MData Mdata = AttackDatas.GetMagicByID(MagicID);
        MCoff = Mdata.magicCoff;

        

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void BasicDamage(){
        if (CritRate >= Random.Range(0, 100)){
            Damage = PlayerDamage * CritDamage * WCoff;
            Debug.Log("Crit B attack " + Damage);
        }
        else{
            Damage = PlayerDamage * WCoff;
            Debug.Log("B attack "+ Damage);
            Debug.Log("Coff "+ WCoff);
        }     
    }

    public void EnchDamage(){
        if (CritRate >= Random.Range(0, 100)){
            Damage = PlayerDamage * CritDamage * WCoff*1.5f;
            Debug.Log("Crit E attack " + Damage);
        }
        else{
            Damage = PlayerDamage * WCoff*1.5f;
            Debug.Log("E attack "+ Damage);
        }     
    }

    

    public float MagicDamage(){
        if (CritRate >= Random.Range(0, 100)){
            Damage = PlayerDamage * CritDamage * MCoff *1f;
            Debug.Log("Crit M attack " + Damage);
            return Damage;
        }
        else{
            Damage = PlayerDamage * MCoff *1f;
            Debug.Log("M attack "+ Damage);
            return Damage;
        }     
    }
}
