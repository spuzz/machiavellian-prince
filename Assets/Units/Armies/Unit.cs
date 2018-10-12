using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour {

    [SerializeField] string unitName;
    [SerializeField] int power;
    [SerializeField] int maintenance;
    [SerializeField] [Range(0,100)] float health;

    
    public string GetName()
    {
        return unitName;
    }

    public int GetPower()
    {
        return Convert.ToInt32(Convert.ToDouble(power) * (health / 100.0f));
    }

    public float GetBasePower()
    {
        return power;
    }

    public int GetMaintenance()
    {
        return maintenance;
    }

    public float GetHealth()
    {
        return maintenance;
    }

    public bool Damage(float v)
    {
        health -= v;
        if(health <= 0 )
        {
            return false;
        }
        return true;
    }

    public void heal(float restore)
    {
        health += restore;
        if(health > 100)
        {
            health = 100;
        }
    }
}
