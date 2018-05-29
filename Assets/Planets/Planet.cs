using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Planet : MonoBehaviour {
    [SerializeField] Universe m_universe;
    [SerializeField] int m_currentPopulation;
    [Range(0, 100)] [SerializeField] float m_happyPopulationPerc;
    [SerializeField] float m_powerAvailable;
    [SerializeField] float m_powerConsumption;

    [SerializeField] float m_foodAvailable;
    [SerializeField] float m_foodConsumption;

    [SerializeField] float m_growthRate;

    public int GetCurrentPopulation()
    {
        return m_currentPopulation;
    }
    public float GetGrowthRate()
    {
        return m_growthRate;
    }

    public float GetHappyPopPerc()
    {
        return m_happyPopulationPerc;
    }


    public void Start()
    {
        m_universe = FindObjectOfType<Universe>();
        m_universe.onDayChanged += ProcessDayChange;
    }

    private void ProcessDayChange(int days)
    {
        for(int day = 0; day < days; day++)
        {
            m_currentPopulation += Mathf.FloorToInt(m_currentPopulation * m_growthRate);
            m_growthRate += UnityEngine.Random.Range(-0.05f, 0.05f);
            m_happyPopulationPerc += UnityEngine.Random.Range(-1, 2);
        }
        
    }

    public void UpdateStats()
    {
        
    }
}
